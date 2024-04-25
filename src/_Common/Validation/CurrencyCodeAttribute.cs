using System.ComponentModel.DataAnnotations;

namespace _Common.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CurrencyCodeAttribute : ValidationAttribute
    {


        private static readonly string[] ValidCurrencyCodes =
        {
        "USD",
        "AUD",
        "CAD",
        "CZK",
        "EUR",
        "GBP",
        "JPY",
        "RSD"
        };

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var stateCode = value?.ToString()?.ToUpperInvariant();
            if (string.IsNullOrWhiteSpace(stateCode))
            {
                return ValidationResult.Success;
            }

            if (ValidCurrencyCodes.Contains(stateCode))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(this.FormatErrorMessage(stateCode));
        }
    }

}

