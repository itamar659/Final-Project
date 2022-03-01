using Firebase.Auth;
using System.Windows.Input;

namespace Project.UI;

public partial class SignUpPage : ContentPage
{
	public ICommand LoginCommand { get; set; }

	public SignUpPage()
	{
		InitializeComponent();

		BindingContext = this;

		LoginCommand = new Command(async () => await Navigation.PopAsync());
	}

	private async void SignUpBtn_Clicked(object sender, EventArgs e)
	{
		try
		{
			var res = await FirebaseUserService.AuthProvider.CreateUserWithEmailAndPasswordAsync(UsernameEntry.Text + "@MyApp.com", PasswordEntry.Text);

			FirebaseUserService.SessionToken = res.FirebaseToken;
			FirebaseUserService.SessionUser = res.User;

			await Navigation.PopAsync();
		}
		catch (FirebaseAuthException ex)
        {
			switch (ex.Reason)
            {
				case AuthErrorReason.EmailExists:
					await App.Current.MainPage.DisplayAlert("Error", "Email already exists.", "Ok");
					break;
				default:
					throw ex;
            }
		}
	}
}