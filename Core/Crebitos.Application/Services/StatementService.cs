using Crebitos.Infra;

namespace Crebitos.Application;

public class StatementService
{
    private IBalanceRepository balanceRepository;
    private ITransactionRepository transactionRepository;

    public StatementService(IBalanceRepository balanceRepository, ITransactionRepository transactionRepository)
    {
        this.balanceRepository = balanceRepository;
        this.transactionRepository = transactionRepository;
    }

    public GetStatementDTO GetStatementByCustomerId(int customerId)
    {
        var balance = balanceRepository.GetByCustomerId(customerId);
        var transaction = transactionRepository.GetLatestByCustomerId(customerId);

        throw new NotImplementedException();
    }
}
