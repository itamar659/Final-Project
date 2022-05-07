namespace Host.Services;
public interface IServerApi : IDisposable
{
    Task<bool> ConnectAsync(string password);
    Task StartSessionAsync();
    Task StopSessionAsync();
    Task<bool> IsSessionLiveAsync();
    Task<object> FetchLastVote();
    Task<int> FetchActiveUsers();
    Task<int> FetchTotalUsers();
    Task<TimeSpan> FetchSessionTime();
    Task SetSessionPinCodeAsync(int pinCode);
    Task UpdateSongAsync(object song);
    string GetSessionKey();
}
