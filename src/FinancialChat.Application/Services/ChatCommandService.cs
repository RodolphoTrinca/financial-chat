using FinancialChat.Application.Entities.MessageModels;
using FinancialChat.Application.Interfaces.Gateways;
using FinancialChat.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace FinancialChat.Application.Services
{
    public class ChatCommandService : IChatCommandService
    {
        private readonly ILogger<ChatCommandService> _logger;
        private readonly IStockRequestProducer _stockRequest;

        public ChatCommandService(ILogger<ChatCommandService> logger, IStockRequestProducer stockRequestProducer)
        {
            _logger = logger;
            _stockRequest = stockRequestProducer;
        }

        public bool SendMessageWithStockPrice(string stockTicker)
        {
            var message = new StockMessageModel()
            {
                StockTicker = stockTicker
            };

            return _stockRequest.GetStockPrice(message);
        }
    }
}
