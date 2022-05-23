namespace Client;

public partial class HostPage : ContentPage
{
    private HostViewModel _vm;

    public HostPage(HostViewModel vm)
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

        Dispatcher.DispatchDelayed(TimeSpan.Zero, _vm.FetchSessionDetails);
    }

    private async void JoinSession_Clicked(object sender, EventArgs e)
    {
        string result = await DisplayPromptAsync("2-Way Authentication", "Enter Host's personal pincode. for further information please contact the host.", keyboard: Keyboard.Numeric);

        // TODO: make it dynamic password inside of the host object.
        if (result == "1234")
        {
            await Shell.Current.GoToAsync(nameof(HostHomePage));
        }
        else
        {
            await DisplayAlert("Error", "Wrong Password. please try again.", "OK");
        }
    }
}