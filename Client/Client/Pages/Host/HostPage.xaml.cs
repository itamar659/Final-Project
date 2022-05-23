using Client.Pages.Host;

namespace Client;

public partial class HostPage : ContentPage
{
    public HostPage(HostViewModel vm)
    {
        InitializeComponent();

        // Bad Initialize. Only for Practice.
        // TODO: Add Host Object inorder to make it more expendabily
        vm.Host = "Clara";
        vm.Song = "Kendrick Lamar: ADHD";
        vm.ActiveUsers = "542";
        vm.Genre = "Pop & Rock";

        BindingContext = vm;
    }

    private async void Button_Clicked(object sender, EventArgs e)
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