using Client.Auth0;

namespace Client.Pages;

public partial class LogoutPage : ContentPage
{
    private readonly Auth0Client auth0Client;

    public LogoutPage(Auth0Client client)
    {
        InitializeComponent();

        auth0Client = client;

        logout();
    }

    private async void logout()
    {
        // Do logout logic
        var logoutResult = await auth0Client.LogoutAsync();

        if (!logoutResult.IsError)
        {
            Configuration.Token = string.Empty;
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
        else
        {
            await DisplayAlert("We couldn't connect. try again later.", logoutResult.ErrorDescription, "OK");
        }
    }
}