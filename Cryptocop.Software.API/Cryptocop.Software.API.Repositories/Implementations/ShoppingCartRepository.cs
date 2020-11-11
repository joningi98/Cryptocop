using System.Collections.Generic;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Repositories.Contexts;
using System.Linq;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.Exceptions;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly CryptocopDbContext _dbContext;

        public ShoppingCartRepository(CryptocopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private User GetUser(string email)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) { throw new ResourceNotFoundException("User not found"); }
            return user; 
        }

        private ShoppingCart GetShoppingCart(int userId)
        {
            var shoppingCart = _dbContext.ShoppingCarts.FirstOrDefault(s => s.UserId == userId);
            return shoppingCart;
        }

        public IEnumerable<ShoppingCartItemDto> GetCartItems(string email)
        {
            // Get user
            var user = GetUser(email);

            // Get shoppingCart
            var shoppingCart = GetShoppingCart(user.Id);

            var cartItems = _dbContext
                                .ShoppingCartItems
                                .Where(i => i.ShoppingCartId == shoppingCart.Id)
                                .Select(c => new ShoppingCartItemDto
                                {
                                    Id = c.Id,
                                    ProductIdentifier = c.ProductIdentifier,
                                    Quantity = c.Quantity,
                                    UnitPrice = c.UnitPrice,
                                    TotalPrice = c.Quantity * c.UnitPrice
                                }).ToList();
            return cartItems;
        }

        public void AddCartItem(string email, ShoppingCartItemInputModel shoppingCartItemItem, float priceInUsd)
        {
            // Get user
            var user = GetUser(email);

            // Get shoppingCart
            var shoppingCart = GetShoppingCart(user.Id);

            // If shoppingCart does not exist then create one 
            if (shoppingCart == null)
            {
                var newShoppingCart = new ShoppingCart
                {
                    UserId = user.Id
                };

                _dbContext.ShoppingCarts.Add(newShoppingCart);
                _dbContext.SaveChanges();
            }

            // Create shoppingCartItem
            var entity = new ShoppingCartItem
            {
                ShoppingCartId = shoppingCart.Id,
                ProductIdentifier = shoppingCartItemItem.ProductIdentifier,
                Quantity = (float) shoppingCartItemItem.Quantity,
                UnitPrice = priceInUsd
            };

            _dbContext.ShoppingCartItems.Add(entity);
            _dbContext.SaveChanges();
        }

        public void RemoveCartItem(string email, int itemId)
        {
            //Get user 
            var user = GetUser(email);

            //Get shoppingCart
            var shoppingCart = GetShoppingCart(user.Id);

            // Get cartItem
            var cartItem = _dbContext.ShoppingCartItems.FirstOrDefault(i => i.Id == itemId && i.ShoppingCartId == shoppingCart.Id);

            // Remove Item & Save
            _dbContext.ShoppingCartItems.Remove(cartItem);
            _dbContext.SaveChanges();
        }

        public void UpdateCartItemQuantity(string email, int id, float quantity)
        {
            //Get user 
            var user = GetUser(email);

            //Get shoppingCart
            var shoppingCart = GetShoppingCart(user.Id);

            // Get cartItem and update value
            var cartItem =_dbContext.ShoppingCartItems.FirstOrDefault(i => i.ShoppingCartId == shoppingCart.Id && i.Id == id);
            if (cartItem == null) { throw new System.Exception("Item not found"); }
            System.Console.WriteLine(quantity);
            cartItem.Quantity = quantity;
            _dbContext.SaveChanges();
        }

        public void ClearCart(string email)
        {
            //Get user 
            var user = GetUser(email);

            //Get shoppingCart
            var shoppingCart = GetShoppingCart(user.Id);

            // Get query to delete & save changes
            var deleteQuery = _dbContext.ShoppingCartItems.Where(i => i.ShoppingCartId == shoppingCart.Id);
            _dbContext.ShoppingCartItems.RemoveRange(deleteQuery);
            _dbContext.SaveChanges();
        }

        public void DeleteCart(string email)
        {
            //Get user 
            var user = GetUser(email);

            //Get shoppingCart
            var shoppingCart = GetShoppingCart(user.Id);

            // Delete all items in cart
            ClearCart(email);

            // Get cart to delete
            var cart = _dbContext.ShoppingCarts.FirstOrDefault(c => c.UserId == user.Id);

            // Delete & Save
            _dbContext.ShoppingCarts.Remove(cart);
            _dbContext.SaveChanges();
        }
    }
}