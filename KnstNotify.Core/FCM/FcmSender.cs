using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KnstNotify.Core.FCM
{
    internal class FcmSender : IFcmSender
    {
        public IEnumerable<FcmConfig> FcmConfigs { get; }
        private readonly IHttpClientFactory _httpClientFactory;

        public FcmSender(IEnumerable<FcmConfig> fcmConfigs, IHttpClientFactory httpClientFactory)
        {
            FcmConfigs = fcmConfigs ?? throw new ArgumentNullException(nameof(fcmConfigs));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public Task<FcmResult> SendAsync(FcmPayload notification, Func<IFcmSender, FcmConfig> func)
        {
            if (notification.RegistrationIds?.Count() > 1000) throw new ArgumentOutOfRangeException($"{nameof(notification.RegistrationIds)} Out Of Range 1000");
            FcmConfig fcmConfig = func(this);
            return SendAsync(notification, fcmConfig);
        }

        public Task<FcmResult> SendAsync(FcmPayload notification)
        {
            return SendAsync(notification, sender => sender.FcmConfigs.Single());
        }

        public Task<IEnumerable<FcmResult>> SendAsync(IEnumerable<FcmPayload> notifications)
        {
            return SendAsync(notifications, sender => sender.FcmConfigs.Single());
        }

        public Task<IEnumerable<FcmResult>> SendAsync(IEnumerable<FcmPayload> notifications, Func<IFcmSender, FcmConfig> func)
        {
            if (notifications.Any(x => x.RegistrationIds?.Count() > 1000)) throw new ArgumentOutOfRangeException($"RegistrationIds Out Of Range 1000");
            FcmConfig fcmConfig = func(this);
            return SendAsync(notifications, fcmConfig);
        }

        /// <summary>
        /// https://firebase.google.com/docs/cloud-messaging/concept-options#notifications
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="fcmConfig"></param>
        /// <returns></returns>
        public async Task<FcmResult> SendAsync(FcmPayload notification, FcmConfig fcmConfig)
        {
            string json = JsonSerializer.Serialize(notification);

            using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, fcmConfig.FcmUrl))
            {
                httpRequest.Headers.Add("Authorization", $"key = {fcmConfig.ServerKey}");
                if (!string.IsNullOrWhiteSpace(fcmConfig.SenderId))
                {
                    httpRequest.Headers.Add("Sender", $"id = {fcmConfig.SenderId}");
                }
                httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpClient client = _httpClientFactory.CreateClient();
                using (var response = await client.SendAsync(httpRequest))
                {
                    string content = await response.Content.ReadAsStringAsync();
                    FcmResult result = JsonSerializer.Deserialize<FcmResult>(content);
                    result.FcmPayload = notification;
                    return result;
                }
            }
        }

        public async Task<IEnumerable<FcmResult>> SendAsync(IEnumerable<FcmPayload> notifications, FcmConfig fcmConfig)
        {
            Task<FcmResult>[] sendTasks = notifications.Select(notification => SendAsync(notification, fcmConfig)).ToArray();
            FcmResult[] result = await Task.WhenAll(sendTasks);

            return result;
        }
    }
}