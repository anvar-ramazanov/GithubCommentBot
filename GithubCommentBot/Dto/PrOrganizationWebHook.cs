using Newtonsoft.Json;

namespace GithubCommentBot.Dto
{
    public class PrOrganizationWebHook : PrWebHook
    {
        [JsonProperty("organization")]
        public Organization Organization { get; set; }
    }
}
