using FinancialChat.Application.Interfaces.Gateway;
using FinancialChat.Application.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FinancialChat.Tests.Services.Stock
{
    public class StockServiceBase
    {
        protected ILogger<StockService> _logger;
        protected IStockRequestProducer _stockRequest;
        protected StockService _service;

        public StockServiceBase()
        {
            _logger = Substitute.For<ILogger<StockService>>();
            _stockRequest = Substitute.For<IStockRequestProducer>();

            _service = new StockService(_logger, _stockRequest);
        }
    }
}
