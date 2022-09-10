using Host.Models;
using Host.Models.ServerMessages;
using Host.Services;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Host;

public class ServerlessApi : IServerApi
{
    //private readonly string _apiBaseUrl = "https://csharp-project.azurewebsites.net/jukeboxhosts";
    private readonly string _apiBaseUrl = DeviceInfo.Platform == DevicePlatform.WinUI ?
        "http://localhost:5038" : "http://10.0.2.2:5038";

    private readonly HttpClient _client;
    private string _token;
    private string _roomId;

    public ServerlessApi()
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri(_apiBaseUrl)
        };

        _token = string.Empty;
        _roomId = string.Empty;
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
        catch (Exception)
        {
        }

        return default;
    }

    public async Task EditProfileAsync(HostMessage profile)
    {
        profile.Token = _token;
        await postResponseOrDefault<bool>("/Host/EditProfile", profile);
    }

    public async Task<string> ChangeRoomPinCodeAsync()
    {
        var obj = new { Token = _token };
        return await postResponseOrDefault<string>("/Room/ChangePinCode", obj);
    }

    public async Task CloseRoomAsync()
    {
        var obj = new { Token = _token };
        await postResponseOrDefault<string>("/Host/CloseRoom", obj);
    }

    public async Task<HostMessage?> ConnectAsync(string username)
    {
        var obj = new { Username = username };
        var profile = await postResponseOrDefault<HostMessage>("/Host/Connect", obj);

        if (profile != null)
            _token = profile.Token;

        return profile;
    }

    public async Task CreatePollAsync(PollMessage pollRequest)
    {
        pollRequest.Token = _token;
        await postResponseOrDefault<bool>("/Room/CreatePoll", pollRequest);
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public async Task<HostMessage> FetchHostProfileAsync(string token)
    {
        var obj = new { Token = token };
        HostMessage profile = await postResponseOrDefault<HostMessage>("/Host/Get", obj);

        if (profile != null)
            _token = profile.Token;

        return profile;
    }

    public async Task<PollMessage> FetchPollAsync()
    {
        var obj = new { RoomId = _roomId };
        List<PollOption> poll = await postResponseOrDefault<List<PollOption>>("/Room/GetPoll", obj);

        if (poll == null)
            return null;

        return new PollMessage
        {
            Options = poll
        };
    }

    public async Task<RoomMessage> FetchRoomUpdateAsync()
    {
        var obj = new { RoomId = _roomId };
        RoomMessage room = await postResponseOrDefault<RoomMessage>("/Room/Get", obj);

        return room;
    }

    public string GetRoomId()
    {
        return _roomId;
    }

    public async Task<string> OpenRoomAsync()
    {
        var obj = new { Token = _token };
        RoomMessage room = await postResponseOrDefault<RoomMessage>("/Host/OpenRoom", obj);

        if (room != null)
            _roomId = room.RoomId;

        return _roomId;
    }

    public async Task RemovePollAsync()
    {
        var obj = new { Token = _token };
        await postResponseOrDefault<bool>("/Room/RemovePoll", obj);
    }

    public async Task UpdateSongAsync(SongMessage song)
    {
        var obj = new { };
        await postResponseOrDefault<bool>("/Room/ChangeSong", obj);
    }
}
