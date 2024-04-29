using _Common.Api.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace AcmePay.Api.Controllers.Transactions.Requests;

public class AuthorizeRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
    public decimal Amount { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
    [CurrencyCode(ErrorMessage = "{0} is not a valid currency code.")]
    public string Currency { get; set; } = string.Empty;
    [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
    public string CardHolderNumber { get; set; } = string.Empty;
    [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
    public string CardHolderName { get; set; } = string.Empty;
    [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
    public int ExpirationMonth { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
    public int ExpirationYear { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
    public int CVV { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
    public string OrderReference { get; set; } = string.Empty;
}
