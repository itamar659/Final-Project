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
    private readonly string _apiBaseUrl = "https://csharp-project.azurewebsites.net/jukeboxhosts/";

    private HttpClient _client;

    public ServerlessApi()
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri(_apiBaseUrl)
        };
    }

    async Task<bool> IServerApi.ConnectAsync(string token)
    {
        try
        {
            var response = await _client.GetAsync(token);
            if (response.IsSuccessStatusCode)
            {
                var contentStr = await response.Content.ReadFromJsonAsync<JukeboxHostResponse>();

                return true;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex); // TODO: For Testing only, not for production
        }
        return false;
    }

    void IDisposable.Dispose()
    {
        _client.Dispose();
    }

    int IServerApi.FetchActiveUsers()
    {
        throw new NotImplementedException();
    }

    object IServerApi.FetchLastVote()
    {
        throw new NotImplementedException();
    }

    TimeSpan IServerApi.FetchSessionTime()
    {
        throw new NotImplementedException();
    }

    int IServerApi.FetchTotalUsers()
    {
        throw new NotImplementedException();
    }

    bool IServerApi.IsSessionLive()
    {
        throw new NotImplementedException();
    }

    void IServerApi.SetSessionPinCode(int pinCode)
    {
        throw new NotImplementedException();
    }

    void IServerApi.StartSession()
    {
        throw new NotImplementedException();
    }

    void IServerApi.StopSession()
    {
        throw new NotImplementedException();
    }

    void IServerApi.UpdateSong(object song)
    {
        throw new NotImplementedException();
    }
}
