using Client.Models;
using Client.Models.Responses;
using Client.Services;
using System.Collections.ObjectModel;

namespace Client;

[QueryProperty(nameof(SessionKey), nameof(SessionKey))]
public class HostFrontPageViewModel : BaseViewModel
{
    private readonly IServerApi _serverApi;

    public ObservableCollection<Song> Songs { get; set; }
    public ObservableCollection<HostInformation> Information { get; set; }

    private string _sessionKey;
    public string SessionKey
    {
        get { return _sessionKey; }
        set
        {
            if (_sessionKey != value)
            {
                _sessionKey = value;
                OnPropertyChanged(nameof(SessionKey));
                FetchSessionDetailsAsync();
            }
        }
    }

    private string _avatarIconUrl;
    public string AvatarIconUrl
    {
        get { return _avatarIconUrl; }
        set
        {
            _avatarIconUrl = value;
            OnPropertyChanged(nameof(AvatarIconUrl));
        }
    }

    private string _hostSummary;
    public string HostSummary
    {
        get { return _hostSummary; }
        set
        {
            _hostSummary = value;
            OnPropertyChanged(nameof(HostSummary));
        }
    }

    private string _hostDescription;
    public string HostDescription
    {
        get { return _hostDescription; }
        set
        {
            _hostDescription = value;
            OnPropertyChanged(nameof(HostDescription));
        }
    }

    private string _bannerImageUrl;
    public string BannerImageUrl
    {
        get { return _bannerImageUrl; }
        set
        {
            _bannerImageUrl = value;
            OnPropertyChanged(nameof(BannerImageUrl));
        }
    }


    private string _hostname;
    public string HostName
    {
        get { return _hostname; }
        set
        {
            _hostname = value;
            OnPropertyChanged(nameof(HostName));
        }
    }

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

    public async void FetchSessionDetailsAsync()
    {
        JukeboxSessionResponse details = await _serverApi.FetchSessionDetailsAsync(SessionKey);
        if (details == null)
            return; // Return to last page...

        HostName = details.OwnerName;
        HostSummary = "No Summary";
        HostDescription = "Empty Description";
        AvatarIconUrl = "profile_icon";
        BannerImageUrl = "landscape";

        Information.Add(new HostInformation { Icon = "info_icon", Title = "Active Users", Details = details.ActiveUsers.ToString() });
        Information.Add(new HostInformation { Icon = "info_icon", Title = "Total Users", Details = details.TotalUsers.ToString() });
        Information.Add(new HostInformation { Icon = "info_icon", Title = "Song", Details = details.SongName });
    }

    public async Task<bool> JoinSessionAsync(string pinCode)
    {
        var success = await _serverApi.JoinSessionAsync(HostName, pinCode);
        if (success)
            await Shell.Current.GoToAsync(nameof(HostHomePage));

        return success;
    }
}
