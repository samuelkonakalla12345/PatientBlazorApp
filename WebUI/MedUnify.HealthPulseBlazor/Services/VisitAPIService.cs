using MedUnify.Domain.HealthPulse;
using MedUnify.HealthPulseBlazor.Pages.Patients;
using MedUnify.HealthPulseBlazor.Providers;
using MedUnify.ResourceModel.HealthPulse;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace MedUnify.HealthPulseBlazor.Services
{
    public class VisitAPIService
    {
        private readonly TokenAuthStateProvider _tokenAuthStateProvider;

        private readonly HttpClient _httpClient;
        private List<Visit> visits = new();

        public VisitAPIService(HttpClient httpClient, TokenAuthStateProvider tokenAuthStateProvider)
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

        public async Task<List<Visit>> GetVisitsByPatientIdAsync(int patientId)
        {
            await AddAuthorizationHeaderAsync();
            return await _httpClient.GetFromJsonAsync<List<Visit>>("Visits/patient/"+ patientId);
        }

        public async Task<HttpResponseMessage> AddVisitAsync(Visit vis)
        {
            try
            {
                await AddAuthorizationHeaderAsync();
                var response = await _httpClient.PostAsJsonAsync($"Visits/AddVisit", vis);
                return response;

            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
        public async Task<PatientRM> GetPatientByIdAsync(int patientId)
        {
            await AddAuthorizationHeaderAsync();
            return await _httpClient.GetFromJsonAsync<PatientRM>($"Patients/GetPatient?patientId={patientId}");
        }


        

        public void AddProgressNoteToVisit(int visitId, ProgressNote progressNote)
        {
            var visit = visits.FirstOrDefault(v => v.VisitId == visitId);
            if (visit != null)
            {
                visit.ProgressNotes ??= new List<ProgressNote> { progressNote };
            }
        }
    }
}
