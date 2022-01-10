using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Lab3
{
    class Account
    {
        [JsonPropertyName("id")]
        public string ID;

        [JsonPropertyName("money")]
        public int Money;

        [JsonPropertyName("deletionTime")]
        public string DeletionTime;
    }
}
