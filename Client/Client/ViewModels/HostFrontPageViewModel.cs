using Client.Models;
using Client.Models.ServerMessages;
using Client.Services;
using System.Collections.ObjectModel;

namespace Client;

[QueryProperty(nameof(RoomId), nameof(RoomId))]
public class HostFrontPageViewModel : BaseViewModel
{
    private readonly IServerApi _serverApi;

    private string _roomId;
    private string _hostname;
    private string _hostSummary;
    private string _hostDescription;
    private string _avatarIconUrl;
    private string _bannerImageUrl;

    public HostFrontPageViewModel(IServerApi serverApi)
    {
        _serverApi = serverApi;

        Songs = new ObservableCollection<Song>();
        Information = new ObservableCollection<HostInformation>();

        BannerImageUrl = "landscape";
        AvatarIconUrl = "avatar_icon";
        HostName = "Carla Bruni";
        HostSummary = "Bar of Wine";
        HostDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
    }

    public ObservableCollection<Song> Songs { get; set; }

    public ObservableCollection<HostInformation> Information { get; set; }

    public string RoomId
    {
        get { return _roomId; }
        set
        {
            if (_roomId != value)
            {
                _roomId = value;
                OnPropertyChanged(nameof(RoomId));
            }
        }
    }

    public string HostName
    {
        get { return _hostname; }
        set
        {
            _hostname = value;
            OnPropertyChanged(nameof(HostName));
        }
    }

    public string HostSummary
    {
        get { return _hostSummary; }
        set
        {
            _hostSummary = value;
            OnPropertyChanged(nameof(HostSummary));
        }
    }

    public string HostDescription
    {
        get { return _hostDescription; }
        set
        {
            _hostDescription = value;
            OnPropertyChanged(nameof(HostDescription));
        }
    }

    public string AvatarIconUrl
    {
        get { return _avatarIconUrl; }
        set
        {
            _avatarIconUrl = value;
            OnPropertyChanged(nameof(AvatarIconUrl));
        }
    }

    public string BannerImageUrl
    {
        get { return _bannerImageUrl; }
        set
        {
            _bannerImageUrl = value;
            OnPropertyChanged(nameof(BannerImageUrl));
        }
    }

    public async Task FetchRoomDetailsAsync()
    {
        var room = await _serverApi.FetchRoomUpdateAsync(RoomId);
        if (room == null)
            return;

        HostName = room.Hostname;
        HostSummary = "No Summary";
        HostDescription = "Empty Description";
        AvatarIconUrl = "profile_icon";
        BannerImageUrl = "landscape";

        Information.Clear();
        Information.Add(new HostInformation { Icon = "info_icon", Title = "Online Users", Details = room.OnlineUsers.ToString() });
        Information.Add(new HostInformation { Icon = "info_icon", Title = "Song", Details = room.SongName });
    }

    public async Task<bool> JoinRoomAsync(string pinCode)
    {
        var success = await _serverApi.JoinRoomAsync(RoomId, pinCode);
        HubService service = new HubService();
        if (success)
        {
            UserProfile.Instance.RoomId = RoomId;
            await UserProfile.Instance.Hub.JoinRoom(RoomId);
            await Shell.Current.GoToAsync(($"../{nameof(HostHomePage)}"));
        }

        return success;
    }
}
