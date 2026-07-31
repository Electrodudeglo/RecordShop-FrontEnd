namespace RecordShop.FrontEnd.Services
{

    using System.Net.Http.Json;
    using System.Web;
    public class DeezerService
    {
        private readonly HttpClient _http = new();
        public DeezerService(HttpClient http)
        {
            _http = http;
            _http.BaseAddress = new Uri("https://api.deezer.com/");
        }


        public async Task<DeezerAlbumDetails?> FindAlbumSync(string title, string artist)
        {
            var query = HttpUtility.UrlEncode($"{title} {artist}");
            var searchUrl = $"search/album?q={query}&limit=1";

            var searchResults = await _http.GetFromJsonAsync<DeezerSearchResult>(searchUrl);

            if (searchResults?.Data == null || searchResults.Data.Count() == 0) return null;

            var albumId = searchResults.Data[0].Id;

            return await _http.GetFromJsonAsync<DeezerAlbumDetails>($"album/{albumId}");     
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
        public string CoverBig { get; set; } = String.Empty;
        public string CoverXl { get; set; } = String.Empty;
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
        public string CoverBig { get; set; } = String.Empty;
        public string CoverXl { get; set; } = String.Empty;
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
