using FinancialChat.Application.Entities.Configuration.Gateways;
using FinancialChat.Application.Entities.Configuration.RabbitMQ;
using FinancialChat.Application.Gateways;
using FinancialChat.Application.Interfaces.Gateways;
using FinancialChat.Application.Interfaces.Repositorys;
using FinancialChat.Application.Interfaces.Services;
using FinancialChat.Application.Services;
using FinancialChat.Infra.Context;
using FinancialChat.Infra.Gateways;
using FinancialChat.Infra.RabbitMQ.Configuration;
using FinancialChat.Infra.RabbitMQ.Consumers;
using FinancialChat.Infra.RabbitMQ.Producers;
using FinancialChat.Infra.Repository;
using FinancialChat.Worker;
using Microsoft.EntityFrameworkCore;
using Serilog;

try
{
    var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

    IConfiguration config = configBuilder.Build();

    var builder = Host.CreateApplicationBuilder(args);
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(config)
        .Enrich.FromLogContext()
        .CreateLogger();

    Log.Information("Starting Worker...");

    builder.Services.AddSerilog();

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    builder.Services.AddDbContext<MessagesDbContext>(options =>
        options.UseNpgsql(connectionString));

    //Configure Rabbit MQ
    builder.Services.Configure<RabbitMQConfiguration>(builder.Configuration.GetSection("RabbitMQConnectionSettings"));
    builder.Services.Configure<RabbitMQQueueNames>(builder.Configuration.GetSection("RabbitMQQueueNames"));
    builder.Services.AddSingleton<IRabbitMQConnectionFactory, RabbitMQConnectionFactory>();
    builder.Services.AddSingleton<IRabbitMQConsumer, StockRequestConsumer>();

    //Configuring services
    builder.Services.Configure<StockPriceConfig>(builder.Configuration.GetSection("StockPriceConfig"));
    builder.Services.AddScoped<IStockPriceGateway, StockPriceGateway>();
    builder.Services.AddScoped<IStockService, StockService>();
    builder.Services.AddScoped<ICSVParseService, CSVParseService>();
    builder.Services.AddScoped<ISendHubMessageProducer, SendHubMessageProducer>();
    builder.Services.AddScoped<IChatService, ChatService>();
    builder.Services.AddScoped<IMessagesRepository, MessagesRepository>();

    builder.Services.AddHostedService<ConsumerWorker>();

    Log.Debug("Building host");
    var host = builder.Build();

    Log.Information("Running worker");
    host.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"FATAL ERROR: {ex}");
    Environment.Exit(1);
}
finally
{
    Log.CloseAndFlush();
}
