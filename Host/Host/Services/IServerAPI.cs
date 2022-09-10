using Host.Models.ServerMessages;

namespace Host.Services;

public interface IServerApi : IDisposable
{
    Task<string> ChangeRoomPinCodeAsync();
    Task CloseRoomAsync();
    Task<HostMessage> ConnectAsync(string username);
    Task CreatePollAsync(PollMessage pollRequest);
    Task EditProfileAsync(HostMessage profile);
    Task<HostMessage> FetchHostProfileAsync(string token);
    Task<PollMessage> FetchPollAsync();
    Task<RoomMessage> FetchRoomUpdateAsync();
    string GetRoomId();
    Task<string> OpenRoomAsync();
    Task RemovePollAsync();
    Task UpdateSongAsync(SongMessage song);
}
