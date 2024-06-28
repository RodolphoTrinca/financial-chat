using FinancialChat.Application.Entities.StockData;
using FinancialChat.Tests.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.Tests.Services.CSVParser
{
    public class ParseCSV : CSVParserBase
    {
        [Fact]
        public async Task HappyPath()
        {
            var expectedResult = new List<StockData>()
            {
                new StockData()
                {
                    Symbol = "AAPL.US",
                    Date = new DateOnly(2024, 06, 26),
                    Time = new TimeOnly(22, 01, 01),
                    Open = 211.5m,
                    High = 214.86m,
                    Low = 210.64m,
                    Close = 213.25m,
                    Volume = 66213186m
                }
            };

            var stockTicker = "AAPL";
            var csvByteArray = await new StockPriceMockedGateway()
                .GetStockPriceAsync(stockTicker, new CancellationToken());

            var result = _service.ParseCsv(csvByteArray);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
