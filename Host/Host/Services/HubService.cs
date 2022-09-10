using Host.Models;
using Host.Models.ServerMessages;
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
    private const string UpdateSongInvoke = "UpdateSong";

    // on methods
    private const string ClientJoinedMethod = "ClientJoined";
    private const string ClientLeavedMethod = "ClientLeaved";
    private const string RoomClosedMethod = "RoomClosed";
    private const string HostProfileUpdatedMethod = "HostProfileUpdated";
    private const string PollVotesUpdatedMethod = "PollVotesUpdated";
    private const string PollCreatedMethod = "PollCreated";
    private const string SongUpdatedMethod = "SongUpdated";

    private readonly string _apiBaseUrl = Configuration.DevelopServer;

    private readonly HubConnection _connection;

    public HubConnection Hub => _connection;

    public HubService()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl($"{_apiBaseUrl}/RoomHub")
            .WithAutomaticReconnect()
            .Build();

        // set on methods
        _connection.On(ClientJoinedMethod, () => ClientJoinedHandler?.Invoke());
        _connection.On(ClientLeavedMethod, () => ClientLeavedHandler?.Invoke());
        _connection.On(RoomClosedMethod, () => RoomClosedHandler?.Invoke());
        _connection.On<HostMessage>(HostProfileUpdatedMethod, (profile) => HostProfileUpdatedHandler?.Invoke(profile));
        _connection.On<ICollection<PollOption>>(PollVotesUpdatedMethod, (poll) => PollVotesUpdatedHandler?.Invoke(poll));
        _connection.On<ICollection<PollOption>>(PollCreatedMethod, (poll) => PollCreatedHandler?.Invoke(poll));
        _connection.On<SongMessage>(SongUpdatedMethod, (song) => SongUpdatedHandler?.Invoke(song));
    }

    // handlers for on methods

    public Action ClientJoinedHandler { get; set; }
    public Action ClientLeavedHandler { get; set; }
    public Action RoomClosedHandler { get; set; }
    public Action<HostMessage> HostProfileUpdatedHandler { get; set; }
    public Action<ICollection<PollOption>> PollVotesUpdatedHandler { get; set; }
    public Action<ICollection<PollOption>> PollCreatedHandler { get; set; }
    public Action<SongMessage> SongUpdatedHandler { get; set; }

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
        catch (Exception)
        {
        }
    }

    public async Task LeaveRoom(string roomId)
    {
        try
        {
            await _connection.InvokeAsync(ClientLeavedInvoke, roomId);
            await _connection.InvokeAsync(RoomClosedInvoke, roomId);
        }
        catch (Exception)
        {
        }
    }

    public async Task HostProfileUpdated(string roomId, HostMessage profile)
    {
        try
        {
            await _connection.InvokeAsync(HostProfileUpdatedInvoke, roomId, profile);
        }
        catch (Exception)
        {
        }
    }

    public async Task PollVotesUpdated(string roomId, ICollection<PollOption> options)
    {
        try
        {
            await _connection.InvokeAsync(PollVotesUpdatedInvoke, roomId, options);
        }
        catch (Exception)
        {
        }
    }

    public async Task PollCreated(string roomId, ICollection<PollOption> options)
    {
        try
        {
            await _connection.InvokeAsync(PollCreatedInvoke, roomId, options);
        }
        catch (Exception)
        {
        }
    }

    public async Task UpdateSong(string roomId, SongMessage song)
    {
        try
        {
            await _connection.InvokeAsync(UpdateSongInvoke, roomId, song);
        }
        catch (Exception)
        {
        }
    }

}
