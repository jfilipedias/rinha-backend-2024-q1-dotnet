using Crebitos.Domain;

namespace Crebitos.Infra;

public interface IBalanceRepository
{
    public Balance GetByCustomerId(int customerId)
}
