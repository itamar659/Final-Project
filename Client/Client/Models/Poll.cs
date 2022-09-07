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

    public async Task<bool> UpdateVotesAsync()
    {
        JukeboxPollResponse pollResponse = await _serverAPI.FetchPollAsync();

        if (pollResponse.Options == null || pollResponse.Options.Count == 0)
            return false;

        if (PollOptions.Count == 0 || PollOptions[0].Timestamp != pollResponse.Options[0].Timestamp)
        {
            PollOptions.Clear();
            foreach (var option in pollResponse.Options)
                PollOptions.Add(option);

            return true;
        }

        return false;
    }

    public async Task<bool> VoteAsync(int option)
    {
        return await _serverAPI.VoteAsync(option);
    }
}
