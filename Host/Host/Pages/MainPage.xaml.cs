namespace Host;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;

		// TODO: Will call it only once...
		Dispatcher.DispatchDelayed(MainPageViewModel.SERVER_UPDATE_DELAY, vm.FetchViewUpdateAsync);
	}
}

