using Crebitos.Domain;

namespace Crebitos.Application;

public interface IBalanceRepository
{
    Balance GetByCustomerId(int customerId);
}
