using MedUnify.HealthPulseBlazor.Services;

namespace MedUnify.HealthPulseBlazor.Providers
{
    using MedUnify.ResourceModel.Auth;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.Extensions.Logging;
    using Microsoft.JSInterop;
    using System.Net;
    using System.Security.Claims;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class TokenAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly ILogger<TokenAuthStateProvider> _logger;
        private readonly NavigationManager _navManager;

        private readonly AuthAPIService _authAPIService;

        public event Action OnAuthStateChanged;

        public TokenAuthStateProvider(IJSRuntime jsRuntime, NavigationManager navigationManager, AuthAPIService authAPIService, ILogger<TokenAuthStateProvider> logger)
        {
            _jsRuntime = jsRuntime;
            _navManager = navigationManager;
            _authAPIService = authAPIService;
            _logger = logger;
        }

        public async Task<bool> LoginAsync(string clientId, string clientSecret)
        {
            var requestModel = new TokenRequestResourceModel
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            };

            var response = await _authAPIService.GetTokenAsync(requestModel);

            if (response != null)
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", response.AuthToken);
                OnAuthStateChanged?.Invoke();
                return true;
            }

            return false;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            return !string.IsNullOrEmpty(token);
        }

        public async Task LogoutAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
            OnAuthStateChanged?.Invoke();
        }

        public async Task SetTokenAsync(string token, DateTime expiry = default)
        {
            if (token == null)
            {
                await _jsRuntime.InvokeAsync<object>("localStorage.removeItem", "authToken");
                await _jsRuntime.InvokeAsync<object>("localStorage.removeItem", "authTokenExpiry");

                await _jsRuntime.InvokeVoidAsync("sharedController.deleteAllLocalStorage");
                await _jsRuntime.InvokeVoidAsync("sharedController.deleteAllCookies");
            }
            else
            {
                await _jsRuntime.InvokeAsync<object>("localStorage.setItem", "authToken", token);
                await _jsRuntime.InvokeAsync<object>("localStorage.setItem", "authTokenExpiry", expiry);
            }

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<string> GetTokenAsync()
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        }

        public async Task<OAuthClientRM> GetUserClaimsAsync()
        {
            var authenticationState = await this.GetAuthenticationStateAsync();

            OAuthClientRM oAuthClient = new OAuthClientRM();

            foreach (var item in authenticationState.User.Claims)
            {
                if (item.Type.ToLower().Equals("unique_name"))
                {
                    oAuthClient.ClientId = item.Value;
                }

                if (item.Type.ToLower().Contains("organizationid"))
                {
                    oAuthClient.OrganizationId = Convert.ToInt32(item.Value);
                }

                if (item.Type.ToLower().Contains("organizationname"))
                {
                    oAuthClient.OrganizationName = item.Value;
                }
            }

            return oAuthClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await GetTokenAsync(); // Retrieve token from local storage or cookies
            var identity = string.IsNullOrEmpty(token)
                ? new ClaimsIdentity()
                : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

            // Log all claims for debugging
            //foreach (var claim in identity.Claims)
            //{
            //    Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            //}

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        public async Task ValidateUserAuthenticationForPageAsync()
        {
            var authenticationState = await GetAuthenticationStateAsync();

            if (authenticationState?.User?.Identity is null || !authenticationState.User.Identity.IsAuthenticated)
            {
                await _jsRuntime.InvokeVoidAsync("sharedController.redirectTo", "signin");
            }
        }

        public async Task ValidateAuthRequestAsync(HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage.StatusCode == HttpStatusCode.UpgradeRequired)
            {
                _navManager.NavigateTo("updateRequired");
            }
            else if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                await _jsRuntime.InvokeVoidAsync("sharedController.showSweetToastNotification", "warning", "Session expired. Redirecting to login page.", 3500);
                await _jsRuntime.InvokeVoidAsync("sharedController.redirectTo", "signin");
            }
            else if (httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
            {
                await _jsRuntime.InvokeVoidAsync("sharedController.handleForbiddenHttpError");
            }
            else if (httpResponseMessage.StatusCode == HttpStatusCode.InternalServerError)
            {
                string requestDetail = string.Empty;
                string requestId = string.Empty;
                string requestDateTime = string.Empty;

                foreach (var item in httpResponseMessage.Headers)
                {
                    if (item.Key.ToLower() == "x-request-id")
                    {
                        requestId = item.Value.FirstOrDefault();
                    }
                    else if (item.Key.ToLower() == "x-server-datetime")
                    {
                        requestDateTime = item.Value.FirstOrDefault();
                    }
                }

                await _jsRuntime.InvokeVoidAsync("sharedController.handleHttpInternalServerError", requestId, requestDateTime, string.Empty);
            }
        }

        //public async Task ValidateUnAuthRequestAsync(HttpResponseMessage httpResponseMessage)
        //{
        //    if (httpResponseMessage.StatusCode == HttpStatusCode.UpgradeRequired)
        //    {
        //        _navManager.NavigateTo("updateRequired");
        //    }
        //    else if (httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
        //    {
        //        await this._jsRuntime.InvokeVoidAsync("sharedController.handleForbiddenHttpError");
        //    }
        //    else if (httpResponseMessage.StatusCode == HttpStatusCode.InternalServerError)
        //    {
        //        string requestDetail = string.Empty;
        //        string requestId = string.Empty;
        //        string requestDateTime = string.Empty;

        //        foreach (var item in httpResponseMessage.Headers)
        //        {
        //            if (item.Key.ToLower() == "x-request-id")
        //            {
        //                requestId = item.Value.FirstOrDefault();
        //            }
        //            else if (item.Key.ToLower() == "x-server-datetime")
        //            {
        //                requestDateTime = item.Value.FirstOrDefault();
        //            }
        //        }

        //        await this._jsRuntime.InvokeVoidAsync("sharedController.handleHttpInternalServerError", requestId, requestDateTime, string.Empty);
        //    }
        //}
    }
}