using Firebase.Auth;
using Firebase.Database;
using System.Diagnostics;
using System.Windows.Input;

namespace Project.UI;

public partial class LoginPage : ContentPage
{
    public ICommand ForgotPasswordCommand { get; set; }
    public ICommand SignUpCommand { get; private set; }

	public LoginPage()
	{
		InitializeComponent();

        ForgotPasswordCommand = new Command(() => { });

        SignUpCommand = new Command(async () => await Navigation.PushAsync(new SignUpPage()));

		// Binding should be last.
		BindingContext = this;
	}

	private async void LoginBtn_Clicked(object sender, EventArgs e)
	{
		try
		{
			var res = await FirebaseUserService.AuthProvider.SignInWithEmailAndPasswordAsync(UsernameEntry.Text + "@MyApp.com", PasswordEntry.Text);

			FirebaseUserService.SessionToken = res.FirebaseToken;
			FirebaseUserService.SessionUser = res.User;

			App.Current.MainPage = new NavigationPage(new FindHostPage());
		}
		catch (FirebaseAuthException ex)
		{
			switch (ex.Reason)
			{
				case AuthErrorReason.InvalidEmailAddress:
				case AuthErrorReason.MissingEmail:
				case AuthErrorReason.UnknownEmailAddress:
					await App.Current.MainPage.DisplayAlert("Error", "Email doesn't exists.", "Ok");
					break;
				case AuthErrorReason.MissingPassword:
				case AuthErrorReason.WrongPassword:
					await App.Current.MainPage.DisplayAlert("Error", "Wrong email/password.", "Ok");
					break;
				default:
					throw ex;
			}
		}
	}
}