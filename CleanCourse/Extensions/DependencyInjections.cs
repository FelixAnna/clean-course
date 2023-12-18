using DataAccess;
using Database.SQLite;
using Microsoft.Extensions.Configuration;
using Services;
using Shared;

namespace CleanCourse.Extensions;

public static class DependencyInjections
{
    public static IServiceCollection RegisterAllServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<AppState>();

        var state = services.BuildServiceProvider().GetService<AppState>();
        RepositoryDependencyInjection.Register(services, () => state.DefaultCheckingThreshold, () => state.DefaultRecentThreshold);
        ServiceDependencyInjections.Register(services);

        var courseDBFileName = configuration.GetConnectionString("CourseDB");

        var fileName = Task.Run(() => CopyFileToAppDataDirectory(courseDBFileName)).Result;
        Preferences.Set(Constants.DBFileLocationKey, fileName);

        SQLiteDependencyInjections.Register(services, fileName);
        //SqlServerDependencyInjections.Register(services, configuration);

        return services;
    }

    public static async Task<string> CopyFileToAppDataDirectory(string filename)
    {
        var fullFileName = filename;
        if (Path.IsPathRooted(filename))
        {
            return fullFileName;
        }

        fullFileName = Path.Combine(FileSystem.Current.AppDataDirectory, "Data", filename);
        if (File.Exists(fullFileName))
        {
            return fullFileName;
        }

        Directory.CreateDirectory(fullFileName[..fullFileName.LastIndexOfAny(['\\', '/'])]);
        using Stream inputStream = await FileSystem.Current.OpenAppPackageFileAsync(filename);
        using FileStream outputStream = File.Create(fullFileName);
        await inputStream.CopyToAsync(outputStream);

        return fullFileName;
    }
}
