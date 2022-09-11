namespace Client;
public static class Configuration
{
    private static string DevelopServer => DeviceInfo.Platform == DevicePlatform.WinUI
        ? "http://localhost:5230" : "http://10.0.2.2:5230";

    private static string ProductionServer => "https://csharp-project.azurewebsites.net";

    public static string ServerUrl => ProductionServer;

    public static string Token
    {
        get => Preferences.Get("Token", default(string));
        set { Preferences.Set("Token", value); }
    }

    public static bool HasToken => Preferences.ContainsKey("Token");
}
