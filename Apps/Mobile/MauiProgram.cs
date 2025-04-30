using Microsoft.Extensions.Logging;
using Mobile.Source;
using MudBlazor.Services;

namespace Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMudServices();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri("https://api20250228131943.azurewebsites.net/"),
            });

            builder.Services.AddSingleton(Preferences.Default);

            builder.Services.AddSingleton<SharedState>();
            return builder.Build();
        }
    }
}
