using Host.Models.ServerMessages;
using Host.Services;
using System.Windows.Input;
using Host.Models;
using System.Text.Json;
using Microsoft.Maui.Dispatching;

namespace Host;

// TODO: Update UI (pincode) after opening/closing room
public class MainPageViewModel : BaseViewModel
{
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
    public HostPoll Poll { get; set; }

    /// <summary>
    /// A room and saync poll methods to update the server
    /// </summary>
    public HostRoom Room { get; set; }

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
        var dispatcher = Dispatcher.GetForCurrentThread();
        AudioPlayer = audioPlayer;
        AudioPlayer.SongStateChanged += updateStateChangesAsync;
        AudioPlayer.SongEnded += (s, e) => dispatcher?.Dispatch(() => changeSongAsync(s, e));
        AudioPlayer.BufferingEnded += updateStateChangesAsync;

        // init properties
        Poll = new HostPoll(serverAPI);
        Room = new HostRoom(serverAPI);
        HubService = HostProfile.Instance.Hub;

        // init commands
        OpenCloseRoomCommand = new Command(openCloseRoomAsync);
        StartPausePlayerCommand = new Command(startPausePlayerAsync);
        PrevCommand = new Command(prevAsync);
        NextCommand = new Command(nextAsync);

        // set hub handlers
        HubService.ClearHandlers();
        HubService.PollVotesUpdatedHandler = (poll) =>
        {
            Poll.UpdateVotes(poll);
        };

        HubService.ClientJoinedHandler = () =>
        {
            Room.OnlineUsers++;
            updateStateChangesAsync(this, EventArgs.Empty);

        };

        HubService.ClientLeavedHandler = () =>
        {
            Room.OnlineUsers--;
        };
    }

    #endregion

    #region Public Methods

    public async Task FetchProfile()
    {
        await Room.UpdateRoom();
    }

    public Task LoadLocalSavedSongsAsync()
    {
        var songs = loadSongs();
        foreach (var file in songs)
            AudioPlayer.AddToPlaylist(file.Path);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Add a list of song to the playlist asynchronous.
    /// </summary>
    public Task AddSongsAsync(IEnumerable<FileResult> files)
    {
        if (files != null)
            foreach (var file in files)
                AudioPlayer.AddToPlaylist(file.FullPath);

        saveSongs(AudioPlayer.Songs);
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

        saveSongs(AudioPlayer.Songs);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Clear the list of song asynchronous.
    /// </summary>
    public Task ClearSongsAsync()
    {
        AudioPlayer.ClearPlaylist();

        saveSongs(AudioPlayer.Songs);
        return Task.CompletedTask;
    }

    #endregion

    #region Private Methods

    private async void updateStateChangesAsync(object sender, EventArgs e)
    {
        if (Room.RoomId == null)
            return;

        await Room.updateServer(AudioPlayer.SongMessage);
        await HubService.UpdateSong(Room.RoomId, AudioPlayer.SongMessage);
    }

    /// <summary>
    /// Fetch the last results about the poll, change the song in the audio player.
    /// </summary>
    private async void changeSongAsync(object sender, EventArgs e)
    {
        if (Room.IsOpen)
        {
            if (Poll.TopRated != null)
                await AudioPlayer.ChangeSong(Poll.TopRated.SongName);
            else
                await AudioPlayer.NextAsync();

            await Poll.RemovePollAsync();
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

            await HubService.UpdateSong(Room.RoomId, AudioPlayer.SongMessage);

            await Poll.RemovePollAsync();
            await Poll.CreatePollAsync(AudioPlayer.Songs);
        }
        else
        {
            await Poll.RemovePollAsync();
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

    private void saveSongs(ICollection<Song> songs)
    {
        Configuration.SavedSong = JsonSerializer.Serialize(songs);
    }

    private ICollection<Song> loadSongs()
    {
        if (Configuration.SavedSong == default(string))
            return new List<Song>();

        return JsonSerializer.Deserialize<ICollection<Song>>(Configuration.SavedSong);
    }

    #endregion
}
