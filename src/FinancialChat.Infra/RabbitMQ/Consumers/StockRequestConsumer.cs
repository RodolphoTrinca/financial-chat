using FinancialChat.Application.Interfaces.Gateways;
using FinancialChat.Application.Interfaces.Services;
using FinancialChat.Infra.RabbitMQ.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace FinancialChat.Infra.RabbitMQ.Consumers
{
    public class StockRequestConsumer : IStockRequestConsumer, IDisposable
    {
        private readonly ILogger<StockRequestConsumer> _logger;
        private readonly IConnection _connection;
        private readonly IModel _model;
        private readonly RabbitMQQueueNames _queueNames;

        public StockRequestConsumer(
            ILogger<StockRequestConsumer> logger, 
            IRabbitMQConnectionFactory connectionFactory, 
            IOptions<RabbitMQQueueNames> optionsQueueNames)
        {
            _logger = logger;
            _connection = connectionFactory.CreateChannel();
            _model = _connection.CreateModel();
            _queueNames = optionsQueueNames.Value;
            _model.QueueDeclare(
                _queueNames.StockPriceRequest, 
                durable: false, 
                exclusive: false, 
                autoDelete: false);
        }

        public async Task ReadMessgaes()
        {
            _logger.LogInformation("Starting Async Eventing Basic Consumer");
            var consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += async (ch, ea) =>
            {
                _logger.LogInformation("Consuming message...");
                var body = ea.Body.ToArray();
                var text = Encoding.UTF8.GetString(body);

                _logger.LogDebug($"Message received: {text}");
                //Add the consume process here
                await Task.CompletedTask;
                _model.BasicAck(ea.DeliveryTag, false);
            };

            _logger.LogDebug($"Adding Consumer to queue: {_queueNames.StockPriceRequest}");
            _model.BasicConsume(_queueNames.StockPriceRequest, false, consumer);
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            if (_model.IsOpen)
                _model.Close();

            if (_connection.IsOpen)
                _connection.Close();
        }
    }
}
