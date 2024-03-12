using Crebitos.Domain;

namespace Crebitos.Application;

public interface ITransactionRepository
{
    Balance Save(Transaction transaction);
    List<Transaction> GetLatestByCustomerId(int customerId);
}
