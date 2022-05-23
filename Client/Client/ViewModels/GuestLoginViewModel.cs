using Client.Services;
using System.Windows.Input;

namespace Client;

public class GuestLoginViewModel : BaseViewModel
{
    private readonly IServerApi _serverApi;

    private string _errorMsgHolder;

    public string Password { get; set; }
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

    public GuestLoginViewModel(IServerApi serverAPI)
    {
        _serverApi = serverAPI;
        LoginCommand = new Command(connect);
    }

    private async void connect()
    {
        ErrorMsgHolder = string.Empty;

        if (await _serverApi.ConnectAsync(Password))
        {
            await Shell.Current.GoToAsync(nameof(FindHostPage));
        }
        else
        {
            ErrorMsgHolder = "Error occurred";
        }
    }
}
