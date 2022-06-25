using Client.ViewModels;

namespace Client.Pages.Users;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(SongsViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}