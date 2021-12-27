using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lab3
{
    class CasinoResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("account")]
        public Account Account { get; set; }
        [JsonPropertyName("realNumber")]
        public long RealNumber { get; set; }
    }
}
