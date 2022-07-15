using Host.Services;

namespace Host.Models;
public class Room : BaseViewModel
{
    private IServerApi _serverAPI;

    private int _onlineUsers;
    public int OnlineUsers
    {
        get { return _onlineUsers; }
        set
        {
            _onlineUsers = value;
            OnPropertyChanged(nameof(OnlineUsers));
        }
    }

    private bool _isOpen;
    public bool IsOpen
    {
        get { return _isOpen; }
        set
        {
            _isOpen = value;
            OnPropertyChanged(nameof(IsOpen));
        }
    }

    private string _PinCode;
    public string PinCode
    {
        get { return _PinCode; }
        set
        {
            _PinCode = value;
            OnPropertyChanged(nameof(PinCode));
        }
    }

    private DateTime _openningTime;
    public DateTime OpenningTime
    {
        get { return _openningTime; }
        set
        {
            _openningTime = value;
            OnPropertyChanged(nameof(OpenningTime));
            OnPropertyChanged(nameof(LiveTime));
        }
    }

    public TimeSpan LiveTime => _openningTime != DateTime.MinValue ? DateTime.Now - OpenningTime : TimeSpan.Zero;

    public Room(IServerApi serverApi)
    {
        _serverAPI = serverApi;
    }

    public async Task OpenRoomAsync()
    {
        await _serverAPI.OpenSessionAsync();

        OnlineUsers = 0;
        OpenningTime = DateTime.Now;
        IsOpen = true;
    }

    public async Task CloseRoomAsync()
    {
        await _serverAPI.CloseSessionAsync();

        OnlineUsers = 0;
        OpenningTime = DateTime.MinValue;
        IsOpen = false;
    }

    public async Task ChangePinCodeAsync(string pinCode)
    {
        await _serverAPI.ChangeSessionPinCodeAsync(pinCode);
    }
}
