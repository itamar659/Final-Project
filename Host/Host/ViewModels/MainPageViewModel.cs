﻿using Host.Models;
using Host.Models.Requests;
using Host.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Host;
public class MainPageViewModel : BaseViewModel
{
    public static readonly double SERVER_UPDATE_DELAY = TimeSpan.FromSeconds(10).TotalMilliseconds;

    #region Private Members

    private IServerApi _serverAPI;
    private AudioPlayer _audioPlayer;
    private System.Timers.Timer _updateTimer;

    #endregion

    #region Public Properties

    /// <summary>
    /// Update the UI for everything that related to the audio player
    /// </summary>
    public AudioPlayerUpdater AudioPlayer { get; set; }

    /// <summary>
    /// Update the UI for everything that related to the room this host created
    /// </summary>
    public RoomUpdater RoomUpdater { get; set; }

    /// <summary>
    /// List available songs in the host
    /// </summary>
    public ObservableCollection<Song> Songs => _audioPlayer.Songs;

    /// <summary>
    /// A poll and saync poll methods to update the server
    /// </summary>
    public Poll Poll { get; set; }

    /// <summary>
    /// A room and saync poll methods to update the server
    /// </summary>
    public Room Room { get; set; }

    #endregion

    #region Commands

    public ICommand OpenCloseRoomCommand { get; set; }

    public ICommand StartPausePlayerCommand { get; set; }

    public ICommand PrevCommand { get; set; }

    public ICommand NextCommand { get; set; }

    #endregion

    #region Constructor

    public MainPageViewModel(IServerApi serverAPI, AudioPlayer audioPlayer)
    {
        _serverAPI = serverAPI;

        _audioPlayer = audioPlayer;
        _audioPlayer.SongStateChanged += updateServerSongAsync;
        _audioPlayer.SongEnded += changeSongAsync;

        _updateTimer = new System.Timers.Timer(SERVER_UPDATE_DELAY);
        _updateTimer.Elapsed += fetchViewUpdateAsync;

        Poll = new Poll(serverAPI);
        Room = new Room(serverAPI);
        AudioPlayer = new AudioPlayerUpdater(_audioPlayer);
        RoomUpdater = new RoomUpdater(Room);

        OpenCloseRoomCommand = new Command(openCloseRoomAsync);
        StartPausePlayerCommand = new Command(startPausePlayerAsync);
        PrevCommand = new Command(prevAsync);
        NextCommand = new Command(nextAsync);

        _updateTimer.Start();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Add a list of song to the playlist asynchronous.
    /// </summary>
    public Task AddSongsAsync(IEnumerable<FileResult> files)
    {
        if (files != null)
            foreach (var file in files)
                _audioPlayer.AddToPlaylist(file.FullPath);

        return Task.CompletedTask;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Fetch the last results about the poll, change the song in the audio player.
    /// </summary>
    private async void changeSongAsync(object sender, EventArgs e)
    {
        if (Room.IsOpen)
        {
            await Poll.UpdateVotesAsync();

            var choosenName = Poll.GetMostVotedName();

            await _audioPlayer.ChangeSong(choosenName);

            createPoll();
        }
        else
        {
            await _audioPlayer.NextAsync();
        }
    }

    /// <summary>
    /// Update the server about the song state.
    /// </summary>
    private async void updateServerSongAsync(object sender, EventArgs e)
    {
        await _serverAPI.UpdateSongAsync(new SongUpdateRequest { SongName = _audioPlayer.SongName, Duration = _audioPlayer.Duration, Position = _audioPlayer.Position });
    }

    /// <summary>
    /// open / close this host room and update the server
    /// </summary>
    private async void openCloseRoomAsync()
    {
        if (!Room.IsOpen)
        {
            await Room.OpenRoomAsync();
            createPoll();
        }
        else
        {
            await Room.CloseRoomAsync();
            await Poll.RemovePollAsync();
        }
    }

    /// <summary>
    /// change to the prev song in the audio player
    /// </summary>
    private async void prevAsync(object obj)
    {
        await _audioPlayer.PrevAsync();
    }

    /// <summary>
    /// change to the next song in the audio player
    /// </summary>
    private async void nextAsync(object obj)
    {
        await _audioPlayer.NextAsync();
    }

    /// <summary>
    /// start / pause the audio player
    /// </summary>
    private async void startPausePlayerAsync(object obj)
    {
        if (_audioPlayer.IsPlaying)
            await _audioPlayer.PauseAsync();
        else
            await _audioPlayer.PlayAsync();
    }

    /// <summary>
    /// update the UI every known period of time
    /// </summary>
    private async void fetchViewUpdateAsync(object sender, System.Timers.ElapsedEventArgs e)
    {
        await Room.UpdateRoomAsync();

        await Poll.UpdateVotesAsync();
    }

    /// <summary>
    /// Creating new poll and send it to the server
    /// </summary>
    private async void createPoll()
    {
        Poll.GeneratePoll(_audioPlayer.Songs);

        var request = new PollRequest() { Options = Poll.PollOptions.ToList() };

        // TODO: Check the poll in server, the changes in the model can make problems.
        await _serverAPI.CreatePollAsync(request);
    }

    #endregion

}
