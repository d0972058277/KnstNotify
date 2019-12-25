using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
        public IEnumerable<ApnConfig> ApnConfigs { get; }
        private readonly IHttpClientFactory _httpClientFactory;

        public ApnSender(IEnumerable<ApnConfig> apnsConfigs, IHttpClientFactory httpClientFactory)
        {
            ApnConfigs = apnsConfigs ?? throw new ArgumentNullException(nameof(apnsConfigs));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        /// <summary>
        /// https://developer.apple.com/library/archive/documentation/NetworkingInternet/Conceptual/RemoteNotificationsPG/CreatingtheNotificationPayload.html#//apple_ref/doc/uid/TP40008194-CH10-SW1
        /// </summary>
        /// <param name="deviceToken"></param>
        /// <param name="notification"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<ApnResult> SendAsync(string deviceToken, ApnPayload notification, Func<IApnSender, ApnConfig> func, ApnOptions options = null)
        {
            if (options is null) options = new ApnOptions();
            ApnConfig apnConfig = func(this);

            var path = $"/3/device/{deviceToken}";
            var json = JsonSerializer.Serialize(notification);

            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(apnConfig.Server + path))
            {
                Version = new Version(2, 0),
                Content = new StringContent(json)
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", apnConfig.Jwt);
            request.Headers.TryAddWithoutValidation(":method", "POST");
            request.Headers.TryAddWithoutValidation(":path", path);
            request.Headers.Add("apns-topic", apnConfig.AppBundleIdentifier);
            request.Headers.Add("apns-expiration", options.ApnsExpiration.ToString());
            request.Headers.Add("apns-priority", options.ApnsPriority.ToString());
            request.Headers.Add("apns-push-type", options.IsBackground ? "background" : "alert"); // for iOS 13 required

            if (!string.IsNullOrWhiteSpace(options.ApnsId))
            {
                request.Headers.Add(apnConfig.ApnidHeader, options.ApnsId);
            }

            HttpClient client = _httpClientFactory.CreateClient();
            using (var response = await client.SendAsync(request))
            {
                var succeed = response.IsSuccessStatusCode;
                var content = await response.Content.ReadAsStringAsync();
                var error = JsonSerializer.Deserialize<ApnError>(content);

                return new ApnResult
                {
                    IsSuccess = succeed,
                    Error = error
                };
            }
        }

        public Task<ApnResult> SendAsync(string deviceToken, ApnPayload notification, ApnOptions options = null)
        {
            return SendAsync(deviceToken, notification, sender => sender.ApnConfigs.Single(), options);
        }

        public IEnumerable<ApnResult> Send(IEnumerable<string> deviceTokens, ApnPayload notification, ApnOptions options = null)
        {
            return Send(deviceTokens, notification, sender => sender.ApnConfigs.Single(), options);
        }

        public IEnumerable<ApnResult> Send(IEnumerable<string> deviceTokens, ApnPayload notification, Func<IApnSender, ApnConfig> func, ApnOptions options = null)
        {
            if (deviceTokens.Count() > 1000) throw new ArgumentOutOfRangeException($"{nameof(deviceTokens)} Out Of Range 1000");

            ConcurrentBag<ApnResult> result = new ConcurrentBag<ApnResult>();

            Task[] sendTasks = deviceTokens.Select(async deviceToken =>
            {
                ApnResult apnResult = await SendAsync(deviceToken, notification, func, options);
                result.Add(apnResult);
            }).ToArray();
            Task.WaitAll(sendTasks);

            return result;
        }
    }
}