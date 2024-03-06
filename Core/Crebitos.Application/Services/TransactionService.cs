using Crebitos.Domain;
using Crebitos.Infra;

namespace Crebitos.Application;

public class TransactionService : ITransactionService
{
    private ITransactionRepository transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository)
    {
        this.transactionRepository = transactionRepository;
    }

    public CreateTransactionResultDTO CreateTransactionsByCustomerId(int customerId, CreateTransactionDTO createTransactionDTO)
    {
        var validator = new CreateTransactionDTOValidator();
        var result = validator.Validate(createTransactionDTO);

        if (!result.IsValid)
        {
            throw new InvalidDTOException();
        }

        var transaction = new Transaction()
        {
            CustomerId = customerId,
            Description = createTransactionDTO.Description,
            Type = createTransactionDTO.Type,
            Value = createTransactionDTO.Value
        };

        var balance = transactionRepository.Save(transaction);
        var createTransactionResultDTO = new CreateTransactionResultDTO()
        {
            Balance = balance.Total,
            Limit = balance.Limit
        };

        return createTransactionResultDTO;
    }
}
