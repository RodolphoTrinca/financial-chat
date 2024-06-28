using Bogus;
using FinancialChat.Application.Entities.MessageModels;
using FinancialChat.Application.Entities.StockData;
using FluentAssertions;
using NSubstitute;

namespace FinancialChat.Tests.Services.Stock
{
    public class GetStockPrice : StockServiceBase
    {
        [Theory]
        [InlineData("AAPL", "email@email.com")]
        [InlineData("NVDA", "12345@email.com")]
        [InlineData("GOOG", "anotheremail@email.com")]
        public async Task GetPriceWithHappyPathAsync(string stockTicker, string requester)
        {
            var stockMessage = new StockMessageModel()
            {
                StockTicker = stockTicker,
                Requester = requester
            };

            var stockDataList = new Faker<StockData>()
                .RuleFor(sd => sd.Symbol, f => stockTicker)
                .GenerateBetween(1, 1);

            var csvByteArray = new byte[100];

            _gateway
                .GetStockPriceAsync(
                    Arg.Is<string>(st => st.Equals(stockMessage.StockTicker)), 
                    Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(csvByteArray));

            _csvParser.ParseCsv(
                Arg.Is<byte[]>(arg => arg.Equals(csvByteArray)))
                .Returns(stockDataList);

            var result = await _service.GetStockPriceAsync(stockMessage);

            result.Should().Be(stockDataList.First());
        }
    }
}
