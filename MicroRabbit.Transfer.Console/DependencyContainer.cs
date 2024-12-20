using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Application.Services;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Data.Repository;
using MicroRabbit.Transfer.Domain.EventHandlers;
using MicroRabbit.Transfer.Domain.Events;
using MicroRabbit.Transfer.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroRabbit.Transfer.Console
{
    public static class DependencyContainer
    {
        public static void AddTransferServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Subscriptions
            services.AddTransient<TransferEventHandler>();

            //Domain Events
            services.AddTransient<IEventHandler<TransferCreatedEvent>, TransferEventHandler>();

            //Application Services
            services.AddTransient<ITransferService, TransferService>();

            //Data
            services.AddTransient<ITransferLogRepository, TransferLogRepository>();
            services.AddDbContext<TransferDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("TransferDbConnection"));
            });
        }
    }
}
