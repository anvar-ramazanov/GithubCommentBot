using GithubCommentBot.Models;
using System;
using System.Threading.Tasks;

namespace GithubCommentBot.Store
{
    public interface IStore
    {
        Task AddUser(BotUser botUser);
        Boolean HaveUser(string githubName);
        Boolean IsRegistered(long chatId);
        BotUser GetUser(string githubName);
    }
}
