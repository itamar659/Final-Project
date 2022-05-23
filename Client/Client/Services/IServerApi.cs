using Client.Models.Responses;

namespace Client.Services;

public interface IServerApi : IDisposable
{
    Task<bool> LoginAsync(string password);
    Task<bool> AnonymousLoginAsync(string username);
    Task<bool> JoinSessionAsync(string ownerName);
    Task LeaveSessionAsync();
    Task<JukeboxSessionResponse> FetchSessionDetailsAsync();
    Task<List<string>> FetchAvailableSessionsAsync();

    // ADD voting
}
