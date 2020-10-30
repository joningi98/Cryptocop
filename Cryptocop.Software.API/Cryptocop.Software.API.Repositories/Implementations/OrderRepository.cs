using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CryptocopDbContext _dbContext;

        public OrderRepository(CryptocopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private User GetUser(string email)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) { throw new System.Exception("User not found"); }
            else { return user; }
        }

        private Address GetAddress(int addressId)
        {
            var address = _dbContext.Addresses.FirstOrDefault(a => a.Id == addressId);
            if (address == null) { return null; }
            else { return address; }
        }

        private PaymentCard GetPaymentCard(int paymentCardId)
        {
            var card = _dbContext.PaymentCards.FirstOrDefault(c => c.Id == paymentCardId);
            if (card == null) { return null; }
            else { return card; }
        }

        private double totalPrice(int orderId)
        {
            // Get orderItems
            var orders = GetOrderItems(orderId);

            var totalPrice = 0.0;
            foreach(OrderItemDto order in orders)
            {
                totalPrice += order.TotalPrice;
            }

            return totalPrice;
        }

        public List<OrderItemDto> GetOrderItems(int orderId)
        {
            var orders = _dbContext
                            .OrderItems
                            .Where(o => o.OrderId == orderId)
                            .Select(o => new OrderItemDto
                            {
                                Id = o.Id,
                                ProductIdentifier = o.ProductIdentifier,
                                Quantity = o.Quantity,
                                UnitPrice = o.UnitPrice,
                                TotalPrice = o.TotalPrice
                            }).ToList();
            return orders;
        }
        
        public IEnumerable<OrderDto> GetOrders(string email)
        {
            //TODO: Test
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
                                OrderDate = o.OrderDate.ToString("dd.mm.yyyy"),
                                TotalPrice = o.TotalPrice,
                                OrderItems = GetOrderItems(o.Id)
                            }).ToList();
            return orders;
        }

        public OrderDto CreateNewOrder(string email, OrderInputModel order)
        {
          /*   // Get user
            var user = GetUser(email);

            // Get address
            var address = GetAddress(order.AddressId);

            // Get paymentCard
            var paymentCard = GetPaymentCard(order.PaymentCardId);

            var totalPrice = totalPrice(order.)

            if (user != null && address != null)
            {
                var entity = new Order
                {
                    Id = _dbContext.Orders.Take()
                    Email = user.Email,
                    FullName = user.FullName,
                    HouseNumber = address.HouseNumber,
                    ZipCode = address.ZipCode,
                    Country = address.Country,
                    City = address.City,
                    CardHolderName = paymentCard.CardholderName,
                    MaskedCreditCard = "*" + paymentCard.CardNumber.Substring(12),
                    OrderDate = DateTime.Now,
                    TotalPrice = 
                }
            } */
            throw new NotImplementedException();
            
        }
    }
}