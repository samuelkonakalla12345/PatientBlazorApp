namespace MedUnify.HealthPulseBlazor.Services
{
    using MedUnify.ResourceModel.Auth;
    using System.Net.Http.Json;

    public class AuthAPIService
    {
        private readonly HttpClient _httpClient;

        public AuthAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TokenResponseResourceModel> GetTokenAsync(TokenRequestResourceModel requestModel)
        {
            var response = await _httpClient.PostAsJsonAsync("Auth/GetToken", requestModel);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TokenResponseResourceModel>();
        }
    }
}