using GithubCommentBot.Dto;
using GithubCommentBot.Models;
using Newtonsoft.Json;

namespace GithubCommentBot.HookParser
{
    public class HookParserImpl : IHookParser
    {
        public HookParserImpl()
        {
            _settings = new JsonSerializerSettings()
            {
                MissingMemberHandling = MissingMemberHandling.Error
            };
        }

        public PrComentWebHook ParsePrCommentWebHook(string json)
        {
            PrComentWebHook result;
            try
            {
                result = JsonConvert.DeserializeObject<PrComentWebHook>(json, _settings);
            }
            catch (JsonSerializationException ex)
            {
                result = null;
            }
            return result;
        }

        public PrWebHook ParsePrWebHook(string json)
        {
            PrWebHook result;
            try
            {
                result = JsonConvert.DeserializeObject<PrWebHook>(json, _settings);
            }
            catch (JsonSerializationException ex)
            {
                result = null;
            }
            return result;
        }

        private readonly JsonSerializerSettings _settings;
    }
}
