using uss_client_gui.ViewModels;
using uss_client_gui.Views;

namespace uss_client_gui
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


            // only one
            builder.Services.AddSingleton<BridgeStatusViewModel>(); // associate
            builder.Services.AddSingleton<MainPage>();

            // transision
            builder.Services.AddTransient<SwitchDashboardView>();
            builder.Services.AddTransient<SwitchDashboardViewModel>();

            // transision
            builder.Services.AddTransient<ModesDashboardView>();
            builder.Services.AddTransient<ModesDashboardViewModel>();

            return builder.Build();
        }
    }
}