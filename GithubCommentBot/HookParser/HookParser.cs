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

        public PrCommentWebHook ParsePrCommentWebHook(string json)
        {
            return ParseSimplePrCommentHook(json) ?? ParsePrOrganizationCommentWebHook(json);
        }

        public PrWebHook ParsePrWebHook(string json)
        {
            return PaseSimplePrWebHook(json) ?? ParsePrOrganizationWebHook(json);
        }

        private PrCommentWebHook ParseSimplePrCommentHook(string json)
        {
            PrCommentWebHook result;
            try
            {
                result = JsonConvert.DeserializeObject<PrCommentWebHook>(json, _settings);
            }
            catch (JsonSerializationException ex)
            {
                result = null;
            }
            return result;
        }

        private PrOrganizationCommentWebHook ParsePrOrganizationCommentWebHook(string json)
        {
            PrOrganizationCommentWebHook result;
            try
            {
                result = JsonConvert.DeserializeObject<PrOrganizationCommentWebHook>(json, _settings);
            }
            catch (JsonSerializationException ex)
            {
                result = null;
            }
            return result;
        }

        private PrWebHook PaseSimplePrWebHook(string json)
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

        private PrOrganizationWebHook ParsePrOrganizationWebHook(string json)
        {
            PrOrganizationWebHook result;
            try
            {
                result = JsonConvert.DeserializeObject<PrOrganizationWebHook>(json, _settings);
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
