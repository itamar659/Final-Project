namespace Client;

public partial class HostLastPage : ContentPage
{
    public HostLastPage()
    {
        InitializeComponent();

        BindingContext = new Dictionary<string, float>()
        {
            { "Kendrick Lamar", 25 },
            { "Snoop Dog", 14 },
            { "50 Cent", 19 },
            { "Noa Killer", 47 }
        };
    }
}