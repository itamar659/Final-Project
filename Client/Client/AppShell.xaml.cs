namespace Client;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(GuestLoginPage), typeof(GuestLoginPage));

        Routing.RegisterRoute(nameof(FindHostPage), typeof(FindHostPage));
        Routing.RegisterRoute(nameof(HostHomePage), typeof(HostHomePage));
        Routing.RegisterRoute(nameof(HostLastPage), typeof(HostLastPage));
        Routing.RegisterRoute(nameof(HostNotFoundPage), typeof(HostNotFoundPage));
        Routing.RegisterRoute(nameof(HostPage), typeof(HostPage));
    }
}
