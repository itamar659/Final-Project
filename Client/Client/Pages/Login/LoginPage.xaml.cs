using System.Windows.Input;

namespace Client;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel vm)
	{
		InitializeComponent();

		// Binding should be last.
		BindingContext = vm;
	}

	private void LoginBtn_Clicked(object sender, EventArgs e)
	{
	}
}