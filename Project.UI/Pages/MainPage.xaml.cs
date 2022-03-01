using System.Windows.Input;

namespace Project.UI
{
    public partial class MainPage : ContentPage
    {
        public ICommand SignInCommand { get; set; }

        public ICommand GuestLoginCommand { get; set; }

        public MainPage()
        {
            InitializeComponent();

            SignInCommand = new Command(async () => await Navigation.PushAsync(new LoginPage()));
            GuestLoginCommand = new Command(async () => await Navigation.PushAsync(new GuestLoginPage()));

            BindingContext = this;
		}
		protected override void OnNavigatedTo(NavigatedToEventArgs args)
		{
			base.OnNavigatedTo(args);

			// Try login via token.
			FirebaseUserService.SessionUser = null;
			if (tryTokenLogin())
				App.Current.MainPage = new NavigationPage(new FindHostPage());
		}

		private bool tryTokenLogin()
		{
			if (string.IsNullOrEmpty(FirebaseUserService.SessionToken))
				return false;

			return Task.Run(isTokenValid).Result;
		}

		private async Task<bool> isTokenValid()
		{
			try
			{
				var user = await FirebaseUserService.AuthProvider.GetUserAsync(FirebaseUserService.SessionToken);
				FirebaseUserService.SessionUser = user;
				return true;
			}
			catch (Exception)
			{
				FirebaseUserService.SessionToken = string.Empty;
			}

			return false;
		}

	}
}