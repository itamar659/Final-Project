using Host.Models;
using Host.Models.Requests;
using Host.Models.Responses;
using Host.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Host;
public class MainPageViewModel : BaseViewModel
{
    public static readonly TimeSpan SERVER_UPDATE_DELAY = TimeSpan.FromMilliseconds(1000);

    private IServerApi _serverAPI;
    private AudioPlayer _audioPlayer;
    private bool _fetchUpdates;

    public BindableObject View { get; set; }

    public int TotalUsers { get; set; }

    public int ActiveUsers { get; set; }

    public bool IsSessionLive { get; set; } // not in use

    public string SessionPinCode { get; set; }

    public TimeSpan SessionTime { get; set; } // not in use

    public ICommand StartStopSessionCommand { get; set; }
    public ICommand UpdateSongCommand { get; set; } // not in use
    public ICommand ChangeSessionPinCodeCommand { get; set; }
    public ICommand CreatePollCommand { get; set; }
    public ICommand RemovePollCommand { get; set; }

    public ObservableCollection<PollOption> PollOptions { get; set; }

    public MainPageViewModel(IServerApi serverAPI, AudioPlayer audioPlayer)
    {
        _serverAPI = serverAPI;
        _audioPlayer = audioPlayer;

        PollOptions = new ObservableCollection<PollOption>();

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

        UpdateSongCommand = new Command(async () =>
        {
            //await _serverAPI.UpdateSongAsync(new SongUpdateRequest { SongName = SongName, Duration = Duration, Position = Position});
        });

        ChangeSessionPinCodeCommand = new Command(async () =>
        {
            await _serverAPI.ChangeSessionPinCodeAsync(SessionPinCode);
        });

        CreatePollCommand = new Command(async () =>
        {
            await _serverAPI.CreatePollAsync();
        });

        RemovePollCommand = new Command(async () =>
        {
            await _serverAPI.RemovePollAsync();
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

        PollResponse pollResponse = await _serverAPI.FetchPollAsync();
        if (pollResponse != null)
        {
            PollOptions.Clear();
            foreach (var item in pollResponse.Options)
                PollOptions.Add(item);
        }

        TotalUsers = sessionResponse.TotalUsers;
        ActiveUsers = sessionResponse.ActiveUsers;

        OnPropertyChanged(nameof(TotalUsers));
        OnPropertyChanged(nameof(ActiveUsers));

        if (_fetchUpdates)
            View.Dispatcher.DispatchDelayed(SERVER_UPDATE_DELAY, FetchViewUpdateAsync);

        await _serverAPI.UpdateSongAsync(new SongUpdateRequest { SongName = _audioPlayer.SongName, Duration = _audioPlayer.Duration, Position = _audioPlayer.Position });
    }
}
