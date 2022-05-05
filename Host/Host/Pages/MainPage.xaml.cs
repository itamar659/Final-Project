namespace Host;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;

		Dispatcher.DispatchDelayed(MainPageViewModel.SERVER_UPDATE_DELAY, vm.FetchViewUpdate);
	}
}

