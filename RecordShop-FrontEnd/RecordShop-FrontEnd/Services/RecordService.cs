namespace RecordShop_FrontEnd.Services
{
    using RecordShop_FrontEnd.Interfaces;
    using RecordShop_FrontEnd.Models;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Reflection.Metadata.Ecma335;

    public class RecordService
{

        private readonly HttpClient _http;
        private readonly AuthService _auth;
        private readonly IToastService _toast;

        public RecordService(HttpClient http, AuthService auth, IToastService toastService)
        {
            _http = http;
            _auth = auth;
            _toast = toastService;
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
            var result = await _http.GetFromJsonAsync<List<MusicRecordModel>>("api/v1/records") ?? null;
            if(result != null)
            {
                return result;
            }
            return new();
        }

        public async Task<MusicRecordModel?> GetById(int id)
        {
            var response = await _http.GetAsync($"api/v1/records/{id}");
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<MusicRecordModel>();
        }

        public async Task<DeezerAlbumResult> CheckDeezer(DeezerCheckRequest request)
        {
            if (!await AttachToken()) return new DeezerAlbumResult { ResultStatus = DeezerResultStatusEnum.AuthError, Album = null};
            var response = await _http.PostAsJsonAsync("api/v1/records/check-deezer",request);
            
            if(!response.IsSuccessStatusCode)
            {
                return new DeezerAlbumResult { ResultStatus = DeezerResultStatusEnum.ServerError, Album = null };
            }

            var result = await response.Content.ReadFromJsonAsync<DeezerAlbumResult>();

            return result ?? new DeezerAlbumResult
            {
                ResultStatus = DeezerResultStatusEnum.InvalidJson,
                Album = null
            }; 
        }
 
        public Task<bool> AddOneRecord(MusicRecordModel record) =>
            SendWrite(() => _http.PostAsJsonAsync("api/v1/records", record), "Record Added");

        public Task<bool> UpdateRecord(int id, MusicRecordModel record) =>
            SendWrite(() => _http.PutAsJsonAsync($"api/v1/records/{id}", record), "Record Changed");

        public Task<bool> DeleteRecord(int id) =>
            SendWrite(() => _http.DeleteAsync($"api/v1/records/{id}"), "Successfully Deleted");

        // Returns true only when the backend confirms the write with a 2xx response
        private async Task<bool> SendWrite(Func<Task<HttpResponseMessage>> request, string successMessage)
        {
            if (!await AttachToken()) { _toast.Show("Unauthorized", ToastEnum.Error); return false; }

            HttpResponseMessage response;
            try
            {
                response = await request();
            }
            catch (Exception e) when (e is HttpRequestException or TaskCanceledException)
            {
                // Backend unreachable or request timed out
                _toast.Show("Could not reach the server. Please try again later.", ToastEnum.Error);
                return false;
            }

            if (response.IsSuccessStatusCode)
            {
                _toast.Show(successMessage, ToastEnum.Success);
                return true;
            }

            var errorMessage = response.StatusCode switch
            {
                HttpStatusCode.Unauthorized => "Session expired, please log in again",
                HttpStatusCode.NotFound => "Record not found",
                _ => "Something went wrong. Please try again."
            };
            _toast.Show(errorMessage, ToastEnum.Error);
            return false;
        }
}

}
