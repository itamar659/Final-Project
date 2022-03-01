namespace Project.UI;

public partial class GuestLoginPage : ContentPage
{
	public GuestLoginPage()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		var res = await FirebaseUserService.AuthProvider.SignInAnonymouslyAsync();
		FirebaseUserService.SessionToken = res.FirebaseToken;
		FirebaseUserService.SessionUser = res.User;

		Navigation.InsertPageBefore(new FindHostPage(), Navigation.NavigationStack[0]);
		await Navigation.PopToRootAsync();
        //App.Current.MainPage = new NavigationPage(new FindHostPage());
	}
}