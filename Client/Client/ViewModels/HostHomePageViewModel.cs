using Client.Models;
using Client.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Client;
public class HostHomePageViewModel : BaseViewModel
{
    private readonly IServerApi _serverApi;
    private readonly HubService _hub;

    public HostHomePageViewModel(IServerApi serverApi)
    {
        // init private members
        _serverApi = serverApi;
        _hub = UserProfile.Instance.Hub;

        // init properties
        Poll = new HostPoll(serverApi);
        Room = new HostRoom(serverApi, UserProfile.Instance.RoomId);

        // init commands
        ChooseSongCommand = new Command(async (object val) =>
        {
            await Poll.VoteAsync((int)val);
        });

        // set hub handlers
        _hub.RoomClosedHandler = async () =>
        {
            _hub.ClearHandlers();
            await LeaveRoomAsync();
        };

        _hub.SongUpdatedHandler = (song) =>
        {
            Room.SetSongProperties(song);
        };

        _hub.PollCreatedHandler = (poll) =>
        {
            Poll.SetPollProperties(poll);
        };
    }

    public HostRoom Room { get; set; }

    public HostPoll Poll { get; set; }

    public ICommand LeaveSessionCommand { get; set; }

    public ICommand ChooseSongCommand { get; set; }

    public async Task FetchUpdates()
    {
        await Poll.FetchPoll(Room.RoomId);
        await Room.FetchRoom();
    }

    public async Task LeaveRoomAsync()
    {
        _hub.ClearHandlers();
        await _hub.LeaveRoom(Room.RoomId);
        await _serverApi.LeaveRoomAsync();
        UserProfile.Instance.RoomId = null;
        Room.Dispose();
        await Shell.Current.GoToAsync("..");
    }
}
