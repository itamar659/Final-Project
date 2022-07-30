using Client.Services;
using System.Collections.ObjectModel;
using Client.Models.Responses;

namespace Client.Models;
public class Poll
{
    private readonly IServerApi _serverAPI;

    public ObservableCollection<PollOption> PollOptions { get; set; }

    public Poll(IServerApi serverAPI)
    {
        _serverAPI = serverAPI;

        PollOptions = new ObservableCollection<PollOption>();
    }

    public async Task UpdateVotesAsync()
    {
        JukeboxPollResponse pollResponse = await _serverAPI.FetchPollAsync();

        PollOptions.Clear();
        if (pollResponse == null)
            return;

        foreach (var option in pollResponse.Options)
            PollOptions.Add(option);
    }

    public async Task VoteAsync(int option)
    {
        var isVoted = await _serverAPI.VoteAsync(option);
    }
}
