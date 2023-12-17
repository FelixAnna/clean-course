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
            var stream = Task.Run(() => FileSystem.Current.OpenAppPackageFileAsync("appsettings.json")).Result;
            builder.Configuration.AddJsonStream(stream);

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
    }
}
