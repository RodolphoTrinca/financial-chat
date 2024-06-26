using FinancialChat.Application.Interfaces.Gateways;

namespace FinancialChat.Worker
{
    public class ConsumerWorker : BackgroundService
    {
        private readonly ILogger<ConsumerWorker> _logger;
        private readonly IStockRequestConsumer _consumer;

        public ConsumerWorker(ILogger<ConsumerWorker> logger, IStockRequestConsumer stockRequestConsumer)
        {
            _logger = logger;
            _consumer = stockRequestConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Running Consumer");
            stoppingToken.ThrowIfCancellationRequested();

            await _consumer.ReadMessgaes();
        }
    }
}
