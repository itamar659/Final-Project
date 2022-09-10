using Client.Models.ServerMessages;

namespace Client.Services;

public interface IServerApi : IDisposable
{
    Task<ClientMessage> LoginAsync(string username);
    Task<ClientMessage> AnonymousLoginAsync(string username);
    Task<List<RoomMessage>> FetchOpenedRoomsAsync();
    Task<ClientMessage> FetchClientProfileAsync(string token);
    Task<PollMessage> FetchPollAsync(string roomId);
    Task<RoomMessage> FetchRoomUpdateAsync(string roomId);
    Task<bool> JoinRoomAsync(string roomId, string pinCode);
    Task LeaveRoomAsync();
    Task VoteAsync(int pollId);
}
