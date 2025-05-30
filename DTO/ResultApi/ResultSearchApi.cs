namespace MistycPawCraftCore.DTO.ResultApi
{

    public class ResultSearchApi
    {
        public ResultCardAPI[] data { get; set; }
        public bool has_more { get; set; }
        public string _object { get; set; }
        public int total_cards { get; set; }
    }

}
