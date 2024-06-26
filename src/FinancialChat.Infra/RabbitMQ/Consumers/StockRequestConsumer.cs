﻿using FinancialChat.Application.Entities.Chat;
using FinancialChat.Application.Entities.Configuration.RabbitMQ;
using FinancialChat.Application.Entities.MessageModels;
using FinancialChat.Application.Entities.StockData;
using FinancialChat.Application.Interfaces.Gateways;
using FinancialChat.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace FinancialChat.Infra.RabbitMQ.Consumers
{
    public class StockRequestConsumer : IRabbitMQConsumer, IDisposable
    {
        private readonly ILogger<StockRequestConsumer> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConnection _connection;
        private readonly IModel _model;
        private readonly RabbitMQQueueNames _queueNames;

        public StockRequestConsumer(
            ILogger<StockRequestConsumer> logger, 
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

                    var content = JsonConvert.DeserializeObject<StockMessageModel>(messageModel.Content);

                    if (content is null)
                    {
                        throw new NullReferenceException("Error to parse message content");
                    }

                    StockData stockData;
                    //Get Stock price
                    using (IServiceScope scope = _scopeFactory.CreateScope())
                    {
                        var stockService = scope.ServiceProvider.GetRequiredService<IStockService>();
                        stockData = await stockService.GetStockPriceAsync(content);

                        _logger.LogDebug($"Stock data: {stockData}");
                    }

                    //Post a message into chat queue
                    using (IServiceScope scope = _scopeFactory.CreateScope())
                    {
                        var messageData = new MessagesData()
                        {
                            From = "Stock Bot",
                            To = content.Requester
                        };

                        messageData.Message = stockData is null
                            ? $"We couldn't fetch the price of stock {content.StockTicker}, please try again later"
                            : $"{stockData.Symbol} quote is ${stockData.Close} per share";

                        var sendHubMessage = scope.ServiceProvider.GetRequiredService<ISendHubMessageProducer>();
                        var success = sendHubMessage.SendUserMessage(messageData);

                        if (!success)
                        {
                            _logger.LogError("Error to send message to user");
                            await Task.CompletedTask;
                            return;
                        }
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
