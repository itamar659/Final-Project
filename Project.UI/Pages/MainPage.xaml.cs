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

			FirebaseUserService.SessionToken = "";

            SignInCommand = new Command(async () => await Navigation.PushAsync(new LoginPage()));
            GuestLoginCommand = new Command(async () => await Navigation.PushAsync(new GuestLoginPage()));

            BindingContext = this;
		}
		protected override void OnNavigatedTo(NavigatedToEventArgs args)
		{
			base.OnNavigatedTo(args);

			// Try login via token.
			if (FirebaseUserService.IsTokenValid())
				App.Current.MainPage = new NavigationPage(new FindHostPage());
		}
	}
}