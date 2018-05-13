using System;
using Newtonsoft.Json;

namespace GithubCommentBot.Models
{
    public class Comment
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("diff_hunk")]
        public string DiffHunk { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("position")]
        public long Position { get; set; }

        [JsonProperty("original_position")]
        public long OriginalPosition { get; set; }

        [JsonProperty("commit_id")]
        public string CommitId { get; set; }

        [JsonProperty("original_commit_id")]
        public string OriginalCommitId { get; set; }

        [JsonProperty("user")]
        public Sender User { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty("pull_request_url")]
        public string PullRequestUrl { get; set; }

        [JsonProperty("_links")]
        public CommentLinks Links { get; set; }
    }
}