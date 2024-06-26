using RabbitMQ.Client;

namespace FinancialChat.Application.Interfaces.Services
{
    public interface IRabbitMQConnectionFactory
    {
        IConnection CreateChannel();
    }
}
