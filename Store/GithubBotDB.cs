using GithubCommentBot.Models;
using LinqToDB;

namespace GithubCommentBot.Store
{
    public partial class GithubBotDB : LinqToDB.Data.DataConnection
    {
        public ITable<BotUser> BotUsers { get { return this.GetTable<BotUser>(); } }

        public GithubBotDB()
        {
        }

        public GithubBotDB(string configuration) : base(configuration)
        {
        }
    }
}
