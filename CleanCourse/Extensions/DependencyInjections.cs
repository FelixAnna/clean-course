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

        RepositoryDependencyInjection.Register(
            services,
            provider => provider.GetRequiredService<AppState>().DefaultCheckingThreshold,
            provider => provider.GetRequiredService<AppState>().DefaultRecentThreshold);
        ServiceDependencyInjections.Register(services);

        var courseDBFileName = configuration.GetConnectionString("CourseDB");
        var fileName = CopyFileToAppDataDirectory(courseDBFileName).GetAwaiter().GetResult();
        Preferences.Set(Constants.DBFileLocationKey, fileName);

        SQLiteDependencyInjections.Register(services, fileName);
        //SqlServerDependencyInjections.Register(services, configuration);

        return services;
    }

    public static async Task<string> CopyFileToAppDataDirectory(string filename)
    {
        if (Path.IsPathRooted(filename))
        {
            return filename;
        }

        var fullFileName = Path.Combine(FileSystem.Current.AppDataDirectory, "Data", filename);
        if (File.Exists(fullFileName))
        {
            return fullFileName;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(fullFileName)!);
        var temporaryFileName = $"{fullFileName}.{Guid.NewGuid():N}.tmp";

        try
        {
            await using Stream inputStream = await FileSystem.Current.OpenAppPackageFileAsync(filename);
            await using (var outputStream = new FileStream(
                temporaryFileName,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 81920,
                useAsync: true))
            {
                await inputStream.CopyToAsync(outputStream);
            }

            try
            {
                // Never replace a database that was created by an earlier install.
                File.Move(temporaryFileName, fullFileName);
            }
            catch (IOException) when (File.Exists(fullFileName))
            {
            }

            return fullFileName;
        }
        finally
        {
            if (File.Exists(temporaryFileName))
            {
                File.Delete(temporaryFileName);
            }
        }
    }
}
