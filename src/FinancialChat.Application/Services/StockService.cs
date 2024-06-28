using FinancialChat.Application.Entities.MessageModels;
using FinancialChat.Application.Entities.StockData;
using FinancialChat.Application.Gateways;
using FinancialChat.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace FinancialChat.Application.Services
{
    public class StockService : IStockService
    {
        private readonly ILogger<StockService> _logger;
        private readonly IStockPriceGateway _priceGateway;
        private readonly ICSVParseService _csvParser;

        public StockService(ILogger<StockService> logger, IStockPriceGateway priceGateway, ICSVParseService csvParser) {
            _logger = logger;
            _priceGateway = priceGateway;
            _csvParser = csvParser;
        }

        public async Task<StockData?> GetStockPriceAsync(StockMessageModel stockMessage)
        {
            _logger.LogInformation($"Retriving Stock price of {stockMessage.StockTicker} from API");
            var cancelationToken = new CancellationToken();
            var response = await _priceGateway.GetStockPriceAsync(stockMessage.StockTicker, cancelationToken);

            if (response == null)
            {
                _logger.LogWarning($"API response is null");
                return null;
            }

            _logger.LogDebug("Call csv parser");
            var stocksData = _csvParser.ParseCsv(response).ToList();

            return stocksData.FirstOrDefault();
        }
    }
}
