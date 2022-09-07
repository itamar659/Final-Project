using Client.Models;
using Client.Models.Responses;
using Client.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Client;
public class HostHomePageViewModel : BaseViewModel
{
    private readonly IServerApi _serverApi;
    private readonly System.Timers.Timer _songTimer;
    private readonly System.Timers.Timer _updateTimer;

    public Poll Poll { get; set; }

    public ObservableCollection<PollOption> SongsToPick => Poll.PollOptions;

    private bool _canVote;

    public bool CanVote
    {
        get { return _canVote; }
        set
        {
            _canVote = value;
            OnPropertyChanged(nameof(CanVote));
        }
    }


    private double _duration;
    public double Duration
    {
        get => _duration + double.Epsilon;
        set
        {
            _duration = value;
            OnPropertyChanged(nameof(Duration));
        }
    }

    private double _position;
    public double Position
    {
        get { return _position; }
        set
        {
            _position = value;
            OnPropertyChanged(nameof(Position));
        }
    }

    private string _songName;
    public string SongName
    {
        get { return _songName; }
        set
        {
            _songName = value;
            OnPropertyChanged(nameof(SongName));
        }
    }

    private string _hostname;
    public string HostName
    {
        get { return _hostname; }
        set { _hostname = value; }
    }

    public ICommand LeaveSessionCommand { get; set; }

    public ICommand ChooseSongCommand { get; set; }

    public HostHomePageViewModel(IServerApi serverApi)
    {
        CanVote = true;
        _serverApi = serverApi;
        _songTimer = new System.Timers.Timer(500);
        _songTimer.Elapsed += songWorker;
        _updateTimer = new System.Timers.Timer(1000);
        _updateTimer.Elapsed += updateFetcher;
        _updateTimer.Start();

        ChooseSongCommand = new Command(async (object val) =>
        {
            var isVoted = await Poll.VoteAsync((int)val);
            CanVote = !isVoted;
            await fetchSessionDetailsAsync();
        });

        Poll = new Poll(serverApi);
    }

    private async Task fetchSessionDetailsAsync()
    {
        JukeboxSessionResponse details = await _serverApi.FetchSessionDetailsAsync();
        if (details == null)
            return; // Session ended. Return to last page...

        SongName = details.SongName;
        Duration = TimeSpan.FromMilliseconds(details.SongDuration).TotalSeconds;
        Position = TimeSpan.FromMilliseconds(details.SongPosition).TotalSeconds;
        HostName = details.OwnerName;

        await Poll.UpdateVotesAsync();
    }

    private async void updateFetcher(object sender, System.Timers.ElapsedEventArgs e)
    {
        JukeboxSessionResponse details = await _serverApi.FetchSessionDetailsAsync();
        if (details == null)
            return; // Session ended. Return to last page...

        SongName = details.SongName;
        Duration = TimeSpan.FromMilliseconds(details.SongDuration).TotalSeconds;
        Position = TimeSpan.FromMilliseconds(details.SongPosition).TotalSeconds;
        HostName = details.OwnerName;

        if (details.IsPaused)
            _songTimer.Stop();
        else if (!_songTimer.Enabled)
            _songTimer.Start();

        var isChanged = await Poll.UpdateVotesAsync();
        if (isChanged)
            CanVote = true;
    }

    private void songWorker(object sender, System.Timers.ElapsedEventArgs e)
    {
        Position += 0.5;
    }

    public async Task LeaveSessionAsync()
    {
        _songTimer.Stop();
        _updateTimer.Stop();
        await _serverApi.LeaveSessionAsync();
        await Shell.Current.GoToAsync("..");
    }
}
