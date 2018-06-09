using GithubCommentBot.Dto;
using GithubCommentBot.Models;

namespace GithubCommentBot.HookParser
{
    public interface IHookParser
    {
        PrComentWebHook ParsePrCommentWebHook(string json);
        PrWebHook ParsePrWebHook(string json);
    }
}
