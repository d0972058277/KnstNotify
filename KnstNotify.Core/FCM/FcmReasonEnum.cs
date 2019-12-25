using System.Text.Json.Serialization;

namespace KnstNotify.Core.FCM
{
    /// <summary>
    /// https://firebase.google.com/docs/cloud-messaging/http-server-ref.html?hl=zh-cn#error-codes
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FcmReasonEnum
    {
        MissingRegistration,
        InvalidRegistration,
        NotRegistered,
        InvalidPackageName,
        MismatchSenderId,
        InvalidParameters,
        MessageTooBig,
        InvalidDataKey,
        InvalidTtl,
        Unavailable,
        InternalServerError,
        DeviceMessageRate,
        TopicsMessageRate,
        InvalidApnsCredential
    }
}