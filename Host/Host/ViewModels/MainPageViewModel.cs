using Host.Models;
using Host.Models.Requests;
using Host.Models.Responses;
using Host.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Host;
public class MainPageViewModel : BaseViewModel
{
    public static Random RANDOM = new Random();
    public static readonly double SERVER_UPDATE_DELAY = TimeSpan.FromSeconds(10).TotalMilliseconds;

    private IServerApi _serverAPI;
    private System.Timers.Timer _updateTimer;

    private AudioPlayer _audioPlayer;

    public AudioPlayerUpdater AudioPlayer { get; set; }

    public ObservableCollection<Song> Songs => _audioPlayer.Songs;

    public ObservableCollection<PollOption> PollOptions { get; set; }

    public Room MyRoom { get; set; }

    public ICommand OpenCloseRoomCommand { get; set; }

    public ICommand StartStopPlayerCommand { get; set; }

    public ICommand PrevCommand { get; set; }

    public ICommand NextCommand { get; set; }

    public MainPageViewModel(IServerApi serverAPI, AudioPlayer audioPlayer)
    {
        _serverAPI = serverAPI;

        _audioPlayer = audioPlayer;
        _audioPlayer.SongStateChanged += updateSong;

        _updateTimer = new System.Timers.Timer(SERVER_UPDATE_DELAY);
        _updateTimer.Elapsed += fetchViewUpdateAsync;

        AudioPlayer = new AudioPlayerUpdater(_audioPlayer);
        PollOptions = new ObservableCollection<PollOption>();
        MyRoom = new Room(serverAPI);

        OpenCloseRoomCommand = new Command(openCloseRoomAsync);
        StartStopPlayerCommand = new Command(startStopPlayerAsync);
        PrevCommand = new Command(prevAsync);
        NextCommand = new Command(nextAsync);

        _updateTimer.Start();
    }

    /// <summary>
    /// Add a list of song to the playlist asynchronous.
    /// </summary>
    /// <param name="files">list of paths</param>
    /// <returns></returns>
    public Task AddSongsAsync(IEnumerable<FileResult> files)
    {
        if (files != null)
            foreach (var file in files)
                _audioPlayer.AddToPlaylist(file.FullPath);

        return Task.CompletedTask;
    }

    private async void openCloseRoomAsync()
    {
        if (!MyRoom.IsOpen)
            await MyRoom.OpenRoomAsync();
        else
            await MyRoom.CloseRoomAsync();
    }

    private async void prevAsync(object obj)
    {
        await _audioPlayer.PrevAsync();
    }

    private async void nextAsync(object obj)
    {
        await _audioPlayer.NextAsync();
    }

    private async void startStopPlayerAsync(object obj)
    {
        if (_audioPlayer.IsPlaying)
            await _audioPlayer.PauseAsync();
        else
            await _audioPlayer.PlayAsync();
    }

    private async void fetchViewUpdateAsync(object sender, System.Timers.ElapsedEventArgs e)
    {
        JukeboxSessionResponse sessionResponse = await _serverAPI.FetchSessionUpdateAsync();
        if (sessionResponse == null)
            return;

        PollResponse pollResponse = await _serverAPI.FetchPollAsync();
        if (pollResponse != null)
        {
            PollOptions.Clear();
            foreach (var item in pollResponse.Options)
                PollOptions.Add(item);
        }

        MyRoom.OnlineUsers = sessionResponse.ActiveUsers;
    }

    /// <summary>
    /// Update the song when the AudioPlayer fire SongStateChanged event.
    /// </summary>
    private void updateSong(object sender, EventArgs e)
    {
        // Can transfer it to another thread insteaf of creating new timer - almost same but thread stop after completion...
        // Also make Thread as daemon to stop the fking crash on exit.

        //var t = new System.Timers.Timer(100);
        //t.Elapsed += async (s, e) => {
        //    await _serverAPI.UpdateSongAsync(new SongUpdateRequest { SongName = _audioPlayer.SongName, Duration = _audioPlayer.Duration, Position = _audioPlayer.Position });
        //    t.Stop();
        //};
        //t.Start();
    }

    /// <summary>
    /// Creating new poll and send it to the server
    /// </summary>
    private async void createPoll()
    {
        int pollSize = 4;
        var songs = new List<PollOption>();

        for (int i = 0; i < pollSize; i++)
        {
            var chosen = RANDOM.Next(_audioPlayer.Songs.Count);

            songs.Add(new PollOption
            {
                Id = i + 1,
                Name = _audioPlayer.Songs[chosen].Name,
                Votes = 0,
            });
        }

        var request = new PollRequest() { Options = songs };

        // TODO: Check the poll in server, the changes in the model can make problems.
        await _serverAPI.CreatePollAsync(request);
    }
}
