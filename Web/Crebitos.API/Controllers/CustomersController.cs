using Microsoft.AspNetCore.Mvc;
using Crebitos.Application;
using Crebitos.Infra;

namespace Crebitos.API;

[ApiController]
[Route("/clientes")]
public class CustomersController : ControllerBase
{
    private TransactionService transactionService;

    public CustomersController(TransactionService transactionService)
    {
        this.transactionService = transactionService;
    }

    [HttpGet("{customerId}/extrato")]
    public IActionResult FindStatementByCustomerId([FromRoute] int customerId)
    {
        return null;
    }

    [HttpPost("{customerId}/transacoes")]
    public IActionResult CreateTransactionsByCustomerId([FromRoute] int customerId, [FromBody] CreateTransactionDTO createTransactionDTO)
    {
        try
        {
            var result = transactionService.CreateTransactionsByCustomerId(customerId, createTransactionDTO);
            return Ok(result);
        }
        catch (InvalidDTOException)
        {
            return UnprocessableEntity();
        }
        catch (InsufficientFundsException)
        {
            return UnprocessableEntity();
        }
    }
}

