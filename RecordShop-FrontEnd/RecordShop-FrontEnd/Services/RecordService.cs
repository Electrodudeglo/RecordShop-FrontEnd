namespace RecordShop_FrontEnd.Services
{
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using RecordShop_FrontEnd.Models;
    public class RecordService
{

        private readonly HttpClient _http;
        private readonly AuthService _auth;

        public RecordService(HttpClient http, AuthService auth)
        {
            _http = http;
            _auth = auth;
        }


        private async Task<bool> AttachToken()
        {
            var token = await _auth.GetToken();
            if (string.IsNullOrWhiteSpace(token)) return false;
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            return true;     
        }

        public async Task<List<MusicRecordModel>> GetAll()
        {
            await AttachToken();
            var result = await _http.GetFromJsonAsync<List<MusicRecordModel>>("ap/musicrecord");
            return result ?? new List<MusicRecordModel>();
        }

        public async Task<bool> AddOneRecord(MusicRecordModel record)
        {
            if (!await AttachToken()) return false;
            var response = await _http.PostAsJsonAsync("api/musicrecord", record);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateRecord(int id, MusicRecordModel record)
        {
            if (!await AttachToken()) return false;
            var response = await _http.PutAsJsonAsync("api/musicrecord", record);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteRecord(int id)
        {
            if (!await AttachToken()) return false;
            var response = await _http.DeleteAsync($"api/musicrecord{id}");
            return response.IsSuccessStatusCode;
        }       
}

}
