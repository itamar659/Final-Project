namespace Client;

public partial class FindHostPage : ContentPage
{
    public FindHostPage(FindHostPageViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }

    public async void on_click(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(HostPage)}?SessionHostName=[OWNER_NAME]");
    }
}