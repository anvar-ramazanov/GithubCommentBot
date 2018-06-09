using GithubCommentBot.Models;
using Newtonsoft.Json;
using System;

namespace GithubCommentBot.Models
{
    public class Review
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("user")]
        public Sender User { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("submitted_at")]
        public string SubmittedAt { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty("pull_request_url")]
        public string PullRequestUrl { get; set; }

        [JsonProperty("_links")]
        public ReviewLinks Links { get; set; }

        [JsonProperty("node_id")]
        public String NodeId { get; set; }

        [JsonProperty("commit_id")]
        public string CommitId { get; set; }

        [JsonProperty("author_association")]
        public String AuthorAssociation { get; set; }
    }
}
