using Client.Models;
using Client.Models.ServerMessages;
using Client.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Client;

public class FindHostPageViewModel : BaseViewModel
{
    private readonly IServerApi _serverApi;
    private string _welcomeMessage;
    private bool _isRefreshing;

    public ObservableCollection<Room> FavoriteHosts { get; set; }
    public ObservableCollection<Room> AvailableRooms { get; set; }
    public ObservableCollection<Room> FilterAvailableRooms { get; set; }

    public string WelcomeMessage
    {
        get => _welcomeMessage;
        set
        {
            _welcomeMessage = "Welcome " + value;
            OnPropertyChanged(nameof(UserProfile.Username));
        }
    }

    public bool IsRefreshing
    {
        get { return _isRefreshing; }
        set
        {
            _isRefreshing = value;
            OnPropertyChanged(nameof(IsRefreshing));
        }
    }

    public ICommand ViewHostPageCommand { get; set; }

    public ICommand updateAvailableRoomsCommand { get; set; }

    public ICommand SearchRoomCommand { get; set; }

    public FindHostPageViewModel(IServerApi serverApi)
    {
        _serverApi = serverApi;
        FavoriteHosts = new ObservableCollection<Room>();
        AvailableRooms = new ObservableCollection<Room>();
        FilterAvailableRooms = new ObservableCollection<Room>();

        _welcomeMessage = "Welcome " + UserProfile.Instance.Username;

        initFavoriteHosts();

        ViewHostPageCommand = new Command(async (host) =>
        {
            if (host is Room hs)
            {
                await Shell.Current.GoToAsync($"{nameof(HostFrontPage)}?RoomId={hs.RoomId}");
            }
        });

        updateAvailableRoomsCommand = new Command(async () =>
        {
            await updateAvailableSessionsAsync();
        });

        SearchRoomCommand = new Command(query =>
        {
            filterRooms((string)query);
        });

        Task.Run(async () => await updateAvailableSessionsAsync());
    }

    private async Task updateAvailableSessionsAsync()
    {
        List<RoomMessage> availableRooms = null;

        try
        {
            availableRooms = await _serverApi.FetchOpenedRoomsAsync();
        }
        catch (Exception ex)
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
            if (availableRooms.Count == 0)
            {
                addNotAvailableHost(AvailableRooms);
            }
        }
        else
        {
            addNotAvailableHost(AvailableRooms);
        }

        FilterAvailableRooms.Clear();
        foreach (var room in AvailableRooms)
            FilterAvailableRooms.Add(room);

        IsRefreshing = false;
    }

    private void addNotAvailableHost(ICollection<Room> collection)
    {
        collection.Add(new Room
        {
            RoomId = "",
            OnlineUsers = 0,
            Name = "No Hosts Available",
            Picture = "error_icon",
            StatusComment = "Try again later."
        });
    }

    private void initFavoriteHosts()
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

    private void filterRooms(string query)
    {
        if (query == string.Empty)
        {
            FilterAvailableRooms.Clear();
            foreach (var room in AvailableRooms)
                FilterAvailableRooms.Add(room);
        }

        query = query.ToLower();
        var rooms = AvailableRooms.Where(room => room.Name.ToLower().Contains(query)).ToList();

        FilterAvailableRooms.Clear();
        if (rooms.Count != 0)
        {
            foreach (var room in rooms)
                FilterAvailableRooms.Add(room);
        }
        else
        {
            addNotAvailableHost(FilterAvailableRooms);
        }
    }
}