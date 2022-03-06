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
        private MediaPlayer mediaPlayer_and = null;

        #region Interface Properties
        public double CurrentPosition => mediaPlayer_and?.CurrentPosition ?? 0;

        public double Duration => mediaPlayer_and?.Duration ?? 0;

        public bool IsPlaying => mediaPlayer_and?.IsPlaying ?? false;
        #endregion

        public void Play()
        {
            mediaPlayer_and.Start();
        }

        public void SetSong(string path)
        {
            mediaPlayer_and = new MediaPlayer();
            mediaPlayer_and.SetDataSource(path);
            mediaPlayer_and.Prepare();
        }

        public void Stop()
        {
            if (mediaPlayer_and.IsPlaying)
                mediaPlayer_and.Stop();

            mediaPlayer_and.Release();
            mediaPlayer_and = null;
        }
    }
}
