using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GithubCommentBot.Bot;
using GithubCommentBot.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GithubCommentBot.Controllers
{
    [Route("api/[controller]")]
    public class HookController : Controller
    {
        public HookController(IGithubBot bot, ILogger<HookController> logger)
        {
            _bot = bot;
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
                var prWebHook = ParsePrWebHook(json);
                if(prWebHook != null && prWebHook.Comment != null && prWebHook.PullRequest != null)
                {
                    _logger.LogInformation($"Hook is pr comment");
                    if (prWebHook.Action == "created" || prWebHook.Action == "updated")
                    {
                        await _bot.AddHook(prWebHook);
                    }
                }
                else
                {
                    _logger.LogWarning("Unknown webhook");
                }
                return StatusCode(200);
            }
        }

        public static PrWebHook ParsePrWebHook(string json)
        {
            PrWebHook result;
            try
            {
                result = JsonConvert.DeserializeObject<PrWebHook>(json);
            }
            catch (JsonSerializationException ex)
            {
                result = null;
            }
            return result;
        }

        private readonly IGithubBot _bot;
        private readonly ILogger<HookController> _logger;
    }
}
