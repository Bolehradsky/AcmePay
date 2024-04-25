using _Common.Domain;
using _Common.Exceptions;
using _Common.Utils;
using AcmePay.Domain.Enums;
namespace AcmePay.Domain.Model;

public class Transaction : Entity<Guid>, IAuditable
{
    private Transaction(decimal amount,
                       string currency,
                       string cardHolderNumber,
                       string cardHolderName,
                       int expirationMonth,
                       int expirationYear,
                       int cVV,
                       string orderReference,
                       ETransactionStatus eTransactionStatus
                       )
    {
        this.Id = Guid.NewGuid();
        this.Amount = amount;
        this.Currency = currency;
        this.CardHolderNumber = cardHolderNumber;
        this.CardHolderName = cardHolderName;
        this.ExpirationMonth = expirationMonth;
        this.ExpirationYear = expirationYear;
        this.CVV = cVV;
        this.OrderReference = orderReference;
        this.TransactionStatus = eTransactionStatus.ToString();
        this.CreatedAt = DateTime.Now;
        this.UpdatedAt = DateTime.Now;
    }


    public static Transaction Create(decimal amount,
                                     string currency,
                                     string cardHolderNumber,
                                     string cardHolderName,
                                     int expirationMonth,
                                     int expirationYear,
                                     int cVV,
                                     string orderReference,
                                     ETransactionStatus eTransactionStatus)
    {

        CreditCardValidation(cardHolderNumber, cardHolderName, expirationMonth, expirationYear, cVV);
        MoneyValidation(amount, currency);
        OrderReferenceValidation(orderReference);
        // TransactionStatus(eTransactionStatus);

        return new Transaction(amount, currency, cardHolderNumber, cardHolderName, expirationMonth, expirationYear, cVV, orderReference, eTransactionStatus);

    }

    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string CardHolderNumber { get; set; }
    public string CardHolderName { get; set; }
    public int ExpirationMonth { get; set; }
    public int ExpirationYear { get; set; }
    public int CVV { get; set; }
    public string OrderReference { get; set; } = string.Empty;
    public string TransactionStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }



    private static void OrderReferenceValidation(string orderReference)
    {
        if (string.IsNullOrEmpty(orderReference))
        {
            throw new BusinessRuleValidationException("Card holder can not be Empty");
        }
        // TODO check if orderreference is Unique
        //if (transactionRepository.GetAsync(orderReference))
        //{
        //    throw new BusinessRuleValidationException("Order reference already exist");
        //}
    }


    private static void MoneyValidation(decimal ammount, string currency)
    {
        if (ammount <= 0)
        {
            throw new BusinessRuleValidationException("Ammount must greater then zero");
        }

        if (EnumValidator.IsStringInEnum<ECurrency>(currency) == false)
        {
            throw new BusinessRuleValidationException($"'{currency}' is not valid currency symbol.");
        }
    }

    private static void TransactionStatusValidation(string transactionStatus)
    {

        if (EnumValidator.IsStringInEnum<ETransactionStatus>(transactionStatus) == false)
        {
            throw new BusinessRuleValidationException($"Transaction status '{transactionStatus}' does not exist");
        }
    }

    #region CreditCardvalidation
    private static void CreditCardValidation(string cardHolderNumber, string cardHolderName, int expirationMonth, int expirationYear, int cVV)
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
        var expirationDate = new DateOnly(expirationYear, expirationMonth + 1, 1).AddDays(-1);

        if (expirationDate < DateOnly.FromDateTime(DateTime.Now))
        {
            throw new BusinessRuleValidationException("Credit card date has expired");
        }
    }

    private static void CreditCardCvvValidation(int cvv)
    {
        if (cvv.ToString().Length != 3)
        {
            throw new BusinessRuleValidationException("Credit card date has expired");
        }
    }
    #endregion

}
