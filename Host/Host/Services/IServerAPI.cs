namespace Host.Services;
public interface IServerApi : IDisposable
{
    Task<bool> ConnectAsync(string password); // Host controller
    Task StartSessionAsync(); // Host controller
    Task StopSessionAsync(); // Host controller
    string GetSessionKey(); // Host controller
    Task<bool> IsSessionLiveAsync(); // Session controller
    Task<object> FetchLastVote(); // Session controller
    Task<int> FetchActiveUsers(); // Session controller
    Task<int> FetchTotalUsers(); // Session controller
    Task<TimeSpan> FetchSessionTime(); // Session controller
    Task SetSessionPinCodeAsync(int pinCode); // Session controller
    Task UpdateSongAsync(object song); // Session controller
}
