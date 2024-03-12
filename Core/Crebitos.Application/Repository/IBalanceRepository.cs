using Crebitos.Domain;

namespace Crebitos.Application;

public interface IBalanceRepository
{
    Task<Balance> GetByCustomerId(int customerId);
}
