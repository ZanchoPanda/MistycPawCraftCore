namespace MistycPawCraftCore.DTO.ResultApi
{
    public class ResultCardsSymbolsAPI
    {

        public bool appears_in_mana_costs { get; set; }

        public decimal? cmc { get; set; }
        public string english { get; set; }
        public string hybrid { get; set; }
        public decimal? mana_value { get; set; }
        public bool phyrexian { get; set; }
        public string svg_uri { get; set; }
        public string symbol { get; set; }

    }

}
