using Host.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace Host.Services;
public class HubService
{
    // invokes methods
    private const string ClientJoinedInvoke = "JoinRoom";
    private const string ClientLeavedInvoke = "LeaveRoom";
    private const string RoomClosedInvoke = "CloseRoom";
    private const string HostProfileUpdatedInvoke = "UpdateHostProfile";
    private const string PollVotesUpdatedInvoke = "UpdatePollVotes";
    private const string PollCreatedInvoke = "CreatePoll";

    // on methods
    private const string ClientJoinedMethod = "ClientJoined";
    private const string ClientLeavedMethod = "ClientLeaved";
    private const string RoomClosedMethod = "RoomClosed";
    private const string HostProfileUpdatedMethod = "HostProfileUpdated";
    private const string PollVotesUpdatedMethod = "PollVotesUpdated";
    private const string PollCreatedMethod = "PollCreated";

    private readonly string _apiBaseUrl = Configuration.ServerBaseUrl;

    private readonly HubConnection _connection;

    public HubConnection Hub => _connection;

    public HubService()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl($"{_apiBaseUrl}/RoomHub")
            .Build();

        // set on methods
        _connection.On(ClientJoinedMethod, () => ClientJoinedHandler?.Invoke());
        _connection.On(ClientLeavedMethod, () => ClientLeavedHandler?.Invoke());
        _connection.On(RoomClosedMethod, () => RoomClosedHandler?.Invoke());
        _connection.On<HostProfile>(HostProfileUpdatedMethod, (profile) => HostProfileUpdatedHandler?.Invoke(profile));
        _connection.On<ICollection<PollOption>>(PollVotesUpdatedMethod, (poll) => PollVotesUpdatedHandler?.Invoke(poll));
        _connection.On<ICollection<PollOption>>(PollCreatedMethod, (poll) => PollCreatedHandler?.Invoke(poll));
    }

    // handlers for on methods

    public Action ClientJoinedHandler { get; set; }
    public Action ClientLeavedHandler { get; set; }
    public Action RoomClosedHandler { get; set; }
    public Action<HostProfile> HostProfileUpdatedHandler { get; set; }
    public Action<ICollection<PollOption>> PollVotesUpdatedHandler { get; set; }
    public Action<ICollection<PollOption>> PollCreatedHandler { get; set; }

    // public methods

    public async Task StartAsync()
    {
        await _connection.StartAsync();
    }

    public async Task JoinRoom(string roomId)
    {
        try
        {
            await _connection.InvokeAsync(ClientJoinedInvoke, roomId);
        }
        catch(Exception)
        {

        }
    }
}
