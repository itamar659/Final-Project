using Client.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services;

public interface IServerApi : IDisposable
{
    Task<bool> LoginAsync(string password);
    Task<bool> AnonymousLoginAsync(string username);
    Task<bool> JoinSessionAsync();
    Task LeaveSessionAsync();
    Task<JukeboxSessionResponse> FetchSessionUpdateAsync();
    Task<List<string>> FetchAvailableSessionsAsync();

    // ADD voting
}
