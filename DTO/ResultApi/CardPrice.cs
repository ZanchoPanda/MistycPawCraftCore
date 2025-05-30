using Newtonsoft.Json;

namespace MistycPawCraftCore.DTO.ResultApi
{
    public class CardPrice
    {
        public string Name { get; set; }
        public string Set { get; set; }
        public string Currency { get; set; }

        [JsonProperty("prices")]
        public Prices Price { get; set; }

    }

    public class Price
    {
        [JsonProperty("usd")]
        public decimal usd { get; set; }

        [JsonProperty("usd_foil")]
        public decimal usd_foil { get; set; }

        [JsonProperty("eur")]
        public decimal eur { get; set; }

        [JsonProperty("eur_foil")]
        public decimal eur_foil { get; set; }

    }

}
