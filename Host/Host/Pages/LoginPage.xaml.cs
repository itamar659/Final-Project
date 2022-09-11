namespace Host;

public partial class LoginPage : ContentPage
{
	LoginViewModel _vm;

    public LoginPage(LoginViewModel vm)
	{
		InitializeComponent();

		_vm = vm;
		BindingContext = vm;
	}

	protected override async void OnHandlerChanged()
	{
		base.OnHandlerChanged();

		await _vm.TryAutoConnectAsync();
	}
}