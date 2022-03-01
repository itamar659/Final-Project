namespace Project.UI;

public partial class FindHostPage : ContentPage
{
	public FindHostPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        Navigation.PushAsync(new PlayerPage());
    }
}