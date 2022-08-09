using Host.Services;
using System.Collections.ObjectModel;

namespace Host.Models;

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

    public event EventHandler SongEnded;

    public AudioPlayer(IAudioService audioService)
    {
        _audioService = audioService;
        _audioService.SongEnded += (s, e) => { SongEnded?.Invoke(s, e); };
        _playlist = new Playlist();

        State = PlayerState.Created;
    }

    public async Task PlayAsync()
    {
        if (_playlist.SelectedSong == null)
            return;

        if (State != PlayerState.Pause)
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

        _playlist.AddSong(new Song
        {
            Name = getNameFromPath(path),
            Path = path,
        });

        if (_playlist.Songs.Count == 1 && State == PlayerState.Created)
        {
            State = PlayerState.Stop;
            SongEnded?.Invoke(this, EventArgs.Empty);
        }
    }

    public void RemoveFromPlaylist(string name)
    {
        if (name == null)
            return;

        var song = _playlist.Songs.FirstOrDefault(s => s.Name == name);
        if (song == null)
            return;

        _playlist.Songs.Remove(song);

        if (_playlist.Songs.Count == 0)
        {
            State = PlayerState.Created;
            SongStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void RemoveFromPlaylist(Song song)
    {
        if (song == null)
            return;

        _playlist.Songs.Remove(song);

        if (_playlist.Songs.Count == 0)
        {
            State = PlayerState.Created;
            SongStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ClearPlaylist()
    {
        _playlist.Songs.Clear();

        if (_playlist.Songs.Count == 0)
        {
            State = PlayerState.Created;
            SongStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public async Task ChangeSong(string songName)
    {
        if (songName == null)
            return;

        bool autoPlay = State == PlayerState.Play;

        var song = _playlist.Songs.FirstOrDefault(s => s.Name == songName);
        if (song == null)
            return;

        var idx = _playlist.Songs.IndexOf(song);

        _playlist.SetSong(idx);

        if (autoPlay)
            await PlayAsync();
        else
            await StopAsync();
    }

    public async Task PrevAsync()
    {
        if (_playlist.SelectedSong == null)
            return;

        bool autoPlay = State == PlayerState.Play;

        _playlist.PrevSong();
        
        if (autoPlay)
            await PlayAsync();
        else
            await StopAsync();
    }

    public async Task NextAsync()
    {
        if (_playlist.SelectedSong == null)
            return;

        bool autoPlay = State == PlayerState.Play;

        _playlist.NextSong();

        if (autoPlay)
            await PlayAsync();
        else
            await StopAsync();
    }

    private string getNameFromPath(string path)
    {
        var file = new FileInfo(path);
        return file.Name.Replace(file.Extension, "");
    }
}
