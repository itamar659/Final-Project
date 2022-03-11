using Project.UI.Pages.Host;

namespace Project.UI;

public partial class HostPage : ContentPage
{
	public HostPage(string host)
	{
		InitializeComponent();

		// Bad Initialize. Only for Practice.
		// TODO: Add Host Object inorder to make it more expendabily
		HostViewModel hvm = new HostViewModel(host);
		hvm.Song = "Kendrick Lamar: ADHD";
		hvm.ActiveUsers = "542";
		hvm.Genre = "Pop & Rock";

		BindingContext = hvm;
	}

	private void Button_Clicked(object sender, EventArgs e)
	{

	}

}