using CleanCourse.Extensions;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CleanCourse
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

            
            builder.Configuration.AddJsonFile(GetJsonConfigFile());
            builder.Services.RegisterAllServices(builder.Configuration);

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();
            using var scope = app.Services.CreateScope();
            scope.ServiceProvider.GetRequiredService<AbstractCourseContext>().Database.Migrate();
            return app;
        }

        private static string GetJsonConfigFile()
        {
            var configFileName = Path.Combine(FileSystem.Current.AppDataDirectory, "appsettings.json");
            if (!File.Exists(configFileName))
            {
                using var stream = FileSystem.Current.OpenAppPackageFileAsync("appsettings.json").GetAwaiter().GetResult();
                using FileStream outputStream = File.Create(configFileName);
                stream.CopyTo(outputStream);
            }
            return configFileName;
        }
    }
}
