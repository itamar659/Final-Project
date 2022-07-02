using Host.Models;
using Host.Models.Requests;
using Host.Models.Responses;
using Host.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Host;
public class MainPageViewModel : BaseViewModel
{
    public static readonly double SERVER_UPDATE_DELAY = TimeSpan.FromSeconds(10).TotalMilliseconds;

    private IServerApi _serverAPI;
    private AudioPlayer _audioPlayer;
    private System.Timers.Timer _updateTimer;

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
        _audioPlayer.SongStateChanged += updateSongAsync;
        _updateTimer = new System.Timers.Timer(SERVER_UPDATE_DELAY);
        _updateTimer.Elapsed += fetchViewUpdateAsync;

        PollOptions = new ObservableCollection<PollOption>();

        StartStopSessionCommand = new Command(startStopSession);
        ChangeSessionPinCodeCommand = new Command(async () => await _serverAPI.ChangeSessionPinCodeAsync(SessionPinCode));
        CreatePollCommand = new Command(async () => await _serverAPI.CreatePollAsync());
        RemovePollCommand = new Command(async () => await _serverAPI.RemovePollAsync());
    }

    private async void fetchViewUpdateAsync(object sender, System.Timers.ElapsedEventArgs e)
    {
        if (IsSessionLive == false)
            return;

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
    }

    private async void updateSongAsync(object sender, EventArgs e)
    {
        await _serverAPI.UpdateSongAsync(new SongUpdateRequest { SongName = _audioPlayer.SongName, Duration = _audioPlayer.Duration, Position = _audioPlayer.Position });
    }

    private async void startStopSession()
    {
        if (!IsSessionLive)
        {
            await _serverAPI.OpenSessionAsync();
        }
        else
        {
            await _serverAPI.CloseSessionAsync();

            TotalUsers = 0;
            ActiveUsers = 0;
            OnPropertyChanged(nameof(TotalUsers));
            OnPropertyChanged(nameof(ActiveUsers));
        }

        IsSessionLive = _serverAPI.GetSessionKey() != string.Empty;

        OnPropertyChanged(nameof(IsSessionLive));
    }
}
