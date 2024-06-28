using FinancialChat.Application.Entities.Configuration.Gateways;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;

namespace FinancialChat.Application.Gateways
{
    public class StockPriceGateway : IStockPriceGateway
    {
        private readonly StockPriceConfig _config;
        private readonly RestClient _client;
        private readonly ILogger<StockPriceGateway> _logger;

        public StockPriceGateway(ILogger<StockPriceGateway> logger, IOptions<StockPriceConfig> config)
        {
            _logger = logger;
            _config = config.Value;
            var url = _config.Url ?? string.Empty;

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("The fetch url is null or empty");
            }

            _logger.LogDebug($"url to fetch: {url}");
            _client = new RestClient(new RestClientOptions(url) { Timeout = TimeSpan.FromSeconds(3)});
        }

        public async Task<byte[]?> GetStockPriceAsync(string stockTicker, CancellationToken cancelationToken)
        {
            var request = new RestRequest(_config.Path, Method.Get);
            request.AddQueryParameter("s", $"{stockTicker}.us");
            request.AddQueryParameter("f", _config.F);
            request.AddQueryParameter("e", _config.Format);

            _logger.LogDebug($"Requesting data from Stooq with this URL: {_client.BuildUri(request)}");
            return await _client.DownloadDataAsync(request, cancelationToken);
        }
    }
}
