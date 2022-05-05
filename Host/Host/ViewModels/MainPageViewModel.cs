using Host.Services;
using System.Windows.Input;

namespace Host;
public class MainPageViewModel : BaseViewModel
{
    public static readonly TimeSpan SERVER_UPDATE_DELAY = TimeSpan.FromMilliseconds(1000);
    private IServerApi _serverAPI;

    public int TotalUsers { get; set; }

    public int ActiveUsers { get; set; }

    public bool IsSessionLive { get; set; }

    public int SessionPinCode { get; set; }

    public TimeSpan SessionTime { get; set; }

    public ICommand StartStopSessionCommand { get; set; }

    public ICommand UpdateSongCommand { get; set; }

    public ICommand GenerateSessionPinCodeCommand { get; set; }

    public MainPageViewModel(IServerApi serverAPI)
    {
        _serverAPI = serverAPI;

        StartStopSessionCommand = new Command(() =>
        {
            if (IsSessionLive)
                _serverAPI.StartSession();
            else
                _serverAPI.StopSession();

            IsSessionLive = !IsSessionLive;
            OnPropertyChanged(nameof(IsSessionLive));
        });

        UpdateSongCommand = new Command(() =>
        {
            _serverAPI.UpdateSong(new { });
        });

        GenerateSessionPinCodeCommand = new Command(() =>
        {
            int code = generateCode();
            SessionPinCode = code;
            OnPropertyChanged(nameof(SessionPinCode));
            _serverAPI.SetSessionPinCode(code);
        });
    }

    public void FetchViewUpdate()
    {
        TotalUsers = _serverAPI.FetchTotalUsers();
        ActiveUsers = _serverAPI.FetchActiveUsers();
        SessionTime = _serverAPI.FetchSessionTime();

        OnPropertyChanged(nameof(TotalUsers));
        OnPropertyChanged(nameof(ActiveUsers));
        OnPropertyChanged(nameof(SessionTime));
    }

    private int generateCode()
    {
        return 1234;
    }
}
