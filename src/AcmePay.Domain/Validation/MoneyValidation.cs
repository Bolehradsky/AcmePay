using _Common.Exceptions;
using _Common.Validation;
using AcmePay.Domain.Enums;

namespace AcmePay.Domain.Validation
{
    public static class MoneyValidation
    {
        public static void Validate(decimal ammount, string currency)
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
    }
}
