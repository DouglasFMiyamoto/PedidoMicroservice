using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using PedidoMicroservice.Adapters.Database.Extensions;
using PedidoMicroservice.Adapters.Database.PostgreSQL;
using PedidoMicroservice.Adapters.Messaging.Sqs;
using PedidoMicroservice.Adapters.Persistence;
using PedidoMicroservice.Core.Ports;
using PedidoMicroservice.Core.UseCases;

var builder = WebApplication.CreateBuilder(args);

// Configuração do PostgreSQL
builder.Services.AddDbContext<PedidoContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

// Repositórios
builder.Services.AddScoped<IPedidoRepository, PedidoSqlRepository>();

// Casos de uso 
builder.Services.AddScoped<PedidoUseCase>();

builder.Services.AddScoped<ISqsService, SqsService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do FastEndpoints
builder.Services.AddFastEndpoints();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

// Configuração de endpoints (Roteamento)
app.UseFastEndpoints();

app.Run();
