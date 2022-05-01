using Windows.Media.Playback;

namespace Host.Platforms.Windows;

public class AudioService : IAudioService
{
    private MediaPlayer mediaPlayer;

    public double Position => mediaPlayer?.Position.TotalMilliseconds ?? 0;

    public double Duration => mediaPlayer?.NaturalDuration.TotalMilliseconds + double.Epsilon ?? double.Epsilon;

    public bool IsPlaying => mediaPlayer != null && mediaPlayer.CurrentState == MediaPlayerState.Playing;

    public event EventHandler SongEnded;

    public Task PauseAsync()
    {
        if (mediaPlayer != null)
            mediaPlayer?.Pause();

        return Task.CompletedTask;
    }

    public Task PlayAsync()
    {
        if (mediaPlayer != null)
            mediaPlayer.Play();

        return Task.CompletedTask;
    }

    public async Task SetSongAsync(string path)
    {
        if (mediaPlayer == null)
        {
            mediaPlayer = new MediaPlayer
            {
                AudioCategory = MediaPlayerAudioCategory.Media,
            };

            mediaPlayer.SeekCompleted += (s, e) => { SongEnded?.Invoke(this, EventArgs.Empty); };
        }
        else
        {
            await PauseAsync();
        }

        mediaPlayer.SetUriSource(new Uri(path));
    }

    public Task StopAsync()
    {
        if (mediaPlayer != null)
        {
            mediaPlayer.Pause();
            mediaPlayer.Source = null;
        }
        
        return Task.CompletedTask;
    }
}
