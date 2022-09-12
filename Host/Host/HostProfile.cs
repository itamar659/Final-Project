using Host.Services;
using Microsoft.Maui.Dispatching;

namespace Host;
public sealed class HostProfile
{
    private static readonly object _lock = new object();
    private static volatile HostProfile _instance = null;

    private HostProfile()
    {
        Hub = new HubService();
        Dispatcher.GetForCurrentThread()?.Dispatch(async () => await Hub.StartAsync());
    }

    public static HostProfile Instance
    {
        get
        {
            if (_instance is null)
            {
                lock (_lock)
                {
                    _instance ??= new HostProfile();
                }
            }

            return _instance;
        }
    }

    public HubService Hub { get; set; }
}
