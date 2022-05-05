namespace Host.Services;
public interface IServerApi : IDisposable
{
    Task<bool> ConnectAsync(string token);
    void StartSession();
    void StopSession();
    bool IsSessionLive();
    object FetchLastVote();
    int FetchActiveUsers();
    int FetchTotalUsers();
    TimeSpan FetchSessionTime();
    void SetSessionPinCode(int pinCode);
    void UpdateSong(object song);
}
