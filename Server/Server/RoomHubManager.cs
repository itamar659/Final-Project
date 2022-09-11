using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System.Text.RegularExpressions;
using Server.Dto;
using Server.Models;

namespace Server;

public class RoomHubManager
{
    private const string ClientJoinedMethod = "ClientJoined";
    private const string ClientLeavedMethod = "ClientLeaved";
    private const string RoomClosedMethod = "RoomClosed";
    private const string HostProfileUpdatedMethod = "HostProfileUpdated";
    private const string PollVotesUpdatedMethod = "PollVotesUpdated";
    private const string PollCreatedMethod = "PollCreated";

    private readonly IHubContext<RoomHub> _hubContext;

    public RoomHubManager(IHubContext<RoomHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task JoinRoom(string connectionId, string roomId)
    {
        await _hubContext.Groups.AddToGroupAsync(connectionId, roomId);
        await _hubContext.Clients.Group(roomId).SendAsync(ClientJoinedMethod);
    }

    public async Task LeaveRoom(string connectionId, string roomId)
    {
        await _hubContext.Groups.RemoveFromGroupAsync(connectionId, roomId);
        await _hubContext.Clients.Group(roomId).SendAsync(ClientLeavedMethod);
    }

    public async Task CloseRoom(string roomId)
    {
        await _hubContext.Clients.Group(roomId).SendAsync(RoomClosedMethod);
    }

    public async Task UpdateHostProfile(string roomId, JukeboxHostProfileDto host)
    {
        await _hubContext.Clients.Group(roomId).SendAsync(HostProfileUpdatedMethod, host);
    }

    public async Task UpdatePollVotes(string roomId, ICollection<PollOption> poll)
    {
        await _hubContext.Clients.Group(roomId).SendAsync(PollVotesUpdatedMethod, poll);
    }

    public async Task CreatePoll(string roomId, ICollection<PollOption> poll)
    {
        await _hubContext.Clients.Group(roomId).SendAsync(PollCreatedMethod, poll);
    }
}
