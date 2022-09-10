using Client.ViewModels;

namespace Client;

public partial class HostHomePage : ContentPage
{
    private HostHomePageViewModel _vm;
    public HostHomePage(HostHomePageViewModel vm)
    {
        InitializeComponent();

        _vm = vm;
        BindingContext = vm;
    }

    private async void Left_Button_Clicked(object sender, EventArgs e)
    {
        if (await DisplayAlert("Leaving already?",
                               "Are you sure wanna leave ?",
                               "Yes", "No"))
        {
            await _vm.LeaveSessionAsync();
        }
    }
}