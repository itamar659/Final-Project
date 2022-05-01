using System.Windows.Input;

namespace Client;

public partial class LoginPage : ContentPage
{
    public ICommand ForgotPasswordCommand { get; set; }

	public LoginPage()
	{
		InitializeComponent();

        ForgotPasswordCommand = new Command(() => { });

		// Binding should be last.
		BindingContext = this;
	}

	private void LoginBtn_Clicked(object sender, EventArgs e)
	{
		App.Current.MainPage = new NavigationPage(new FindHostPage("Test Username"));
	}
}