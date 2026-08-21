namespace RecordShop_FrontEnd.Services
{
    using RecordShop_FrontEnd.Interfaces;
    using RecordShop_FrontEnd.Models;
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
 
        public async Task<bool> AddOneRecord(MusicRecordModel record)
        {
            if (!await AttachToken()) return false;
            var response = await _http.PostAsJsonAsync("api/v1/records", record);

            if(response.IsSuccessStatusCode)
            {
                _toast.Show("Record Added", ToastEnum.Success);     
            }
            return true;
        }

        public async Task<bool> UpdateRecord(int id, MusicRecordModel record)
        {
            if (!await AttachToken()) { _toast.Show("Unauthorized", ToastEnum.Error); return false; }
            var response = await _http.PutAsJsonAsync($"api/v1/records/{id}", record);

            if (response.IsSuccessStatusCode)
            {
                _toast.Show("Record Changed", ToastEnum.Success);
            }
            return true;
        }

        public async Task<bool> DeleteRecord(int id)
        {
            if (!await AttachToken()) { _toast.Show("Unauthorized",ToastEnum.Error); return false; }
            var response = await _http.DeleteAsync($"api/v1/records/{id}");
            
            if(response.IsSuccessStatusCode)
            {
                _toast.Show("Successfully Deleted", ToastEnum.Success);
            }
            return true;
        }       
}

}
