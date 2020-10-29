using System.Collections.Generic;
using System.Linq;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class AddressRepository : IAddressRepository
    {
        private readonly CryptocopDbContext _dbContext;

        public AddressRepository(CryptocopDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        private User GetUser(string email)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) { throw new System.Exception("User not found"); }
            else { return user; }
        }

        public void AddAddress(string email, AddressInputModel address)
        {
            // Get User
            var user = GetUser(email);

            // Create new address
            var entity = new Address
            {
                UserId = user.Id,
                StreetName = address.StreetName,
                HouseNumber = address.HouseNumber,
                ZipCode = address.ZipCode,
                Country = address.Country,
                City = address.City
            };

            _dbContext.Addresses.Add(entity);
            _dbContext.SaveChanges(); // Commit to db
        }

        public IEnumerable<AddressDto> GetAllAddresses(string email)
        {
            // Get User 
            var user = GetUser(email);

            // Get Addressses
            var addresses = _dbContext
                                .Addresses
                                .Where(a => a.UserId == user.Id)
                                .Select(a => new AddressDto 
                                {
                                    Id = a.Id,
                                    StreetName = a.StreetName,
                                    HouseNumber = a.HouseNumber,
                                    ZipCode = a.ZipCode,
                                    Country = a.Country,
                                    City = a.City
                                }).ToList();
            return addresses;
        }

        public void DeleteAddress(string email, int addressId)
        {
            throw new System.NotImplementedException();
        }
    }
}