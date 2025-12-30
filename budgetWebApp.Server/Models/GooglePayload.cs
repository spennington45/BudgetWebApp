using System.Text.Json.Serialization;

namespace budgetWebApp.Server.Models
{
    public class GooglePayload
    {
        [JsonPropertyName("iss")]
        public string Iss { get; set; } // Issuer

        [JsonPropertyName("azp")]
        public string Azp { get; set; } // Authorized party

        [JsonPropertyName("aud")]
        public string Aud { get; set; } // Audience (your client ID)

        [JsonPropertyName("sub")]
        public string Sub { get; set; } // User's unique Google ID

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("email_verified")]
        public string EmailVerified { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("picture")]
        public string Picture { get; set; }

        [JsonPropertyName("iat")]
        public string Iat { get; set; } // Issued at (Unix timestamp)

        [JsonPropertyName("exp")]
        public string Exp { get; set; }
    }
}
