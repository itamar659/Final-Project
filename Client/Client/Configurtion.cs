namespace Client;
public static class Configuration
{
    public static string DevelopServer => DeviceInfo.Platform == DevicePlatform.WinUI
        ? "http://localhost:5038" : "http://10.0.2.2:5038";

    public static string ProductionServer => "https://csharp-project.azurewebsites.net/jukeboxhosts";

    public static string Token
    {
        get => Preferences.Get("Token", default(string));
        set { Preferences.Set("Token", value); }
    }

    public static bool HasToken => Preferences.ContainsKey("Token");
}
