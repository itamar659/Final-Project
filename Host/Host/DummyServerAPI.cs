using System.Diagnostics;

namespace Host;
public class DummyServerAPI : IServerAPI
{
    int IServerAPI.FetchActiveUsers()
    {
        return 0;
    }

    object IServerAPI.FetchLastVote()
    {
        return new { Total = 15, A = 5, B = 10, C = 0, D = 0 };
    }

    TimeSpan IServerAPI.FetchSessionTime()
    {
        return TimeSpan.FromMinutes(151);
    }

    int IServerAPI.FetchTotalUsers()
    {
        return 20;
    }

    bool IServerAPI.IsSessionLive()
    {
        return true;
    }

    void IServerAPI.SetSessionPinCode(int pinCode)
    {
        Debug.WriteLine("Session Pin Code: " + pinCode);
    }

    void IServerAPI.StartSession()
    {
        Debug.WriteLine("Start Session");
    }

    void IServerAPI.StopSession()
    {
        Debug.WriteLine("Stop Session");
    }

    void IServerAPI.UpdateSong(object song)
    {
        Debug.WriteLine("Update Song");
    }
}
