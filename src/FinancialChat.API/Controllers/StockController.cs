using FinancialChat.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialChat.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StockController : ControllerBase
    {
        private readonly ILogger<StockController> _logger;
        private readonly IStockService _service;

        public StockController(ILogger<StockController> logger, IStockService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetStockPrice([FromQuery] string stockTicker)
        {
            try
            {
                _logger.LogInformation($"Request to stock price from {stockTicker}");

                var result = _service.GetStockPrice(stockTicker);

                if (!result)
                {
                    return BadRequest("Error to publish the stock price request message");
                }

                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error to get stock price");
                return BadRequest();
            }
        }
    }
}