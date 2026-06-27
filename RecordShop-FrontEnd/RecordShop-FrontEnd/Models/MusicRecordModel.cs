using System.Text.Json.Serialization;

namespace RecordShop_FrontEnd.Models
{
    public class MusicRecordModel
    {
        public int Id { get; set; }
        [JsonPropertyName("record_title")]
        public string RecordTitle { get; set; } = String.Empty;
        public string Artists { get; set; } = String.Empty;
        [JsonPropertyName("release_year")]
        public string ReleaseYear { get; set; } = String.Empty;
        public string Genre { get; set; } = String.Empty;
        public int Stock { get; set; }
        public string Cover { get; set; } = String.Empty;

    }



}
