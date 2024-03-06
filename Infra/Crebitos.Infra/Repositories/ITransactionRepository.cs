using Crebitos.Domain;

namespace Crebitos.Infra;

public interface ITransactionRepository
{
    Balance Save(Transaction transaction);
    Transaction[] GetLatestByCustomerId(int customerId);
}
