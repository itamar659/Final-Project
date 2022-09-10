using Host.Models.ServerMessages;
using Host.Services;

namespace Host.Models;
public class HostRoom : BaseViewModel
{
    public const double _delay = 500;

    private System.Timers.Timer _updateLiveTimeTimer;
    private IServerApi _serverAPI;

    private string _hostname;
    private int _onlineUsers;
    private bool _isOpen;
    private string _pinCode;
    private DateTime _openingTime;

    public HostRoom(IServerApi serverApi)
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

    public string LiveTime => (_openingTime != DateTime.MinValue ? DateTime.Now - OpeningTime : TimeSpan.Zero).ToString(@"hh\:mm\:ss");

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

    public async Task UpdateRoom()
    {
        //TODO: update and display:
        var room = await _serverAPI.FetchHostProfileAsync(Configuration.Token);
        if (room == null)
            return;

        Hostname = room.Hostname;
        PinCode = room.PinCode;
        OnlineUsers = room.OnlineUsers;

        var Summary = room.Summary;
        //Description = profile.Desc...
        //BannerUrl
        //AvatarUrl
    }

    public async Task updateServer(SongMessage song)
    {
        await _serverAPI.UpdateSongAsync(song);
    }
}
