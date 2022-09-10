namespace Client;

public partial class HostFrontPage : ContentPage
{
    private HostFrontPageViewModel _vm;

    public HostFrontPage(HostFrontPageViewModel vm)
    {
        InitializeComponent();

        _vm = vm;
        BindingContext = vm;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        // TODO: breackpoint - check if the RoomId query already injected.
        await _vm.FetchRoomDetailsAsync();
    }

    private async void JoinSession_Clicked(object sender, EventArgs e)
    {
        string pinCode = await DisplayPromptAsync("2-Way Authentication",
                                                 "Enter Host's personal pincode. for further information please contact the host.",
                                                 keyboard: Keyboard.Numeric);

        // JoinSession links the client token with the session key for the current session
        if (await _vm.JoinRoomAsync(pinCode) == false)
            await DisplayAlert("Error", "Wrong pin code. please try again.", "OK");
    }
}