using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;

namespace HostApp.Platforms
{
    internal class AudioPlayer : IAudioPlayer
    {
        private MediaPlayer mediaPlayer_win = new MediaPlayer();

        #region Interface Properties
        public double CurrentPosition => mediaPlayer_win?.Position.TotalMilliseconds ?? 0;

        public double Duration => mediaPlayer_win?.NaturalDuration.TotalMilliseconds ?? 0;

        public bool IsPlaying => mediaPlayer_win.CurrentState == MediaPlayerState.Playing;
        #endregion

        public void Play()
        {
            mediaPlayer_win.Play();
        }

        public void SetSong(string path)
        {
            mediaPlayer_win.SetUriSource(new Uri(path));
        }

        public void Stop()
        {
            mediaPlayer_win.Pause();
            mediaPlayer_win.Source = null;
        }
    }
}
