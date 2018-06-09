using System.Threading.Tasks;
using System.IO;
using GithubCommentBot.Bot;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GithubCommentBot.HookParser;

namespace GithubCommentBot.Controllers
{
    [Route("api/[controller]")]
    public class HookController : Controller
    {
        public HookController(IGithubBot bot, IHookParser parser, ILogger<HookController> logger)
        {
            _bot = bot;
            _parser = parser;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var body = HttpContext.Request.Body;
            using (var reader = new StreamReader(body))
            {
                var json = reader.ReadToEnd();
                _logger.LogInformation($"Catch hook: {json}");
                var prCommentWebHook = _parser.ParsePrCommentWebHook(json);
                if(prCommentWebHook != null)
                {
                    _logger.LogInformation($"Hook is pr comment");
                    if (prCommentWebHook.Action == "created" || prCommentWebHook.Action == "updated")
                    {
                        await _bot.AddCommentHook(prCommentWebHook);
                    }
                }
                else
                {
                    var prWebHook = _parser.ParsePrWebHook(json);
                    if (prWebHook != null)
                    {
                        if (prWebHook.Action == "submitted")
                        {
                            await _bot.AddApproveHook(prWebHook);
                        }
                        else if (prWebHook.Action == "dismissed")
                        {
                            await _bot.AddRejectHook(prWebHook);

                        }
                    }
                    _logger.LogWarning("Unknown webhook");
                }
                return StatusCode(200);
            }
        }


        private readonly IGithubBot _bot;
        private readonly IHookParser _parser;
        private readonly ILogger<HookController> _logger;
    }
}
