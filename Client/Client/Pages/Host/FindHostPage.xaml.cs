namespace Client;

public partial class FindHostPage : ContentPage
{
    public FindHostPage(FindHostPageViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
}