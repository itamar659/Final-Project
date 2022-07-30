namespace Client;

public partial class HostLastPage : ContentPage
{
    public HostLastPage(HostLastPageViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
}