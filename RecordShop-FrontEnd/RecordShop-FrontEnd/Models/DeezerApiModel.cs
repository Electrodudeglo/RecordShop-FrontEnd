using System.Text.Json.Serialization;

namespace RecordShop_FrontEnd.Models
{
    public enum DeezerResultStatusEnum
    {
        Success,
        NotFound,
        ServerError,
        NetworkError,
        InvalidJson,
        DeezerErrorPayload,
        AuthError
    }

    public class DeezerApiModel
    {
    }

    public class DeezerCheckRequest
    {
        public DeezerCheckRequest() { }
        public DeezerCheckRequest(string albumName, string artistName)
        {
            AlbumName = albumName;
            ArtistName = artistName;
        }
        public string AlbumName { get; set; } = String.Empty;
        public string ArtistName { get; set; } = String.Empty;
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
