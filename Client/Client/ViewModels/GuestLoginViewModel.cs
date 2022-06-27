using Client.Services;
using System.Windows.Input;

namespace Client;

public class GuestLoginViewModel : BaseViewModel
{
    private readonly IServerApi _serverApi;

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

    public ICommand AnonymousLoginCommand { get; set; }

    public GuestLoginViewModel(IServerApi serverAPI)
    {
        _serverApi = serverAPI;
        AnonymousLoginCommand = new Command(connect);
    }

    private async void connect()
    {
        ErrorMsgHolder = string.Empty;
        try
        {
            if (await _serverApi.AnonymousLoginAsync(Username))
            {
                await Shell.Current.GoToAsync(nameof(FindHostPage));
            }
            else
            {
                ErrorMsgHolder = "Error occurred";
            }
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine(e.Message);
        }
       
    }
}
