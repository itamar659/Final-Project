using Host.Models;
using Host.Services;
using System.Windows.Input;

namespace Host;
public class ProfileViewModel : BaseViewModel
{
    private static Random random = new Random();

    private IServerApi _serverAPI;

    private string _hostname;
    public string Hosname
    {
        get { return _hostname; }
        set
        {
            _hostname = value; OnPropertyChanged(nameof(Hosname));
        }
    }

    private string _pinCode;
    public string PinCode
    {
        get { return _pinCode; }
        set
        {
            _pinCode = value;
            OnPropertyChanged(nameof(PinCode));
        }
    }

    public ICommand ChangePinCodeCommand { get; set; }

    public ICommand ChangeOwnerNameCommand { get; set; }

    public ProfileViewModel(IServerApi serverApi)
    {
        _serverAPI = serverApi;

        ChangeOwnerNameCommand = new Command(() => _serverAPI.EditProfileAsync(new HostProfile { Hostname = Hosname }));

        ChangePinCodeCommand = new Command(async () =>
        {
            PinCode = await _serverAPI.ChangeRoomPinCodeAsync();
        });

        FetchInfo();
    }

    public async Task FetchProfile()
    {
        var profile = await _serverAPI.FetchHostProfileAsync(Configuration.Token);
    }

    public async Task FetchInfo()
    {
        var session = await _serverAPI.FetchRoomUpdateAsync();

        Hosname = session?.Hostname;
    }

    public static string RandomString(int length)
    {
        const string chars = "0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
