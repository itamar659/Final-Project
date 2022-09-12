using Microsoft.AspNetCore.SignalR;
using Server;
using Server.Dto;
using Server.Models;

namespace Server;

public class RoomHub : Hub
{
    // TODO: For security, send the token, allow only host tokens to invoke methods.

    // TODO: Use different methods for client messages that the host will invoke, and the host messages that the client will invoke.
    //       That way it's way way more efficient in messages.

    private const string ClientJoinedMethod = "ClientJoined";
    private const string ClientLeavedMethod = "ClientLeaved";
    private const string RoomClosedMethod = "RoomClosed";
    private const string HostProfileUpdatedMethod = "HostProfileUpdated";
    private const string PollVotesUpdatedMethod = "PollVotesUpdated";
    private const string PollCreatedMethod = "PollCreated";
    private const string SongUpdatedMethod = "SongUpdated";

    public async Task JoinRoom(string roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        await Clients.OthersInGroup(roomId).SendAsync(ClientJoinedMethod);
    }

    public async Task LeaveRoom(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        await Clients.OthersInGroup(roomId).SendAsync(ClientLeavedMethod);
    }

    public async Task CloseRoom(string roomId)
    {
        await Clients.OthersInGroup(roomId).SendAsync(RoomClosedMethod);
    }

    public async Task UpdateHostProfile(string roomId, JukeboxHostProfileDto host)
    {
        await Clients.OthersInGroup(roomId).SendAsync(HostProfileUpdatedMethod, host);
    }

    public async Task UpdatePollVotes(string roomId, ICollection<PollOption> poll)
    {
        await Clients.Group(roomId).SendAsync(PollVotesUpdatedMethod, poll);
    }

    public async Task CreatePoll(string roomId, ICollection<PollOption> poll)
    {
        await Clients.OthersInGroup(roomId).SendAsync(PollCreatedMethod, poll);
    }

    public async Task UpdateSong(string roomId, Song song)
    {
        await Clients.OthersInGroup(roomId).SendAsync(SongUpdatedMethod, song);
    }
}
