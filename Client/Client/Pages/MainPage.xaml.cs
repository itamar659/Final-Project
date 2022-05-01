using System.Windows.Input;

namespace Client
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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // auto login if token available?
        }
	}
}