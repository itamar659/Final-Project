using Client.Models;
using Client.Models.ServerMessages;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Client.Services;

public class ServerlessApi : IServerApi
{
    private readonly string _apiBaseUrl = Configuration.DevelopServer;

    private readonly HttpClient _client;
    private string _token;

    public ServerlessApi()
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri(_apiBaseUrl)
        };

        _token = string.Empty;
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

    public async Task<ClientMessage> LoginAsync(string username)
    {
        var obj = new { Username = username };
        ClientMessage client = await postResponseOrDefault<ClientMessage>("/Client/Login", obj);

        if (client is not null)
            _token = client.Token;

        return client;
    }

    public async Task<ClientMessage> AnonymousLoginAsync(string username)
    {
        var obj = new { Username = username };
        ClientMessage client = await postResponseOrDefault<ClientMessage>("/Client/AnonymousLogin", obj);

        if (client is not null)
            _token = client.Token;

        return client;
    }

    public async Task<bool> JoinRoomAsync(string roomId, string pinCode)
    {
        var obj = new { Token = _token, RoomId = roomId, PinCode = pinCode };
        bool isJoined = await postResponseOrDefault<bool>("/Client/JoinRoom", obj);

        return isJoined;
    }

    public async Task LeaveRoomAsync()
    {
        var obj = new { Token = _token };
        await postResponseOrDefault<bool>("/Client/LeaveRoom", obj);
    }

    public async Task<ClientMessage> FetchClientProfileAsync(string token)
    {
        var obj = new { Token = token };
        ClientMessage client = await postResponseOrDefault<ClientMessage>("/Client/Get", obj);

        if (client is not null)
            _token = client.Token;

        return client;
    }

    public async Task<RoomMessage> FetchRoomUpdateAsync(string roomId)
    {
        var obj = new { RoomId = roomId };
        RoomMessage room = await postResponseOrDefault<RoomMessage>("/Room/Get", obj);

        return room;
    }

    public async Task<List<RoomMessage>> FetchOpenedRoomsAsync()
    {
        List<RoomMessage> rooms = await postResponseOrDefault<List<RoomMessage>>("/Room/OpenRooms", null);

        return rooms;
    }

    public async Task<PollMessage> FetchPollAsync(string roomId)
    {
        var obj = new { RoomId = roomId };
        List<PollOption> pollOptions = await postResponseOrDefault<List<PollOption>>("/Room/GetPoll", obj);

        if (pollOptions == null)
            return null;

        return new PollMessage
        {
            Options = pollOptions
        };
    }

    public async Task VoteAsync(int pollId)
    {
        var obj = new { Token = _token, pollId = pollId };
        await postResponseOrDefault<bool>("/Room/Vote", obj);
    }

    void IDisposable.Dispose()
    {
        _client.Dispose();
    }
}
