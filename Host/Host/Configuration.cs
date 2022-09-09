using Host.Models;

namespace Host;
public static class Configuration
{
    public static string ServerBaseUrl => DeviceInfo.Platform == DevicePlatform.WinUI
        ? "http://localhost:5038" : "http://10.0.2.2:5038";

    public static HostProfile Profile { get; set; }
}
