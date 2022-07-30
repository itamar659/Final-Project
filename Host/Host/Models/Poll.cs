using Host.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Host.Models;
public class Poll
{
    public static readonly Random RANDOM = new Random();

    private readonly IServerApi _serverAPI;

    public int PollSize { get; set; }

    public ObservableCollection<PollOption> PollOptions { get; set; }

    public Poll(IServerApi serverAPI)
    {
        PollSize = 4;
        _serverAPI = serverAPI;

        initializePollOptions();
    }

    public async void GeneratePoll(IReadOnlyList<Song> songs)
    {
        if (songs.Count == 0)
            return;

        for (int i = 0; i < PollSize; i++)
        {
            var chosen = RANDOM.Next(songs.Count);

            PollOptions[i].Name = songs[chosen].Name;
            PollOptions[i].Votes = 0;
        }
    }

    public async Task UpdateVotesAsync()
    {
        var pollResponse = await _serverAPI.FetchPollAsync();
        if (pollResponse == null)
            return;

        for (int i = 0; i < pollResponse.Options.Count && i < PollSize; i++)
            PollOptions[i].Votes = pollResponse.Options[i].Votes;
    }

    public async Task RemovePollAsync()
    {
        await _serverAPI.RemovePollAsync();

        for (int i = 0; i < PollSize; i++)
        {
            PollOptions[i].Name = "None";
            PollOptions[i].Votes = 0;
        }
    }

    public string GetMostVotedName()
    {
        int max = 0;
        int option = 1;

        foreach (var item in PollOptions)
        {
            if (item.Votes > max)
            {
                max = item.Votes;
                option = item.Id;
            }
        }

        return PollOptions[option - 1].Name;
    }

    private void initializePollOptions()
    {
        PollOptions = new ObservableCollection<PollOption>();

        while (PollOptions.Count < PollSize)
            PollOptions.Add(new PollOption()
            {
                Id = PollOptions.Count + 1,
                Name = "None"
            });
    }
}
