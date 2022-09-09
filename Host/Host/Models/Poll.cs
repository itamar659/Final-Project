﻿using Host.Models.Requests;
using Host.Services;
using Microsoft.Maui.Graphics.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static Android.Content.ClipData;

namespace Host.Models;
public class Poll
{
    private readonly Random _rand = new Random();

    private readonly IServerApi _serverAPI;

    public int PollSize { get; set; }

    public ObservableCollection<PollOption> PollOptions { get; set; }

    public PollOption? TopRated => PollOptions?.Max(new MaxOption());

    public Poll(IServerApi serverAPI)
    {
        PollSize = 4;
        _serverAPI = serverAPI;

        PollOptions = new ObservableCollection<PollOption>();
    }

    public async Task CreatePollAsync(IReadOnlyList<Song> songs)
    {
        generatePoll(songs);

        var request = new PollRequest()
        {
            Options = PollOptions.ToList()
        };

        await _serverAPI.RemovePollAsync();
        await _serverAPI.CreatePollAsync(request);
    }

    private void generatePoll(IReadOnlyList<Song> songs)
    {
        if (songs.Count != 0)
            return;

        PollOptions.Clear();

        for (int i = 0; i < PollSize; i++)
        {
            var chosen = songs[_rand.Next(songs.Count)];

            PollOptions.Add(new PollOption
            {
                SongName = chosen.Name,
                PollId = i,
                Votes = 0
            });
        }
    }

    public void UpdateVotes(ICollection<PollOption> options)
    {
        foreach (var op in options)
            PollOptions
                .First(p => p.PollId == op.PollId)
                .Votes = op.Votes;

        PollOptions = new ObservableCollection<PollOption>(PollOptions.OrderByDescending(o => o.Votes));
    }

    public async Task RemovePollAsync()
    {
        await _serverAPI.RemovePollAsync();

        PollOptions.Clear();
    }

    public class MaxOption : IComparer<PollOption>
    {
        int IComparer<PollOption>.Compare(PollOption x, PollOption y)
        {
            return y.Votes - x.Votes;
        }
    }
}
