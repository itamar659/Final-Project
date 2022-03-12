using Project.UI.Pages.Host;

namespace Project.UI;

public partial class HostHomePage : ContentPage
{
	private HostViewModel vm;
	public HostHomePage(HostViewModel hostViewModel)
	{
		InitializeComponent();
		vm = hostViewModel;
		BindingContext = hostViewModel;
	}

	private  void Button_Clicked(object sender, EventArgs e)
	{
		
	}

	private async void Left_Button_Clicked(object sender, EventArgs e)
    {
		bool answer = await DisplayAlert("Leaving already?", "Are you sure wanna leave ?", "Yes", "No");
        if (answer)
		{ 
			Navigation.InsertPageBefore(new FindHostPage(vm.Host), Navigation.NavigationStack[0]);
			await Navigation.PopToRootAsync();
		}

	}
}