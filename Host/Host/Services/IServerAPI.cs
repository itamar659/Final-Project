using Host.Models;
using Host.Models.Requests;
using Host.Models.Responses;

namespace Host.Services;

public interface IServerApi : IDisposable
{
    Task<string> ChangeRoomPinCodeAsync();
    Task CloseRoomAsync();
    Task<HostProfile> ConnectAsync(string username);
    Task CreatePollAsync(PollRequest pollRequest);
    Task EditProfileAsync(HostProfile profile);
    Task<PollResponse> FetchPollAsync();
    Task<RoomResponse> FetchRoomUpdateAsync();
    string GetRoomId();
    Task<string> OpenRoomAsync();
    Task RemovePollAsync();
    Task UpdateSongAsync(SongUpdateRequest song);
}
