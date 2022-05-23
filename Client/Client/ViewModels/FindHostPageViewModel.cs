using Client.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Client;
public class FindHostPageViewModel : BaseViewModel
{
    private readonly IServerApi _serverApi;
    private string _name;

    public ObservableCollection<string> AvailableSessions { get; }

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

    public FindHostPageViewModel(IServerApi serverApi)
    {
        _serverApi = serverApi;
        AvailableSessions = new ObservableCollection<string>();

        ViewHostPageCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(HostPage)));

        updateAvailableSessions();
    }

    private async void updateAvailableSessions()
    {
        List<string> availableSessions = await _serverApi.FetchAvailableSessionsAsync();

        if (availableSessions == null)
            return;

        AvailableSessions.Clear();
        foreach (var sessionName in availableSessions)
            AvailableSessions.Add(sessionName);
    }
}
