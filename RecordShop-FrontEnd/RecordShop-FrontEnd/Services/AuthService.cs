using System.Net;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using RecordShop_FrontEnd.Interfaces;
using RecordShop_FrontEnd.Models;

namespace RecordShop_FrontEnd.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;
        private readonly IToastService _toast;

        private const string TokenKey = "authToken";

        public AuthService(HttpClient http, IJSRuntime js, IToastService toast)
        {
            _http = http;
            _js = js;
            _toast = toast;
        }

        public async Task<LoginResultEnum> Login(LoginRequestModel creds)
        {
            HttpResponseMessage response;
            try
            {
                // Call your backend login endpoint
                response = await _http.PostAsJsonAsync("api/auth/token", creds);
            }
            catch (HttpRequestException)
            {
                // Backend unreachable
                return LoginResultEnum.ServerError;
            }

            if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.BadRequest)
                return LoginResultEnum.InvalidCredentials;

            if (!response.IsSuccessStatusCode)
                return LoginResultEnum.ServerError;

            // Read JSON: { token: "..." }
            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();

            if (result is null || string.IsNullOrWhiteSpace(result.Token))
                return LoginResultEnum.ServerError;

            // Store only the token string
            await _js.InvokeVoidAsync("sessionStorage.setItem", TokenKey, result.Token);

            _toast.Show("Logged in", ToastEnum.Success);

            return LoginResultEnum.Success;
        }

        public async Task<string?> GetToken()
        {
            try
            {
                return await _js.InvokeAsync<string>("sessionStorage.getItem", TokenKey);
            }
            catch
            {
                // During prerendering or if JS isn’t ready
                return null;
            }
        }

        public async Task Logout()
        {
            // Optional: call backend logout (even though JWT logout is stateless)
            await _http.PostAsync("api/auth/logout", null);

            // Remove token from sessionStorage
            await _js.InvokeVoidAsync("sessionStorage.removeItem", TokenKey);

            _toast.Show("Logged out", ToastEnum.Info);
        }
    }

    public class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}
