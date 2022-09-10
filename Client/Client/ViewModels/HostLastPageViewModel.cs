using Client.Models.ServerMessages;
using Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client;

[QueryProperty(nameof(Duration), nameof(Duration))]
[QueryProperty(nameof(Position), nameof(Position))]
public class HostLastPageViewModel : BaseViewModel
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

    public HostLastPageViewModel(IServerApi serverApi)
    {
        _serverApi = serverApi;
        _songTimer = new System.Timers.Timer(500);
        startTimer();

    }

    private async Task fetchSessionDetailsAsync()
    {
        HostMessage details = await _serverApi.FetchSessionDetailsAsync();
        if (details == null)
            return;

        SongName = details.SongName;
        Duration = TimeSpan.FromMilliseconds(details.SongDuration).TotalSeconds;
        Position = TimeSpan.FromMilliseconds(details.SongPosition).TotalSeconds;
        HostName = details.OwnerName;
    }

    private void startTimer()
    {
        if (_songTimer != null)
        {
            _songTimer.Elapsed += songWorker;
            Task.Run(async () =>
            {
                await fetchSessionDetailsAsync();
            });
            _songTimer.Start();
        }
    }

    private async void songWorker(object sender, System.Timers.ElapsedEventArgs e)
    {
        Position += 0.5;
        if (Position >= Duration)
        {
            _songTimer.Stop();
            await Shell.Current.GoToAsync("..");
        }
    }
}

