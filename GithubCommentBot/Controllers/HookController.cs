using System.Threading.Tasks;
using System.IO;
using GithubCommentBot.Bot;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GithubCommentBot.HookParser;
using GithubCommentBot.Models;
using GithubCommentBot.Dto;

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
        [Route("comments")]
        public async Task GetCommentHook(PrCommentWebHook prCommentWebHook)
        {
            _logger.LogInformation($"Got new comment hook");
            if (prCommentWebHook.Action == "created" || prCommentWebHook.Action == "updated")
            {
                _logger.LogInformation($"Send comment to telegram");
                await _bot.AddCommentHook(prCommentWebHook);
            }
        }


        [HttpPost]
        [Route("pr")]
        public async Task GetPrHook(PrWebHook prWebHook)
        {
            _logger.LogInformation($"Got new prWebHook hook");
            if (prWebHook.Action == "submitted")
            {
                await _bot.AddApproveHook(prWebHook);
            }
            else if (prWebHook.Action == "dismissed")
            {
                await _bot.AddRejectHook(prWebHook);

            }
        }


        private readonly IGithubBot _bot;
        private readonly IHookParser _parser;
        private readonly ILogger<HookController> _logger;
    }
}
