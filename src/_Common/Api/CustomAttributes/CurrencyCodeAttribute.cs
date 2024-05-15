using System.ComponentModel.DataAnnotations;

namespace _Common.Api.CustomAttributes
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
            var currencyCode = value?.ToString()?.ToUpperInvariant();


            if (ValidCurrencyCodes.Contains(currencyCode))
                {
                return ValidationResult.Success;
                }

            return new ValidationResult(FormatErrorMessage(currencyCode));
            }
        }

}

