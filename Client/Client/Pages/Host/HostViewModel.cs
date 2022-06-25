using Client.Models;
using Client.Models.Responses;
using Client.Services;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Client;

[QueryProperty(nameof(SessionHostName), nameof(SessionHostName))]
public class HostViewModel : BaseViewModel
{
    private readonly IServerApi _serverApi;

    public ObservableCollection<Song> Songs { get; set; }
    public ObservableCollection<HostInformation> information { get; set; }

    public HostViewModel(IServerApi serverApi)
    {
        _serverApi = serverApi;

        Songs = new ObservableCollection<Song>
            {
                new Song { Name = "Hip-Hop" },
                new Song { Name = "Jazz" },
                new Song { Name = "R&B"}
            };

        information = new ObservableCollection<HostInformation>
            {
                new HostInformation { Icon = "info_icon" , Title="Genre", Description="Hip-Hop" },
                new HostInformation { Icon = "info_icon" , Title="Active Users", Description="1024" },
                new HostInformation { Icon = "info_icon" , Title="Song", Description="21 Savage: a lot" },
                new HostInformation { Icon = "info_icon" , Title="Session Time", Description="1:23:42" }
            };
    }

    private string _sessionHostName;
    public string SessionHostName
    {
        get { return _sessionHostName; }
        set
        {
            _sessionHostName = value;
            OnPropertyChanged(nameof(SessionHostName));
        }
    }

    private string _host;
    public string Host
    {
        get { return _host; }
        set
        {
            _host = value;
            OnPropertyChanged(nameof(Host));
        }
    }

    private string _imgUrl;
    public string ImageUrl
    {
        get { return _imgUrl; }
        set
        {
            _imgUrl = value;
            OnPropertyChanged(nameof(ImageUrl));
        }
    }

    private string _genre;
    public string Genre
    {
        get { return _genre; }
        set
        {
            _genre = value;
            OnPropertyChanged(nameof(Genre));
        }
    }

    private int _activeUsers;
    public int ActiveUsers
    {
        get { return _activeUsers; }
        set
        {
            _activeUsers = value;
            OnPropertyChanged(nameof(ActiveUsers));
        }
    }

    private string _song;
    public string Song
    {
        get { return _song; }
        set
        {
            _song = value;
            OnPropertyChanged(nameof(Song));
        }
    }

    public string SessionTime
    {
        //Todo: Format the DateTime
        get { return "2:54:49 Hours"; }
    }

    public async void FetchSessionDetails()
    {
        JukeboxSessionResponse details = await _serverApi.FetchSessionDetailsAsync();

        Host = details.OwnerName;
        Song = details.SongName;
        ActiveUsers = details.ActiveUsers;
        Genre = "Not Implemented Yet";
    }
}
