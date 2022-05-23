using System.Windows.Input;

namespace Client;
public class FindHostPageViewModel : BaseViewModel
{
    private string _name;

    public string Password { get; set; }
    public string WelcomeMessage
    {
        get => _name;
        set
        {
            _name = "Welcome " + value;
            OnPropertyChanged(nameof(WelcomeMessage));
        }
    }

    public ICommand ViewHostPageCommand { get; set; }

    public FindHostPageViewModel()
    {
        ViewHostPageCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(HostPage)));
    }
}
