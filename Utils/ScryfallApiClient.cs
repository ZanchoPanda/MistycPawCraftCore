using Humanizer;
using MistycPawCraftCore.DTO;
using MistycPawCraftCore.DTO.Filtros;
using MistycPawCraftCore.DTO.ResultApi;
using MistycPawCraftCore.Properties;
using MistycPawCraftCore.Utils.Enums;
using Newtonsoft.Json;
using Svg;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MistycPawCraftCore.Utils
{
    public class ScryfallApiClient
    {

        private readonly HttpClient _httpClient;

        public string BasePath { get; set; }
        public string BasePathImage { get; set; }
        WebClient webClient = new WebClient();

        #region Constructor

        public ScryfallApiClient()
        {
            BasePath = System.Environment.CurrentDirectory;
            //BasePath = Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName;
            BasePathImage = Settings.Default.ImageBasePath;

            HttpClientHandler hch = new HttpClientHandler();
            hch.Proxy = null;
            hch.UseProxy = false;
            hch.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;

            _httpClient = new HttpClient(hch)
            {
                BaseAddress = new Uri("https://api.scryfall.com/"),
            };

            _httpClient.DefaultRequestHeaders.Add("User-Agent", "ScryfallApiClient/1.0");

            //LoadSetsAsync();
            //LoadCardsSymbolsAsync();

        }

        /// <summary>
        /// Initiate connection to API of Scryfall
        /// </summary>
        /// <param name="LoadSet">Parameter to Load all sets. Default = false</param>
        public ScryfallApiClient(bool LoadSet = false) : this()
        {
            //BasePath = Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName;
            //BasePathImage = $"{BasePath}\\Images\\";

            //HttpClientHandler hch = new HttpClientHandler();
            //hch.Proxy = null;
            //hch.UseProxy = false;

            //_httpClient = new HttpClient(hch)
            //{
            //    BaseAddress = new Uri("https://api.scryfall.com/"),
            //};

            //_httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("ScryfallApiClient/1.0");

            if (LoadSet)
            {
                LoadSetsAsync();
                LoadCardsSymbolsAsync();
            }
        }

        public async Task LoadSetsAsync(bool ReLoadSets = false)
        {
            try
            {
                DictionarySet = new Dictionary<string, SetDTO>();
                HttpResponseMessage response = await _httpClient.GetAsync("sets");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(content);
                    ParseJsonToDictionarySets(data, ReLoadSets);

                }
                else
                {
                    throw new Exception($"Error al realizar la solicitud. Código de estado: {response.StatusCode}");
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error al comunicarse con la API: {ex.Message}");
            }
        }

        public async Task LoadCardsSymbolsAsync()
        {
            try
            {
                DictionaryMana = new Dictionary<string, Uri>();
                HttpResponseMessage response = await _httpClient.GetAsync("symbology");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(content);
                    ParseJsonToDictionarySymbols(data);
                }
                else
                {
                    throw new Exception($"Error al realizar la solicitud. Código de estado: {response.StatusCode}");
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error al comunicarse con la API: {ex.Message}");
            }
        }

        #endregion

        #region Methods

        public string CardNameResearch { get; set; }
        public bool AmpliarBusqueda { get; set; }
        public async Task<List<CardDto>> GetCardByName(string cardName, string setCode = "")
        {
            #region Version 1 de busqueda (Funcional pero algo lenta)

            //try
            //{
            //    CardNameResearch = cardName;
            //    HttpResponseMessage response;

            //    if (AmpliarBusqueda)
            //    {
            //        response = await _httpClient.GetAsync($"cards/search?q=lang:{Properties.Settings.Default.Language} name:{Uri.EscapeDataString(cardName)}&fuzzy=true&order=Name");
            //    }
            //    else
            //    {
            //        response = await _httpClient.GetAsync($"cards/named?exact={Uri.EscapeDataString(cardName)}");
            //    }


            //    if (response.IsSuccessStatusCode == false && response.StatusCode == HttpStatusCode.NotFound)
            //    {
            //        if (AmpliarBusqueda)
            //        {
            //            AmpliarBusqueda = false;
            //            return null;
            //        }
            //        else
            //        {
            //            AmpliarBusqueda = true;
            //            return await GetCardByName(cardName);
            //        }
            //    }

            //    if (response.IsSuccessStatusCode)
            //    {
            //        List<CardDto> cards = new List<CardDto>();
            //        string content = await response.Content.ReadAsStringAsync();
            //        ResultSearchApi Vcards = JsonConvert.DeserializeObject<ResultSearchApi>(content);
            //        if (Vcards == null || Vcards.data == null)
            //        {
            //            ResultCardAPI Vcard = JsonConvert.DeserializeObject<ResultCardAPI>(content);
            //            if (Vcards != null)
            //            {
            //                CardDto data = ConvertJsonToCard(Vcard);
            //                cards.Add(data);
            //            }
            //        }
            //        else
            //        {
            //            foreach (ResultCardAPI CardApi in Vcards.data)
            //            {
            //                List<CardDto> data = ParseJsonToCardDtoList(CardApi);
            //                cards.AddRange(data);
            //            }
            //        }
            //        //List<CardDto> cards = ParseJsonToCardDtoList(data);
            //        return cards;
            //    }
            //    else
            //    {
            //        ComprobarError(response.StatusCode);
            //        return new List<CardDto>();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception($"Error al comunicarse con la API: {ex.Message}");
            //}
            //finally
            //{
            //    AmpliarBusqueda = false;
            //}

            #endregion

            #region Version 2

            try
            {
                CardNameResearch = cardName;
                List<CardDto> cards = new List<CardDto>();

                string ApiCall = $"cards/named?exact={Uri.EscapeDataString(cardName)}&lang={Properties.Settings.Default.Language}";

                if (!string.IsNullOrWhiteSpace(setCode))
                {
                    ApiCall += $"&set={setCode.ToLower()}";
                }

                // Intentar búsqueda exacta primero
                HttpResponseMessage response = await _httpClient.GetAsync(ApiCall);

                // Si no la encuentra (404), intentar búsqueda fuzzy
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    string ApiCallGen = $"cards/search?q=lang:{Properties.Settings.Default.Language} name:{Uri.EscapeDataString(cardName)}&fuzzy=true&order=Name";
                    if (!string.IsNullOrWhiteSpace(setCode))
                    {
                        ApiCallGen += $"&set={setCode.ToLower()}";
                    }

                    //response = await _httpClient.GetAsync($"cards/search?q=lang:{Properties.Settings.Default.Language} name:{Uri.EscapeDataString(cardName)}&fuzzy=true&order=Name");
                    response = await _httpClient.GetAsync(ApiCallGen);
                    //response = await _httpClient.GetAsync($"cards/named?fuzzy={Uri.EscapeDataString(cardName)}&lang={Properties.Settings.Default.Language}");
                }

                // Si aún así falla, manejar el error
                if (!response.IsSuccessStatusCode)
                {
                    ComprobarError(response.StatusCode);
                    return new List<CardDto>();
                }

                // Procesar la respuesta JSON
                string content = await response.Content.ReadAsStringAsync();
                ResultSearchApi result = JsonConvert.DeserializeObject<ResultSearchApi>(content);

                if (result?.data != null)
                {
                    foreach (var cardApi in result.data)
                    {
                        cards.AddRange(ParseJsonToCardDtoList(cardApi));
                    }
                }
                else
                {
                    ResultCardAPI singleCard = JsonConvert.DeserializeObject<ResultCardAPI>(content);
                    if (singleCard != null)
                    {
                        cards.Add(ConvertJsonToCard(singleCard));
                    }
                }

                return cards;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al comunicarse con la API: {ex.Message}");
            }

            #endregion

        }

        public async Task<List<CardDto>> GetCardsByName(string cardName)
        {
            try
            {
                CardNameResearch = cardName;
                HttpResponseMessage response = await _httpClient.GetAsync($"cards/search?q={Uri.EscapeDataString(cardName)}");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(content);
                    List<CardDto> cards = ParseJsonToCardDtoList(data);
                    return cards;
                }
                else
                {
                    ComprobarError(response.StatusCode);

                    return new List<CardDto>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al comunicarse con la API: {ex.Message}");
            }
        }

        public async Task<List<CardDto>> GetCardsWithFilters(FiltroDTO filtros)
        {
            try
            {
                string FiltroCreado = CreateFilterString(filtros);

                if (string.IsNullOrWhiteSpace(FiltroCreado))
                {
                    return new List<CardDto>();
                }
                else
                {
                    HttpResponseMessage response = await _httpClient.GetAsync(FiltroCreado);
                    if (response.IsSuccessStatusCode)
                    {
                        List<CardDto> cards = new List<CardDto>();
                        string content = await response.Content.ReadAsStringAsync();
                        //dynamic data = JsonConvert.DeserializeObject(content);
                        //List<CardDto> cards = ParseJsonToCardDtoList(data);

                        ResultSearchApi Vcards = JsonConvert.DeserializeObject<ResultSearchApi>(content);
                        foreach (ResultCardAPI CardApi in Vcards.data)
                        {
                            List<CardDto> data = ParseJsonToCardDtoList(CardApi);
                            cards.AddRange(data);
                        }

                        cards = OrderCardsByFilter(cards, filtros.Order);

                        return cards;
                    }
                    else
                    {
                        ComprobarError(response.StatusCode);
                        return new List<CardDto>();
                    }
                }

                return new List<CardDto>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al comunicarse con la API: {ex.Message}");
            }
        }

        private List<CardDto> OrderCardsByFilter(List<CardDto> cards, OrderTypesEnum order)
        {
            try
            {
                switch (order)
                {
                    case OrderTypesEnum.Name:
                    default:
                        return cards.OrderBy(k => k.Name).ToList();
                        break;
                    case OrderTypesEnum.Release_date:
                        return cards.OrderByDescending(k => k.Set?.ReleasedDate).ToList();
                        break;
                    case OrderTypesEnum.Set_Number:
                        return cards.OrderByDescending(k => k.Set?.CodeSet).ToList();
                        break;
                    case OrderTypesEnum.Rarity:
                        return cards.OrderBy(k => k.Rarity).ToList();
                        break;
                    case OrderTypesEnum.Color:
                        return cards;
                        break;
                    case OrderTypesEnum.Price_EUR:
                        return cards.OrderBy(k => k.CardMarketPrice.eur).ToList();
                        break;
                    case OrderTypesEnum.Price_USD:
                        return cards.OrderBy(k => k.CardMarketPrice.usd).ToList();
                        break;
                    case OrderTypesEnum.Mana_Cost:
                        return cards.OrderBy(k => k.CMC).ToList();
                        break;
                    case OrderTypesEnum.Power:
                        return cards.OrderBy(k => k.Power).ToList();
                        break;
                    case OrderTypesEnum.Thoughness:
                        return cards.OrderBy(k => k.Toughness).ToList();
                        break;
                    case OrderTypesEnum.Artist:
                        return cards.OrderBy(k => k.Artist).ToList();
                        break;
                    case OrderTypesEnum.Set_Review:
                        return cards.OrderByDescending(k => k.Set?.NameSet).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Land-Types / Sets / Various...

        public Dictionary<string, SetDTO> DictionarySet { get; set; }
        public Dictionary<string, Uri> DictionaryMana { get; set; }

        #endregion

        #endregion

        #region Function

        private string CreateFilterString(FiltroDTO filtros)
        {
            try
            {
                string ValueReturn = $"cards/search?q=lang:{Properties.Settings.Default.Language}";

                //string apiUrl = $"https://api.scryfall.com/cards/search?q=lang:{LanguageFilter}";
                //                                          "cards/search?q=lang:{language} name:{LanguageFilter}&fuzzy=";
                //string apiUrl = "https://api.scryfall.com/cards/search?q=lang:es name:Jac&fuzzy=";

                StringBuilder sb = new StringBuilder();
                sb.Append(ValueReturn);
                //sb.Append($"?q=lang:{Properties.Settings.Default.Language}");

                #region Filtros

                if (!string.IsNullOrWhiteSpace(filtros.Name))
                {
                    sb.Append($" name:{filtros.Name}");
                }

                if (filtros.SelectedColors > 0)
                {
                    if (filtros.SelectedColors == 1)
                    {
                        if (!sb.ToString().EndsWith("="))
                        {
                            sb.Append($"+");
                        }
                        if (filtros.White)
                        {
                            sb.Append($"colors%3AW");
                        }

                        if (filtros.Black)
                        {
                            sb.Append($"colors%3AB");
                        }

                        if (filtros.Blue)
                        {
                            sb.Append($"colors%3AU");
                        }

                        if (filtros.Green)
                        {
                            sb.Append($"colors%3AG");
                        }

                        if (filtros.Red)
                        {
                            sb.Append($"colors%3AR");
                        }
                    }
                    else
                    {
                        if (!sb.ToString().EndsWith("="))
                        {
                            sb.Append($"+");
                        }

                        sb.Append($"colors%3D");
                        if (filtros.White)
                        {
                            sb.Append($"W,");
                        }

                        if (filtros.Black)
                        {
                            sb.Append($"B,");
                        }

                        if (filtros.Blue)
                        {
                            sb.Append($"U,");
                        }

                        if (filtros.Green)
                        {
                            sb.Append($"G,");
                        }

                        if (filtros.Red)
                        {
                            sb.Append($"R");
                        }

                        if (sb.ToString().EndsWith(","))
                        {
                            sb.Length--;
                        }

                    }
                }

                if (filtros.ManaCost.HasValue)
                {
                    if (!sb.ToString().EndsWith("="))
                    {
                        sb.Append($"+");
                    }

                    sb.Append($" cmc:{filtros.ManaCost}");
                }

                if (!string.IsNullOrWhiteSpace(filtros.Power))
                {
                    if (!sb.ToString().EndsWith("="))
                    {
                        sb.Append($"+");
                    }

                    sb.Append($" power:{filtros.Power}");

                }

                if (!string.IsNullOrWhiteSpace(filtros.Thoughness))
                {
                    if (!sb.ToString().EndsWith("="))
                    {
                        sb.Append($"+");
                    }

                    sb.Append($" toughness:{filtros.Thoughness}");

                }

                if (filtros.TypeCard != MtgCardType.Other)
                {
                    //if (!sb.ToString().EndsWith("="))
                    //{
                    //    sb.Append($"+");
                    //}

                    sb.Append($" t:{filtros.TypeCardString}");
                }

                if (!string.IsNullOrWhiteSpace(filtros.SuperTypeCardString))
                {
                    //if (!sb.ToString().EndsWith("="))
                    //{
                    //    sb.Append($"+");
                    //}

                    sb.Append($" t:{filtros.SuperTypeCardString}");
                }

                #endregion

                if (sb.ToString() == ValueReturn)
                {
                    return string.Empty;
                }
                else
                {
                    sb.Append("&fuzzy=true");

                    string order = filtros.Order.ToString();
                    sb.Append($"&order={order}");

                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private CardDto ParseJsonToCardDto(string json)
        {
            try
            {
                CardDto Carta = new CardDto();

                if (string.IsNullOrWhiteSpace(json))
                {
                    return Carta;
                }

                ResultCardAPI parsedJson = JsonConvert.DeserializeObject<ResultCardAPI>(json);

                Carta = ConvertJsonToCard(parsedJson);

                return Carta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<CardDto> ParseJsonToCardDtoList(ResultCardAPI jsonData)
        {
            try
            {
                List<CardDto> cards = new List<CardDto>();

                cards.Add(ConvertJsonToCard(jsonData));

                //if (jsonData.data == null)
                //{
                //    cards.Add(ConvertJsonToCard(jsonData));
                //}
                //else
                //{
                //    foreach (var item in jsonData.data)
                //    {
                //        string ItemS = Convert.ToString(item);

                //        ResultCardAPI parsedJson = JsonConvert.DeserializeObject<ResultCardAPI>(ItemS);

                //        if (parsedJson.variation)
                //        {
                //            continue;
                //        }
                //        else
                //        {
                //            CardDto Card = ConvertJsonToCard(parsedJson);
                //            if (!string.IsNullOrWhiteSpace(Card.OracleText) && !string.IsNullOrWhiteSpace(Card.TypeAndClass.SuperType.FullSuperType))
                //            {
                //                cards.Add(Card);
                //            }
                //        }
                //    }
                //}

                return cards;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DateTime FormatDateTime(string released_at)
        {
            try
            {
                if (DateTime.TryParse(released_at, out DateTime fecha))
                {
                    // Obtiene la información de formato de la cultura actual del sistema
                    CultureInfo culturaActual = CultureInfo.CurrentCulture;

                    // Formatea la fecha según la cultura actual
                    string fechaFormateada = fecha.ToString("D", culturaActual);

                    return fecha;
                }
                else
                {
                    return DateTime.MinValue;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Retrieve local path of image
        /// </summary>
        /// <param name="UriString">Uri to download Image</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private string DownloadImageToLocal(string UriString, string FolderName, string NameImage, TipoImagenEnum TipoImagen)
        {
            try
            {
                string LocalImagePAth = string.Empty;

                switch (TipoImagen)
                {
                    case TipoImagenEnum.Set:
                        BasePathImage = BasePathImage + "Sets";
                        if (Directory.Exists($"{BasePathImage}\\{FolderName}"))
                        {
                            if (!File.Exists($"{BasePathImage}\\{FolderName}\\{NameImage}.png"))
                            {
                                webClient.DownloadFile(UriString, $"{BasePathImage}\\{FolderName}\\{NameImage}.png");
                                LocalImagePAth = $"{BasePathImage}\\{FolderName}\\{NameImage}.png";
                            }
                            else
                            {
                                LocalImagePAth = $"{BasePathImage}\\{FolderName}\\{NameImage}.png";
                            }

                        }
                        break;
                    case TipoImagenEnum.Symbol:
                        BasePathImage = BasePathImage + "Symbols";
                        if (Directory.Exists($"{BasePathImage}\\"))
                        {
                            if (!File.Exists($"{BasePathImage}\\{NameImage}.png"))
                            {
                                webClient.DownloadFile(UriString, $"{BasePathImage}\\{NameImage}.png");
                                LocalImagePAth = $"{BasePathImage}\\{NameImage}.png";
                            }
                            else
                            {
                                LocalImagePAth = $"{BasePathImage}\\{NameImage}.png";
                            }

                        }
                        break;

                    case TipoImagenEnum.Card:
                        if (!string.IsNullOrWhiteSpace(Settings.Default.ImageCardPath))
                        {
                            if (Directory.Exists(Settings.Default.ImageCardPath))
                            {
                                byte[] imageBytes = new HttpClient().GetByteArrayAsync(UriString).Result;

                                File.WriteAllBytes($"{Settings.Default.ImageCardPath}{FolderName}_{NameImage}.png", imageBytes);

                            }

                        }
                        break;

                    default:
                        break;
                }
                return LocalImagePAth;
            }
            catch (Exception ex)
            {
                var aux = ex;
                return string.Empty;
            }
        }
        private async Task<string> DownloadImageToLocalAsync(string uriString, string folderName, string nameImage, TipoImagenEnum tipoImagen)
        {
            try
            {
                #region Antiguo

                //string basePath = Path.Combine(BasePathImage, tipoImagen == TipoImagenEnum.Set ? "Sets" : "Symbols");
                //string fullFolderPath = tipoImagen == TipoImagenEnum.Set ? Path.Combine(basePath, folderName) : basePath;
                //string filePath = tipoImagen == TipoImagenEnum.Set ?
                //    Path.Combine(fullFolderPath, $"{nameImage}.png") :
                //    Path.Combine(fullFolderPath, $"{nameImage}.png");
                //  if (!Directory.Exists(fullFolderPath))
                // Directory.CreateDirectory(fullFolderPath);
                #endregion

                #region Nuevo
                string basePath = BasePathImage;

                switch (tipoImagen)
                {
                    case TipoImagenEnum.Set:
                        basePath = Path.Combine(basePath, "Sets", folderName);
                        break;
                    case TipoImagenEnum.Symbol:
                        basePath = Path.Combine(basePath, "Symbols");
                        break;
                    case TipoImagenEnum.Card:
                        basePath = Settings.Default.ImageCardPath;
                        break;
                    default:
                        return string.Empty;
                }
                #endregion

                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                string filePath = Path.Combine(basePath, $"{basePath}{folderName}_{nameImage}.png");

                if (File.Exists(filePath))
                    return filePath;

                // Descargar imagen asíncronamente
                var imageBytes = await _httpClient.GetByteArrayAsync(uriString);
                await Task.Run(() => File.WriteAllBytes(filePath, imageBytes));

                return filePath;

                //if (!File.Exists(filePath))
                //{
                //    using (HttpClient client = new HttpClient())
                //    {
                //        byte[] fileBytes = await client.GetByteArrayAsync(uriString);
                //        await Task.Run(() => File.WriteAllBytes(filePath, fileBytes));
                //    }
                //}

                //return filePath;
            }
            catch
            {
                return string.Empty;
            }
        }

        private string SearchIconSet(string codeSet)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codeSet))
                {
                    return string.Empty;
                }

                string PathImageSet = string.Empty;
                string setImagePath = $"{BasePathImage}\\MTG_{codeSet}";

                Directory.CreateDirectory($"{setImagePath}");
                if (Directory.Exists($"{setImagePath}"))
                {
                    if (!File.Exists($"{setImagePath}\\{codeSet}.png"))
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return $"{setImagePath}\\{codeSet}.png";
                    }
                }

                return PathImageSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private SetDTO CreateSet(dynamic Json)
        {
            try
            {
                SetDTO set = new SetDTO();
                set.CodeSet = Json.code;
                set.NameSet = Json.name;
                set.id = Json.id;
                set.UriSet = Json.uri;
                set.ScryfallUri = Json.scryfall_uri;

                string[] ReleaseDate = (Json.released_at).ToString().Split('-');
                if (ReleaseDate.Count() == 3)
                {
                    set.ReleasedDate = new DateTime(int.Parse(ReleaseDate[0]), int.Parse(ReleaseDate[1]), int.Parse(ReleaseDate[2]));
                }

                set.SetType = Json.set_type;
                set.TotalCards = Json.card_count;
                set.Digital = Json.digital;
                set.nonfoil_only = Json.nonfoil_only;
                set.foil_only = Json.foil_only;
                set.svg_Uri = Json.icon_svg_uri;

                string setImagePath = $"{BasePathImage}\\MTG_{set.CodeSet}";

                Directory.CreateDirectory(BasePathImage);

                Directory.CreateDirectory(setImagePath);

                if (!File.Exists($"{setImagePath}\\{set.CodeSet}.png"))
                {
                    webClient.DownloadFile(set.svg_Uri.AbsoluteUri, $"{setImagePath}\\{set.CodeSet}.svg");

                    try
                    {
                        SvgDocument svg = SvgDocument.Open($"{setImagePath}\\{set.CodeSet}.svg");
                        if (svg != null)
                        {
                            var bitmap = svg.Draw();
                            bitmap.Save($"{setImagePath}\\{set.CodeSet}.png", ImageFormat.Png);
                        }
                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {
                        if (File.Exists($"{setImagePath}\\{set.CodeSet}.svg"))
                        {
                            File.Delete($"{setImagePath}\\{set.CodeSet}.svg");
                        }
                    }

                    set.ImageLocalPath = $"{setImagePath}\\{set.CodeSet}.png";

                    if (DictionarySet.ContainsKey(set.CodeSet) == false)
                    {
                        DictionarySet.Add(set.CodeSet, set);
                    }

                }

                return set;
            }
            catch (Exception ex)
            {
                var aux = ex;
                return new SetDTO();
            }

        }

        private void ParseJsonToDictionarySets(dynamic jsonData, bool ReLoadSets = false)
        {
            try
            {
                if (DictionarySet == null)
                {
                    DictionarySet = new Dictionary<string, SetDTO>();
                }

                foreach (var item in jsonData.data)
                {
                    string ItemS = Convert.ToString(item);

                    dynamic parsedJson = JsonConvert.DeserializeObject(ItemS);

                    ConvertJsonToDictionaryItem(parsedJson, ReLoadSets);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ParseJsonToDictionarySymbols(dynamic jsonData)
        {
            try
            {
                if (DictionaryMana == null)
                {
                    DictionaryMana = new Dictionary<string, Uri>();
                }

                foreach (var item in jsonData.data)
                {
                    string ItemS = Convert.ToString(item);
                    ResultCardsSymbolsAPI parsedJson = JsonConvert.DeserializeObject<ResultCardsSymbolsAPI>(ItemS);
                    ConvertJsonToDictionarySymbolItem(parsedJson);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private CardDto ConvertJsonToCard(ResultCardAPI Json)
        {
            try
            {
                CardDto Carta = new CardDto();

                if (Json == null)
                {
                    return Carta;
                }

                #region Codigo Antiguo Funcional (Comentado)
                //if (Json != null)
                //{
                //    //TODO: Modificar el cambio de datos si la carta en cuestion tiene varias caras
                //    if (Json.layout == "transform")
                //    {

                //    }

                //    Carta.id = Json.id;
                //    Carta.Name = Json.printed_name ?? Json.name;
                //    Carta.PrintedName = Json.printed_name ?? Json.name;
                //    Carta.ManaCost = Json.mana_cost ?? string.Empty;
                //    Carta.ManaCostView = ConvertManaCostToView(Json.mana_cost);
                //    Carta.CMC = Json.cmc;

                //    Carta.OracleText = $"{Environment.NewLine}{Json.printed_text?.Replace("• ", Environment.NewLine)}{Environment.NewLine}";
                //    Carta.Power = Json.power ?? string.Empty;
                //    Carta.Toughness = Json.toughness ?? string.Empty;
                //    Carta.Set = (DictionarySet != null && DictionarySet.Count > 0 && DictionarySet.ContainsKey(Json.set.ToString())) ? DictionarySet[Json.set.ToString()] :
                //                new SetDTO() { CodeSet = Json.set, NameSet = Json.setname, ImageLocalPath = $"{Settings.Default.ImageSetPath}{Json.set}\\{Json.set}.png" };
                //    Carta.Rarity = Json.rarity;
                //    Carta.Artist = Json.artist;
                //    Carta.Flavor_Text = Json.flavor_text;

                //    Carta.HasReprints = !string.IsNullOrWhiteSpace(Json.prints_search_uri);
                //    if (Carta.HasReprints)
                //    {
                //        Carta.RePrintsUri = !string.IsNullOrWhiteSpace(Json.prints_search_uri) ? new Uri(Json.prints_search_uri) : new Uri(string.Empty);
                //    }

                //    ConvertJsonToPricesDTO(Json.name, Carta);
                //    //Carta.CardMarketPrice = Json.id != null ? ConvertJsonToPricesDTO(Json.id, Carta) : new PricesDTO();
                //    Carta.PurchasesUri = Json.purchase_uris != null ? ConvertJsonToPurchasesURIsDTO(Json.purchase_uris) : new PurchasesUrisDTO();

                //    Carta.TypeAndClass = Json.printed_type_line != null ? ConvertToSpecific(Json.printed_type_line.ToString()) : Json.type_line != null ? ConvertToSpecific(Json.type_line.ToString()) : new TypeAndSubTypeDto();
                //    Carta.Legalities = Json.legalities != null ? GetCardLegalities(Json.legalities) : new LegalitiesDto();

                //    string imageUrl = null;

                //    if (Json.image_uris != null)
                //    {
                //        imageUrl = Json.image_uris.normal;
                //    }
                //    else if (Json.card_faces != null && Json.card_faces.Count > 0)
                //    {
                //        foreach (CardFace Reverse in Json.card_faces)
                //        {
                //            if (Carta.CardFaces == null)
                //            {
                //                Carta.CardFaces = new List<CardFace>();
                //            }
                //            Carta.CardFaces.Add(Reverse);
                //        }
                //        imageUrl = Json.card_faces[0].image_uris?.normal;
                //    }

                //    string TextCard = string.Empty;
                //    if (Json.card_faces?.Count > 0)
                //    {
                //        Carta.OracleText = Json.card_faces[0].oracle_text.Replace("• ", Environment.NewLine);
                //        Carta.ManaCost = Json.card_faces[0].mana_cost;
                //        Carta.ManaCostView = ConvertManaCostToView(Carta.ManaCost);

                //        Carta.Power = Json.card_faces[0].power;
                //        Carta.Toughness = Json.card_faces[0].toughness;

                //        Carta.TypeAndClass = ConvertToSpecific(Json.card_faces[0].type_line.ToString());

                //    }

                //    Carta.ImageUris = Json.image_uris != null ? FillUris(Json.image_uris) : new ImageUriDto();

                //}
                #endregion

                #region Codigo Nuevo

                Carta = new CardDto
                {
                    id = Json.id,
                    Name = Json.printed_name ?? Json.name,
                    PrintedName = Json.printed_name ?? Json.name,
                    CMC = Json.cmc,
                    Rarity = Json.rarity,
                    Artist = Json.artist,
                    Flavor_Text = Json.flavor_text,
                    HasReprints = !string.IsNullOrWhiteSpace(Json.prints_search_uri),
                    PurchasesUri = Json.purchase_uris != null ? ConvertJsonToPurchasesURIsDTO(Json.purchase_uris) : new PurchasesUrisDTO(),
                    Legalities = Json.legalities != null ? GetCardLegalities(Json.legalities) : new LegalitiesDto(),
                    Set = (DictionarySet != null && DictionarySet.TryGetValue(Json.set, out var setDto))
                ? setDto
                : new SetDTO
                {
                    CodeSet = Json.set,
                    NameSet = Json.setname,
                    ImageLocalPath = $"{Settings.Default.ImageSetPath}{Json.set}\\{Json.set}.png"
                }
                };

                if (Carta.HasReprints)
                {
                    Carta.RePrintsUri = new Uri(Json.prints_search_uri);
                }

                // Si la carta tiene múltiples caras
                if (Json.card_faces != null && Json.card_faces.Count > 0)
                {
                    Carta.CardFaces = new List<CardFace>(Json.card_faces);

                    var caraPrincipal = Json.card_faces.FirstOrDefault();
                    var caraSecundary = Json.card_faces.LastOrDefault();
                    Carta.Name = $"{caraPrincipal?.printed_name ?? caraPrincipal.name}/{caraSecundary?.printed_name ?? caraSecundary.name}";

                    Carta.ManaCost = caraPrincipal?.mana_cost ?? string.Empty;
                    Carta.ManaCostView = ConvertManaCostToView(Carta.ManaCost);
                    Carta.OracleText = !string.IsNullOrWhiteSpace(caraPrincipal?.printed_text)
                                            ? caraPrincipal.printed_text.Replace("• ", Environment.NewLine)
                                            : caraPrincipal?.oracle_text?.Replace("• ", Environment.NewLine) ?? string.Empty;

                    Carta.Power = caraPrincipal?.power ?? string.Empty;
                    Carta.Toughness = caraPrincipal?.toughness ?? string.Empty;
                    Carta.TypeAndClass = ConvertToSpecific(caraPrincipal?.type_line ?? string.Empty);
                    // Si no hay image_uris global, usamos la de la cara principal
                    Carta.ImageUris = caraPrincipal?.image_uris != null
                        ? FillUris(caraPrincipal?.image_uris ?? new Image_Uris())
                        : new ImageUriDto();
                }
                else
                {
                    // Carta normal (una sola cara)
                    Carta.ManaCost = Json.mana_cost ?? string.Empty;
                    Carta.ManaCostView = ConvertManaCostToView(Json.mana_cost);
                    Carta.OracleText = (string.IsNullOrWhiteSpace(Json.printed_text)) ? Json.oracle_text : $"{Json.printed_text?.Replace("• ", Environment.NewLine)}";
                    Carta.Power = Json.power ?? string.Empty;
                    Carta.Toughness = Json.toughness ?? string.Empty;
                    Carta.TypeAndClass = Json.printed_type_line != null
                        ? ConvertToSpecific(Json.printed_type_line)
                        : ConvertToSpecific(Json.type_line ?? string.Empty);

                    Carta.ImageUris = Json.image_uris != null
                        ? FillUris(Json.image_uris)
                        : new ImageUriDto();
                }

                ConvertJsonToPricesDTO(Json.name, Carta);

                #endregion

                return Carta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<ManaViewDto> ConvertManaCostToView(string mana_cost)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(mana_cost))
                {
                    return new List<ManaViewDto>();
                }

                List<ManaViewDto> Uris = new List<ManaViewDto>();
                string[] stringSplitted = mana_cost.Split('}');

                foreach (string item in stringSplitted)
                {
                    if (string.IsNullOrWhiteSpace(item))
                    {
                        continue;
                    }

                    string ruta = $"{BasePathImage}Symbol\\";
                    string valorString = $"{item.Replace("{", string.Empty)}.png";
                    valorString = valorString.Replace("/", string.Empty);

                    if (File.Exists($"{ruta}{valorString}"))
                    {
                        ManaViewDto VisualMana = new ManaViewDto();
                        VisualMana.Symbol = $"{item.Replace("{", string.Empty)}";
                        VisualMana.SymbolPath = $"{ruta}{valorString}";

                        Uris.Add(VisualMana);
                    }
                    //Uris.Add(DictionaryMana.FirstOrDefault(k => k.Key == valorString).Value);

                }

                return Uris;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private PurchasesUrisDTO ConvertJsonToPurchasesURIsDTO(Purchase_Uris purchase_uris)
        {
            try
            {
                PurchasesUrisDTO Purchases = new PurchasesUrisDTO();

                if (purchase_uris != null)
                {
                    if (!string.IsNullOrWhiteSpace(purchase_uris.cardhoarder))
                        Purchases.cardhoader = new Uri(purchase_uris.cardhoarder);

                    if (!string.IsNullOrWhiteSpace(purchase_uris.cardmarket))
                        Purchases.cardmarket = new Uri(purchase_uris.cardmarket);

                    if (!string.IsNullOrWhiteSpace(purchase_uris.tcgplayer))
                        Purchases.tcgplayer = new Uri(purchase_uris.tcgplayer);
                }

                return Purchases;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private PricesDTO ConvertJsonToPricesDTO(Prices prices)
        {
            try
            {
                PricesDTO Prices = new PricesDTO();
                if (prices != null)
                {
                    //Prices.eur = (decimal?)prices.eur ?? 0.0M;
                    //Prices.eurFoil = (decimal?)prices.eur_foil ?? 0.0M;
                    //Prices.usd = (decimal?)prices.usd ?? 0.0M;
                    //Prices.usdFoil = (decimal?)prices.usd_foil ?? 0.0M;
                }
                return Prices;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ConvertJsonToPricesDTO(string CardName, CardDto Carta)
        {
            try
            {
                CultureInfo CulturaAnterior = Thread.CurrentThread.CurrentCulture;

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "ScryfallApiClient/1.0");

                    string apiURL = $"https://api.scryfall.com/cards/named?exact={Uri.EscapeDataString(CardName)}&lang=en";

                    HttpResponseMessage response = client.GetAsync(apiURL).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = response.Content.ReadAsStringAsync().Result;
                        CardPrice cardPrice = JsonConvert.DeserializeObject<CardPrice>(jsonResponse);
                        if (cardPrice != null & cardPrice.Price != null)
                        {
                            decimal salida = 0.0M;
                            Carta.CardMarketPrice = new PricesDTO() { };

                            if (CulturaAnterior.NumberFormat.NumberDecimalSeparator == ",")
                            {
                                if (cardPrice.Price.eur != null && cardPrice.Price.eur.Contains("."))
                                {
                                    cardPrice.Price.eur = cardPrice.Price.eur.Replace(".", ",");
                                }
                                if (cardPrice.Price.eur_foil != null && cardPrice.Price.eur_foil.Contains("."))
                                {
                                    cardPrice.Price.eur_foil = cardPrice.Price.eur_foil.Replace(".", ",");
                                }
                                if (cardPrice.Price.usd != null && cardPrice.Price.usd.Contains("."))
                                {
                                    cardPrice.Price.usd = cardPrice.Price.usd.Replace(".", ",");
                                }
                                if (cardPrice.Price.usd_foil != null && cardPrice.Price.usd_foil.Contains("."))
                                {
                                    cardPrice.Price.usd_foil = cardPrice.Price.usd_foil.Replace(".", ",");
                                }
                            }
                            else
                            {
                                if (cardPrice.Price.eur != null && cardPrice.Price.eur.Contains(","))
                                {
                                    cardPrice.Price.eur = cardPrice.Price.eur.Replace(",", ".");
                                }
                                if (cardPrice.Price.eur_foil != null && cardPrice.Price.eur_foil.Contains(","))
                                {
                                    cardPrice.Price.eur_foil = cardPrice.Price.eur_foil.Replace(",", ".");
                                }
                                if (cardPrice.Price.usd != null && cardPrice.Price.usd.Contains(","))
                                {
                                    cardPrice.Price.usd = cardPrice.Price.usd.Replace(",", ".");
                                }
                                if (cardPrice.Price.usd_foil != null && cardPrice.Price.usd_foil.Contains(","))
                                {
                                    cardPrice.Price.usd_foil = cardPrice.Price.usd_foil.Replace(",", ".");
                                }
                            }

                            Carta.CardMarketPrice.eur = decimal.TryParse(cardPrice.Price.eur, out salida) ? salida : 0.0M;
                            Carta.CardMarketPrice.eurFoil = decimal.TryParse(cardPrice.Price.eur_foil, out salida) ? salida : 0.0M;
                            Carta.CardMarketPrice.usd = decimal.TryParse(cardPrice.Price.usd, out salida) ? salida : 0.0M;
                            Carta.CardMarketPrice.usdFoil = decimal.TryParse(cardPrice.Price.usd_foil, out salida) ? salida : 0.0M;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error en la solicitud: {response.StatusCode}");
                    }

                    Thread.CurrentThread.CurrentCulture = CulturaAnterior;
                    Thread.CurrentThread.CurrentUICulture = CulturaAnterior;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ImageUriDto FillUris(Image_Uris image_uris)
        {
            try
            {
                ImageUriDto Images = new ImageUriDto();
                if (image_uris != null)
                {
                    Images.Normal = new Uri(image_uris.normal);
                    Images.Small = new Uri(image_uris.small);
                    Images.Large = new Uri(image_uris.large);
                    Images.PNG = new Uri(image_uris.png);
                }

                return Images;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ConvertJsonToDictionaryItem(dynamic Json, bool ReLoadSets = false)
        {
            try
            {
                if (Json != null)
                {
                    SetDTO set = new SetDTO();
                    set.CodeSet = Json.code;
                    set.NameSet = Json.name;
                    set.id = Json.id;
                    set.UriSet = Json.uri;
                    set.ScryfallUri = Json.scryfall_uri;

                    string[] ReleaseDate = (Json.released_at).ToString().Split('-');
                    if (ReleaseDate.Count() == 3)
                    {
                        set.ReleasedDate = new DateTime(int.Parse(ReleaseDate[0]), int.Parse(ReleaseDate[1]), int.Parse(ReleaseDate[2]));
                    }

                    set.SetType = Json.set_type;
                    set.TotalCards = Json.card_count;
                    set.Digital = Json.digital;
                    set.nonfoil_only = Json.nonfoil_only;
                    set.foil_only = Json.foil_only;
                    set.svg_Uri = Json.icon_svg_uri;

                    if (!Settings.Default.ImageSetPath.EndsWith("\\"))
                    {
                        Settings.Default.ImageSetPath = $"{Settings.Default.ImageSetPath}\\";
                    }
                    string setImagePath = $"{Settings.Default.ImageSetPath}{set.CodeSet}";

                    Directory.CreateDirectory(BasePathImage);

                    Directory.CreateDirectory(setImagePath);

                    string pathSet = $"{setImagePath}\\{set.CodeSet}.png";

                    if (!File.Exists(pathSet))
                    {
                        webClient.DownloadFile(set.svg_Uri.AbsoluteUri, $"{setImagePath}\\{set.CodeSet}.svg");

                        try
                        {
                            SvgDocument svg = SvgDocument.Open($"{setImagePath}\\{set.CodeSet}.svg");
                            if (svg != null)
                            {
                                var bitmap = svg.Draw();
                                bitmap.Save($"{setImagePath}\\{set.CodeSet}.png", ImageFormat.Png);
                            }
                        }
                        catch (Exception)
                        {

                        }
                        finally
                        {
                            if (File.Exists($"{setImagePath}\\{set.CodeSet}.svg"))
                            {
                                File.Delete($"{setImagePath}\\{set.CodeSet}.svg");
                            }
                        }

                    }
                    else if (ReLoadSets)
                    {
                        File.Delete($"{setImagePath}\\{set.CodeSet}.png");
                        webClient.DownloadFile(set.svg_Uri.AbsoluteUri, $"{setImagePath}\\{set.CodeSet}.svg");

                        try
                        {
                            SvgDocument svg = SvgDocument.Open($"{setImagePath}\\{set.CodeSet}.svg");
                            if (svg != null)
                            {
                                var bitmap = svg.Draw();
                                bitmap.Save($"{setImagePath}\\{set.CodeSet}.png", ImageFormat.Png);
                            }
                        }
                        catch (Exception)
                        {

                        }
                        finally
                        {
                            if (File.Exists($"{setImagePath}\\{set.CodeSet}.svg"))
                            {
                                File.Delete($"{setImagePath}\\{set.CodeSet}.svg");
                            }
                        }
                    }

                    set.ImageLocalPath = $"{setImagePath}\\{set.CodeSet}.png";

                    if (DictionarySet.ContainsKey(set.CodeSet) == false)
                    {
                        DictionarySet.Add(set.CodeSet, set);
                    }

                }
            }
            catch (Exception ex)
            {
                var aux = ex;
            }
        }

        private void ConvertJsonToDictionarySymbolItem(ResultCardsSymbolsAPI Json)
        {
            try
            {
                if (Json != null)
                {
                    string setImagePath = $"{Settings.Default.ImageSymbolPath}";

                    Directory.CreateDirectory(BasePathImage);

                    Directory.CreateDirectory(setImagePath);

                    string GoodSymbol = Json.symbol.Replace("{", string.Empty).Replace("}", string.Empty);
                    GoodSymbol = GoodSymbol.Replace("/", string.Empty);

                    if (!File.Exists($"{setImagePath}\\{GoodSymbol}.png"))
                    {
                        webClient.DownloadFile(Json.svg_uri, $"{setImagePath}\\{GoodSymbol}.svg");

                        try
                        {
                            SvgDocument svg = SvgDocument.Open($"{setImagePath}\\{GoodSymbol}.svg");
                            if (svg != null)
                            {
                                var bitmap = svg.Draw();
                                bitmap.Save($"{setImagePath}\\{GoodSymbol}.png", ImageFormat.Png);
                            }
                        }
                        catch (Exception)
                        {

                        }
                        finally
                        {
                            if (File.Exists($"{setImagePath}\\{GoodSymbol}.svg"))
                            {
                                File.Delete($"{setImagePath}\\{GoodSymbol}.svg");
                            }
                        }

                    }

                    DictionaryMana.Add(Json.symbol.Replace("{", string.Empty).Replace("}", string.Empty), new Uri(Json.svg_uri));
                }
            }
            catch (Exception ex)
            {
                var aux = ex;
            }
        }

        #region Reprints

        private List<ReprintsDTO> ParseJsonToReprintsDtoList(dynamic Json)
        {
            try
            {
                List<ReprintsDTO> cards = new List<ReprintsDTO>();

                if (Json.data == null)
                {
                    cards.Add(ConvertJsonToReprintDto(Json));
                }
                else
                {
                    foreach (var item in Json.data)
                    {
                        string ItemS = Convert.ToString(item);

                        ResultCardAPI parsedJson = JsonConvert.DeserializeObject<ResultCardAPI>(ItemS);

                        //if (parsedJson.variation)
                        //{
                        //    continue;
                        //}
                        //else
                        //{
                        ReprintsDTO Card = ConvertJsonToReprintDto(parsedJson);
                        cards.Add(Card);
                        //}
                    }
                }

                return cards;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<List<ReprintsDTO>> ParseJsonToReprintsDtoListAsync(dynamic json)
        {
            List<ReprintsDTO> cards = new List<ReprintsDTO>();

            if (json.data == null)
            {
                var singleCard = await ConvertJsonToReprintDtoAsync(json);
                cards.Add(singleCard);
            }
            else
            {
                foreach (var item in json.data)
                {
                    string itemStr = Convert.ToString(item);
                    var parsedJson = JsonConvert.DeserializeObject<ResultCardAPI>(itemStr);
                    var card = await ConvertJsonToReprintDtoAsync(parsedJson);
                    cards.Add(card);
                }
            }

            return cards;
        }

        private ReprintsDTO ConvertJsonToReprintDto(dynamic jsonData)
        {
            try
            {
                ReprintsDTO Reprint = new ReprintsDTO();
                if (jsonData != null)
                {
                    Reprint.Artist = jsonData.artist;



                    Reprint.ReleaseAt = FormatDateTime(jsonData.released_at);
                    Reprint.ImageUris = jsonData.image_uris != null ? FillUris(jsonData.image_uris) : new ImageUriDto();
                    Reprint.PurchaseUris = jsonData.purchase_uris != null ? ConvertJsonToPurchasesURIsDTO(jsonData.purchase_uris) : new PurchasesUrisDTO();
                    Reprint.Set = (DictionarySet != null && DictionarySet.Count > 0 && DictionarySet.ContainsKey(jsonData.set.ToString())) ? DictionarySet[jsonData.set.ToString()] :
                        CreateSet(jsonData.set);
                    Reprint.Set.ImageLocalPath = SearchIconSet(Reprint.Set.CodeSet);
                    Reprint.LocalImagePath = string.IsNullOrWhiteSpace(Reprint.ImageUris.PNG?.ToString()) ? string.Empty : DownloadImageToLocal(Reprint.ImageUris?.PNG?.ToString(), $"MTG_{Reprint.Set.CodeSet}", $"{jsonData.name}", TipoImagenEnum.Card);
                }
                return Reprint;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task<ReprintsDTO> ConvertJsonToReprintDtoAsync(dynamic jsonData)
        {
            var reprint = new ReprintsDTO();

            if (jsonData != null)
            {
                reprint.Artist = jsonData.artist;
                reprint.ReleaseAt = FormatDateTime(jsonData.released_at);
                reprint.ImageUris = jsonData.image_uris != null ? FillUris(jsonData.image_uris) : new ImageUriDto();
                reprint.PurchaseUris = jsonData.purchase_uris != null ? ConvertJsonToPurchasesURIsDTO(jsonData.purchase_uris) : new PurchasesUrisDTO();
                reprint.Set = (DictionarySet != null && DictionarySet.Count > 0 && DictionarySet.ContainsKey(jsonData.set.ToString()))
                    ? DictionarySet[jsonData.set.ToString()]
                    : CreateSet(jsonData.set);

                reprint.Set.ImageLocalPath = SearchIconSet(reprint.Set.CodeSet);

                // Aquí espera a la descarga async sin bloquear
                if (jsonData.image_uris != null)
                {
                    reprint.LocalImagePath = !string.IsNullOrWhiteSpace(reprint.ImageUris?.PNG.ToString())
                        ? await DownloadImageToLocalAsync(reprint.ImageUris.PNG.ToString(), $"MTG_{reprint.Set.CodeSet}", $"{jsonData.name}", TipoImagenEnum.Card)
                        : string.Empty;
                }
            }

            //reprint.Artist = jsonData.artist;
            //reprint.ReleaseAt = FormatDateTime(jsonData.released_at);
            //reprint.ImageUris = jsonData.image_uris != null ? FillUris(jsonData.image_uris) : new ImageUriDto();
            //reprint.PurchaseUris = jsonData.purchase_uris != null ? ConvertJsonToPurchasesURIsDTO(jsonData.purchase_uris) : new PurchasesUrisDTO();

            //reprint.Set = DictionarySet.TryGetValue((string)jsonData.set, out var set) ? set : CreateSet(jsonData.set);
            //reprint.Set.ImageLocalPath = SearchIconSet(reprint.Set.CodeSet);

            //if (!string.IsNullOrWhiteSpace(reprint.ImageUris.PNG?.ToString()))
            //{
            //    reprint.LocalImagePath = await DownloadImageToLocalAsync(reprint.ImageUris.PNG?.ToString(), $"MTG_{reprint.Set.CodeSet}", $"{jsonData.name}", TipoImagenEnum.Set);
            //}

            return reprint;
        }

        public async Task<List<ReprintsDTO>> FillReprints(Uri prints_search_uri)
        {
            try
            {
                List<ReprintsDTO> Reprints = new List<ReprintsDTO>();
                HttpResponseMessage response = await _httpClient.GetAsync($"{prints_search_uri}");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(content);
                    List<ReprintsDTO> cards = ParseJsonToReprintsDtoList(data);
                    return cards.ToList();
                }

                return Reprints.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ReprintsDTO>> FillReprintsAsync(string prints_search_uri)
        {
            prints_search_uri = prints_search_uri + $"&lang:{Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName}";
            List<ReprintsDTO> reprints = new List<ReprintsDTO>();
            HttpResponseMessage response = await _httpClient.GetAsync(prints_search_uri);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject(content);

                reprints = await ParseJsonToReprintsDtoListAsync(data);
            }

            return reprints;
        }

        public async Task<List<ReprintsDTO>> FillReprintsPaginatedAsync(Uri uri, int pageSize = 10, int page = 0)
        {
            var paginatedUri = $"{uri}&page={page + 1}"; // Scryfall usa páginas 1-based
            var response = await _httpClient.GetAsync(paginatedUri);

            if (!response.IsSuccessStatusCode)
                return new List<ReprintsDTO>();

            string content = await response.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(content);
            return await ParseJsonToReprintsDtoListAsync(data);
        }

        #region Paginacion



        #endregion

        #endregion


        #endregion

        #region Format JSON + Other Methods

        private string FormatJson(string json)
        {
            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, Newtonsoft.Json.Formatting.Indented);
        }

        private dynamic FormatResultWeb(string data)
        {
            return JsonConvert.DeserializeObject(data);
        }

        private string CapitalizeString(string Texto)
        {
            try
            {
                return $"{char.ToUpper(Texto[0])}{Texto.Substring(1)}";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ComprobarError(HttpStatusCode StatusCode)
        {
            try
            {
                switch (StatusCode)
                {
                    case System.Net.HttpStatusCode.Continue:
                        break;
                    case System.Net.HttpStatusCode.SwitchingProtocols:
                        break;
                    case System.Net.HttpStatusCode.OK:
                        break;
                    case System.Net.HttpStatusCode.Created:
                        break;
                    case System.Net.HttpStatusCode.Accepted:
                        break;
                    case System.Net.HttpStatusCode.NonAuthoritativeInformation:
                        break;
                    case System.Net.HttpStatusCode.NoContent:
                        break;
                    case System.Net.HttpStatusCode.ResetContent:
                        break;
                    case System.Net.HttpStatusCode.PartialContent:
                        break;
                    case System.Net.HttpStatusCode.Ambiguous:
                        break;
                    case System.Net.HttpStatusCode.Moved:
                        break;
                    case System.Net.HttpStatusCode.Found:
                        break;
                    case System.Net.HttpStatusCode.RedirectMethod:
                        break;
                    case System.Net.HttpStatusCode.NotModified:
                        break;
                    case System.Net.HttpStatusCode.UseProxy:
                        break;
                    case System.Net.HttpStatusCode.Unused:
                        break;
                    case System.Net.HttpStatusCode.RedirectKeepVerb:
                        break;
                    case System.Net.HttpStatusCode.BadRequest:
                        break;
                    case System.Net.HttpStatusCode.Unauthorized:
                        break;
                    case System.Net.HttpStatusCode.PaymentRequired:
                        break;
                    case System.Net.HttpStatusCode.Forbidden:
                        break;
                    case System.Net.HttpStatusCode.NotFound:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine();
                        Console.WriteLine("Card/s not found with this filter/s.");
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Gray;
                        if (AmpliarBusqueda)
                        {
                            GetCardByName(CardNameResearch);
                        }
                        break;
                    case System.Net.HttpStatusCode.MethodNotAllowed:
                        break;
                    case System.Net.HttpStatusCode.NotAcceptable:
                        break;
                    case System.Net.HttpStatusCode.ProxyAuthenticationRequired:
                        break;
                    case System.Net.HttpStatusCode.RequestTimeout:
                        break;
                    case System.Net.HttpStatusCode.Conflict:
                        break;
                    case System.Net.HttpStatusCode.Gone:
                        break;
                    case System.Net.HttpStatusCode.LengthRequired:
                        break;
                    case System.Net.HttpStatusCode.PreconditionFailed:
                        break;
                    case System.Net.HttpStatusCode.RequestEntityTooLarge:
                        break;
                    case System.Net.HttpStatusCode.RequestUriTooLong:
                        break;
                    case System.Net.HttpStatusCode.UnsupportedMediaType:
                        break;
                    case System.Net.HttpStatusCode.RequestedRangeNotSatisfiable:
                        break;
                    case System.Net.HttpStatusCode.ExpectationFailed:
                        break;
                    case System.Net.HttpStatusCode.InternalServerError:
                        break;
                    case System.Net.HttpStatusCode.NotImplemented:
                        break;
                    case System.Net.HttpStatusCode.BadGateway:
                        break;
                    case System.Net.HttpStatusCode.ServiceUnavailable:
                        break;
                    case System.Net.HttpStatusCode.GatewayTimeout:
                        break;
                    case System.Net.HttpStatusCode.HttpVersionNotSupported:
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Fill Card Type and SubType

        private TypeAndSubTypeDto ConvertToSpecific(string type_line)
        {
            try
            {
                string DataCardType = type_line.ToLower();

                TypeAndSubTypeDto DataType = new TypeAndSubTypeDto();
                if (DataCardType.Contains('—'))
                {
                    string[] SplittedCardType = DataCardType.Split('—');

                    DataType.SuperType.FullSuperType = SplittedCardType[0].Humanize(LetterCasing.Title);
                    DataType.CardType.FullCardType = SplittedCardType[1].Humanize(LetterCasing.Title);
                }
                else
                {
                    DataType.SuperType.FullSuperType = DataCardType.Split('—')[0].Humanize(LetterCasing.Title);
                }

                return DataType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private MtgCardType AssignTypeCard(string item)
        {
            try
            {
                switch (item)
                {
                    case var a when item.Contains("creature"):
                        return MtgCardType.Creature;

                    case var a when item.Contains("instant"):
                        return MtgCardType.Instant;

                    case var a when item.Contains("sorcery"):
                        return MtgCardType.Sorcery;

                    case var a when item.Contains("artifact"):
                        return MtgCardType.Artifact;

                    case var a when item.Contains("enchantment"):
                        return MtgCardType.Enchantment;

                    case var a when item.Contains("planeswalker"):
                        return MtgCardType.Planeswalker;

                    case var a when item.Contains("land"):
                        return MtgCardType.Land;

                    default:
                        return MtgCardType.Other;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Fill Legalities

        private LegalitiesDto GetCardLegalities(Legalities legalities)
        {
            try
            {
                return new LegalitiesDto()
                {
                    Standard = legalities.standard.Contains("not") ? false : true,
                    //(!legalities["standard"].ToString().Contains("not")) ? true : false,
                    Pioneer = legalities.pioneer.Contains("not") ? false : true,
                    //(!legalities["pioneer"].ToString().Contains("not")) ? true : false,
                    Modern = legalities.modern.Contains("not") ? false : true,
                    //(!legalities["modern"].ToString().Contains("not")) ? true : false,
                    Legacy = legalities.legacy.Contains("not") ? false : true,
                    //(!legalities["legacy"].ToString().Contains("not")) ? true : false,
                    Pauper = legalities.pauper.Contains("not") ? false : true,
                    //(!legalities["pauper"].ToString().Contains("not")) ? true : false,
                    Commander = legalities.commander.Contains("not") ? false : true,
                    //(!legalities["commander"].ToString().Contains("not")) ? true : false,
                    Historic = legalities.historic.Contains("not") ? false : true,
                    //(!legalities["historic"].ToString().Contains("not")) ? true : false,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal string CleanNameCard(string CardName)
        {
            try
            {
                string CardNameClean = string.Empty;

                string[] CardNameSplited = CardName.Split(' ');
                for (int i = 0; i < CardNameSplited.Length; i++)
                {
                    if (CardNameSplited[i].StartsWith("(") | int.TryParse(CardNameSplited[i], out int number))
                    {
                        continue;
                    }
                    else
                    {
                        CardNameClean = $"{CardNameClean} {CardNameSplited[i]}";
                    }
                }

                return CardNameClean;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Converters


        #endregion

    }
}
