namespace Cryptocop.Software.API.Models.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string StreetNumber { get; set; }
        public string HouseNumber { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}