using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace KnstNotify.Core.APN
{
    public class ApnConfig : ISenderConfig
    {
        public ApnConfig(string p8PrivateKey, string p8PrivateKeyId, string teamId, string topic, ApnServerType apnServerType)
        {
            P8PrivateKey = p8PrivateKey ?? throw new ArgumentNullException(nameof(p8PrivateKey));
            P8PrivateKeyId = p8PrivateKeyId ?? throw new ArgumentNullException(nameof(p8PrivateKeyId));
            TeamId = teamId ?? throw new ArgumentNullException(nameof(teamId));
            Topic = topic ?? throw new ArgumentNullException(nameof(topic));
            serverType = apnServerType;
        }

        public string ApnidHeader { get; } = "apns-id";
        public string P8PrivateKey { get; }
        public string P8PrivateKeyId { get; }
        public string TeamId { get; }
        public string Topic { get; }
        public string Server { get => servers[serverType]; }
        private ApnServerType serverType { get; }

        private string _jwt;
        public string Jwt => _jwt ??= CreateJwt();

        private string CreateJwt()
        {
            var header = JsonSerializer.Serialize(new { alg = "ES256", kid = P8PrivateKeyId });
            var payload = JsonSerializer.Serialize(new { iss = TeamId, iat = ToEpoch(DateTime.UtcNow) });

            using (ECDsa key = ECDsa.Create())
            {
                key.ImportPkcs8PrivateKey(Convert.FromBase64String(P8PrivateKey), out _);

                var headerBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(header));
                var payloadBasae64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payload));
                var unsignedJwtData = $"{headerBase64}.{payloadBasae64}";
                byte[] encodedRequest = Encoding.UTF8.GetBytes(unsignedJwtData);
                byte[] signature = key.SignData(encodedRequest, HashAlgorithmName.SHA256);
                return $"{unsignedJwtData}.{Convert.ToBase64String(signature)}";
            }
        }

        private static int ToEpoch(DateTime time)
        {
            var span = time - new DateTime(1970, 1, 1);
            return Convert.ToInt32(span.TotalSeconds);
        }

        private static readonly Dictionary<ApnServerType, string> servers = new Dictionary<ApnServerType, string>
        {
            {ApnServerType.Development, "https://api.development.push.apple.com:443" },
            {ApnServerType.Production, "https://api.push.apple.com:443" }
        };
    }
}