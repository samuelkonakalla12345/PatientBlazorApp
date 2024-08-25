namespace MedUnify.HealthPulseBlazor.Pages.Auth
{
    using Microsoft.AspNetCore.Components;

    public class LoginBase : ComponentBase
    {
        [Inject]
        protected NavigationManager Navigation { get; set; }

        protected bool IsBusy { get; set; }

        protected void SetBusy(bool busy)
        {
            IsBusy = busy;
            StateHasChanged();
        }
    }
}