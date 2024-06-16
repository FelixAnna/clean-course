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

            MigrateDatabase(builder);

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        private static void MigrateDatabase(MauiAppBuilder builder)
        {
            var dbContext = builder.Services.BuildServiceProvider().GetService<AbstractCourseContext>()!;
            dbContext.Database.Migrate();
        }

        private static string GetJsonConfigFile()
        {
            var configFileName = Path.Combine(FileSystem.Current.AppDataDirectory, "appsettings.json");
            if (!File.Exists(configFileName))
            {
                var stream = Task.Run(() => FileSystem.Current.OpenAppPackageFileAsync("appsettings.json")).Result;
                using FileStream outputStream = File.Create(configFileName);
                stream.CopyTo(outputStream);
            }
            return configFileName;
        }
    }
}
