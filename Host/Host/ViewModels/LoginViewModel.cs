using Host.Services;
using System.Windows.Input;

namespace Host;
public class LoginViewModel : BaseViewModel
{
    private readonly IServerApi _serverAPI;
    private string _errorMsgHolder;

    public string Username { get; set; }
    public string ErrorMsgHolder
    {
        get => _errorMsgHolder;
        set
        {
            _errorMsgHolder = value;
            OnPropertyChanged(nameof(ErrorMsgHolder));
        }
    }

    public ICommand LoginCommand { get; set; }

    public LoginViewModel(IServerApi serverAPI)
    {
        _serverAPI = serverAPI;
        LoginCommand = new Command(connect);
    }

    private async void connect()
    {
        ErrorMsgHolder = string.Empty;

        if (await _serverAPI.ConnectAsync(Username) != null)
        {
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
        else
        {
            ErrorMsgHolder = "Error occurred";
        }
    }
}
