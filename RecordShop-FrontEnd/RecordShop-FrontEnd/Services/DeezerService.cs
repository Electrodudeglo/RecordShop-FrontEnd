namespace RecordShop_FrontEnd.Services
{
    using RecordShop_FrontEnd;
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Web;
   
    
    public class DeezerService
    {
        private readonly HttpClient _http = new();
        public DeezerService(HttpClient http)
        {
            _http = http;
        }

        public async Task<DeezerAlbumResult?> FindAlbumSync(string title, string artist)
        {
            try
            {
                // fuzzy album search
                var query = HttpUtility.UrlEncode($"{title} {artist}");
                var searchUrl = $"search/album?q={query}&limit=1";

                var response = await _http.GetAsync(searchUrl);

                if (!response.IsSuccessStatusCode)
                {
                    var status = response.StatusCode switch
                    {
                        System.Net.HttpStatusCode.NotFound => DeezerResultStatusEnum.NotFound,
                        System.Net.HttpStatusCode.BadRequest => DeezerResultStatusEnum.ServerError,
                        System.Net.HttpStatusCode.InternalServerError => DeezerResultStatusEnum.ServerError,
                        System.Net.HttpStatusCode.ServiceUnavailable => DeezerResultStatusEnum.ServerError,
                        System.Net.HttpStatusCode.TooManyRequests => DeezerResultStatusEnum.ServerError,
                        _ => DeezerResultStatusEnum.ServerError
                    };

                    return new DeezerAlbumResult { ResultStatus = status };
                }

                var rawJson = await response.Content.ReadAsStringAsync();

                if (rawJson.Contains("\"error\""))
                {
                    var errorObj = JsonSerializer.Deserialize<DeezerErrorResponse>(rawJson);

                    if (errorObj?.Error != null)
                    {
                        return new DeezerAlbumResult
                        {
                            ResultStatus = DeezerResultStatusEnum.DeezerErrorPayload
                        };
                    }
                }

                DeezerSearchResult? deezerSearchResult = new();
                try
                {
                    deezerSearchResult = await response.Content.ReadFromJsonAsync<DeezerSearchResult>();
                }
                catch
                {
                    return new DeezerAlbumResult{ ResultStatus = DeezerResultStatusEnum.InvalidJson };
                }

                if(deezerSearchResult?.Data == null || deezerSearchResult.Data.Count == 0)
                {
                    return new DeezerAlbumResult { ResultStatus = DeezerResultStatusEnum.NotFound };
                }

                // Search album by ID below

                var albumId = deezerSearchResult.Data[0].Id;

                var albumResponse = await _http.GetAsync($"album/{albumId}");

                if(!albumResponse.IsSuccessStatusCode)
                {

                    var status = albumResponse.StatusCode switch
                    {
                        System.Net.HttpStatusCode.NotFound => DeezerResultStatusEnum.NotFound,
                        System.Net.HttpStatusCode.InternalServerError => DeezerResultStatusEnum.ServerError,
                        System.Net.HttpStatusCode.ServiceUnavailable => DeezerResultStatusEnum.ServerError,
                        System.Net.HttpStatusCode.TooManyRequests => DeezerResultStatusEnum.ServerError,
                        _ => DeezerResultStatusEnum.ServerError
                    };
                    return new DeezerAlbumResult { ResultStatus = status };
                }

                var albumRawJson = await albumResponse.Content.ReadAsStringAsync();

                if (albumRawJson.Contains("\"error\""))
                {
                    var errorObj = JsonSerializer.Deserialize<DeezerErrorResponse>(albumRawJson);

                    if (errorObj?.Error != null)
                    {
                        return new DeezerAlbumResult
                        {
                            ResultStatus = DeezerResultStatusEnum.DeezerErrorPayload
                        };
                    }
                }
    
                DeezerAlbumDetails? album = new();

                try
                {
                    album = await albumResponse.Content.ReadFromJsonAsync<DeezerAlbumDetails>();
                }
                catch
                {
                    return new DeezerAlbumResult { ResultStatus = DeezerResultStatusEnum.InvalidJson };
                }

                return new DeezerAlbumResult { ResultStatus = DeezerResultStatusEnum.Success, Album = album};
            }
            
            catch
            {
                return new DeezerAlbumResult { ResultStatus = DeezerResultStatusEnum.NetworkError };
            }
        }
    }


    public class DeezerAlbumResult
    {
        public DeezerResultStatusEnum ResultStatus { get; set; }
        public DeezerAlbumDetails? Album { get; set; }
    }

    public class DeezerSearchResult
    {
        public List<DeezerAlbumSearchItem> Data { get; set; } = new();

    }

    public class DeezerAlbumSearchItem
    {
        public long Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public DeezerArtist Artist { get; set; } = new();
        public string Cover { get; set; } = String.Empty;
        public string Cover_Small { get; set; } = String.Empty;
        public string Cover_Medium { get; set; } = String.Empty;
        public string Cover_Big { get; set; } = String.Empty;
        public string Cover_Xl { get; set; } = String.Empty;
    }

    public class DeezerArtist
    {
        public string Name { get; set; } = String.Empty;
    }

    public class DeezerAlbumDetails
    {
        public long Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public DeezerArtist Artist { get; set; } = new();
        public string Release_Date { get; set; } = String.Empty;
        public DeezerGenreContainer Genres { get; set; } = new();
        public string Cover { get; set; } = String.Empty;
        public string Cover_small { get; set; } = String.Empty;
        public string Cover_medium { get; set; } = String.Empty;
        public string Cover_big { get; set; } = String.Empty;
        public string Cover_xl { get; set; } = String.Empty;
        public int Fans { get; set; }
        public DeezerResultStatusEnum ResultStatus { get; set; }

    }

    public class DeezerGenreContainer
    {
        public List<DeezerGenre> Data { get; set; } = new();
    }

    public class DeezerGenre
    {
        public string Name { get; set; } = String.Empty;
    }

    public class DeezerErrorResponse
    {
        [JsonPropertyName("error")]
        public DeezerErrorDetail? Error { get; set; }
    }

    public class DeezerErrorDetail
    {
        public string Type { get; set; } = "";
        public string Message { get; set; } = "";
        public int Code { get; set; }
    }

}
