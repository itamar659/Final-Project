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
		vm = new HostViewModel("Carla " + host);
		vm.Song = "Kendrick Lamar: ADHD";
		vm.ActiveUsers = "542";
		vm.Genre = "Pop & Rock";

		BindingContext = vm;
	}

	private async void Button_Clicked(object sender, EventArgs e)
	{
		string result = await DisplayPromptAsync("2-Way Authentication", "Enter Host's personal pincode. for further information please contact the host.", keyboard: Keyboard.Numeric);

		// TODO: make it dynamic password inside of the host object.
		if(result == "1234")
        {

			Navigation.InsertPageBefore(new HostHomePage(vm), Navigation.NavigationStack[0]);
			await Navigation.PopToRootAsync();
        }
        else
        {
			await DisplayAlert("Error", "Wrong Password. please try again.", "OK");
        }
	}

}