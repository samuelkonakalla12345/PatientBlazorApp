namespace MedUnify.HealthPulseBlazor.Pages.Patients
{
    using MedUnify.HealthPulseBlazor.Services;
    using MedUnify.ResourceModel;
    using MedUnify.ResourceModel.HealthPulse;
    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;
    using System.Text.Json;

    public class BasePatients : ComponentBase
    {
        [Inject]
        protected PatientAPIService PatientService { get; set; }

        [Inject]
        protected IJSRuntime _jsRuntime { get; set; }

        protected List<PatientRM> patients;

        protected bool showDeletePatientConfirmDialog;
        protected int patientIdToDelete;
        protected int _expandedPatientId = -1;

        protected async Task GetAllPatientsAsync()
        {
            patients = await PatientService.GetPatientsAsync();
        }

        #region Delete Patient

        protected void ConfirmDelete(int patientId)
        {
            patientIdToDelete = patientId;
            showDeletePatientConfirmDialog = true;
        }

        protected void CloseConfirmationDeleteDialog()
        {
            showDeletePatientConfirmDialog = false;
        }

        protected async Task DeletePatientConfirmed(bool confirmed)
        {
            if (confirmed)
            {
                var response = await PatientService.DeletePatientAsync(patientIdToDelete);

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var returnStatusRM = JsonSerializer.Deserialize<ReturnStatusResourseModel>(body);

                    await this._jsRuntime.InvokeVoidAsync("appScripts.showSweetAlertPopup",
                                     returnStatusRM.Status,
                                     returnStatusRM.Title, returnStatusRM.Message);
                }
                else if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var returnStatusRM = JsonSerializer.Deserialize<ReturnStatusResourseModel>(body);
                    await this._jsRuntime.InvokeVoidAsync("appScripts.showSweetAlertPopup",
                                     returnStatusRM.Status,
                                     returnStatusRM.Title, returnStatusRM.Message);
                }

                await GetAllPatientsAsync();
                StateHasChanged();
            }
        }

        #endregion Delete Patient
    }
}