using Host.Services;
using System.Diagnostics;
using System.Text.Json;

namespace Host;
public record JukeboxHostDTO
{
    public int id { get; set; }

    public string? token { get; set; }

    public string? sessionKey { get; set; }
}

public class DummyServerAPI : IServerAPI
{
    bool IServerAPI.ConnectAsync(string token)
    {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("https://csharp-project.azurewebsites.net/jukeboxhosts/");
        var res = client.GetAsync(token).Result;

        var a = JsonSerializer.Deserialize(res.Content.ReadAsStringAsync().Result, typeof(JukeboxHostDTO));
        if (a is JukeboxHostDTO)
            return true;

        return false;
    }

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
