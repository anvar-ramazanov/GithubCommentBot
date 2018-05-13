using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GithubCommentBot.Bot;
using GithubCommentBot.Models;
using Microsoft.AspNetCore.Mvc;

namespace GithubCommentBot.Controllers
{
    [Route("api/[controller]")]
    public class HookController : Controller
    {
        public HookController(IGithubBot bot)
        {
            _bot = bot;
        }

        [HttpPost]
        public async void Post([FromBody] PrWebHook comment)
        {
            await _bot.AddHook(comment);
        }

        private readonly IGithubBot _bot;
    }
}
