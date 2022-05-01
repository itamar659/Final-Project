namespace Client;

public partial class GuestLoginPage : ContentPage
{
	public GuestLoginPage()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		//Navigation.InsertPageBefore(new FindHostPage(entry.Text), Navigation.NavigationStack[0]);
		//await Navigation.PopToRootAsync();
        App.Current.MainPage = new NavigationPage(new FindHostPage(entry.Text));
    }
}