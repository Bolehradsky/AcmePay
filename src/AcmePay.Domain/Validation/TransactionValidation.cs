using _Common.Exceptions;
using _Common.Validation;
using AcmePay.Domain.Enums;
using System.Runtime.CompilerServices;


[assembly: InternalsVisibleTo("AcmePay.Test")]
namespace AcmePay.Domain.Validation;

internal class TransactionValidation
    {

    public static void OrderReferenceValidation(string orderReference)
        {
        if (string.IsNullOrEmpty(orderReference))
            {
            throw new BusinessRuleValidationException("Card holder can not be Empty");
            }
        }


    public static void TransactionStatusValidation(string transactionStatus)
        {
        if (EnumValidator.IsStringInEnum<ETransactionStatus>(transactionStatus) == false)
            {
            throw new BusinessRuleValidationException($"Transaction status '{transactionStatus}' does not exist");
            }
        }
    }

