using System.Collections.Generic;

namespace Cryptocop.Software.API.Models.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        // Navigation Properties 
        public User user { get; set; }
        public List<ShoppingCartItem> shoppingCartItems { get; set; }
    }
}