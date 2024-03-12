using AutoMapper;
using Crebitos.Domain;

namespace Crebitos.Application;

public class BalanceMapper : Profile
{
    public BalanceMapper()
    {
        CreateMap<Balance, BalanceDTO>();
        CreateMap<Balance, CreateTransactionResultDTO>();
    }
}
