using Newtonsoft.Json;

namespace GithubCommentBot.Models
{
    public class Statuses
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }

}