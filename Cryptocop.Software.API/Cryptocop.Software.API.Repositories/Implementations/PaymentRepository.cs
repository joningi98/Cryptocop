﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Helpers;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly CryptocopDbContext _dbContext;

        public PaymentRepository(CryptocopDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private User GetUser(string email)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) { throw new ResourceNotFoundException("User not found"); }
            return user;
        }
        public void AddPaymentCard(string email, PaymentCardInputModel paymentCard)
        {
            // Get user
            var user = GetUser(email);

            // Remove all white space
            var formattedCard = paymentCard.CardNumber = Regex.Replace(paymentCard.CardNumber, @"\s+", "");

            // Create paymentCard
            var entity = new PaymentCard
            {
                UserId = user.Id,
                CardholderName = paymentCard.CardholderName,
                CardNumber = PaymentCardHelper.MaskPaymentCard(formattedCard),
                Month = paymentCard.Month,
                Year = paymentCard.Year
            };

            _dbContext.PaymentCards.Add(entity);
            _dbContext.SaveChanges();

        }

        public IEnumerable<PaymentCardDto> GetStoredPaymentCards(string email)
        {
            // Get user
            var user = GetUser(email);

            var cards = _dbContext
                            .PaymentCards
                            .Where(c => c.UserId == user.Id)
                            .Select(c => new PaymentCardDto
                            {
                                Id = c.Id,
                                CardholderName = c.CardholderName,
                                CardNumber = c.CardNumber,
                                Month = c.Month,
                                Year = c.Year
                            });

            return cards;
        }
    }
}