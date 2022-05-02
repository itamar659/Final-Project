namespace Host;
public interface IServerAPI
{
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
