using Host.Models;
using Host.Models.Requests;
using Host.Services;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Windows.Input;

namespace Host;


/* TODO:
 * _updateTimer:
 *   update online users counter
 *   update votes live
 *   update SongUpdateRequest in the server
 * update song when the audio player change song (use hub)
 * notify all the time about the audio player -> the position, is paused, and so on...
 * 
 */


public class MainPageViewModel : BaseViewModel
{
    public static readonly double SERVER_UPDATE_DELAY = TimeSpan.FromSeconds(1).TotalMilliseconds;

    #region Private Members

    private IServerApi _serverAPI;

    #endregion

    #region Public Properties

    /// <summary>
    /// the room hub
    /// </summary>
    public HubService HubService { get; set; }

    /// <summary>
    /// Audio Player
    /// </summary>
    public AudioPlayerActive AudioPlayer { get; set; }

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

    public MainPageViewModel(IServerApi serverAPI, AudioPlayerActive audioPlayer)
    {
        // init private members
        _serverAPI = serverAPI;
        AudioPlayer = audioPlayer;
        AudioPlayer.SongEnded += changeSongAsync;

        // init properties
        Poll = new Poll(serverAPI);
        Room = new Room(serverAPI);
        HubService = new HubService();

        // init commands
        OpenCloseRoomCommand = new Command(openCloseRoomAsync);
        StartPausePlayerCommand = new Command(startPausePlayerAsync);
        PrevCommand = new Command(prevAsync);
        NextCommand = new Command(nextAsync);

        // set hub handlers
        HubService.PollVotesUpdatedHandler = (poll) =>
        {
            Poll.UpdateVotes(poll);
        };

        HubService.ClientJoinedHandler = () =>
        {
            Room.OnlineUsers++;
        };

        HubService.ClientLeavedHandler = () =>
        {
            Room.OnlineUsers--;
        };
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
                AudioPlayer.AddToPlaylist(file.FullPath);



        return Task.CompletedTask;
    }

    /// <summary>
    /// /// Remove songs from the list of song by names asynchronous.
    /// </summary>
    public Task RemoveSongsAsync(IEnumerable<object> songs)
    {
        if (songs != null)
            foreach (var songName in songs.Select(s => ((Song)s).Name).ToList())
                AudioPlayer.RemoveFromPlaylist(songName);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Clear the list of song asynchronous.
    /// </summary>
    public Task ClearSongsAsync()
    {
        AudioPlayer.ClearPlaylist();

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
            await AudioPlayer.ChangeSong(Poll.TopRated.SongName);
            await Poll.CreatePollAsync(AudioPlayer.Songs);
        }
        else
        {
            await AudioPlayer.NextAsync();
        }
    }

    /// <summary>
    /// open / close this host room and update the server
    /// </summary>
    private async void openCloseRoomAsync()
    {
        if (!Room.IsOpen)
        {
            await Room.OpenRoomAsync();
            await HubService.JoinRoom(Room.RoomId);

            await HubService.UpdateSong(new SongUpdateRequest {
                SongName = AudioPlayer.SongName,
                Duration = AudioPlayer.Duration,
                Position = AudioPlayer.Position,
                IsPlaying = AudioPlayer.IsPlaying
            });

            await Poll.RemovePollAsync();
            await Poll.CreatePollAsync(AudioPlayer.Songs);
        }
        else
        {
            //await Poll.RemovePollAsync(); // no need, the server doing it already

            await HubService.LeaveRoom(Room.RoomId);
            await Room.CloseRoomAsync();
        }
    }

    /// <summary>
    /// change to the prev song in the audio player
    /// </summary>
    private async void prevAsync(object obj)
    {
        await AudioPlayer.PrevAsync();
    }

    /// <summary>
    /// change to the next song in the audio player
    /// </summary>
    private async void nextAsync(object obj)
    {
        await AudioPlayer.NextAsync();
    }

    /// <summary>
    /// start / pause the audio player
    /// </summary>
    private async void startPausePlayerAsync(object obj)
    {
        if (AudioPlayer.IsPlaying)
            await AudioPlayer.PauseAsync();
        else
            await AudioPlayer.PlayAsync();
    }

    #endregion
}
