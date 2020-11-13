using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Helpers;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CryptocopDbContext _dbContext;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public OrderRepository(CryptocopDbContext dbContext, IShoppingCartRepository shoppingCartRepository)
        {
            _dbContext = dbContext;
            _shoppingCartRepository = shoppingCartRepository;
        }

        private User GetUser(string email)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) { throw new ResourceNotFoundException("User not found"); }
            return user; 
        }

        private Address GetAddress(int addressId, User user)
        {
            var address = _dbContext.Addresses.FirstOrDefault(a => a.Id == addressId && a.UserId == user.Id);
            if (address == null) { throw new ResourceNotFoundException("Address not found"); }
            return address; 
        }

        private PaymentCard GetPaymentCard(int paymentCardId)
        {
            var card = _dbContext.PaymentCards.FirstOrDefault(c => c.Id == paymentCardId);
            if (card == null) { throw new ResourceNotFoundException("PaymentCard not found"); }
            return card;
        }

        private double GetTotalPrice(string email)
        {
            // Get orderItems
            var cartItems = _shoppingCartRepository.GetCartItems(email);

            var totalPrice = 0.0;
            foreach(ShoppingCartItemDto item in cartItems)
            {
                totalPrice += item.TotalPrice;
            }
            return totalPrice;
        }

        private List<OrderItemDto> GetAllOrderItems(int orderId)
        {
            var orderItems = _dbContext
                                .OrderItems
                                .Where(i => i.OrderId == orderId)
                                .Select(i => new OrderItemDto
                                {
                                    Id = i.Id,
                                    ProductIdentifier = i.ProductIdentifier,
                                    Quantity = i.Quantity,
                                    UnitPrice = i.UnitPrice,
                                    TotalPrice = i.TotalPrice
                                }).ToList();
            return orderItems;
        }

        public IEnumerable<OrderDto> GetOrders(string email)
        {
            // Get user
            var user = GetUser(email);


            var orders = _dbContext
                            .Orders
                            .Where(o => o.userId == user.Id)
                            .Select(o => new OrderDto
                            {
                                Id = o.Id,
                                Email = o.Email,
                                FullName = o.FullName,
                                StreetName = o.StreetName,
                                HouseNumber = o.HouseNumber,
                                ZipCode = o.ZipCode,
                                Country = o.Country,
                                City = o.City,
                                CardholderName = o.CardHolderName,
                                CreditCard = o.MaskedCreditCard,
                                OrderDate = o.OrderDate.ToString("dd'.'MM'.'yyyy"),
                                TotalPrice = o.TotalPrice,
                                OrderItems = null
                            }).ToList();
            foreach(var order in orders)
            {
                order.OrderItems = GetAllOrderItems(order.Id);
            }
            return orders;
        }

        private void CreateOrderItems(string email, int orderId)
        {
            // Get all the items from the shoppingCart
            var cartItems = _shoppingCartRepository.GetCartItems(email);

            // Insert all orderItems 
            foreach(var item in cartItems)
            {
                var entity = new OrderItem
                {
                    OrderId = orderId,
                    ProductIdentifier = item.ProductIdentifier,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.TotalPrice
                };

                _dbContext.OrderItems.Add(entity);
            }

            // Save changes 
            _dbContext.SaveChanges();
        }

        public OrderDto CreateNewOrder(string email, OrderInputModel order)
        {
            // Get user
            var user = GetUser(email);
            if (user == null) { throw new ResourceNotFoundException("user not found"); } 

            // Get address
            var address = GetAddress(order.AddressId, user);
            if (address == null) { throw new ResourceNotFoundException("address not found"); } 

            // Get paymentCard
            var paymentCard = GetPaymentCard(order.PaymentCardId);
            if (paymentCard == null) { throw new ResourceNotFoundException("paymentCard not found"); } 

            // Get the total price from shopping cart 
            var totalPrice = GetTotalPrice(email);

            var entity = new Order
            {
                Email = user.Email,
                FullName = user.FullName,
                StreetName = address.StreetName,
                HouseNumber = address.HouseNumber,
                ZipCode = address.ZipCode,
                Country = address.Country,
                City = address.City,
                CardHolderName = paymentCard.CardholderName,
                MaskedCreditCard = PaymentCardHelper.MaskPaymentCard(paymentCard.CardNumber), 
                OrderDate = DateTime.Now,
                TotalPrice = (float) totalPrice,
                userId = user.Id
            };

            // Add the order to db
            _dbContext.Orders.Add(entity);
            _dbContext.SaveChanges();

            // Create all the orderItems 
            CreateOrderItems(email, entity.Id);

            return new OrderDto
            {
                Id = entity.Id,
                Email = entity.Email,
                FullName = entity.FullName,
                StreetName = entity.StreetName,
                HouseNumber = entity.HouseNumber,
                ZipCode = entity.ZipCode,
                Country = entity.Country,
                City = entity.City,
                CardholderName = entity.CardHolderName,
                CreditCard = entity.MaskedCreditCard, 
                OrderDate = entity.OrderDate.ToString("dd.mm.yyyy", CultureInfo.InvariantCulture),
                TotalPrice = entity.TotalPrice,
                OrderItems = GetAllOrderItems(entity.Id)
            };
        }
    }
}