using System.Reflection;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.IoC;
using MicroRabbit.Transfer.Console;
using MicroRabbit.Transfer.Domain.EventHandlers;
using MicroRabbit.Transfer.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Press any key to exit...");

var builder = Host.CreateDefaultBuilder(args)            
            .ConfigureServices((context, services) =>
            {
                services.AddMicroRabbitServices();
                services.AddTransferServices(context.Configuration);
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            });

using var host = builder.Build();

var eventBus = host.Services.GetRequiredService<IEventBus>();
await eventBus.Subscribe<TransferCreatedEvent, TransferEventHandler>();

//await host.RunAsync();