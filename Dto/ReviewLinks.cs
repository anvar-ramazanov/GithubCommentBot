using GithubCommentBot.Models;
using Newtonsoft.Json;

namespace GithubCommentBot.Models
{
    public class ReviewLinks
    {
        [JsonProperty("html")]
        public Statuses Html { get; set; }

        [JsonProperty("pull_request")]
        public Statuses PullRequest { get; set; }
    }
}
