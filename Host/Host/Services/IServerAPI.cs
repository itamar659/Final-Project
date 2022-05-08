using Host.Models.Responses;

namespace Host.Services;
public interface IServerApi : IDisposable
{
    Task<bool> ConnectAsync(string password); // [X] Host controller
    Task<bool> OpenSessionAsync(); // [X] Host controller
    Task CloseSessionAsync(); // [X] Host controller
    string GetSessionKey(); // [X] Host controller
    Task<JukeboxSessionResponse> FetchSessionUpdateAsync(); // [X] Session controller
    Task<PollResponse> FetchPollAsync(); // [X] Poll controller
    Task<bool> CreatePollAsync(); // Poll controller
    Task<bool> RemovePollAsync(); // Poll controller
    Task<bool> ChangeSessionPinCodeAsync(string pinCode); // [X] Host controller
    Task UpdateSongAsync(object song); // Host controller
}
