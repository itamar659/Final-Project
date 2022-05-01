using Android.Media;

namespace Host.Platforms.Android;

public class AudioService : IAudioService
{
    private MediaPlayer mediaPlayer;

    public double Position => mediaPlayer?.CurrentPosition ?? 0;

    public double Duration => mediaPlayer?.Duration + double.Epsilon ?? double.Epsilon;

    public bool IsPlaying => mediaPlayer?.IsPlaying ?? false;

    public event EventHandler SongEnded;

    public Task PauseAsync()
    {
        mediaPlayer.Pause();

        return Task.CompletedTask;
    }

    public Task PlayAsync()
    {
        if (mediaPlayer != null)
        {
            mediaPlayer.Start();
        }

        return Task.CompletedTask;
    }

    public async Task SetSongAsync(string path)
    {
        if (mediaPlayer == null)
        {
            mediaPlayer = new MediaPlayer();
            mediaPlayer.SeekComplete += (s, e) => { SongEnded?.Invoke(this, EventArgs.Empty); };
        }
        else
        {
            await PauseAsync();
        }

        mediaPlayer.SetDataSource(path);
        mediaPlayer.Prepare();
    }

    public Task StopAsync()
    {
        if (mediaPlayer.IsPlaying)
            mediaPlayer.Stop();

        mediaPlayer.Release();
        mediaPlayer = null;

        return Task.CompletedTask;
    }
}
