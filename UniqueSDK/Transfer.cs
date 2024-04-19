using System.Text.Json.Serialization;

namespace UniqueSDK
{
	public class Transfer
	{
        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("destination")]
        public string Destination { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
    }
}

