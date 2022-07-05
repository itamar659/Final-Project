﻿using Client.Models;
using Client.Models.Responses;
using Client.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Client;
public class FindHostPageViewModel : BaseViewModel
{
    private readonly IServerApi _serverApi;
    private string _name;

    public ObservableCollection<Host> FavoriteHosts { get; set; }
    public ObservableCollection<Host> AvailableHosts { get; set; }

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
        FavoriteHosts = new ObservableCollection<Host>();
        AvailableHosts = new ObservableCollection<Host>();

        InitFavoriteHosts();

        ViewHostPageCommand = new Command(async (host) =>
        {
            if (host is Host hs)
            {
                await Shell.Current.GoToAsync($"{nameof(HostFrontPage)}?SessionKey={hs.SessionKey}");
            }
        });

        updateAvailableSessions();
    }

    private async void updateAvailableSessions()
    {
        List<JukeboxSessionResponse> availableSessions = await _serverApi.FetchAvailableSessionsAsync();

        if (availableSessions == null)
            return;

        AvailableHosts.Clear();
        foreach (var session in availableSessions)
        {
            AvailableHosts.Add(new Host
            {
                SessionKey = session.SessionKey,

                OnlineUsers = session.ActiveUsers,
                Name = session.OwnerName,
                IsOnline = true,
                Picture = "host_icon",
                StatusComment = "Online"
            });
        }
    }

    private void InitFavoriteHosts()
    {
        FavoriteHosts.Clear();
        FavoriteHosts.Add(new Host
        {
            SessionKey = "",
            OnlineUsers = 12,
            Name = "ADD",
            IsOnline = false,
            Picture = "default_favorite_host",
            StatusComment = "Add favorite host"
        });
    }

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
