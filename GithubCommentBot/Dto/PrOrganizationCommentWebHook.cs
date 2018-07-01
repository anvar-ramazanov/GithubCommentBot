using GithubCommentBot.Models;
using Newtonsoft.Json;

namespace GithubCommentBot.Dto
{
    public class PrOrganizationCommentWebHook : PrCommentWebHook
    {
        [JsonProperty("organization")]
        public Organization Organization { get; set; }
    }
}
