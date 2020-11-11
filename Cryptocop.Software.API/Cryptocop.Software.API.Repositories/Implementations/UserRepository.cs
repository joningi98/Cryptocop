
using System.Linq;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Helpers;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly CryptocopDbContext _dbContext;

        public UserRepository(CryptocopDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public UserDto CreateUser(RegisterInputModel inputModel)
        {
            // Get new JWT token 
            var token = new JwtToken();
            _dbContext.Add(token);
            _dbContext.SaveChanges();

            // Check if user is in database 
            var email = _dbContext.Users.FirstOrDefault(u => u.Email == inputModel.Email);
            if (email != null) { throw new ConflictException("User already exists");} 

            // Creating new user 
            var user = new User
            {
                Email = inputModel.Email,
                FullName = inputModel.FullName,
                HashedPassword = HashingHelper.HashPassword(inputModel.Password)
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return new UserDto
            {
                Id = user.Id, 
                Email = inputModel.Email, 
                FullName = inputModel.FullName,
                TokenId = token.Id    
            };
        }

        public UserDto AuthenticateUser(LoginInputModel loginInputModel)
        {
            var user = _dbContext.Users.FirstOrDefault(u =>
                u.Email == loginInputModel.Email && 
                u.HashedPassword == HashingHelper.HashPassword(loginInputModel.Password));
            
            // If user is not in the db 
            if (user == null) { return null; }
            
            var token = new JwtToken();
            _dbContext.Add(token);
            _dbContext.SaveChanges();

            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                TokenId = token.Id              
            };
        }
    }
}