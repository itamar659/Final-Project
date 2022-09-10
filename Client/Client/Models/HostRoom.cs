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
        _updateLiveTimeTimer.Elapsed += (s, e) => OnPropertyChanged(nameof(LiveTime));
        _updateLiveTimeTimer.Start();
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

    public async Task UpdateRoomAsync()
    {
        //TODO: update and display:
        //var profile = await _serverAPI.FetchHostProfileAsync(Configuration.Token);
        //if (profile == null)
        //    return;

        //Summary = profile.Summary
        //Description = profile.Desc...
        //BannerUrl
        //AvatarUrl

        var room = await _serverAPI.FetchRoomUpdateAsync(RoomId);
        if (room == null)
            return;

        Hostname = room.Hostname;
        OpeningTime = room.OpeningTime;
        Position = room.Position.TotalSeconds;
        Duration = room.Duration.TotalSeconds;

        if (room.IsPlaying && !_updateLiveTimeTimer.Enabled)
        {
            _updateLiveTimeTimer.Start();
        }
    }

    public void Dispose()
    {
        _updateLiveTimeTimer.Dispose();
    }

    internal void SetSongProperties(SongMessage song)
    {
        SongName = song.SongName;
        Position = song.Position;
        Duration = song.Duration;

        if (song.IsPlaying && !_updateLiveTimeTimer.Enabled)
        {
            _updateLiveTimeTimer.Start();
        }
        else if (!song.IsPlaying && _updateLiveTimeTimer.Enabled)
        {
            _updateLiveTimeTimer.Stop();
        }
    }
}
