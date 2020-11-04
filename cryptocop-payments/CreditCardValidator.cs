using System;
using CreditCardValidator;

namespace cryptocop_payments
{
    public class CreditCardValidator
    {
        public static void ValidateCard(string card)
        {
            var detector = new CreditCardDetector(card);
            Console.WriteLine(detector.IsValid() ? "Valid credit card" : "Invalid credit card");
        }
    }
}