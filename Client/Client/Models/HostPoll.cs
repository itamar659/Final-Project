using Client.Services;
using System.Collections.ObjectModel;

namespace Client.Models;
public class HostPoll : BaseViewModel
{
    private readonly IServerApi _serverAPI;

    private bool _canVote;

    public HostPoll(IServerApi serverAPI)
    {
        _serverAPI = serverAPI;

        CanVote = true;
        Options = new ObservableCollection<PollOption>();
    }

    public ObservableCollection<PollOption> Options { get; }

    public bool CanVote
    {
        get { return _canVote; }
        private set
        {
            _canVote = value;
            OnPropertyChanged(nameof(CanVote));
        }
    }

    public async Task FetchPoll(string roomId)
    {
        var poll = await _serverAPI.FetchPollAsync(roomId);
        if (poll is null)
            return;

        SetPollProperties(poll.Options);
    }

    public async Task VoteAsync(int option)
    {
        await _serverAPI.VoteAsync(Options[option].PollId);
        CanVote = false;
    }

    internal void SetPollProperties(ICollection<PollOption> poll)
    {
        Options.Clear();

        foreach (var item in poll)
            Options.Add(item);

        CanVote = true;
    }
}
