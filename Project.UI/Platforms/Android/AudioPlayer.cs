using Android.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.UI.Platforms
{
    internal class AudioPlayer : IAudioPlayer
    {
        private MediaPlayer _mediaPlayer;

        [Obsolete]
        public AudioPlayer()
        {
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.SetDataSource("https://soundbible.com/mp3/heavy-rain-daniel_simon.mp3");
            _mediaPlayer.Prepare();
        }

        public double CurrentPosition => _mediaPlayer?.CurrentPosition/1000.0 ?? 0;

        public double Duration => _mediaPlayer?.Duration/1000.0 ?? 0;

        public bool IsPlaying => _mediaPlayer?.IsPlaying ?? false;

        public void Play()
        {
            _mediaPlayer.Start();
        }

        public void Stop()
        {
            _mediaPlayer.Stop();
        }
    }
}
