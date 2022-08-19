using Client.Services;
using IdentityModel.OidcClient;
using Microsoft.Maui.Dispatching;
using System.Windows.Input;

namespace Client;
public class MainPageViewModel : BaseViewModel
{
    private readonly IServerApi _serverApi;

    public string Username { get; set; }

    public MainPageViewModel(IServerApi serverAPI)
    {
        //Preferences.Default.Set("Token", 123); // Save token for next startup

        _serverApi = serverAPI;
        //SignInCommand = new Command(async () => await Shell.Current.GoToAsync($"{nameof(FindHostPage)}"));
    }

    public async Task<Tuple<bool, string>> AnonymousConnectAsync()
    {
        string error = string.Empty;
        bool errorOccured = false;

        try
        {
            if (await _serverApi.AnonymousLoginAsync(Username))
            {
                await Shell.Current.GoToAsync($"//{nameof(FindHostPage)}?WelcomeMessage={Username}");
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

    public async Task<bool> LoginAsync(string username)
    {
        try
        {
            if (await _serverApi.LoginAsync(username))
            {
                await Shell.Current.GoToAsync($"//{nameof(FindHostPage)}?WelcomeMessage={username}");
            }
            else if (await _serverApi.CreateAsync(username))
            {
                await Shell.Current.GoToAsync($"//{nameof(FindHostPage)}?WelcomeMessage={username}");
            }
            else
            {
                return false;
            }
        }
        catch (NullReferenceException e)
        {
            return false;
        }

        return true;
    }
}
