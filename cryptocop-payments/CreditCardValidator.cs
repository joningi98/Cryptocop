using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using CreditCardValidator;

namespace cryptocop_emails
{
    public class CarditCardValidator
    {
        static void ValidateCard(string card)
        {
            CreditCardDetector detector = new CreditCardDetector(card);
            
            if (detector.IsValid())
            {
                Console.WriteLine("Valid credit card");
            }
            else {
                Console.WriteLine("Invalid credit card");
            }
        }
    }
}