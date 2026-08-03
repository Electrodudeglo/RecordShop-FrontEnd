namespace RecordShop_FrontEnd.Services
{
    using System.Net.Http.Json;
    using System.Web;
   
    
    public class DeezerService
    {
        private readonly HttpClient _http = new();
        public DeezerService(HttpClient http)
        {
            _http = http;
        }


        public async Task<DeezerAlbumDetails?> FindAlbumSync(string title, string artist)
        {
            try
            {
                var query = HttpUtility.UrlEncode($"{title} {artist}");
                var searchUrl = $"search/album?q={query}&limit=1";

                var response = await _http.GetAsync(searchUrl);

                if (!response.IsSuccessStatusCode)
                    return null; // handles 404, 500, etc.

                var searchResults = await response.Content.ReadFromJsonAsync<DeezerSearchResult>();

                if (searchResults?.Data == null || searchResults.Data.Count == 0)
                    return null;

                var albumId = searchResults.Data[0].Id;

                var albumResponse = await _http.GetAsync($"album/{albumId}");

                if (!albumResponse.IsSuccessStatusCode)
                    return null; // handles 404 on album lookup

                return await albumResponse.Content.ReadFromJsonAsync<DeezerAlbumDetails>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Deezer error: {ex.Message}");
                return null;
            }
        }

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

    }

    public class DeezerGenreContainer
    {
        public List<DeezerGenre> Data { get; set; } = new();
    }

    public class DeezerGenre
    {
        public string Name { get; set; } = String.Empty;
    }


}
