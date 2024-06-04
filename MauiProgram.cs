using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;

namespace RinconLosCuatroTPVDesktop
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
                    fonts.AddFont("materialdesignicons-webfont.ttf", "MaterialDesignIcons");
                });

#if DEBUG
    		builder.Logging.AddDebug();
            builder.UseMauiApp<App>().ConfigureSyncfusionCore();
#endif

            return builder.Build();
        }
    }
}
