using FinancialChat.Application.Entities.MessageModels;
using NSubstitute;

namespace FinancialChat.Tests.Services.Stock
{
    public class GetStockPrice : StockServiceBase
    {
        [Theory]
        [InlineData("AAPL")]
        [InlineData("NVDA")]
        [InlineData("GOOG")]
        public void GetPriceWithHappyPath(string stockTicker)
        {
            _stockRequest
                .GetStockPrice(Arg.Is<StockMessageModel>(st => st.StockTicker.Equals(stockTicker)))
                .Returns(true);

            var result = _service.GetStockPrice(stockTicker);

            Assert.True(result);
        }
    }
}
