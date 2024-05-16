using _Common.Domain;
using _Common.Exceptions;
using _Common.Validation;
using AcmePay.Domain.Enums;
using AcmePay.Domain.Validation;
using System.Runtime.CompilerServices;


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
        TransactionValidation.OrderReferenceValidation(orderReference);
        TransactionValidation.TransactionStatusValidation(eTransactionStatus.ToString());

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
        TransactionValidation.TransactionStatusValidation(newStatus.ToString());
        if (transaction.Status != ETransactionStatus.Authorized.ToString())
            {
            throw new BusinessRuleValidationException($"Transaction is already {transaction.Status}");
            }
        transaction.Status = newStatus.ToString();
        transaction.UpdatedAt = DateTime.Now;
        }


    public decimal Amount { get; init; }
    public string Currency { get; init; } = string.Empty;
    public string CardHolderNumber { get; init; } = string.Empty;
    public string CardHolderName { get; init; } = string.Empty;
    public int ExpirationMonth { get; init; }
    public int ExpirationYear { get; init; }
    public int CVV { get; init; }
    public string OrderReference { get; init; } = string.Empty;
    public string Status { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    }
