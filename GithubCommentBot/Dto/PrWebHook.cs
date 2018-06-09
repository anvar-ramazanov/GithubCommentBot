using GithubCommentBot.Models;
using Newtonsoft.Json;

namespace GithubCommentBot.Dto
{
    public class PrWebHook
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("review")]
        public Review Review { get; set; }

        [JsonProperty("pull_request")]
        public PullRequest PullRequest { get; set; }

        [JsonProperty("repository")]
        public Repo Repository { get; set; }

        [JsonProperty("sender")]
        public Sender Sender { get; set; }
    }
}
