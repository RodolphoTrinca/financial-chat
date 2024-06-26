using FinancialChat.Application.Entities.MessageModels;
using FinancialChat.Application.Interfaces.Gateways;
using FinancialChat.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace FinancialChat.Application.Services
{
    public class StockService : IStockService
    {
        private readonly ILogger<StockService> _logger;
        private readonly IStockRequestProducer _stockRequest;

        public StockService(ILogger<StockService> logger, IStockRequestProducer stockRequestProducer)
        {
            _logger = logger;
            _stockRequest = stockRequestProducer;
        }

        public bool GetStockPrice(string stockTicker)
        {
            var message = new StockMessageModel()
            {
                StockTicker = stockTicker
            };

            return _stockRequest.GetStockPrice(message);
        }
    }
}
