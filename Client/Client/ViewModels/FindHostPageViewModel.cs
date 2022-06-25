using Client.Models;
using Client.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Client;
public class FindHostPageViewModel : BaseViewModel
{
    private readonly IServerApi _serverApi;
    private string _name;

    public ObservableCollection<string> AvailableSessions { get; }
    public ObservableCollection<Host> hosts { get; set; }
    public ObservableCollection<HostStatus> hostStatus { get; set; }

    //public FindHostPageViewModel()
    //{
        

    //    hostStatus = new ObservableCollection<HostStatus>()
    //        {
    //            new HostStatus { Picture = "host_icon", Name="Alice" , StatusComment="Online", IsOnline=true},
    //            new HostStatus { Picture = "avatar_icon", Name="Pauline" , StatusComment="Online", IsOnline=true},
    //            new HostStatus { Picture = "avatar_icon", Name="Martin" , StatusComment="Online", IsOnline=true},
    //            new HostStatus { Picture = "avatar_icon", Name="Fanny" , StatusComment="Last activity 35 min", IsOnline=false },
    //            new HostStatus { Picture = "host_icon", Name="Celine" , StatusComment="Last activity 25 min", IsOnline=false}
    //        };

    //}

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

    public ICommand ViewHostPage { get; set; }

    public FindHostPageViewModel(IServerApi serverApi)
    {
        _serverApi = serverApi;
        AvailableSessions = new ObservableCollection<string>();

        ViewHostPageCommand = new Command(async () =>
        {

            if (await _serverApi.JoinSessionAsync("[OWNER_NAME]"))
                await Shell.Current.GoToAsync($"{nameof(HostPage)}?SessionHostName=[OWNER_NAME]");

        });

        ViewHostPage = new Command(async () =>
        {
            await Shell.Current.GoToAsync($"{nameof(HostPage)}?SessionHostName=[OWNER_NAME]");

        });

        hosts = new ObservableCollection<Host>()
            {
                new Host{ Name ="Carla Bruni", Picture="avatar_icon"},
                new Host{ Name ="Dizi", Picture="host_icon"}
            };

        updateAvailableSessions();
    }

    private async void updateAvailableSessions()
    {
        List<string> availableSessions = await _serverApi.FetchAvailableSessionsAsync();

        if (availableSessions == null)
            return;

        AvailableSessions.Clear();
        foreach (var sessionName in availableSessions)
        {
            AvailableSessions.Add(sessionName);
            hostStatus.Add(new HostStatus { Name = sessionName, IsOnline = true, Picture = "host_icon", StatusComment = "Online" });
        }

    }
}


//private readonly IServerApi _serverApi;
//private string _name;

//public ObservableCollection<string> AvailableSessions { get; }

//public string WelcomeMessage
//{
//    get => _name;
//    set
//    {
//        _name = "Welcome " + value;
//        OnPropertyChanged(nameof(WelcomeMessage));
//    }
//}

//public ICommand ViewHostPageCommand { get; set; }

//public FindHostPageViewModel(IServerApi serverApi)
//{
//    _serverApi = serverApi;
//    AvailableSessions = new ObservableCollection<string>();

//    ViewHostPageCommand = new Command(async () => {

//        if (await _serverApi.JoinSessionAsync("[OWNER_NAME]"))
//            await Shell.Current.GoToAsync($"{nameof(HostPage)}?SessionHostName=[OWNER_NAME]");

//        });

//    updateAvailableSessions();
//}

//private async void updateAvailableSessions()
//{
//    List<string> availableSessions = await _serverApi.FetchAvailableSessionsAsync();

//    if (availableSessions == null)
//        return;

//    AvailableSessions.Clear();
//    foreach (var sessionName in availableSessions)
//        AvailableSessions.Add(sessionName);
//}
