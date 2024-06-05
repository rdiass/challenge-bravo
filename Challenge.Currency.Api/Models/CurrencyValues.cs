using System.Text.Json.Serialization;

namespace Challenge.Bravo.Api.Models
{
    public class CurrencyValues
    {
        public string Name { get; set; }

        [JsonPropertyName("symbol_native")]
        public string Symbol { get; set; }

        [JsonPropertyName("decimal_digits")]
        public decimal DecimalDigits { get; set; }

        public string Code { get; set; }

        [JsonPropertyName("name_plural")]
        public string NamePlural { get; set; }

        public string Type { get; set; }
    }
}
