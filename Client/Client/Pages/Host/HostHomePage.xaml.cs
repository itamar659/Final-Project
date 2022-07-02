using Client.ViewModels;

namespace Client;

public partial class HostHomePage : ContentPage
{
    public HostHomePage(HostHomePageViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(HostLastPage));
    }

    private async void Left_Button_Clicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Leaving already?", "Are you sure wanna leave ?", "Yes", "No");
        if (answer)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}