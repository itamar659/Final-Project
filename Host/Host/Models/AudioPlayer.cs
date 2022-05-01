using System.Collections.ObjectModel;

namespace Host;

public class AudioPlayer
{
    private readonly IAudioService _audioService;
    private readonly Playlist _playlist;

    public bool IsPlaying { get; set; }

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
    }

    public async Task PlayAsync()
    {
        if (_playlist.SelectedSong == null)
            return;

        await _audioService.SetSongAsync(_playlist.SelectedSong.Path);
        await _audioService.PlayAsync();

        IsPlaying = true;
        SongStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public async Task PauseAsync()
    {
        await _audioService.PauseAsync();

        IsPlaying = false;
        SongStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public async Task StopAsync()
    {
        await _audioService.StopAsync();

        IsPlaying = false;
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
            SongStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public async Task PrevAsync()
    {
        if (_playlist.SelectedSong == null)
            return;

        _playlist.PrevSong();
        await StopAsync();
        SongStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public async Task NextAsync()
    {
        if (_playlist.SelectedSong == null)
            return;

        _playlist.NextSong();
        await StopAsync();
        SongStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private string nameFromPath(string path)
    {
        var file = new FileInfo(path);
        return file.Name.Replace(file.Extension, "");
    }
}
