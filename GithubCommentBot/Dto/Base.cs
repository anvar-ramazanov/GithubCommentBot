using Newtonsoft.Json;

namespace GithubCommentBot.Models
{
    public class Base
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("ref")]
        public string Ref { get; set; }

        [JsonProperty("sha")]
        public string Sha { get; set; }

        [JsonProperty("user")]
        public Sender User { get; set; }

        [JsonProperty("repo")]
        public Repo Repo { get; set; }
    }
}