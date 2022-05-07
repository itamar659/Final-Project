using Host.Models.Responses;
using Host.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Host;
internal class ServerlessApi : IServerApi
{
    //private readonly string _apiBaseUrl = "https://csharp-project.azurewebsites.net/jukeboxhosts";
    private readonly string _apiBaseUrl = "https://localhost:7230";

    private readonly HttpClient _client;
    private string _token;
    private string _sessionKey;

    public ServerlessApi()
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri(_apiBaseUrl)
        };

        _token = string.Empty;
        _sessionKey = string.Empty;
    }

    private async Task<T> getResponseOrDefault<T>(string url, object jsonObj)
    {
        try
        {
            var json = JsonSerializer.Serialize(jsonObj);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(url, data);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<T>();

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex); // TODO: For Testing only, not for production
        }

        return default;
    }

    async Task<bool> IServerApi.ConnectAsync(string password)
    {
        var obj = new { Password = password };
        JukeboxHostResponse jukeboxHost = await getResponseOrDefault<JukeboxHostResponse>("/jukeboxhosts/connect", obj);

        if (jukeboxHost is null)
            return false;

        _token = jukeboxHost.Token;

        return true;
    }

    async Task IServerApi.StartSessionAsync()
    {
        var obj = new { Token = _token };
        JukeboxNewSessionResponse jukeboxSession = await getResponseOrDefault<JukeboxNewSessionResponse>("/jukeboxhosts/OpenSession", obj);

        if (jukeboxSession is not null)
            _sessionKey = jukeboxSession.SessionKey;
    }

    async Task IServerApi.StopSessionAsync()
    {
        var obj = new { Token = _token };
        await getResponseOrDefault<JukeboxNewSessionResponse>("/jukeboxhosts/CloseSession", obj);

        _sessionKey = string.Empty;
    }

    string IServerApi.GetSessionKey()
    {
        return _sessionKey;
    }

    void IDisposable.Dispose()
    {
        _client.Dispose();
    }

    async Task<int> IServerApi.FetchActiveUsers()
    {
        return 0;
    }

    async Task<object> IServerApi.FetchLastVote()
    {
        return null;
    }

    async Task<TimeSpan>  IServerApi.FetchSessionTime()
    {
        return TimeSpan.Zero;
    }

    async Task<int> IServerApi.FetchTotalUsers()
    {
        return 0;
    }

    async Task<bool> IServerApi.IsSessionLiveAsync()
    {
        return false;
    }

    async Task IServerApi.SetSessionPinCodeAsync(int pinCode)
    {
    }

    async Task IServerApi.UpdateSongAsync(object song)
    {
    }
}
