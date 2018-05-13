using System.Threading.Tasks;
using GithubCommentBot.Models;

namespace GithubCommentBot.Bot
{
    public interface IGithubBot
    {
        void Start();
        void Stop();
        Task AddHook(PrWebHook comment);
    }
}