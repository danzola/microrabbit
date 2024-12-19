using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Application.Services;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Data.Repository;
using MicroRabbit.Transfer.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Transfer.Api
{
    public static class DependencyContainer
    {
        public static void AddTransferServices(this IServiceCollection services, IConfiguration configuration)
        {
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
