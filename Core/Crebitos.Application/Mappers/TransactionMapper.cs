using AutoMapper;
using Crebitos.Domain;

namespace Crebitos.Application;

public class TransactionMapper : Profile
{
    public TransactionMapper()
    {
        CreateMap<CreateTransactionDTO, Transaction>();
        CreateMap<Transaction, TransactionDTO>();
    }
}
