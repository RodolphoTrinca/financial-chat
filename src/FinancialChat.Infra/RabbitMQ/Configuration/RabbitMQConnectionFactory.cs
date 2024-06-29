using FinancialChat.Application.Entities.Configuration.RabbitMQ;
using FinancialChat.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace FinancialChat.Infra.RabbitMQ.Configuration
{
    public class RabbitMQConnectionFactory : IRabbitMQConnectionFactory
    {
        private readonly ILogger<RabbitMQConnectionFactory> _logger;
        private readonly RabbitMQConfiguration _configuration;

        public RabbitMQConnectionFactory(ILogger<RabbitMQConnectionFactory> logger, IOptions<RabbitMQConfiguration> optionsConfiguration)
        {
            _logger = logger;
            _configuration = optionsConfiguration.Value;
        }

        public IConnection CreateChannel()
        {
            ConnectionFactory connection = new ConnectionFactory()
            {
                Uri = new Uri($"amqp://{_configuration.Username}:{_configuration.Password}@{_configuration.HostName}:{_configuration.Port}"),
            };
            connection.DispatchConsumersAsync = true;
            var channel = connection.CreateConnection();
            return channel;
        }
    }
}
