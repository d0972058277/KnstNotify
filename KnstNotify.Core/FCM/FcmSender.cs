using System;
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

        /// <summary>
        /// https://firebase.google.com/docs/cloud-messaging/concept-options#notifications
        /// </summary>
        /// <param name="deviceToken"></param>
        /// <param name="notification"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<FcmResult> SendAsync(FcmPayload notification, Func<IFcmSender, FcmConfig> func)
        {
            if (notification.registration_ids.Count() > 1000) throw new ArgumentOutOfRangeException($"{nameof(notification.registration_ids)} Out Of Range 1000");
            FcmConfig fcmConfig = func(this);
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
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<FcmResult>(content);
                    return result;
                }
            }
        }

        public Task<FcmResult> SendAsync(FcmPayload notification)
        {
            return SendAsync(notification, sender => sender.FcmConfigs.Single());
        }
    }
}