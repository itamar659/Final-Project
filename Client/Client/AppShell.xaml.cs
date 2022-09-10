namespace Client;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(FindHostPage), typeof(FindHostPage));
        Routing.RegisterRoute(nameof(HostHomePage), typeof(HostHomePage));
        Routing.RegisterRoute(nameof(HostNotFoundPage), typeof(HostNotFoundPage));
        Routing.RegisterRoute(nameof(HostFrontPage), typeof(HostFrontPage));
    }
}
