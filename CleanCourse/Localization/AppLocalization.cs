using System.Globalization;

namespace CleanCourse.Localization;

public sealed class AppLocalization
{
    private const string CulturePreferenceKey = "AppCulture";
    private static readonly string DefaultCultureName = "zh-CN";

    public event Action CultureChanged;

    public string CultureName { get; private set; } = DefaultCultureName;

    public void Initialize()
    {
        var savedCultureName = Preferences.Get(CulturePreferenceKey, DefaultCultureName);
        SetCulture(savedCultureName, persist: false);
    }

    public void SetCulture(string cultureName)
    {
        SetCulture(cultureName, persist: true);
    }

    public void ApplyCurrentCulture()
    {
        var culture = CultureInfo.GetCultureInfo(CultureName);
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
    }

    private void SetCulture(string cultureName, bool persist)
    {
        var normalizedCultureName = cultureName switch
        {
            "en" or "en-US" => "en-US",
            "zh" or "zh-CN" => "zh-CN",
            _ => throw new ArgumentOutOfRangeException(nameof(cultureName), cultureName, "Only Chinese and English are supported.")
        };

        var cultureChanged = CultureName != normalizedCultureName;
        var culture = CultureInfo.GetCultureInfo(normalizedCultureName);
        CultureName = normalizedCultureName;
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        ApplyCurrentCulture();

        if (persist)
        {
            Preferences.Set(CulturePreferenceKey, normalizedCultureName);
        }

        if (cultureChanged)
        {
            CultureChanged?.Invoke();
        }
    }
}
