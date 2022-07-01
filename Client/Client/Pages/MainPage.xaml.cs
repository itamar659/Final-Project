namespace Client;

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

    private void GoogleSignIn_Clicked(object sender, EventArgs e)
    {

    }
}