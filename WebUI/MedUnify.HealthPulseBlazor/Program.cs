namespace MedUnify.HealthPulseBlazor
{
    using Blazorise;
    using Blazorise.Bootstrap5;
    using Blazorise.Icons.FontAwesome;
    using MedUnify.HealthPulseBlazor;
    using MedUnify.HealthPulseBlazor.Providers;
    using MedUnify.HealthPulseBlazor.Services;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.AspNetCore.Components.Web;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.Extensions.Configuration;
    using Toolbelt.Blazor.Extensions.DependencyInjection;
    using static System.Net.WebRequestMethods;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddLoadingBarService(options =>
            {
                options.LoadingBarColor = "blue";
                //options.ContainerSelector = "#selector-of-container";
            });

            var http = new HttpClient()
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            };

            builder.Services.AddScoped(sp => http);

            await ConfigureServices(builder, http);

            Console.WriteLine(builder.Configuration["API_URLS:AuthAPIService"]);

            builder.Services.AddHttpClient<AuthAPIService>((sp, client) =>
            {
                client.Timeout = TimeSpan.FromSeconds(Convert.ToInt32(60));

                client.BaseAddress = new Uri(builder.Configuration["API_URLS:AuthAPIService"]);
                client.EnableIntercept(sp);
            });

            builder.Services.AddHttpClient<PatientAPIService>((sp, client) =>
            {
                client.Timeout = TimeSpan.FromSeconds(Convert.ToInt32(60));
                client.BaseAddress = new Uri(builder.Configuration["API_URLS:PatientAPIService"]);
                client.EnableIntercept(sp);
            });
            builder.Services.AddHttpClient<VisitAPIService>((sp, client) =>
            {
                client.Timeout = TimeSpan.FromSeconds(Convert.ToInt32(60));
                client.BaseAddress = new Uri(builder.Configuration["API_URLS:PatientAPIService"]);
                client.EnableIntercept(sp);
            });
            builder.Services.AddHttpClient<ProgressNotesAPIService>((sp, client) =>
            {
                client.Timeout = TimeSpan.FromSeconds(Convert.ToInt32(60));
                client.BaseAddress = new Uri(builder.Configuration["API_URLS:PatientAPIService"]);
                client.EnableIntercept(sp);
            });
            //builder.Services.AddScoped<AuthAPIService>();
            //builder.Services.AddScoped<PatientAPIService>();

            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<TokenAuthStateProvider>();

            builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<TokenAuthStateProvider>());

            builder.UseLoadingBar();

            builder.Services
                .AddBlazorise(options =>
                {
                    options.Immediate = true;
                })
                .AddBootstrap5Providers()
                .AddFontAwesomeIcons();

            await builder.Build().RunAsync();
        }

        public static async Task ConfigureServices(WebAssemblyHostBuilder builder, HttpClient http)
        {
            string appSettingsFile = string.Empty;

#if DEBUG
            appSettingsFile = "appsettings.json";
#else
    appSettingsFile = $"appsettings.Development.json";
    builder.Logging.SetMinimumLevel(LogLevel.Warning);
#endif

            using var response = await http.GetAsync(appSettingsFile);
            using var stream = await response.Content.ReadAsStreamAsync();

            builder.Configuration.AddJsonStream(stream);
        }
    }
}