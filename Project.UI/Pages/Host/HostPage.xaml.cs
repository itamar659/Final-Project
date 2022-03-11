using Project.UI.Pages.Host;

namespace Project.UI;

public partial class HostPage : ContentPage
{	
	private HostViewModel vm;

	public HostPage(string host)
	{
		InitializeComponent();

		// Bad Initialize. Only for Practice.
		// TODO: Add Host Object inorder to make it more expendabily
		vm = new HostViewModel(host);
		vm.Song = "Kendrick Lamar: ADHD";
		vm.ActiveUsers = "542";
		vm.Genre = "Pop & Rock";

		BindingContext = vm;
	}

	private async void Button_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new HostHomePage(vm));
	}

}