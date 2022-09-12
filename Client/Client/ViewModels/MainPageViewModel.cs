using Client.Models.ServerMessages;
using Client.Services;

namespace Client;
public class MainPageViewModel : BaseViewModel
{
    private readonly IServerApi _serverApi;

    public MainPageViewModel(IServerApi serverAPI)
    {
        _serverApi = serverAPI;
    }

    public string Username { get; set; }

    public async Task<bool> LoginAsync(string username)
    {
        try
        {
            var profile = await _serverApi.LoginAsync(username);

            if (profile is not null)
            {
                await changePageAfterLogin(profile);
            }
            else
            {
                return false;
            }
        }
        catch (NullReferenceException)
        {
            return false;
        }

        return true;
    }

    public async Task<Tuple<bool, string>> AnonymousConnectAsync()
    {
        string error = string.Empty;
        bool errorOccured = false;

        try
        {
            var profile = await _serverApi.AnonymousLoginAsync(Username);

            if (profile is not null)
            {
                await changePageAfterLogin(profile);
            }
            else
            {
                error = "Error occurred. Please change the username.";
                errorOccured = true;
            }
        }
        catch (NullReferenceException e)
        {
            error = e.Message;
            errorOccured = true;
        }

        return new (errorOccured, error);
    }

    public async Task TryAutoConnectAsync()
    {
        if (Configuration.HasToken)
        {
            var profile = await _serverApi.FetchClientProfileAsync(Configuration.Token);
            if (profile != null)
            {
                await changePageAfterLogin(profile);
            }
        }
    }

    private async Task changePageAfterLogin(ClientMessage profile)
    {
        Configuration.Token = profile.Token;
        UserProfile.Instance.Username = profile.Username;
        UserProfile.Instance.Token = profile.Token;
        UserProfile.Instance.AvatarUrl = profile.AvatarUrl;
        await Shell.Current.GoToAsync($"//{nameof(FindHostPage)}");
    }
}
