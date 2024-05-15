using _Common.Exceptions;
using _Common.Utils;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("AcmePay.Test")]
namespace AcmePay.Domain.Validation
    {
    
    internal static class CreditCardValidation
    {
        internal static void Validate(string cardHolderNumber, string cardHolderName, int expirationMonth, int expirationYear, int cVV)
        {
            CreditCardNumberValidation(cardHolderNumber);

            if (string.IsNullOrEmpty(cardHolderName))
            {
                throw new BusinessRuleValidationException("Card holder can not be Empty");
            }

            ExpirationDateValidation(expirationMonth, expirationYear);
            CreditCardCvvValidation(cVV);
        }

        private static void CreditCardNumberValidation(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber))
            {
                throw new BusinessRuleValidationException("Card number can not be Empty");
            }
            cardNumber = cardNumber.Replace(" ", "").Replace("-", "");

            if (!NumberUtils.IsNumeric(cardNumber) || cardNumber.Length < 13 || cardNumber.Length > 19)
            {
                throw new BusinessRuleValidationException("Credit CardNumber is not valid");
            }

            // Apply Luhn algorithm
            int sum = 0;
            bool alternate = false;
            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                int digit = int.Parse(cardNumber[i].ToString());

                if (alternate)
                {
                    digit *= 2;
                    if (digit > 9)
                    {
                        digit -= 9;
                    }
                }

                sum += digit;
                alternate = !alternate;
            }

            if (sum % 10 != 0)
            {
                throw new BusinessRuleValidationException("Credit CardNumber is not valid");
            }
        }



        private static void ExpirationDateValidation(int expirationMonth, int expirationYear)
        {
            var expirationDate = new DateOnly(expirationYear, expirationMonth, 1).AddMonths(1).AddDays(-1);

            if (expirationDate < DateOnly.FromDateTime(DateTime.Now))
            {
                throw new BusinessRuleValidationException("Credit card date has expired");
            }
        }

        private static void CreditCardCvvValidation(int cvv)
        {
            if (cvv.ToString().Length != 3)
            {
                throw new BusinessRuleValidationException("Credit card CVV not invalid");
            }
        }
    }
}
