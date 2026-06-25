namespace RecordShop_FrontEnd.Services
{

    using System.Net.Http.Json;
    using Microsoft.JSInterop;
    using RecordShop_FrontEnd.Models;

    public class AuthService {

        private readonly HttpClient _http;
        private readonly IJSRuntime _js;

        private const string TokenKey = "authToken";


        public AuthService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;
        }


        public async Task<bool> Login(LoginRequestModel creds)
        {
            var response = await _http.PostAsJsonAsync("auth/login",creds);
            if (!response.IsSuccessStatusCode) return false;
            var token = await response.Content.ReadAsStringAsync();
            await _js.InvokeVoidAsync("sessionStorage.setItem", TokenKey, token);
            return true;
        }

        public async Task<string?> GetToken()
        {
            return await _js.InvokeAsync<string>("sessionStorage.getItem", TokenKey);
        }

        public async Task Logout()
        {
            await _js.InvokeVoidAsync("sessionStorage.removeItem", TokenKey);
        }
    }
}