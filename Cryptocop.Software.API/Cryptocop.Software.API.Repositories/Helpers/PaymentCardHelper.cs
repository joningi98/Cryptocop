using System.Text.RegularExpressions;

namespace Cryptocop.Software.API.Repositories.Helpers
{
    public class PaymentCardHelper
    {
        public static string MaskPaymentCard(string paymentCardNumber)
        {
            return "************" + paymentCardNumber.Substring(12);
        }
    }
}