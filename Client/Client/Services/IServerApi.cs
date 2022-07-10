using Client.Models.Responses;

namespace Client.Services;

public interface IServerApi : IDisposable
{
    Task<bool> LoginAsync(string password);
    Task<bool> AnonymousLoginAsync(string username);
    Task<bool> JoinSessionAsync(string ownerName, string pinCode);
    Task LeaveSessionAsync();
    Task<JukeboxSessionResponse> FetchSessionDetailsAsync(string sessionKey = null);
    Task<List<JukeboxSessionResponse>> FetchAvailableSessionsAsync();
    Task<JukeboxPollResponse> FetchPollAsync(string sessionKey);
    Task<bool> VoteAsync(int voteOption);
}
