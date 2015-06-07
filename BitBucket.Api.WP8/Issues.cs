using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitBucket.Api
{

    public class IssuesResponse
    {
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("filter")]
        public Filter Filter { get; set; }
        [JsonProperty("search")]
        public object Search { get; set; }
        [JsonProperty("issues")]
        public Issue[] Issues { get; set; }
    }

    public class Filter
    {
    }

    public class Issue
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("priority")]
        public string Priority { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("reported_by")]
        public Reported_By ReportedBy { get; set; }
        [JsonProperty("utc_last_updated")]
        public string UtcLastUpdated { get; set; }
        [JsonProperty("responsible")]
        public Responsible Responsible { get; set; }
        [JsonProperty("created_on")]
        public DateTime CreatedOn { get; set; }
        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("comment_count")]
        public int CommentCount { get; set; }
        [JsonProperty("local_id")]
        public int LocalId { get; set; }
        [JsonProperty("follower_count")]
        public int FollowerCount { get; set; }
        [JsonProperty("utc_created_on")]
        public string UtcCreatedOn { get; set; }
        [JsonProperty("resource_uri")]
        public string ResourceUri { get; set; }
        [JsonProperty("is_spam")]
        public bool IsSpam { get; set; }
    }

    public class Reported_By
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        [JsonProperty("is_staff")]
        public bool IsStaff { get; set; }
        [JsonProperty("avatar")]
        public string Avatar { get; set; }
        [JsonProperty("resource_uri")]
        public string ResourceUri { get; set; }
        [JsonProperty("is_team")]
        public bool IsTeam { get; set; }
    }

    public class Responsible
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        [JsonProperty("is_staff")]
        public bool IsStaff { get; set; }
        [JsonProperty("avatar")]
        public string Avatar { get; set; }
        [JsonProperty("resource_uri")]
        public string ResourceUri { get; set; }
        [JsonProperty("is_team")]
        public bool IsTeam { get; set; }
    }

    public class Metadata
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }
        [JsonProperty("version")]
        public object Version { get; set; }
        [JsonProperty("component")]
        public string Component { get; set; }
        [JsonProperty("milestone")]
        public string Milestone { get; set; }
    }


    public class Rootobject
    {
        public string status { get; set; }
        public string priority { get; set; }
        public string title { get; set; }
        public Reported_By reported_by { get; set; }
        public string utc_last_updated { get; set; }
        public Responsible responsible { get; set; }
        public DateTime created_on { get; set; }
        public Metadata metadata { get; set; }
        public string content { get; set; }
        public int comment_count { get; set; }
        public int local_id { get; set; }
        public int follower_count { get; set; }
        public string utc_created_on { get; set; }
        public string resource_uri { get; set; }
        public bool is_spam { get; set; }
    }

   
}
