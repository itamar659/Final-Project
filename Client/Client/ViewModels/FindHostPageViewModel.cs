using Client.Models;
using Client.Models.ServerMessages;
using Client.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Client;

public class FindHostPageViewModel : BaseViewModel
{
    private readonly IServerApi _serverApi;
    private string _name;

    public ObservableCollection<Room> FavoriteHosts { get; set; }
    public ObservableCollection<Room> AvailableRooms { get; set; }

    public string WelcomeMessage
    {
        get => _name;
        set
        {
            _name = "Welcome " + value;
            OnPropertyChanged(nameof(UserProfile.Username));
        }
    }

    public ICommand ViewHostPageCommand { get; set; }

    public FindHostPageViewModel(IServerApi serverApi)
    {
        _serverApi = serverApi;
        FavoriteHosts = new ObservableCollection<Room>();
        AvailableRooms = new ObservableCollection<Room>();

        _name = "Welcome " + UserProfile.Instance.Username;

        InitFavoriteHosts();

        ViewHostPageCommand = new Command(async (host) =>
        {
            if (host is Room hs)
            {
                await Shell.Current.GoToAsync($"{nameof(HostFrontPage)}?RoomId={hs.RoomId}");
            }
        });

        updateAvailableSessionsAsync();
    }

    private async void updateAvailableSessionsAsync()
    {
        List<RoomMessage> availableRooms = null;

        try
        {
             availableRooms = await _serverApi.FetchOpenedRoomsAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine("Couldn't fetch Available Hosts");
        }
       
        AvailableRooms.Clear();

        if (availableRooms != null)
        {
            foreach (var room in availableRooms)
            {
                AvailableRooms.Add(new Room
                {
                    RoomId = room.RoomId,

                    OnlineUsers = room.OnlineUsers,
                    Name = room.Hostname,
                    IsOnline = true,
                    Picture = "host_icon",
                    StatusComment = "Online"
                });
            }
            if(availableRooms.Count == 0)
            {
                addNotAvailableHost();
            }
        }
        else
        {
            addNotAvailableHost();
        }

    }

    private void addNotAvailableHost()
    {
        AvailableRooms.Add(new Room
        {
            RoomId = "",
            OnlineUsers = 0,
            Name = "No Hosts Available",
            Picture = "error_icon",
            StatusComment = "Try again later."
        });
    }

    private void InitFavoriteHosts()
    {
        FavoriteHosts.Clear();
        FavoriteHosts.Add(new Room
        {
            RoomId = "",
            OnlineUsers = 12,
            Name = "ADD",
            IsOnline = false,
            Picture = "default_favorite_host",
            StatusComment = "Add favorite host"
        });
    }
}