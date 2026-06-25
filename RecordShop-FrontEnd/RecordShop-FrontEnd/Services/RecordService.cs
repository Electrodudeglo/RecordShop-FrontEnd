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

            
}

}
