using Client.Models.Responses;
using Client.Services;
using System.Windows.Input;

namespace Client;

[QueryProperty(nameof(SessionHostName), nameof(SessionHostName))]
public class HostViewModel : BaseViewModel
{
    private readonly IServerApi _serverApi;

    public HostViewModel(IServerApi serverApi)
    {
        _serverApi = serverApi;
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
