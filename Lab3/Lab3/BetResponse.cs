using System.Text.Json.Serialization;

namespace Lab3
{
    class BetResponse
    {
        [JsonPropertyName("message")]
        public string Message;

        [JsonPropertyName("account")]
        public Account Account;

        [JsonPropertyName("realNumber")]
        public long RealNumber;
    }
}
