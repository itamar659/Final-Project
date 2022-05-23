namespace Client;

public partial class GuestLoginPage : ContentPage
{
	public GuestLoginPage(GuestLoginViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}