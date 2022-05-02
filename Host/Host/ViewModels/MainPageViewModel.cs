using System.Windows.Input;

namespace Host;
public class MainPageViewModel : BaseViewModel
{
    private static readonly TimeSpan SERVER_UPDATE_DELAY = TimeSpan.FromMilliseconds(1000);
    private IServerAPI _serverAPI;

    public int TotalUsers { get; set; }

    public int ActiveUsers { get; set; }

    public bool IsSessionLive { get; set; }

    public TimeSpan SessionTime { get; set; }

    public ICommand StartStopSessionCommand { get; set; }

    public ICommand UpdateSongCommand { get; set; }

    public ICommand SetSessionPinCodeCommand { get; set; }

    public MainPageViewModel(IServerAPI serverAPI)
    {
        _serverAPI = serverAPI;

        StartStopSessionCommand = new Command(() =>
        {
            if (IsSessionLive)
                _serverAPI.StartSession();
            else
                _serverAPI.StopSession();

            IsSessionLive = !IsSessionLive;
        });
        UpdateSongCommand = new Command(() =>
        {
            _serverAPI.UpdateSong(new { });
        });
        SetSessionPinCodeCommand = new Command(value =>
        {
            if (value is int intval)
                _serverAPI.SetSessionPinCode(intval);
        });

        Device.StartTimer(SERVER_UPDATE_DELAY, fetchUpdate);
    }

    private bool fetchUpdate()
    {
        TotalUsers = _serverAPI.FetchTotalUsers();
        ActiveUsers = _serverAPI.FetchActiveUsers();
        SessionTime = _serverAPI.FetchSessionTime();

        return true;
    }
}
