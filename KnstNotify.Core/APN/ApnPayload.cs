using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KnstNotify.Core.APN
{
    /// <summary>
    /// https://developer.apple.com/documentation/usernotifications/setting_up_a_remote_notification_server/generating_a_remote_notification
    /// </summary>
    public class ApnPayload : ISendPayload
    {
        [JsonPropertyName("aps")]
        public Aps Aps { get; set; } = new Aps();
    }

    public class Aps : IDictionary<string, object>
    {
        [JsonPropertyName("alert")]
        public ApsAlert Alert
        {
            get
            {
                if (!_aps.ContainsKey("alert"))
                {
                    _aps["alert"] = new ApsAlert();
                }
                return (ApsAlert)_aps["alert"];
            }
            set => _aps["alert"] = value;
        }

        [JsonPropertyName("badge")]
        public int Badge { get => (int)_aps["badge"]; set => _aps["badge"] = value; }

        [JsonPropertyName("sound")]
        public string Sound { get => (string)_aps["sound"]; set => _aps["sound"] = value; }

        [JsonPropertyName("thread-id")]
        public string ThreadId { get => (string)_aps["thread-id"]; set => _aps["thread-id"] = value; }

        [JsonPropertyName("category")]
        public string Category { get => (string)_aps["category"]; set => _aps["category"] = value; }

        [JsonPropertyName("content-available")]
        public int ContentAvailable { get => (int)_aps["content-available"]; set => _aps["content-available"] = value; }

        [JsonPropertyName("mutable-content")]
        public int MutableContent { get => (int)_aps["mutable-content"]; set => _aps["mutable-content"] = value; }

        [JsonPropertyName("target-content-id")]
        public string TargetContentId { get => (string)_aps["target-content-id"]; set => _aps["target-content-id"] = value; }

        #region IDictionary<string, object>
        private IDictionary<string, object> _aps = new Dictionary<string, object>();

        public object this[string key] { get => _aps[key]; set => _aps[key] = value; }

        public ICollection<string> Keys => _aps.Keys;

        public ICollection<object> Values => _aps.Values;

        public int Count => _aps.Count;

        public bool IsReadOnly => false;

        public void Add(string key, object value)
        {
            _aps.Add(key, value);
        }

        public void Add(KeyValuePair<string, object> item)
        {
            _aps.Add(item);
        }

        public void Clear()
        {
            _aps.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return _aps.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return _aps.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            _aps.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _aps.GetEnumerator();
        }

        public bool Remove(string key)
        {
            return _aps.Remove(key);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return _aps.Remove(item);
        }

        public bool TryGetValue(string key, out object value)
        {
            return _aps.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _aps.GetEnumerator();
        }
        #endregion
    }

    public class ApsAlert : IDictionary<string, object>
    {
        [JsonPropertyName("title")]
        public string Title { get => (string)_alert["title"]; set => _alert["title"] = value; }

        [JsonPropertyName("subtitle")]
        public string Subtitle { get => (string)_alert["subtitle"]; set => _alert["subtitle"] = value; }

        [JsonPropertyName("body")]
        public string Body { get => (string)_alert["body"]; set => _alert["body"] = value; }

        [JsonPropertyName("launch-image")]
        public string LaunchImage { get => (string)_alert["launch-image"]; set => _alert["launch-image"] = value; }

        [JsonPropertyName("title-loc-key")]
        public string TitleLocKey { get => (string)_alert["title-loc-key"]; set => _alert["title-loc-key"] = value; }

        [JsonPropertyName("title-loc-args")]
        public IEnumerable<string> TitleLocArgs { get => (IEnumerable<string>)_alert["title-loc-args"]; set => _alert["title-loc-args"] = value; }

        [JsonPropertyName("subtitle-loc-key")]
        public string SubtitleLocKey { get => (string)_alert["subtitle-loc-key"]; set => _alert["subtitle-loc-key"] = value; }

        [JsonPropertyName("title-loc-args")]
        public IEnumerable<string> SubtitleLocArgs { get => (IEnumerable<string>)_alert["subtitle-loc-args"]; set => _alert["subtitle-loc-args"] = value; }

        [JsonPropertyName("action-loc-key")]
        public string ActionLocKey { get => (string)_alert["action-loc-key"]; set => _alert["action-loc-key"] = value; }

        [JsonPropertyName("loc-key")]
        public string LocKey { get => (string)_alert["loc-key"]; set => _alert["loc-key"] = value; }

        [JsonPropertyName("loc-args")]
        public IEnumerable<string> LocArgs { get => (IEnumerable<string>)_alert["loc-args"]; set => _alert["loc-args"] = value; }

        #region IDictionary<string, object>
        private IDictionary<string, object> _alert = new Dictionary<string, object>();

        public object this[string key] { get => _alert[key]; set => _alert[key] = value; }

        public ICollection<string> Keys => _alert.Keys;

        public ICollection<object> Values => _alert.Values;

        public int Count => _alert.Count;

        public bool IsReadOnly => false;

        public void Add(string key, object value)
        {
            _alert.Add(key, value);
        }

        public void Add(KeyValuePair<string, object> item)
        {
            _alert.Add(item);
        }

        public void Clear()
        {
            _alert.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return _alert.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return _alert.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            _alert.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _alert.GetEnumerator();
        }

        public bool Remove(string key)
        {
            return _alert.Remove(key);
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return _alert.Remove(item);
        }

        public bool TryGetValue(string key, out object value)
        {
            return _alert.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _alert.GetEnumerator();
        }
        #endregion
    }
}