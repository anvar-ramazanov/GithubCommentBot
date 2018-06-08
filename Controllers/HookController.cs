using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using GithubCommentBot.Bot;
using GithubCommentBot.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GithubCommentBot.Dto;

namespace GithubCommentBot.Controllers
{
    [Route("api/[controller]")]
    public class HookController : Controller
    {
        public HookController(IGithubBot bot, ILogger<HookController> logger)
        {
            _bot = bot;
            _logger = logger;
            _settings = new JsonSerializerSettings()
            {
                MissingMemberHandling = MissingMemberHandling.Error
            };
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var body = HttpContext.Request.Body;
            using (var reader = new StreamReader(body))
            {
                var json = reader.ReadToEnd();
                _logger.LogInformation($"Catch hook: {json}");
                var prCommentWebHook = ParsePrCommentWebHook(json);
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
                    var prWebHook = ParsePrWebHook(json);
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

        public PrComentWebHook ParsePrCommentWebHook(string json)
        {
            PrComentWebHook result;
            try
            {
                result = JsonConvert.DeserializeObject<PrComentWebHook>(json, _settings);
            }
            catch (JsonSerializationException ex)
            {
                result = null;
            }
            return result;
        }

        public PrWebHook ParsePrWebHook(string json)
        {
            PrWebHook result;
            try
            {
                result = JsonConvert.DeserializeObject<PrWebHook>(json, _settings);
            }
            catch (JsonSerializationException ex)
            {
                result = null;
            }
            return result;
        }

        private readonly IGithubBot _bot;
        private readonly ILogger<HookController> _logger;
        private readonly JsonSerializerSettings _settings;
    }
}
