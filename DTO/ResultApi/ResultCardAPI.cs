using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MistycPawCraftCore.DTO.ResultApi
{
    public class ResultCardAPI
    {
        public string _object { get; set; }
        public Guid id { get; set; }
        public Guid oracle_id { get; set; }
        public int[] multiverse_ids { get; set; }
        public string name { get; set; }
        public string printed_name { get; set; }
        public string lang { get; set; }
        public string released_at { get; set; }
        public string uri { get; set; }
        public string scryfall_uri { get; set; }
        public string layout { get; set; }
        public bool highres_image { get; set; }
        public string image_status { get; set; }
        public Image_Uris image_uris { get; set; }
        public string mana_cost { get; set; }
        public float cmc { get; set; }
        public string type_line { get; set; }
        public string printed_type_line { get; set; }
        public string oracle_text { get; set; }
        public string printed_text { get; set; }
        public string power { get; set; }
        public string toughness { get; set; }
        public object[] colors { get; set; }
        public object[] color_identity { get; set; }
        public string[] keywords { get; set; }
        public Legalities legalities { get; set; }
        public string[] games { get; set; }
        public bool reserved { get; set; }
        public bool foil { get; set; }
        public bool nonfoil { get; set; }
        public string[] finishes { get; set; }
        public bool oversized { get; set; }
        public bool promo { get; set; }
        public bool reprint { get; set; }
        public bool variation { get; set; }
        [JsonProperty("set_id")]
        public string setid { get; set; }
        public string set { get; set; }
        [JsonProperty("set_name")]
        public string setname { get; set; }
        public string set_type { get; set; }
        [JsonProperty("set_uri")]
        public string seturi { get; set; }
        public string set_search_uri { get; set; }
        public string scryfall_set_uri { get; set; }
        public string rulings_uri { get; set; }
        public string prints_search_uri { get; set; }
        public string collector_number { get; set; }
        public bool digital { get; set; }
        public string rarity { get; set; }
        public string flavor_text { get; set; }
        public string card_back_id { get; set; }
        public string artist { get; set; }
        public string[] artist_ids { get; set; }
        public string illustration_id { get; set; }
        public string border_color { get; set; }
        public string frame { get; set; }
        public bool full_art { get; set; }
        public bool textless { get; set; }
        public bool booster { get; set; }
        public bool story_spotlight { get; set; }
        public int edhrec_rank { get; set; }
        public int penny_rank { get; set; }
        public Prices prices { get; set; }
        public Related_Uris related_uris { get; set; }
        public Purchase_Uris purchase_uris { get; set; }
        public List<CardFace> card_faces { get; set; }
    }

    public class Image_Uris
    {
        public string small { get; set; }
        public string normal { get; set; }
        public string large { get; set; }
        public string png { get; set; }
        public string art_crop { get; set; }
        public string border_crop { get; set; }
    }

    public class CardFace : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string PropertyName)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string _name;
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged("name");
                }
            }
        }

        private string _printed_name;
        public string printed_name
        {
            get
            {
                return _printed_name;
            }
            set
            {
                if (value != _printed_name)
                {
                    _printed_name = value;
                    OnPropertyChanged("printed_name");
                }
            }
        }

        private string _mana_cost;
        public string mana_cost
        {
            get
            {
                return _mana_cost;
            }
            set
            {
                if (value != _mana_cost)
                {
                    _mana_cost = value;
                    OnPropertyChanged("mana_cost");
                }
            }
        }

        private string _power;
        public string power
        {
            get
            {
                return _power;
            }
            set
            {
                if (value != _power)
                {
                    _power = value;
                    OnPropertyChanged("power");
                }
            }
        }

        private string _toughness;
        public string toughness
        {
            get
            {
                return _toughness;
            }
            set
            {
                if (value != _toughness)
                {
                    _toughness = value;
                    OnPropertyChanged("toughness");
                }
            }
        }

        private string _defense;
        public string defense
        {
            get
            {
                return _defense;
            }
            set
            {
                if (value != _defense)
                {
                    _defense = value;
                    OnPropertyChanged("defense");
                }
            }
        }

        private string _loyalty;
        public string loyalty
        {
            get
            {
                return _loyalty;
            }
            set
            {
                if (value != _loyalty)
                {
                    _loyalty = value;
                    OnPropertyChanged("loyalty");
                }
            }
        }

        private string _type_line;
        public string type_line
        {
            get
            {
                return _type_line;
            }
            set
            {
                if (value != _type_line)
                {
                    _type_line = value;
                    OnPropertyChanged("type_line");
                }
            }
        }

        private string _oracle_text;
        public string oracle_text
        {
            get
            {
                return _oracle_text;
            }
            set
            {
                if (value != _oracle_text)
                {
                    _oracle_text = value;
                    OnPropertyChanged("oracle_text");
                }
            }
        }

        private string _printed_text;
        public string printed_text
        {
            get
            {
                return _printed_text;
            }
            set
            {
                if (value != _printed_text)
                {
                    _printed_text = value;
                    OnPropertyChanged("printed_text");
                }
            }
        }

        public Image_Uris image_uris { get; set; }
    }

    public class Legalities
    {
        public string standard { get; set; }
        public string future { get; set; }
        public string historic { get; set; }
        public string timeless { get; set; }
        public string gladiator { get; set; }
        public string pioneer { get; set; }
        public string explorer { get; set; }
        public string modern { get; set; }
        public string legacy { get; set; }
        public string pauper { get; set; }
        public string vintage { get; set; }
        public string penny { get; set; }
        public string commander { get; set; }
        public string oathbreaker { get; set; }
        public string brawl { get; set; }
        public string historicbrawl { get; set; }
        public string alchemy { get; set; }
        public string paupercommander { get; set; }
        public string duel { get; set; }
        public string oldschool { get; set; }
        public string premodern { get; set; }
        public string predh { get; set; }
    }

    public class Prices
    {
        public string usd { get; set; }
        public string usd_foil { get; set; }
        public string usd_etched { get; set; }
        public string eur { get; set; }
        public string eur_foil { get; set; }
        public string tix { get; set; }
    }

    public class Related_Uris
    {
        public string gatherer { get; set; }
        public string tcgplayer_infinite_articles { get; set; }
        public string tcgplayer_infinite_decks { get; set; }
        public string edhrec { get; set; }
    }

    public class Purchase_Uris
    {
        public string tcgplayer { get; set; }
        public string cardmarket { get; set; }
        public string cardhoarder { get; set; }
    }


}
