using System.Windows.Input;

namespace Client;
public class MainPageViewModel : BaseViewModel
{
    public ICommand SignInCommand { get; set; }
    public ICommand GuestLoginCommand { get; set; }

    public MainPageViewModel()
    {
        SignInCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(LoginPage)));
        GuestLoginCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(GuestLoginPage)));
    }
}
