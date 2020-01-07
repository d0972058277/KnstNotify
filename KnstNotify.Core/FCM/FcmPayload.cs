using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KnstNotify.Core.FCM
{
    /// <summary>
    /// https://firebase.google.com/docs/cloud-messaging/http-server-ref.html
    /// </summary>
    public class FcmPayload : ISendPayload
    {
        [JsonPropertyName("to")]
        public string To { get; set; }
        [JsonPropertyName("registration_ids")]
        public IEnumerable<string> RegistrationIds { get; set; }
        [JsonPropertyName("condition")]
        public string Condition { get; set; }
        [JsonPropertyName("collapse_key")]
        public string CollapseKey { get; set; }
        [JsonPropertyName("priority")]
        public string Priority { get; set; } = "high";
        [JsonPropertyName("content_available")]
        public bool? ContentAvailable { get; set; }
        [JsonPropertyName("mutable_content")]
        public bool? MutableContent { get; set; }
        [JsonPropertyName("time_to_live")]
        public decimal? TimeToLive { get; set; }
        [JsonPropertyName("restricted_package_name")]
        public string RestrictedPackageName { get; set; }
        [JsonPropertyName("dry_run")]
        public bool? DryRun { get; set; }
        [JsonPropertyName("data")]
        public IDictionary<string, object> Data { get; set; }
        [JsonPropertyName("notification")]
        public IDictionary<string, object> Notification { get; set; }
    }
}