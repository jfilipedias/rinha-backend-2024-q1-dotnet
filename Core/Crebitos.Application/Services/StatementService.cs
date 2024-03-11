using AutoMapper;
using Crebitos.Domain;

namespace Crebitos.Application;

public class StatementService
{
    private IBalanceRepository balanceRepository;
    private ITransactionRepository transactionRepository;
    private IMapper mapper;

    public StatementService(IBalanceRepository balanceRepository, ITransactionRepository transactionRepository, IMapper mapper)
    {
        this.balanceRepository = balanceRepository;
        this.transactionRepository = transactionRepository;
        this.mapper = mapper;
    }

    public GetStatementDTO GetStatementByCustomerId(int customerId)
    {
        var balance = balanceRepository.GetByCustomerId(customerId);
        var latestsTransactions = transactionRepository.GetLatestByCustomerId(customerId);

        var balanceDTO = mapper.Map<Balance, BalanceDTO>(balance);
        var latestsTransactionsDTO = mapper.Map<List<Transaction>, List<TransactionDTO>>(latestsTransactions);

        var getStatementDTO = new GetStatementDTO()
        {
            Balance = balanceDTO,
            Transactions = latestsTransactionsDTO,
        };

        return getStatementDTO;
    }
}
