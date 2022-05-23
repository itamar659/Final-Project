using Client.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services;

public interface IServerApi : IDisposable
{
    Task<bool> ConnectAsync(string password); // [X] Host controller
    Task<bool> OpenSessionAsync(); // [X] Host controller
    Task CloseSessionAsync(); // [X] Host controller
    string GetSessionKey(); // [X] Host controller
    Task<JukeboxSessionResponse> FetchSessionUpdateAsync();

    // ADD voting
}
