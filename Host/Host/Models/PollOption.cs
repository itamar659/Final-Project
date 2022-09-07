using System.ComponentModel;

namespace Host.Models;
public class PollOption : BaseViewModel
{
    private int _id;
    public int Id
    {
        get { return _id; }
        set
        {
            _id = value;
            OnPropertyChanged(nameof(Id));
        }
    }

    private string _name;
    public string Name
    {
        get { return _name; }
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
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

    public int Timestamp { get; set; }
}
