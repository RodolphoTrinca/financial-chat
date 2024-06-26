using FinancialChat.Application.Entities.MessageModels;
using FinancialChat.Application.Interfaces.Gateways;
using FinancialChat.Application.Interfaces.Services;
using FinancialChat.Infra.RabbitMQ.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace FinancialChat.Infra.RabbitMQ.Producers
{
    public class StockRequestProducer : IStockRequestProducer, IDisposable
    {
        private readonly RabbitMQConfiguration _configuration;
        private readonly ILogger<StockRequestProducer> _logger;
        private readonly string _queueName = "messages";
        private readonly IConnection _connection;

        public StockRequestProducer(ILogger<StockRequestProducer> logger, IOptions<RabbitMQConfiguration> optionsConfiguration, IOptions<RabbitMQQueueNames> optionsQueueNames, IRabbitMQConnectionFactory connectionFactory)
        {
            _configuration = optionsConfiguration.Value;
            _queueName = optionsQueueNames.Value.StockPriceRequest;
            _logger = logger;
            _connection = connectionFactory.CreateChannel();
        }

        public bool GetStockPrice(StockMessageModel messageModel)
        {
            try
            {
                _logger.LogDebug("Creating model");
                using (var channel = _connection.CreateModel())
                {
                    channel.ConfirmSelect();

                    _logger.LogDebug($"Declaring queue: {_queueName}");
                    channel.QueueDeclare(
                        queue: _queueName,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var stringfiedMessage = JsonConvert.SerializeObject(messageModel);
                    var messageWrapper = new MessageInputModel()
                    {
                        Content = stringfiedMessage
                    };

                    var stringfiedBody = JsonConvert.SerializeObject(messageWrapper);
                    var bytesMessage = Encoding.UTF8.GetBytes(stringfiedBody);

                    _logger.LogInformation($"Publishing message into queue name: {_queueName}");
                    _logger.LogDebug($"Message body: {stringfiedMessage}");
                    channel.BasicPublish(
                        exchange: string.Empty,
                        routingKey: _queueName,
                        basicProperties: null,
                        body: bytesMessage);

                    channel.WaitForConfirmsOrDie(timeout: TimeSpan.FromSeconds(5));

                    _logger.LogInformation("Message published with success!");
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error to get stock price publish message: ");
                throw;
            }
        }

        public void Dispose()
        {
            if (_connection.IsOpen)
                _connection.Close();
        }
    }
}
