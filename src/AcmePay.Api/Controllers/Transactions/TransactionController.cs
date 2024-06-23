using AcmePay.Api.Controllers.Transactions.Requests;
using AcmePay.Api.Controllers.Transactions.Responses;
using AcmePay.Application.UseCases.Commands;
using AcmePay.Application.UseCases.Queries;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace AcmePay.Api.Controllers.Transactions;

[ApiController]
[Route("api/authorize")]
public class TransactionController : ControllerBase
{
    private readonly ILogger<TransactionController> _logger;
    private readonly ISender _sender;

    public TransactionController(ILogger<TransactionController> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(string))]
    public async Task<IActionResult> AuthorizeTransaction(
    [FromBody] AuthorizeRequest request)
    {
        var useCaseContract = request.Adapt<AuthorizeTransaction.Contract>();
        var useCaseResponse = await _sender.Send(useCaseContract);
        return this.Ok(useCaseResponse.Adapt<AuthorizeResponse>());
    }


    [HttpPost]
    [Route($"{{id}}/voided")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(string))]
    public async Task<IActionResult> TransactionVoided(
        [FromBody] TransactionChangeStatusRequest request,
        [FromRoute] string id)
    {
        var useCaseContract = new VoidTransaction.Contract
        {
            Id = id,
            OrderReference = request.OrderReference,
        };
        var useCaseResponse = await _sender.Send(useCaseContract);
       
        return this.Ok(useCaseResponse.Adapt<TransactionChangeStatusResponse>());
    }



    [HttpPost]
    [Route($"{{id}}/captured")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(string))]
    public async Task<IActionResult> TransactionCaptured(
       [FromBody] TransactionChangeStatusRequest request,
       [FromRoute] string id)
    {
        var useCaseContract = new CaptureTransaction.Contract
        {
            Id = id,
            OrderReference = request.OrderReference,
        };
        var useCaseResponse = await _sender.Send(useCaseContract);
     
        return this.Ok(useCaseResponse.Adapt<TransactionChangeStatusResponse>());
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReadTransactionsPaginateResponse))]
    public async Task<IActionResult> ReadTransactions(
                                     [FromQuery] ReadTransactionsRequest request)
    {
        var useCaseContract = new ReadTransactions.Contract
        {
            PaginationRequest = new _Common.Fetch.Pagination.PaginationRequest(request.CurrentPage, request.PageSize)
        };
        var useCaseResponse = await _sender.Send(useCaseContract);
        var response = useCaseResponse.Adapt<ReadTransactionsPaginateResponse>();
        _logger.Log(LogLevel.Information, "hello from controller");
        return this.Ok(response);
    }
}
