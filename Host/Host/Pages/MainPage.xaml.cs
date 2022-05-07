namespace Host;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel vm)
	{
		InitializeComponent();

		vm.View = this;
		BindingContext = vm;
	}
}

