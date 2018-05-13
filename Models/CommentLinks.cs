using Newtonsoft.Json;

namespace GithubCommentBot.Models
{
    public class CommentLinks
    {
        [JsonProperty("self")]
        public Statuses Self { get; set; }

        [JsonProperty("html")]
        public Statuses Html { get; set; }

        [JsonProperty("pull_request")]
        public Statuses PullRequest { get; set; }
    }
}