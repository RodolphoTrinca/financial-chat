using CsvHelper;
using FinancialChat.Application.Gateways;
using FinancialChat.Application.Interfaces.Gateways;
using FinancialChat.Application.Interfaces.Services;
using FinancialChat.Application.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FinancialChat.Tests.Services.Stock
{
    public class StockServiceBase
    {
        protected ILogger<StockService> _logger;
        protected IStockPriceGateway _gateway;
        protected ICSVParseService _csvParser;
        protected StockService _service;

        public StockServiceBase()
        {
            _logger = Substitute.For<ILogger<StockService>>();
            _gateway = Substitute.For<IStockPriceGateway>();
            _csvParser = Substitute.For<ICSVParseService>();

            _service = new StockService(_logger, _gateway, _csvParser);
        }
    }
}
