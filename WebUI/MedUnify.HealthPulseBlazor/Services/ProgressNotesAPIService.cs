using MedUnify.Domain.HealthPulse;
using MedUnify.HealthPulseBlazor.Pages.Patients;
using MedUnify.HealthPulseBlazor.Providers;
using MedUnify.ResourceModel.HealthPulse;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MedUnify.HealthPulseBlazor.Services
{
    public class ProgressNotesAPIService
    {
        private readonly TokenAuthStateProvider _tokenAuthStateProvider;

        private readonly HttpClient _httpClient;
        private List<Visit> visits = new();

        public ProgressNotesAPIService(HttpClient httpClient, TokenAuthStateProvider tokenAuthStateProvider)
        {
            _tokenAuthStateProvider = tokenAuthStateProvider;
            _httpClient = httpClient;
        }

        private async Task AddAuthorizationHeaderAsync()
        {
            // Assume you have a method to retrieve the token
            string token = await _tokenAuthStateProvider.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        public async Task<HttpResponseMessage> AddProgressNotesAsync(ProgressNote progressNote)
        {
                await AddAuthorizationHeaderAsync();
                var response = await _httpClient.PostAsJsonAsync($"ProgressNotes", progressNote);
                return response;
        }
        public async Task<List<ProgressNote>> GetProgressnotesByVisitIdAsync(int visitid)
        {
            await AddAuthorizationHeaderAsync();
            return await _httpClient.GetFromJsonAsync<List<ProgressNote>>($"ProgressNotes/visit/" + visitid);
        }
    }
}
