using RestSharp;

namespace FinancialChat.Application.Gateways
{
    public interface IStockPriceGateway
    {
        Task<byte[]?> GetStockPriceAsync(string stockTicker, CancellationToken cancelationToken);
    }
}