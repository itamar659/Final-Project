using Client.Models;
using Client.Services;
using System.Windows.Input;

// TODO: Fetch the poll from the server, update the host song, fetch the song from the serever
// TODO: Fetch the poll from the server, update the host song, fetch the song from the serever
// TODO: Fetch the poll from the server, update the host song, fetch the song from the serever
// TODO: Fetch the poll from the server, update the host song, fetch the song from the serever
// TODO: Fetch the poll from the server, update the host song, fetch the song from the serever
// TODO: Fetch the poll from the server, update the host song, fetch the song from the serever
// TODO: Fetch the poll from the server, update the host song, fetch the song from the serever
// TODO: Fetch the poll from the server, update the host song, fetch the song from the serever
// TODO: Fetch the poll from the server, update the host song, fetch the song from the serever
// TODO: Fetch the poll from the server, update the host song, fetch the song from the serever
// TODO: Fetch the poll from the server, update the host song, fetch the song from the serever
// TODO: Fetch the poll from the server, update the host song, fetch the song from the serever
// TODO: Fetch the poll from the server, update the host song, fetch the song from the serever
// TODO: Fetch the poll from the server, update the host song, fetch the song from the serever
// TODO: Fetch the poll from the server, update the host song, fetch the song from the serever
// TODO: Fetch the poll from the server, update the host song, fetch the song from the serever
// TODO: Fetch the poll from the server, update the host song, fetch the song from the serever

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

        // current thread dispatcher to be able to invoke hub methods from different thread
        // var dispatcher = Dispatcher.GetForCurrentThread(); // TODO: Check if dispatcher is needed.

        _hub.RoomClosedHandler = async () =>
        {
            _hub.ClearHandlers();
            await LeaveSessionAsync();
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

    public async Task LeaveSessionAsync()
    {
        Room.Dispose();
        await _serverApi.LeaveRoomAsync();
        await Shell.Current.GoToAsync("..");
    }
}
