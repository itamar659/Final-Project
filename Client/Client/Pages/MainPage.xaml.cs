using Client.Auth0;

namespace Client;

public partial class MainPage : ContentPage
{
    private MainPageViewModel _vm;
    private readonly Auth0Client auth0Client;

    public MainPage(MainPageViewModel vm, Auth0Client client)
    {
        InitializeComponent();

        _vm = vm;
        auth0Client = client;
        BindingContext = _vm;
	}

    private async void AnonymousSignIn_Clicked(object sender, EventArgs e)
    {
        var res = await _vm.AnonymousConnectAsync();

        if (res.Item1 == true)
            await DisplayAlert("Error", res.Item2, "OK");
    }

    private async void AppleFakeSignIn_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync(nameof(FindHostPage));
        }
        catch(Exception ex) 
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async void GoogleSignIn_Clicked(object sender, EventArgs e)
    {
        try
        {
            var loginResult = await auth0Client.LoginAsync();

            if (!loginResult.IsError)
            {
                await Shell.Current.GoToAsync($"//{nameof(FindHostPage)}?WelcomeMessage={loginResult.User.Identity.Name}");
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
        //await Shell.Current.GoToAsync($"//{nameof(FindHostPage)}");
    }
}