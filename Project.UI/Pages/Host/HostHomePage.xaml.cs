using Project.UI.Pages.Host;

namespace Project.UI;

public partial class HostHomePage : ContentPage
{
	public HostHomePage(HostViewModel hostViewModel)
	{
		InitializeComponent();

		BindingContext = hostViewModel;
	}

	private  void Button_Clicked(object sender, EventArgs e)
	{
		
	}
}