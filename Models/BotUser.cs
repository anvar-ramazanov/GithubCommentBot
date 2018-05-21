using System;
using System.ComponentModel.DataAnnotations;

namespace GithubCommentBot.Models
{
    public class BotUser
    {
        [Key]
        public Int64 ChatId { get; set; }
        public String TelegramName { get; set; }
        public String GithubName { get; set; }
    }
}
