using System.Collections.ObjectModel;

namespace Host;

public class AudioPlayer
{
    public enum PlayerState
    {
        Created,
        Play,
        Pause,
        Stop,
    }

    private readonly IAudioService _audioService;
    private readonly Playlist _playlist;

    public bool IsPlaying => State == PlayerState.Play;

    public PlayerState State { get; set; }

    public double Position => _audioService?.Position ?? 0;

    public double Duration => _audioService?.Duration ?? double.Epsilon;

    public string SongName => _playlist?.SelectedSong?.Name ?? "No Song Selected";

    public ObservableCollection<Song> Songs => _playlist.Songs;

    public event EventHandler SongStateChanged;

    public AudioPlayer(IAudioService audioService)
    {
        _audioService = audioService;
        _audioService.SongEnded += async (s, e) => { await NextAsync(); await PlayAsync(); };
        _playlist = new Playlist();

        State = PlayerState.Created;
    }

    public async Task PlayAsync()
    {
        if (_playlist.SelectedSong == null)
            return;

        await _audioService.SetSongAsync(_playlist.SelectedSong.Path);
        await _audioService.PlayAsync();

        State = PlayerState.Play;
        SongStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public async Task PauseAsync()
    {
        await _audioService.PauseAsync();

        State = PlayerState.Pause;
        SongStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public async Task StopAsync()
    {
        await _audioService.StopAsync();

        State = PlayerState.Stop;
        SongStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void AddToPlaylist(string path)
    {
        if (path == null)
            return;

        // TODO: How to use future access for faster buffering the songs
        // Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(file);

        _playlist.AddSong(new Song
        {
            Name = nameFromPath(path),
            Path = path,
        });

        if (_playlist.Songs.Count == 1)
        {
            State = PlayerState.Stop;
            SongStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public async Task PrevAsync()
    {
        if (_playlist.SelectedSong == null)
            return;

        _playlist.PrevSong();
        await StopAsync();
    }

    public async Task NextAsync()
    {
        if (_playlist.SelectedSong == null)
            return;

        _playlist.NextSong();
        await StopAsync();
    }

    private string nameFromPath(string path)
    {
        var file = new FileInfo(path);
        return file.Name.Replace(file.Extension, "");
    }
}
