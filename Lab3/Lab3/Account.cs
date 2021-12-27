using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lab3
{
    class Account
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("money")]
        public int Money { get; set; }
        [JsonPropertyName("deletionTime")]
        public DateTime DeletionTime { get; set; }
    }
}
