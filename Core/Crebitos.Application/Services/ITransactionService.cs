using Crebitos.Infra;

namespace Crebitos.Application;

public interface ITransactionService
{
    CreateTransactionResultDTO CreateTransactionsByCustomerId(int customerId, CreateTransactionDTO createTransactionDTO);
}
