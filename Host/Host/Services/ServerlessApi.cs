using Host.Models;
using Host.Models.Requests;
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
public class ServerlessApi : IServerApi
{
    //private readonly string _apiBaseUrl = "https://csharp-project.azurewebsites.net/jukeboxhosts";
    private readonly string _apiBaseUrl = DeviceInfo.Platform == DevicePlatform.WinUI ? "http://localhost:5230" : "http://10.0.2.2:5230";

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

    private async Task<T> postResponseOrDefault<T>(string url, object jsonObj)
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
        JukeboxHostResponse jukeboxHost = await postResponseOrDefault<JukeboxHostResponse>("/JukeboxHosts/Connect", obj);

        if (jukeboxHost is null)
            return false;

        _token = jukeboxHost.Token;

        return true;
    }

    async Task<bool> IServerApi.OpenSessionAsync()
    {
        var obj = new { Token = _token };
        JukeboxNewSessionResponse jukeboxSession = await postResponseOrDefault<JukeboxNewSessionResponse>("/JukeboxHosts/OpenSession", obj);

        if (jukeboxSession is null)
            return false;

        _sessionKey = jukeboxSession.SessionKey;

        return true;
    }

    async Task IServerApi.CloseSessionAsync()
    {
        var obj = new { Token = _token };
        await postResponseOrDefault<JukeboxNewSessionResponse>("/JukeboxHosts/CloseSession", obj);

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

    async Task<JukeboxSessionResponse> IServerApi.FetchSessionUpdateAsync()
    {
        var obj = new { SessionKey = _sessionKey };
        JukeboxSessionResponse session = await postResponseOrDefault<JukeboxSessionResponse>("/JukeboxSessions/GetSession", obj);

        return session;
    }

    async Task<bool> IServerApi.ChangeSessionPinCodeAsync(string pinCode)
    {
        var obj = new { Token = _token, PinCode = pinCode };
        return await postResponseOrDefault<bool>("/JukeboxHosts/ChangePinCode", obj);
    }

    async Task<PollResponse> IServerApi.FetchPollAsync()
    {
        var obj = new { SessionKey = _sessionKey };
        List<PollOption> poll = await postResponseOrDefault<List<PollOption>>("/Poll/GetPoll", obj);

        if (poll == null)
            return null;

        return new PollResponse
        {
             Options = poll
        };
    }

    async Task<bool> IServerApi.CreatePollAsync(PollRequest pollRequest)
    {
        pollRequest.Token = _token;

        return await postResponseOrDefault<bool>("/Poll/Create", pollRequest);
    }

    async Task<bool> IServerApi.RemovePollAsync()
    {
        var obj = new { Token = _token };
        return await postResponseOrDefault<bool>("/Poll/Remove", obj);
    }

    async Task IServerApi.UpdateSongAsync(SongUpdateRequest song)
    {
        var obj = new { Token = _token, SongName = song.SongName, Duration = song.Duration, Position = song.Position };
        await postResponseOrDefault<bool>("/JukeboxHosts/ChangeSong", obj);
    }

    async Task<bool> IServerApi.ChangeOwnerNameAsync(string ownerName)
    {
        var obj = new { Token = _token, OwnerName = ownerName };
        return await postResponseOrDefault<bool>("/JukeboxHosts/ChangeOwnerName", obj);
    }
}
