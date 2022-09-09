using Host.Services;

namespace Host.Models;
public class Room : BaseViewModel
{
    public const double _delay = 500;

    private System.Timers.Timer _updateLiveTimeTimer;
    private IServerApi _serverAPI;

    private string _hostname;
    private int _onlineUsers;
    private bool _isOpen;
    private string _pinCode;
    private DateTime _openingTime;

    public Room(IServerApi serverApi)
    {
        _serverAPI = serverApi;

        _updateLiveTimeTimer = new System.Timers.Timer(_delay);
        _updateLiveTimeTimer.Elapsed += (s, e) => OnPropertyChanged(nameof(LiveTime));
        _updateLiveTimeTimer.Start();
    }

    public string Hostname
    {
        get { return _hostname; }
        set
        {
            _hostname = value;
            OnPropertyChanged(nameof(Hostname));
        }
    }

    public int OnlineUsers
    {
        get { return _onlineUsers; }
        set
        {
            _onlineUsers = value;
            OnPropertyChanged(nameof(OnlineUsers));
        }
    }

    public bool IsOpen
    {
        get { return _isOpen; }
        set
        {
            _isOpen = value;
            OnPropertyChanged(nameof(IsOpen));
        }
    }

    public string PinCode
    {
        get { return _pinCode; }
        set
        {
            _pinCode = value;
            OnPropertyChanged(nameof(PinCode));
        }
    }

    public DateTime OpeningTime
    {
        get { return _openingTime; }
        set
        {
            _openingTime = value;
            OnPropertyChanged(nameof(OpeningTime));
            OnPropertyChanged(nameof(LiveTime));
        }
    }

    public TimeSpan LiveTime => _openingTime != DateTime.MinValue ? DateTime.Now - OpeningTime : TimeSpan.Zero;

    public string RoomId { get; private set; }

    public async Task OpenRoomAsync()
    {
        RoomId = await _serverAPI.OpenRoomAsync();

        OnlineUsers = 0;
        OpeningTime = DateTime.Now;
        IsOpen = true;
    }

    public async Task CloseRoomAsync()
    {
        await _serverAPI.CloseRoomAsync();

        OnlineUsers = 0;
        OpeningTime = DateTime.MinValue;
        IsOpen = false;
    }

    public void UpdateRoom(HostProfile profile)
    {
        OnlineUsers = OnlineUsers;
        Hostname = profile.Hostname;

        //TODO: update and display:

        //Summary
        //Description
        //BannerUrl
        //AvatarUrl

    }
}
