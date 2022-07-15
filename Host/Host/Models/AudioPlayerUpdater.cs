
using static Host.Models.AudioPlayer;

namespace Host.Models;
public class AudioPlayerUpdater : BaseViewModel
{
    public static readonly double PROPERTIES_UPDATE_DELAY = TimeSpan.FromSeconds(0.5).TotalMilliseconds;

    private System.Timers.Timer _updateTimer;
    private AudioPlayer _audioPlayer;

    public bool IsPlaying => _audioPlayer.IsPlaying;

    public PlayerState State => _audioPlayer.State;

    public double Position => _audioPlayer.Position;

    public double Duration => _audioPlayer.Duration;

    public string SongName => _audioPlayer.SongName;


    public AudioPlayerUpdater(AudioPlayer audioPlayer)
	{
		_audioPlayer = audioPlayer;
        _audioPlayer.SongStateChanged += _audioPlayer_SongStateChanged;

		_updateTimer = new System.Timers.Timer(PROPERTIES_UPDATE_DELAY);
		_updateTimer.Elapsed += _updateTimer_Elapsed;
        _updateTimer.Start();
	}

    private void _audioPlayer_SongStateChanged(object sender, EventArgs e)
    {
        OnPropertyChanged(nameof(IsPlaying));
        OnPropertyChanged(nameof(State));
        OnPropertyChanged(nameof(Position));
        OnPropertyChanged(nameof(Duration));
        OnPropertyChanged(nameof(SongName));
    }

    private void _updateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        OnPropertyChanged(nameof(IsPlaying));
        OnPropertyChanged(nameof(State));
        OnPropertyChanged(nameof(Position));
        OnPropertyChanged(nameof(Duration));
        OnPropertyChanged(nameof(SongName));
    }
}
