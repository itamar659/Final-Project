using System.Windows.Input;

namespace Host;
public class LoginViewModel : BaseViewModel
{
    private IServerAPI _serverAPI;
    private string _errorMsgHolder;

    public string Token { get; set; }
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

    public LoginViewModel(IServerAPI serverAPI)
    {
        _serverAPI = serverAPI;
        LoginCommand = new Command(connect);
    }

    private void connect()
    {
        ErrorMsgHolder = string.Empty;

        if (_serverAPI.ConnectAsync(Token))
        {
        }
        else
        {
            ErrorMsgHolder = "Error occurred";
        }
    }
}
