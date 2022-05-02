using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Host;

public class AudioPlayerViewModel : BaseViewModel
{
    private readonly AudioPlayer _playerService;

    /// <summary>
    /// List of all the songs the host "uploaded"
    /// </summary>
    public ObservableCollection<Song> Songs => _playerService.Songs;

    /// <summary>
    /// Current song name
    /// </summary>
    public string SongName => _playerService.SongName;

    /// <summary>
    /// Current song duration
    /// </summary>
    public double Duration => _playerService.Duration;

    /// <summary>
    /// playing song current position
    /// </summary>
    public double Position { get => _playerService.Position; set { } }

    /// <summary>
    /// If something is playing
    /// </summary>
    public bool IsPlaying => _playerService.IsPlaying;

    /// <summary>
    /// Start / Stop the music
    /// </summary>
    public ICommand PlayStopCommand { get; }

    /// <summary>
    /// Skip to the next song
    /// </summary>
    public ICommand SkipCommand { get; }

    /// <summary>
    /// Main Constructor
    /// </summary>
    /// <param name="_audioPlayer">The Audio Service for the Player</param>
    public AudioPlayerViewModel(IAudioService _audioPlayer)
    {
        _playerService = new AudioPlayer(_audioPlayer);
        _playerService.SongStateChanged += _playerService_SongStateChanged;

        SkipCommand = new Command(async () => { await _playerService.NextAsync(); });
        PlayStopCommand = new Command(async () =>
        {
            if (_playerService.IsPlaying)
            {
                await _playerService.StopAsync();
            }
            else
            {
                await _playerService.PlayAsync();
                Device.StartTimer(TimeSpan.FromMilliseconds(500), notifyChange);
            }
        });
    }

    /// <summary>
    /// Add a list of song to the playlist asynchronous.
    /// </summary>
    /// <param name="files">list of paths</param>
    /// <returns></returns>
    public Task AddSongsAsync(IEnumerable<FileResult> files)
    {
        foreach (var file in files)
            _playerService.AddToPlaylist(file.FullPath);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Update the SongName / IsPlaying every time the song state changes
    /// </summary>
    private void _playerService_SongStateChanged(object sender, EventArgs e)
    {
        OnPropertyChanged(nameof(IsPlaying));
        OnPropertyChanged(nameof(SongName));
    }

    /// <summary>
    /// Update the duration / Position as long as song is plays.
    /// </summary>
    /// <returns></returns>
    private bool notifyChange()
    {
        OnPropertyChanged(nameof(Duration));
        OnPropertyChanged(nameof(Position));
        return IsPlaying;
    }
}
