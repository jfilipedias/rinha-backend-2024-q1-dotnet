using Microsoft.AspNetCore.Mvc;
using Crebitos.Application;

namespace Crebitos.API;

[ApiController]
[Route("/clientes")]
public class CustomersController : ControllerBase
{
    private TransactionService transactionService;
    private StatementService statementService;

    public CustomersController(TransactionService transactionService, StatementService statementService)
    {
        this.transactionService = transactionService;
        this.statementService = statementService;
    }

    [HttpGet("{customerId}/extrato")]
    public IActionResult FindStatementByCustomerId([FromRoute] int customerId)
    {
        try
        {
            var result = statementService.GetStatementByCustomerId(customerId);
            return Ok(result);
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
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
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }
}

