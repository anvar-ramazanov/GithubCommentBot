using System;

namespace GithubCommentBot.Models
{
    public class BotUser
    {
        public Int64 ChatId { get; set; }
        public String TelegramName { get; set; }
        public String GithubName { get; set; }
    }
}
