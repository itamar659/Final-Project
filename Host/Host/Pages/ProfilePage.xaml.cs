using Host.Models;

namespace Host;

public partial class ProfilePage : ContentPage
{
    private ProfileViewModel _vm;

    public ProfilePage(ProfileViewModel vm)
	{
		InitializeComponent();

        _vm = vm;
		BindingContext = vm;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        await _vm.FetchProfile();
    }
}