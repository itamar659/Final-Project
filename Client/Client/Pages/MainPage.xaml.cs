﻿namespace Client;

public partial class MainPage : ContentPage
{
    private MainPageViewModel _vm;

    public MainPage(MainPageViewModel vm)
    {
        InitializeComponent();

        _vm = vm;
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
            //Uri uri = new Uri("https://www.google.com");
            //await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);

            WebAuthenticatorResult authResult = await WebAuthenticator.Default.AuthenticateAsync(
                    new Uri("https://mysite.com/mobileauth/Microsoft"),
                    new Uri("myapp://")
                    );

            string accessToken = authResult?.AccessToken;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        //await Shell.Current.GoToAsync($"//{nameof(FindHostPage)}");
    }
}