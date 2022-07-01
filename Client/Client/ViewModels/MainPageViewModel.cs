using Client.Services;
using System.Windows.Input;

namespace Client;
public class MainPageViewModel : BaseViewModel
{
    public ICommand SignInCommand { get; set; }
    public ICommand GuestLoginCommand { get; set; }


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

    //public MainPageViewModel()
    //{
    //    //SignInCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(LoginPage)));
    //    //GuestLoginCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(GuestLoginPage)));
    //}    public ICommand AnonymousLoginCommand { get; set; }

    public ICommand AnonymousLoginCommand { get; set; }


    public MainPageViewModel(IServerApi serverAPI)
    {
        _serverApi = serverAPI;
        AnonymousLoginCommand = new Command(connect);
        SignInCommand = new Command(async () => await Shell.Current.GoToAsync($"{nameof(FindHostPage)}"));
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
