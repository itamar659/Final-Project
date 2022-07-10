using Host.Models.Requests;
using Host.Models.Responses;

namespace Host.Services;
public interface IServerApi : IDisposable
{
    Task<bool> ConnectAsync(string password);
    Task<bool> OpenSessionAsync();
    Task CloseSessionAsync();
    string GetSessionKey();
    Task<JukeboxSessionResponse> FetchSessionUpdateAsync();
    Task<PollResponse> FetchPollAsync();
    Task<bool> CreatePollAsync(PollRequest pollRequest);
    Task<bool> RemovePollAsync();
    Task<bool> ChangeSessionPinCodeAsync(string pinCode);
    Task UpdateSongAsync(SongUpdateRequest song);
}
