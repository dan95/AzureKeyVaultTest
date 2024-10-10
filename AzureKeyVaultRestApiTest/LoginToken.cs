using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AzureKeyVaultRestApiTest
{
    public class LoginToken
    {
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = string.Empty;
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonIgnore]
        public DateTime TokenCreatedAt { get; set; } = DateTime.Now;
        [JsonIgnore]
        public DateTime TokenExpiresAt => TokenCreatedAt.AddSeconds(ExpiresIn);
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;
    }
}
