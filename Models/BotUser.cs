using LinqToDB.Mapping;
using System;

namespace GithubCommentBot.Models
{
    [Table("Users")]
    public class BotUser
    {
        [PrimaryKey]
        public Int64 ChatId { get; set; }
        [Column]
        public String TelegramName { get; set; }
        [Column]
        public String GithubName { get; set; }
    }
}
