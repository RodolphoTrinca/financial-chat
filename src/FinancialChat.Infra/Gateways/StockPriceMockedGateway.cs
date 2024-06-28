using FinancialChat.Application.Gateways;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.Infra.Gateways
{
    public class StockPriceMockedGateway : IStockPriceGateway
    {
        public async Task<byte[]?> GetStockPriceAsync(string stockTicker, CancellationToken cancelationToken)
        {
            var text = File.ReadAllText("aapl.us (3).csv");

            return await File.ReadAllBytesAsync("aapl.us (3).csv", cancelationToken);
        }
    }
}
