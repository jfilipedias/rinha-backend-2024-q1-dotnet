using Crebitos.Domain;

namespace Crebitos.Application;

public interface ITransactionRepository
{
    Task<Balance> Save(Transaction transaction);
    Task<List<Transaction>> GetLatestByCustomerId(int customerId);
}
