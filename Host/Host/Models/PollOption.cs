using System.ComponentModel;

namespace Host.Models;
public class PollOption : BaseViewModel
{
    private int _pollId;
    public int PollId
    {
        get { return _pollId; }
        set
        {
            _pollId = value;
            OnPropertyChanged(nameof(PollId));
        }
    }

    private string _songName;
    public string SongName
    {
        get { return _songName; }
        set
        {
            _songName = value;
            OnPropertyChanged(nameof(SongName));
        }
    }

    private int _votes;
    public int Votes
    {
        get { return _votes; }
        set
        {
            _votes = value;
            OnPropertyChanged(nameof(Votes));
        }
    }
}
