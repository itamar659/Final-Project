using Client.Models.Responses;
using Client.Services;

namespace Client;
public class HostHomePageViewModel : BaseViewModel
{
    private readonly IServerApi _serverApi;
    private readonly System.Timers.Timer _songTimer;


    private double _duration;
    public double Duration
    {
        get { return _duration + double.Epsilon; }
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

    public HostHomePageViewModel(IServerApi serverApi)
    {
        _serverApi = serverApi;
        _songTimer = new System.Timers.Timer(500);
        _songTimer.Elapsed += songWorker;

        fetchSessionDetailsAsync();

        _songTimer.Start();
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
    }

    private async void songWorker(object sender, System.Timers.ElapsedEventArgs e)
    {
        Position += 0.5;
        if (Position >= Duration)
        {
            _songTimer.Stop();
            await fetchSessionDetailsAsync();
            _songTimer.Start();
        }
    }
}
