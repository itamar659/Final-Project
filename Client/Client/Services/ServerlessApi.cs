using Client.Models;
using Client.Models.Responses;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Client.Services;

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

    async Task<bool> IServerApi.LoginAsync(string username)
    {
        var obj = new { Password = username };
        JukeboxClientResponse jukeboxClient = await postResponseOrDefault<JukeboxClientResponse>("/JukeboxClients/Login", obj);

        if (jukeboxClient is null)
            return false;

        _token = jukeboxClient.Token;

        return true;
    }

    async Task<bool> IServerApi.CreateAsync(string username)
    {
        var obj = new { Password = username };
        JukeboxClientResponse jukeboxClient = await postResponseOrDefault<JukeboxClientResponse>("/JukeboxClients/Create", obj);

        if (jukeboxClient is null)
            return false;

        _token = jukeboxClient.Token;

        return true;
    }

    async Task<bool> IServerApi.AnonymousLoginAsync(string username)
    {
        var obj = new { Username = username };
        JukeboxClientResponse jukeboxHost = await postResponseOrDefault<JukeboxClientResponse>("/JukeboxClients/AnonymousLogin", obj);

        if (jukeboxHost is null)
            return false;

        _token = jukeboxHost.Token;

        return true;
    }

    async Task<bool> IServerApi.JoinSessionAsync(string ownerName, string pinCode)
    {
        var obj = new { Token = _token, OwnerName = ownerName, PinCode = pinCode };
        JukeboxJoinSessionResponse jukeboxSession = await postResponseOrDefault<JukeboxJoinSessionResponse>("/JukeboxClients/JoinSession", obj);

        if (jukeboxSession is null)
            return false;

        _sessionKey = jukeboxSession.SessionKey;

        return true;
    }

    async Task IServerApi.LeaveSessionAsync()
    {
        var obj = new { Token = _token };
        if (await postResponseOrDefault<bool>("/JukeboxClients/LeaveSession", obj))
            _sessionKey = string.Empty;
    }

    void IDisposable.Dispose()
    {
        _client.Dispose();
    }

    async Task<JukeboxSessionResponse> IServerApi.FetchSessionDetailsAsync(string sessionKey = null)
    {
        var obj = new { SessionKey = (sessionKey is null ? _sessionKey : sessionKey) };
        JukeboxSessionResponse session = await postResponseOrDefault<JukeboxSessionResponse>("/JukeboxSessions/GetSession", obj);

        return session;
    }

    async Task<List<JukeboxSessionResponse>> IServerApi.FetchAvailableSessionsAsync()
    {
        List<JukeboxSessionResponse> sessionNames = await postResponseOrDefault<List<JukeboxSessionResponse>>("/JukeboxSessions/AvailableSessions", null);

        return sessionNames;
    }

    async Task<JukeboxPollResponse> IServerApi.FetchPollAsync()
    {
        var obj = new { SessionKey = _sessionKey };
        List<PollOption> pollOptions = await postResponseOrDefault<List<PollOption>>("/Poll/GetPoll", obj);
        JukeboxPollResponse pollResponse = new JukeboxPollResponse
        {
            Options = pollOptions
        };

        return pollResponse;
    }

    async Task<bool> IServerApi.VoteAsync(int voteOption)
    {
        var obj = new { Token = _token, OptionId = voteOption };
        bool hasVoteSuccessfully = await postResponseOrDefault<bool>("/Poll/Vote", obj);

        return hasVoteSuccessfully;
    }
}
