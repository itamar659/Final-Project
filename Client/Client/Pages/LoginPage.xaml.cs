using Client.Auth0;

namespace Client;

public partial class LoginPage : ContentPage
{
    private readonly Auth0Client auth0Client;

    public LoginPage(Auth0Client client)
	{
		InitializeComponent();

        auth0Client = client;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var loginResult = await auth0Client.LoginAsync();

        if (!loginResult.IsError)
        {
            LoginView.IsVisible = false;
            HomeView.IsVisible = true;
        }
        else
        {
            await DisplayAlert("We couldn't connect. try again later.", loginResult.ErrorDescription, "OK");
        }
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        var logoutResult = await auth0Client.LogoutAsync();

        if (!logoutResult.IsError)
        {
            HomeView.IsVisible = false;
            LoginView.IsVisible = true;
        }
        else
        {
            await DisplayAlert("We couldn't connect. try again later.", logoutResult.ErrorDescription, "OK");
        }
    }
}