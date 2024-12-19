using System.Reflection;
using MicroRabbit.Banking.Api;
using MicroRabbit.Infra.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBankingServices(builder.Configuration);
builder.Services.AddMicroRabbitServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
