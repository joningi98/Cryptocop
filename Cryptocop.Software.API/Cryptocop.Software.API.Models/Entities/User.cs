using System.Collections.Generic;

namespace Cryptocop.Software.API.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }

        // Navigation Properties 
        public List<Address> addresses { get; set; }
        public List<ShoppingCart> shoppingCarts { get; set; }
        public List<PaymentCard> paymentCards { get; set; }
        public List<Order> orders { get; set; }

    }
}