using System.ComponentModel.DataAnnotations;

namespace AcmePay.Api.Controllers.Transactions.Requests;

public record TransactionChangeStatusRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
    public string OrderReference { get; init; } = string.Empty;
}

