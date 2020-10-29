namespace Cryptocop.Software.API.Models.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        // Navigation Properties
        public User user { get; set; }
    }
}