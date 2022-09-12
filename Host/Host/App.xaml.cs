namespace Host;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
        Application.Current.UserAppTheme = AppTheme.Light;
    }

    //protected override Window CreateWindow(IActivationState activationState)
    //{
    //    return new SimpleWindow(MainPage);
    //}
}
//public class SimpleWindow : Window
//{
//    public SimpleWindow()
//    {
//    }

//    public SimpleWindow(Page mainPage) : base(mainPage)
//    {
//    }

//    protected override void OnDestroying()
//    {
//        HostProfile.Instance.Hub?.StopAsync();

//        base.OnDestroying();
//    }
//}