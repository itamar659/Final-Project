﻿using Client.Services;
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

    public ObservableCollection<PollOption> Options { get; set; }

    public bool CanVote
    {
        get { return _canVote; }
        private set
        {
            _canVote = value;
            OnPropertyChanged(nameof(CanVote));
        }
    }

    public async Task VoteAsync(int option)
    {
        await _serverAPI.VoteAsync(Options[option].PollId);
    }

    internal void SetPollProperties(ICollection<PollOption> poll)
    {
        var newOptions = new ObservableCollection<PollOption>();
        foreach (var item in poll)
            newOptions.Add(item);

        Options = newOptions;
        CanVote = true;
    }
}
