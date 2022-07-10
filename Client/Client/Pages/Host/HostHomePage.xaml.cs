using Client.ViewModels;

namespace Client;

public partial class HostHomePage : ContentPage
{
    private HostHomePageViewModel _vm;
    public HostHomePage(HostHomePageViewModel vm)
    {
        InitializeComponent();

        _vm = vm;
        BindingContext = _vm;
    }

    private async void VoteBtn_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(HostLastPage));
    }

    private async void Left_Button_Clicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Leaving already?", "Are you sure wanna leave ?", "Yes", "No");
        if (answer)
        {
            await _vm.LeaveSessionAsync();
        }
    }
}