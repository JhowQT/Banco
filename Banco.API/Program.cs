using System.Text.Json.Serialization;
using Banco.API.Application.Services;
using Banco.API.BackgroundServices;
using Banco.API.Configurations;
using Banco.API.Infrastructure.Data;
using Banco.API.Infrastructure.Messaging;
using Banco.API.Infrastructure.Repositories;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// SERILOG
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

// OPENTELEMETRY
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService("Banco.API"))

            .AddAspNetCoreInstrumentation()

            .AddConsoleExporter();
    });

// DATABASE ORACLE
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(
        builder.Configuration.GetConnectionString("Oracle")));

// RABBITMQ CONFIG
builder.Services.Configure<RabbitMQConfig>(
    builder.Configuration.GetSection("RabbitMQ"));

// REPOSITORIES
builder.Services.AddScoped<ClienteRepository>();
builder.Services.AddScoped<AgenciaRepository>();
builder.Services.AddScoped<ContratacaoRepository>();

// SERVICES
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<AgenciaService>();
builder.Services.AddScoped<ContratacaoService>();

// RABBITMQ
builder.Services.AddSingleton<RabbitMQPublisher>();
builder.Services.AddSingleton<RabbitMQConsumer>();

// BACKGROUND SERVICE
builder.Services.AddHostedService<ContratacaoConsumerService>();

// CONTROLLERS
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles;
    });

// SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// HEALTH CHECKS
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>();

var app = builder.Build();

// SWAGGER
app.UseSwagger();

app.UseSwaggerUI();

// HTTPS
app.UseHttpsRedirection();

app.UseAuthorization();

// CONTROLLERS
app.MapControllers();

// HEALTH
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();

public partial class Program
{
}