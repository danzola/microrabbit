using MediatR;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Data.Repository;
using MicroRabbit.Banking.Domain.CommandHandlers;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Banking.Api
{
    public static class DependencyContainer
    {
        public static void AddBankingServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Domain Commands
            services.AddTransient<IRequestHandler<CreateTransferCommand, bool>, TransferCommandHandler>();
            services.AddTransient<IRequestHandler<CancelAccountCommand, bool>, CancelAccountCommandHandler>();

            //Application Services
            services.AddTransient<IAccountService, AccountService>();            

            //Data
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddDbContext<BankingDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("BankingDbConnection"));
            });
        }
    }
}
