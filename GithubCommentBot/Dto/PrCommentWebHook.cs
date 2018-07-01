using Newtonsoft.Json;

namespace GithubCommentBot.Models
{
    public class PrCommentWebHook
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("comment")]
        public Comment Comment { get; set; }

        [JsonProperty("pull_request")]
        public PullRequest PullRequest { get; set; }

        [JsonProperty("repository")]
        public Repo Repository { get; set; }

        [JsonProperty("sender")]
        public Sender Sender { get; set; }
    }
}
