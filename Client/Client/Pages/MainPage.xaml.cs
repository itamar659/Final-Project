using Client.Auth0;
using System.Security.Claims;

namespace Client;

public partial class MainPage : ContentPage
{
    private readonly MainPageViewModel _vm;
    private readonly Auth0Client auth0Client;

    public MainPage(MainPageViewModel vm, Auth0Client client)
    {
        InitializeComponent();

        _vm = vm;
        auth0Client = client;
        BindingContext = _vm;
	}

    protected override async void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        await _vm.TryAutoConnectAsync();
    }

    private async void AnonymousSignIn_Clicked(object sender, EventArgs e)
    {
        var res = await _vm.AnonymousConnectAsync();

        if (res.Item1 == true)
            await DisplayAlert("Error", res.Item2, "OK");
    }

    private async void SocialSignIn_Clicked(object sender, EventArgs e)
    {
        try
        {
            var loginResult = await auth0Client.LoginAsync();


            foreach (Claim claim in loginResult.User.Claims)
            {
                var str = "CLAIM TYPE: " + claim.Type + "; CLAIM VALUE: " + claim.Value;
            }

            if (loginResult.IsError || !await _vm.LoginAsync(loginResult.User.Identity.Name))
                    await DisplayAlert("We couldn't connect. try again later.", loginResult.ErrorDescription, "OK");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}