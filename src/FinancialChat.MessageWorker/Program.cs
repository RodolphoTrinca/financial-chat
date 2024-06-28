using FinancialChat.Application.Entities.Configuration.RabbitMQ;
using FinancialChat.Application.Interfaces.Gateways;
using FinancialChat.Application.Interfaces.Services;
using FinancialChat.Infra.RabbitMQ.Configuration;
using FinancialChat.Infra.RabbitMQ.Consumers;
using FinancialChat.MessageWorker;
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

    //Configure Rabbit MQ
    builder.Services.Configure<RabbitMQConfiguration>(builder.Configuration.GetSection("RabbitMQConnectionSettings"));
    builder.Services.Configure<RabbitMQQueueNames>(builder.Configuration.GetSection("RabbitMQQueueNames"));
    builder.Services.AddSingleton<IRabbitMQConnectionFactory, RabbitMQConnectionFactory>();
    builder.Services.AddSingleton<IRabbitMQConsumer, ChatMessageConsumer>();
    builder.Services.AddHostedService<ConsumerWorker>();

    var host = builder.Build();
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
