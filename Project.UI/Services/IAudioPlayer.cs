using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.UI
{
    public interface IAudioPlayer
    {
        void Play();
        void Stop();
        //void Pause();

        double CurrentPosition { get; }
        double Duration { get; }
        bool IsPlaying { get; }
    }
}
