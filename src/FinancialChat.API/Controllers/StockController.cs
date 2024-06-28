using FinancialChat.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinancialChat.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StockController : ControllerBase
    {
        private readonly ILogger<StockController> _logger;
        private readonly IChatCommandService _service;
        private readonly UserManager<IdentityUser> _userManager;

        public StockController(ILogger<StockController> logger, IChatCommandService service, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _service = service;
            _userManager = userManager;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetStockPrice([FromQuery] string stockTicker)
        {
            try
            {
                var user = User.Identity.Name;
                _logger.LogInformation($"Request to stock price from {stockTicker}");

                var result = _service.SendMessageWithStockPrice(stockTicker, user);

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