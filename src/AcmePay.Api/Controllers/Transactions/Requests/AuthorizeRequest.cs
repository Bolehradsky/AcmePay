using _Common.Api.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace AcmePay.Api.Controllers.Transactions.Requests;

public record AuthorizeRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
    public decimal Amount { get; init; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
    [CurrencyCode(ErrorMessage = "{0} is not a valid currency code.")]
    public string Currency { get; init; } = string.Empty;
    [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
    public string CardHolderNumber { get; init; } = string.Empty;
    [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
    public string CardHolderName { get; init; } = string.Empty;
    [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
    public int ExpirationMonth { get; init; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
    public int ExpirationYear { get; init; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
    public int CVV { get; init; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
    public string OrderReference { get; init; } = string.Empty;
}
