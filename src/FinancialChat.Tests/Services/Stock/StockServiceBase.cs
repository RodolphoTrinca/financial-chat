using FinancialChat.Application.Interfaces.Gateway;
using FinancialChat.Application.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FinancialChat.Tests.Services.Stock
{
    public class StockServiceBase
    {
        protected ILogger<ChatCommandService> _logger;
        protected IStockRequestProducer _stockRequest;
        protected ChatCommandService _service;

        public StockServiceBase()
        {
            _logger = Substitute.For<ILogger<ChatCommandService>>();
            _stockRequest = Substitute.For<IStockRequestProducer>();

            _service = new StockService(_logger, _stockRequest);
        }
    }
}
