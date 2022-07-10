namespace Client;

public partial class HostFrontPage : ContentPage
{
    private HostFrontPageViewModel _vm;

    public HostFrontPage(HostFrontPageViewModel vm)
    {
        InitializeComponent();

        // Bad Initialize. Only for Practice.
        // TODO: Add Host Object inorder to make it more expendabily

        _vm = vm;
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    private async void JoinSession_Clicked(object sender, EventArgs e)
    {
        string result = await DisplayPromptAsync("2-Way Authentication",
                                                 "Enter Host's personal pincode. for further information please contact the host.",
                                                 keyboard: Keyboard.Numeric);


        if (await _vm.JoinSessionAsync(result) == false) // JoinSession links the client token with the session key for the current session
        {
            await DisplayAlert("Error", "Wrong pin code. please try again.", "OK");
        }
        else if (result == "1234")
        {
            await Shell.Current.GoToAsync(nameof(HostHomePage));
        }
    }
}