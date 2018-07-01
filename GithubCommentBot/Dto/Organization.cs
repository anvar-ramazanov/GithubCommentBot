using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GithubCommentBot.Dto
{
    public class Organization
    {
        [JsonProperty("id")]
        public Int64 Id { get; set; }
        [JsonProperty("login")]
        public String Login { get; set; }
        [JsonProperty("node_id")]
        public String NodeId { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("repos_url")]
        public string ReposURL { get; set; }
        [JsonProperty("events_url")]
        public string EventsURL { get; set; }
        [JsonProperty("hooks_url")]
        public string HooksURL { get; set; }
        [JsonProperty("issues_url")]
        public string IssuesURL { get; set; }
        [JsonProperty("members_url")]
        public string MembersURL { get; set; }
        [JsonProperty("public_members_url")]
        public string PublicMembersURL { get; set; }
        [JsonProperty("avatar_url")]
        public string AvatarURL { get; set; }
        [JsonProperty("Description")]
        public String Description { get; set; }
    }
}
