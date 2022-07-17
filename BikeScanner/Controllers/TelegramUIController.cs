using System.Threading.Tasks;
using BikeScanner.Telegram.Bot;
using BikeScanner.Сonfigs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;

namespace BikeScanner.Controllers
{
    public class TelegramUIController : BaseApiController
    {
        private readonly BikeScannerBot     _bot;
        private readonly string             _apiKey;

        public TelegramUIController(
            BikeScannerBot bot,
            IOptions<TelegramAccessConfig> options
            )
        {
            _bot = bot;
            _apiKey = options.Value.Key;
        }

        [HttpPost("bot/{apiKey}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> HandleUpdate(Update update, string apiKey)
        {
            if (apiKey != _apiKey)
                return Unauthorized();

            await _bot.Handle(update);
            return Ok();
        }
    }
}

