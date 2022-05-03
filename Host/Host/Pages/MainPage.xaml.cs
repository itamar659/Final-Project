namespace Host;

public partial class MainPage : ContentPage
{
	private MainPageViewModel _vm;
	public MainPage()
	{
		InitializeComponent();

		_vm = new MainPageViewModel(new DummyServerAPI());

		BindingContext = _vm;

		Dispatcher.DispatchDelayed(MainPageViewModel.SERVER_UPDATE_DELAY, _vm.FetchViewUpdate);
	}
}

