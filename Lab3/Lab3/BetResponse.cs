using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
