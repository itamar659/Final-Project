using Client.Models.ServerMessages;
using Client.Services;

namespace Client.Models;

public class HostRoom : BaseViewModel, IDisposable
{
    public const double _delay = 500;

    private readonly System.Timers.Timer _updateLiveTimeTimer;
    private readonly IServerApi _serverAPI;

    private string _hostname;
    private string _songName;
    private double _duration;
    private double _position;
    private DateTime _openingTime;

    public HostRoom(IServerApi serverApi, string roomId)
    {
        _serverAPI = serverApi;
        RoomId = roomId;

        _updateLiveTimeTimer = new System.Timers.Timer(_delay);
        _updateLiveTimeTimer.Elapsed += updatePosition;
    }

    public string Hostname
    {
        get { return _hostname; }
        set
        {
            _hostname = value;
            OnPropertyChanged(nameof(Hostname));
        }
    }

    public string SongName
    {
        get { return _songName; }
        set
        {
            _songName = value;
            OnPropertyChanged(nameof(SongName));
        }
    }

    public double Duration
    {
        get => _duration + double.Epsilon;
        set
        {
            _duration = value;
            OnPropertyChanged(nameof(Duration));
        }
    }

    public double Position
    {
        get { return _position; }
        set
        {
            _position = value;
            OnPropertyChanged(nameof(Position));
        }
    }

    public DateTime OpeningTime
    {
        get { return _openingTime; }
        set
        {
            _openingTime = value;
            OnPropertyChanged(nameof(OpeningTime));
            OnPropertyChanged(nameof(LiveTime));
        }
    }

    public string LiveTime => (_openingTime != DateTime.MinValue ? DateTime.Now - OpeningTime : TimeSpan.Zero).ToString(@"hh\:mm\:ss");

    public string RoomId { get; }

    public async Task FetchRoom()
    {
        var room = await _serverAPI.FetchRoomUpdateAsync(RoomId);
        if (room == null)
            return;

        Hostname = room.Hostname;
        SongName = room.SongName;
        Duration = room.Duration.TotalSeconds;
        Position = room.Position.TotalSeconds;
        OpeningTime = room.OpeningTime;

        //Summary = profile.Summary
        //Description = profile.Desc...
        //BannerUrl
        //AvatarUrl


        if (room.IsPlaying && !_updateLiveTimeTimer.Enabled)
        {
            _updateLiveTimeTimer.Start();
        }
    }

    public void Dispose()
    {
        _updateLiveTimeTimer.Dispose();
    }

    public void SetSongProperties(SongMessage song)
    {
        SongName = song.SongName;
        Position = song.Position.TotalSeconds;
        Duration = song.Duration.TotalSeconds;

        if (song.IsPlaying && !_updateLiveTimeTimer.Enabled)
        {
            _updateLiveTimeTimer.Start();
        }
        else if (!song.IsPlaying && _updateLiveTimeTimer.Enabled)
        {
            _updateLiveTimeTimer.Stop();
        }
    }

    private void updatePosition(object sender, System.Timers.ElapsedEventArgs e)
    {
        Position += 0.5;
        OnPropertyChanged(nameof(Position));
    }
}
