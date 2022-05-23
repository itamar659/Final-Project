namespace Client;

public partial class HostHomePage : ContentPage
{
    public HostHomePage(HostViewModel vm)
    {
        InitializeComponent();

        // Bad Initialize. Only for Practice.
        // TODO: Add Host Object inorder to make it more expendabily

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