using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostApp
{
    public interface IAudioPlayer
    {
        /// <summary>
        /// The song current time in milliseconds
        /// Returns -1 if no song selected
        /// </summary>
        double CurrentPosition { get; }

        /// <summary>
        /// The total duration of the song
        /// Returns -1 if no song selected
        /// </summary>
        double Duration { get; }

        /// <summary>
        /// Returns true if the player playing the song
        /// </summary>
        bool IsPlaying { get; }

        /// <summary>
        /// Set a new song and prepare it if needed
        /// </summary>
        /// <param name="path">File name</param>
        void SetSong(string path);

        /// <summary>
        /// Play the song
        /// </summary>
        void Play();

        /// <summary>
        /// Stop the song
        /// </summary>
        void Stop();

        /// <summary>
        /// Pause the song
        /// </summary>
        //void Pause();
    }
}
