using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Host.Models;

public class Playlist
{
    public ObservableCollection<Song> Songs { get; }
    public Song SelectedSong { get; private set; }

    public Playlist()
    {
        Songs = new ObservableCollection<Song>();
        SelectedSong = null;
    }

    public void AddSong(Song song)
    {
        if (SelectedSong == null)
            SelectedSong = song;

        if (Songs.FirstOrDefault(s => song.Path == s.Path || song.Name == s.Name) == null)
            Songs.Add(song);
    }

    public void NextSong()
    {
        changeSelectedSong(true);
    }

    public void PrevSong()
    {
        changeSelectedSong(false);
    }

    public void SetSong(int idx)
    {
        SelectedSong = Songs[idx];
    }

    private void changeSelectedSong(bool isNext)
    {
        var diff = isNext ? 1 : -1;
        var currentIndex = Songs.IndexOf(SelectedSong);

        var idx = (currentIndex + diff) % Songs.Count;
        SelectedSong = Songs[idx < 0 ? idx = Songs.Count - 1 : idx];
    }
}
