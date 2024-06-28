using FinancialChat.Application.Entities.Configuration.RabbitMQ;
using FinancialChat.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using FinancialChat.Application.Entities.MessageModels;
using FinancialChat.Application.Entities.Chat;
using FinancialChat.Application.Interfaces.Gateways;
using Microsoft.AspNetCore.SignalR;
using FinancialChat.API.Controllers;
using FinancialChat.Application.SignalR;

namespace FinancialChat.Infra.RabbitMQ.Consumers
{
    public class HubConnectionMessageConsumer : IRabbitMQConsumer
    {
        private readonly ILogger<HubConnectionMessageConsumer> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConnection _connection;
        private readonly IModel _model;
        private readonly RabbitMQQueueNames _queueNames;

        public HubConnectionMessageConsumer(
            ILogger<HubConnectionMessageConsumer> logger,
            IRabbitMQConnectionFactory connectionFactory,
            IOptions<RabbitMQQueueNames> optionsQueueNames,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _scopeFactory = serviceScopeFactory;
            _connection = connectionFactory.CreateChannel();
            _model = _connection.CreateModel();
            _queueNames = optionsQueueNames.Value;
            _model.QueueDeclare(
                _queueNames.HubConnectionMessages,
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
                try
                {
                    _logger.LogInformation("Consuming message...");
                    var body = ea.Body.ToArray();
                    var text = Encoding.UTF8.GetString(body);

                    _logger.LogDebug($"Message received: {text}");
                    //Parse message and get the content
                    var messageModel = JsonConvert.DeserializeObject<MessageInputModel>(text);

                    if (messageModel is null)
                    {
                        throw new NullReferenceException("Error to parse message model");
                    }

                    var content = JsonConvert.DeserializeObject<MessagesData>(messageModel.Content);

                    if (content is null)
                    {
                        throw new NullReferenceException("Error to parse message content");
                    }

                    //Send message to client
                    using (IServiceScope scope = _scopeFactory.CreateScope())
                    {
                        var hub = scope.ServiceProvider.GetRequiredService<IHubContext<ChatHub>>();
                        hub.Clients.User(content.To).SendAsync("StockBotMessage", content.From, content.Message);
                    }

                    await Task.CompletedTask;
                    _logger.LogDebug("Message consumed");
                    _model.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error to consume message");
                }
            };

            _logger.LogDebug($"Adding Consumer to queue: {_queueNames.HubConnectionMessages}");
            _model.BasicConsume(_queueNames.HubConnectionMessages, false, consumer);
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
