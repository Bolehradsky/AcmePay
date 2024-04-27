using _Common.Domain;
using _Common.Exceptions;
using _Common.Validation;
using AcmePay.Domain.Enums;
using AcmePay.Domain.Validation;
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
        this.Status = eTransactionStatus.ToString();
        this.CreatedAt = DateTime.Now;
        this.UpdatedAt = DateTime.Now;
    }

    public Transaction()
    {
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

        CreditCardValidation.Validate(cardHolderNumber, cardHolderName, expirationMonth, expirationYear, cVV);
        MoneyValidation.Validate(amount, currency);
        OrderReferenceValidation(orderReference);
        TransactionStatusValidation(eTransactionStatus.ToString());

        return new Transaction(amount,
                               currency,
                               cardHolderNumber,
                               cardHolderName,
                               expirationMonth,
                               expirationYear,
                               cVV,
                               orderReference,
                               eTransactionStatus
                               );

    }


    public static void UpdateStatus(Transaction transaction, ETransactionStatus newStatus)
    {
        TransactionStatusValidation(newStatus.ToString());
        if (transaction.Status != ETransactionStatus.Authorized.ToString())
        {
            throw new BusinessRuleValidationException($"Transaction is already {transaction.Status.ToString()}");
        }
        transaction.Status = newStatus.ToString();
        transaction.UpdatedAt = DateTime.Now;
    }


    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string CardHolderNumber { get; set; }
    public string CardHolderName { get; set; }
    public int ExpirationMonth { get; set; }
    public int ExpirationYear { get; set; }
    public int CVV { get; set; }
    public string OrderReference { get; set; } = string.Empty;
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }



    private static void OrderReferenceValidation(string orderReference)
    {
        if (string.IsNullOrEmpty(orderReference))
        {
            throw new BusinessRuleValidationException("Card holder can not be Empty");
        }
    }


    private static void TransactionStatusValidation(string transactionStatus)
    {

        if (EnumValidator.IsStringInEnum<ETransactionStatus>(transactionStatus) == false)
        {
            throw new BusinessRuleValidationException($"Transaction status '{transactionStatus}' does not exist");
        }
    }
}
