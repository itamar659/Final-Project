using Host.Models.ServerMessages;
using Host.Services;
using System.Windows.Input;

namespace Host;
public class ProfileViewModel : BaseViewModel
{
    private static Random random = new Random();

    private IServerApi _serverAPI;

    private string _hostname;
    public string Hostname
    {
        get { return _hostname; }
        set
        {
            _hostname = value; OnPropertyChanged(nameof(Hostname));
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

        ChangeOwnerNameCommand = new Command(() => _serverAPI.EditProfileAsync(new HostMessage { Hostname = Hostname }));

        ChangePinCodeCommand = new Command(async () =>
        {
            PinCode = await _serverAPI.ChangeRoomPinCodeAsync();
        });
    }

    public async Task FetchProfile()
    {
        var profile = await _serverAPI.FetchHostProfileAsync(Configuration.Token);

        Hostname = profile?.Hostname;
        // desc, summary, banner image...

        var room = await _serverAPI.FetchRoomUpdateAsync();

        PinCode = room?.PinCode;
    }

    public static string RandomString(int length)
    {
        const string chars = "0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
