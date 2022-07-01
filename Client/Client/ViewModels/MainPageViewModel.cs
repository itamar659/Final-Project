using Client.Services;
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
                await Shell.Current.GoToAsync(nameof(FindHostPage));
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
}
