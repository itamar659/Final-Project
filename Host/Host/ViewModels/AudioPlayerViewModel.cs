using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Host;

public class AudioPlayerViewModel : BaseViewModel
{
    private readonly AudioPlayer _playerService;

    public ObservableCollection<Song> Songs => _playerService.Songs;
    public string SongName => _playerService.SongName;
    public double Duration => _playerService.Duration;
    public double Position { get => _playerService.Position; set { } }
    public bool IsPlaying => _playerService.IsPlaying;

    public ICommand PlayStopCommand { get; }
    public ICommand SkipCommand { get; }

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

    public Task AddSongsAsync(IEnumerable<FileResult> files)
    {
        foreach (var file in files)
            _playerService.AddToPlaylist(file.FullPath);

        return Task.CompletedTask;
    }

    private void _playerService_SongStateChanged(object sender, EventArgs e)
    {
        OnPropertyChanged(nameof(IsPlaying));
        OnPropertyChanged(nameof(SongName));
    }

    private bool notifyChange()
    {
        OnPropertyChanged(nameof(Duration));
        OnPropertyChanged(nameof(Position));
        return IsPlaying;
    }
}
