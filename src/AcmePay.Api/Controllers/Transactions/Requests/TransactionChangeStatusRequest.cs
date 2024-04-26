using System.ComponentModel.DataAnnotations;

namespace AcmePay.Api.Controllers.Transactions.Requests;

public class TransactionChangeStatusRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
    public string OrderReference { get; set; } = string.Empty;
}

