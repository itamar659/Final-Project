using System.Collections.ObjectModel;
using static Host.Models.AudioPlayer;

namespace Host.Models;
public class AudioPlayerActive : BaseViewModel, IDisposable
{
    private const double _delay = 500;

    private System.Timers.Timer _viewNotifyTimer;
    private AudioPlayer _audioPlayer;

    public event EventHandler SongStateChanged;

    public event EventHandler SongEnded;

    public AudioPlayerActive(AudioPlayer audioPlayer)
	{
		_audioPlayer = audioPlayer;
        _audioPlayer.SongEnded += (s, e) => { SongEnded?.Invoke(s, e); };
        _audioPlayer.SongStateChanged += (s, e) => { SongStateChanged?.Invoke(s, e); };
        _audioPlayer.BufferingEnded += _audioPlayer_SongStateChanged;
        _audioPlayer.SongStateChanged += _audioPlayer_SongStateChanged;

        _viewNotifyTimer = new System.Timers.Timer(_delay);
		_viewNotifyTimer.Elapsed += _updateTimer_Elapsed;
        _viewNotifyTimer.Start();
    }

    public bool IsPlaying => _audioPlayer.IsPlaying;

    public PlayerState State => _audioPlayer.State;

    public double Position => _audioPlayer.Position;

    public double Duration => _audioPlayer.Duration;

    public string SongName => _audioPlayer.SongName;

    public ObservableCollection<Song> Songs => _audioPlayer.Songs;

    public async Task PlayAsync()
    {
        await _audioPlayer.PlayAsync();
    }

    public async Task PauseAsync()
    {
        await _audioPlayer.PauseAsync();
    }

    public async Task StopAsync()
    {
        await _audioPlayer.StopAsync();
    }

    public void AddToPlaylist(string path)
    {
        _audioPlayer.AddToPlaylist(path);
    }

    public void RemoveFromPlaylist(string name)
    {
        _audioPlayer.RemoveFromPlaylist(name);
    }

    public void RemoveFromPlaylist(Song song)
    {
        _audioPlayer.RemoveFromPlaylist(song);
    }

    public void ClearPlaylist()
    {
        _audioPlayer.ClearPlaylist();
    }

    public async Task ChangeSong(string songName)
    {
        await _audioPlayer.ChangeSong(songName);
    }

    public async Task PrevAsync()
    {
        await _audioPlayer.PrevAsync();
    }

    public async Task NextAsync()
    {
        await _audioPlayer.NextAsync();
    }

    public void Dispose()
    {
        _viewNotifyTimer.Dispose();
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
