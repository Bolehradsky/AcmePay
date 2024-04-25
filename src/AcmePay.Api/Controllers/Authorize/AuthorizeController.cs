using AcmePay.Api.Controllers.Authorize.Requests;
using AcmePay.Api.Controllers.Authorize.Responses;
using AcmePay.Application.UseCases.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace AcmePay.Api.Controllers.Authorize;

[ApiController]
[Route("api/authorize")]
public class AuthorizeController : ControllerBase
{
    private readonly ILogger<AuthorizeController> _logger;
    private readonly ISender _sender;

    public AuthorizeController(ILogger<AuthorizeController> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(string))]
    public async Task<IActionResult> AuthorizeTransaction(

    [FromBody] AuthorizeRequest request)
    {
        var useCaseContract = new CreateTransaction.Contract
        {
            Amount = request.Amount,
            Currency = request.Currency,
            CardHolderName = request.CardHolderName,
            CardHolderNumber = request.CardHolderNumber,
            CVV = request.CVV,
            ExpirationMonth = request.ExpirationMonth,
            ExpirationYear = request.ExpirationYear,
            OrderReference = request.OrderReference
        };
        var useCaseResponse = await _sender.Send(useCaseContract);

        var response = new AuthorizeResponse { Id = useCaseResponse.Id, OrderReference = useCaseResponse.OrderReference };

        return this.Ok(response);
    }

    [HttpPost]
    [Route($"{{id}}/voided")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(string))]
    public async Task<IActionResult> TransactionVoided(
        [FromBody] TransactionChangeStatusRequest request,
        [FromRoute] string id)
    {

        //var useCaseContract = request.Adapt<CreateTransaction.Contract>();
        /// Ovde pozivamo UseCase iz Apllication
        /// 
        var useCaseContract = new SetTransactionStatusVoided.Contract
        {
            Id = id,
            OrderReference = request.OrderReference,

        };
        var useCaseResponse = await _sender.Send(useCaseContract);

        var response = new TransactionChangeStatusResponse { Id = useCaseResponse.Id, TransactionStatus = useCaseResponse.TransactionStatus };

        return this.Ok(response);
    }




    [HttpGet("Health-Test")]
    public string Get()
    {
        return "Hello, I'm alive!";
    }
}
