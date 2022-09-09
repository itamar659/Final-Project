using Host.Models;
using Host.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Host;
public class ProfileViewModel : BaseViewModel
{
    private static Random random = new Random();

    private IServerApi _serverApi;

    private string _ownerName;
    public string OwnerName
    {
        get { return _ownerName; }
        set
        {
            _ownerName = value; OnPropertyChanged(nameof(OwnerName));
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
        _serverApi = serverApi;

        ChangeOwnerNameCommand = new Command(() => _serverApi.EditProfileAsync(new HostProfile { Hostname = OwnerName }));

        ChangePinCodeCommand = new Command(async () =>
        {
            PinCode = await _serverApi.ChangeRoomPinCodeAsync();
        });

        fetchInfo();
    }

    public async Task fetchInfo()
    {
        var session = await _serverApi.FetchRoomUpdateAsync();

        OwnerName = session?.Hostname;
    }

    public static string RandomString(int length)
    {
        const string chars = "0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
