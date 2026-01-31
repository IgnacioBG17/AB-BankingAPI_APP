using AutoMapper;
using BankingSolution.Application.Features.Accounts.Commands.CreateAccount;
using BankingSolution.Application.Features.Accounts.Commands.CreateClient;
using BankingSolution.Application.Features.Accounts.Querys.Vms;
using BankingSolution.Domain.Entities;

namespace BankingSolution.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Commands -> Entities
            CreateMap<CreateClientCommand, Client>();

            CreateMap<CreateAccountCommand, BankAccount>()
                .ForMember(d => d.AccountNumber, o => o.Ignore())
                .ForMember(d => d.Balance, o => o.Ignore());

            // Entities -> VMs
            CreateMap<Transaction, TransactionVm>()
                .ForMember(d => d.Type, o => o.MapFrom(s => s.Type.ToString()));
        }
    }
}
