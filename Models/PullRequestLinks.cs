using Newtonsoft.Json;

namespace GithubCommentBot.Models
{
    public class PullRequestLinks
    {
        [JsonProperty("self")]
        public Statuses Self { get; set; }

        [JsonProperty("html")]
        public Statuses Html { get; set; }

        [JsonProperty("issue")]
        public Statuses Issue { get; set; }

        [JsonProperty("comments")]
        public Statuses Comments { get; set; }

        [JsonProperty("review_comments")]
        public Statuses ReviewComments { get; set; }

        [JsonProperty("review_comment")]
        public Statuses ReviewComment { get; set; }

        [JsonProperty("commits")]
        public Statuses Commits { get; set; }

        [JsonProperty("statuses")]
        public Statuses Statuses { get; set; }
    }

}