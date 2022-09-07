using Client.Auth0;

namespace Client;

public partial class MainPage : ContentPage
{
    private MainPageViewModel _vm;
    private readonly Auth0Client auth0Client;
    private readonly UserSingleton _user;

    public MainPage(MainPageViewModel vm, Auth0Client client, UserSingleton user)
    {
        InitializeComponent();

        _vm = vm;
        auth0Client = client;
        _user = user;
        BindingContext = _vm;
	}

    private async void AnonymousSignIn_Clicked(object sender, EventArgs e)
    {
        var res = await _vm.AnonymousConnectAsync();

        if (res.Item1 == true)
            await DisplayAlert("Error", res.Item2, "OK");
    }

    private async void GoogleSignIn_Clicked(object sender, EventArgs e)
    {
        try
        {
            var loginResult = await auth0Client.LoginAsync();

            if (!loginResult.IsError)
            {
                if (!await _vm.LoginAsync(loginResult.User.Identity.Name))
                {
                    await DisplayAlert("We couldn't connect. try again later.", loginResult.ErrorDescription, "OK");
                }
                else
                {
                    _user.fillUser(loginResult);
                }
            }
            else
            {
                await DisplayAlert("We couldn't connect. try again later.", loginResult.ErrorDescription, "OK");
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}