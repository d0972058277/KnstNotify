using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static KnstNotify.Core.APN.ApnResult;

namespace KnstNotify.Core.APN
{
    public class ApnSender : IApnSender
    {
        public IEnumerable<ApnConfig> Configs { get; }
        private readonly IHttpClientFactory _httpClientFactory;

        public ApnSender(IEnumerable<ApnConfig> apnsConfigs, IHttpClientFactory httpClientFactory)
        {
            Configs = apnsConfigs ?? throw new ArgumentNullException(nameof(apnsConfigs));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public Task<ApnResult> SendAsync(ApnPayload notification, Func<IApnSender, ApnConfig> func, ApnOptions options = null)
        {
            ApnConfig apnConfig = func(this);
            return SendAsync(notification, apnConfig, options);
        }

        public Task<ApnResult> SendAsync(ApnPayload notification, ApnOptions options = null)
        {
            return SendAsync(notification, sender => sender.Configs.Single(), options);
        }

        public Task<IEnumerable<ApnResult>> SendAsync(IEnumerable<ApnPayload> notifications, ApnOptions options = null)
        {
            return SendAsync(notifications, sender => sender.Configs.Single(), options);
        }

        public Task<IEnumerable<ApnResult>> SendAsync(IEnumerable<ApnPayload> notifications, Func<IApnSender, ApnConfig> func, ApnOptions options = null)
        {
            ApnConfig apnConfig = func(this);
            return SendAsync(notifications, apnConfig, options);
        }

        /// <summary>
        /// https://developer.apple.com/library/archive/documentation/NetworkingInternet/Conceptual/RemoteNotificationsPG/CreatingtheNotificationPayload.html#//apple_ref/doc/uid/TP40008194-CH10-SW1
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="apnConfig"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<ApnResult> SendAsync(ApnPayload notification, ApnConfig apnConfig, ApnOptions options = null)
        {
            if (options is null) options = new ApnOptions();
            string path = $"/3/device/{notification.DeviceToken}";
            string json = JsonSerializer.Serialize(notification);

            using (var request = new HttpRequestMessage(HttpMethod.Post, new Uri(apnConfig.Server + path)) { Version = new Version(2, 0), Content = new StringContent(json) })
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", apnConfig.Jwt);
                request.Headers.TryAddWithoutValidation(":method", "POST");
                request.Headers.TryAddWithoutValidation(":path", path);
                request.Headers.Add("apns-topic", apnConfig.Topic);
                request.Headers.Add("apns-expiration", options.ApnsExpiration.ToString());
                request.Headers.Add("apns-priority", options.ApnsPriority.ToString());
                request.Headers.Add("apns-collapse-id", options.CollapseId);
                request.Headers.Add("apns-push-type", options.IsBackground ? "background" : "alert"); // for iOS 13 required

                if (!string.IsNullOrWhiteSpace(options.ApnsId))
                {
                    request.Headers.Add(apnConfig.ApnidHeader, options.ApnsId);
                }

                HttpClient client = _httpClientFactory.CreateClient("APN");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                using (var response = await client.SendAsync(request))
                {
                    bool succeed = response.IsSuccessStatusCode;
                    string content = await response.Content.ReadAsStringAsync();

                    return new ApnResult
                    {
                        ApnPayload = notification,
                        IsSuccess = succeed,
                        Error = string.IsNullOrWhiteSpace(content) ? null : JsonSerializer.Deserialize<ApnError>(content)
                    };
                }
            }
        }

        public async Task<IEnumerable<ApnResult>> SendAsync(IEnumerable<ApnPayload> notifications, ApnConfig apnConfig, ApnOptions options = null)
        {
            Task<ApnResult>[] sendTasks = notifications.Select(notification => SendAsync(notification, apnConfig, options)).ToArray();
            ApnResult[] result = await Task.WhenAll(sendTasks);

            return result;
        }
    }
}