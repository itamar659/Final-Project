namespace Host.Services;

public interface IAudioService
{
    event EventHandler SongEnded;
    double Position { get; }
    double Duration { get; }
    bool IsPlaying { get; }
    Task SetSongAsync(string path);
    Task PlayAsync();
    Task PauseAsync();
    Task StopAsync();
}
