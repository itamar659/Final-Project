using Host.Models.Responses;
using Host.Services;
using System.Windows.Input;

namespace Host;
public class MainPageViewModel : BaseViewModel
{
    public static readonly TimeSpan SERVER_UPDATE_DELAY = TimeSpan.FromMilliseconds(1000);

    private IServerApi _serverAPI;
    private bool _fetchUpdates;

    public BindableObject View { get; set; }

    public int TotalUsers { get; set; }

    public int ActiveUsers { get; set; }

    public bool IsSessionLive { get; set; } // not in use

    public int SessionPinCode { get; set; } // not in use

    public TimeSpan SessionTime { get; set; } // not in use

    public ICommand StartStopSessionCommand { get; set; }

    public ICommand UpdateSongCommand { get; set; } // not in use

    public ICommand GenerateSessionPinCodeCommand { get; set; } // not in use

    public MainPageViewModel(IServerApi serverAPI)
    {
        _serverAPI = serverAPI;

        StartStopSessionCommand = new Command(async () =>
        {
            if (!IsSessionLive)
            {
                bool successfulOpen = await _serverAPI.OpenSessionAsync();

                if (successfulOpen)
                    StartFetchUpdates();
            }
            else
            {
                await _serverAPI.CloseSessionAsync();
                StopFetchUpdates();

                TotalUsers = 0;
                ActiveUsers = 0;
                OnPropertyChanged(nameof(TotalUsers));
                OnPropertyChanged(nameof(ActiveUsers));
            }

            IsSessionLive = !IsSessionLive;

            IsSessionLive = _serverAPI.GetSessionKey() != string.Empty;

            OnPropertyChanged(nameof(IsSessionLive));
        });

        UpdateSongCommand = new Command(() =>
        {
            _serverAPI.UpdateSongAsync(new { });
        });

        GenerateSessionPinCodeCommand = new Command(() =>
        {
            int code = generateCode();
            SessionPinCode = code;
            OnPropertyChanged(nameof(SessionPinCode));
            _serverAPI.SetSessionPinCodeAsync(code);
        });
    }

    public void StartFetchUpdates()
    {
        View.Dispatcher.DispatchDelayed(TimeSpan.Zero, FetchViewUpdateAsync);
        _fetchUpdates = true;
    }

    public void StopFetchUpdates()
    {
        _fetchUpdates = false;
    }

    public async void FetchViewUpdateAsync()
    {
        JukeboxSessionResponse sessionResponse = await _serverAPI.FetchSessionUpdateAsync();
        if (sessionResponse == null)
            return;

        TotalUsers = sessionResponse.TotalUsers;
        ActiveUsers = sessionResponse.ActiveUsers;

        OnPropertyChanged(nameof(TotalUsers));
        OnPropertyChanged(nameof(ActiveUsers));

        if (_fetchUpdates)
            View.Dispatcher.DispatchDelayed(SERVER_UPDATE_DELAY, FetchViewUpdateAsync);
    }

    private int generateCode()
    {
        return 1234;
    }
}
