using Client.Services;
using Microsoft.Maui.Dispatching;

namespace Client;
public sealed class UserProfile
{
    private static readonly object _lock = new object();
    private static volatile UserProfile _instance = null;

    private UserProfile()
    {
        Hub = new HubService();
        Dispatcher.GetForCurrentThread()?.Dispatch(async () => await Hub.StartAsync());
    }

    public static UserProfile Instance
    {
        get
        {
            if (_instance is null)
            {
                lock(_lock)
                {
                    _instance ??= new UserProfile();
                }
            }

            return _instance;
        }
    }

    public HubService Hub { get; set; }

    public string Token { get; set; }

    public string Username { get; set; }

    public string RoomId { get; set; } = null;

    public string AvatarUrl { get; set; }
}
