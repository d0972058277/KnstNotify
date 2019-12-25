using System.Collections.Generic;

namespace KnstNotify.Core.FCM
{
    /// <summary>
    /// https://firebase.google.com/docs/cloud-messaging/http-server-ref.html?hl=zh-cn#%E4%B8%8B%E8%A1%8C-http-%E6%B6%88%E6%81%AF-json
    /// </summary>
    public class FcmPayload : ISendPayload
    {
        public string to { get; set; }
        public IEnumerable<string> registration_ids { get; set; }
        public string condition { get; set; }
        public string collapse_key { get; set; }
        public string priority { get; set; } = "high";
        public bool? content_available { get; set; }
        public bool? mutable_content { get; set; }
        public decimal? time_to_live { get; set; }
        public string restricted_package_name { get; set; }
        public bool? dry_run { get; set; }
        public IDictionary<string, object> data { get; set; }
        public IDictionary<string, object> notification { get; set; }
    }
}