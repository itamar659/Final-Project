using HostUserShare;
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

            //FirebaseUserService.SessionToken = ""; // TODO: For testing. Remove.

            SignInCommand = new Command(async () => await Navigation.PushAsync(new LoginPage()));
            GuestLoginCommand = new Command(async () => await Navigation.PushAsync(new GuestLoginPage()));

            BindingContext = this;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

			// Try login via token.
			if (FirebaseUserService.IsTokenValid())
            {

                Navigation.InsertPageBefore(new FindHostPage(), Navigation.NavigationStack[0]);
                Navigation.PopToRootAsync();

				//App.Current.MainPage = new NavigationPage(new FindHostPage());
            }
        }
	}
}