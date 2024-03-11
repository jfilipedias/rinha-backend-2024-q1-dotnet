using AutoMapper;
using Crebitos.Domain;

namespace Crebitos.Application;

public class TransactionService
{
    private ITransactionRepository transactionRepository;
    private IMapper mapper;

    public TransactionService(ITransactionRepository transactionRepository, IMapper mapper)
    {
        this.transactionRepository = transactionRepository;
        this.mapper = mapper;
    }

    public CreateTransactionResultDTO CreateTransactionsByCustomerId(int customerId, CreateTransactionDTO createTransactionDTO)
    {
        var validator = new CreateTransactionDTOValidator();
        var result = validator.Validate(createTransactionDTO);

        if (!result.IsValid)
        {
            throw new InvalidDTOException();
        }

        var transaction = mapper.Map<CreateTransactionDTO, Transaction>(createTransactionDTO);
        transaction.CustomerId = customerId;

        var balance = transactionRepository.Save(transaction);
        var createTransactionResultDTO = mapper.Map<Balance, CreateTransactionResultDTO>(balance);

        return createTransactionResultDTO;
    }
}
