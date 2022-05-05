using Host.Services;
using System.Diagnostics;

namespace Host;

public class DummyServerAPI : IServerApi
{
    Task<bool> IServerApi.ConnectAsync(string token)
    {
        return Task.FromResult(true);
    }

    void IDisposable.Dispose()
    {
        Debug.WriteLine("Dispose Called");
    }

    int IServerApi.FetchActiveUsers()
    {
        return 0;
    }

    object IServerApi.FetchLastVote()
    {
        return new { Total = 15, A = 5, B = 10, C = 0, D = 0 };
    }

    TimeSpan IServerApi.FetchSessionTime()
    {
        return TimeSpan.FromMinutes(151);
    }

    int IServerApi.FetchTotalUsers()
    {
        return 20;
    }

    bool IServerApi.IsSessionLive()
    {
        return true;
    }

    void IServerApi.SetSessionPinCode(int pinCode)
    {
        Debug.WriteLine("Session Pin Code: " + pinCode);
    }

    void IServerApi.StartSession()
    {
        Debug.WriteLine("Start Session");
    }

    void IServerApi.StopSession()
    {
        Debug.WriteLine("Stop Session");
    }

    void IServerApi.UpdateSong(object song)
    {
        Debug.WriteLine("Update Song");
    }
}
