using Client.Models;
using Client.Models.Responses;
using Client.Services;
using System.Windows.Input;

namespace Client;
public class HostHomePageViewModel : BaseViewModel
{
    private readonly IServerApi _serverApi;
    private readonly System.Timers.Timer _songTimer;

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
    public System.Collections.ObjectModel.ObservableCollection<Song> SongsToPick { get; set; }
    public HostHomePageViewModel(IServerApi serverApi)
    {
        _serverApi = serverApi;
        _songTimer = new System.Timers.Timer(500);
        startTimer();

        ChooseSongCommand = new Command(async () =>
        {
            await Shell.Current.GoToAsync(nameof(HostLastPage));
        });

        SongsToPick = new System.Collections.ObjectModel.ObservableCollection<Song>()
        {
            new Song { Name = "Kendrick Lamar: ADHD", Path = ""},
            new Song { Name = "50 Cent: Lolipop", Path = ""},
            new Song { Name = "Jay-z: 4:44", Path = ""},
            new Song { Name = "Dr.Dre: Xplicit", Path = ""},
        };
    }

    private void startTimer()
    {
        if(_songTimer != null)
        {
            _songTimer.Elapsed += songWorker;

            Task.Run(async () =>
            {
                await fetchSessionDetailsAsync();
            });

            _songTimer.Start();

        }
    }

    private async Task fetchSessionDetailsAsync()
    {
        //JukeboxSessionResponse details = await _serverApi.FetchSessionDetailsAsync();
        //if (details == null)
        //    return; // Session ended. Return to last page...

        //SongName = details.SongName;
        //Duration = TimeSpan.FromMilliseconds(details.SongDuration).TotalSeconds;
        //Position = TimeSpan.FromMilliseconds(details.SongPosition).TotalSeconds;
        //HostName = details.OwnerName;

        SongName = "Kendrick Lamar: ADHD";
        Duration = 182.22;
        Position = 20;
        HostName = "Bruni";
    }

    private async void songWorker(object sender, System.Timers.ElapsedEventArgs e)
    {
        Position += 1.0;
        if (Position >= Duration)
        {
            _songTimer.Stop();
            await fetchSessionDetailsAsync();
            _songTimer.Start();
        }
    }

    public async Task LeaveSessionAsync()
    {
        await _serverApi.LeaveSessionAsync();
        await Shell.Current.GoToAsync("..");
    }
}
