using Host.Models.Responses;

namespace Host.Services;
public interface IServerApi : IDisposable
{
    Task<bool> ConnectAsync(string password); // Host controller
    Task<bool> OpenSessionAsync(); // Host controller
    Task CloseSessionAsync(); // Host controller
    string GetSessionKey(); // Host controller
    Task<JukeboxSessionResponse> FetchSessionUpdateAsync(); // Session controller
    Task<object> FetchLastVote(); // Session controller
    Task<bool> ChangeSessionPinCodeAsync(string pinCode); // Host controller
    Task UpdateSongAsync(object song); // Host controller
}
