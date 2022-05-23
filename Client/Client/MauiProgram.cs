using Client.Services;

namespace Client;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        builder.Services.AddSingleton<IServerApi, ServerlessApi>();

        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<GuestLoginViewModel>();
        builder.Services.AddTransient<FindHostPageViewModel>();
        builder.Services.AddTransient<HostViewModel>();

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<GuestLoginPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<FindHostPage>();
        builder.Services.AddTransient<HostHomePage>();
        builder.Services.AddTransient<HostLastPage>();
        builder.Services.AddTransient<HostNotFoundPage>();
        builder.Services.AddTransient<HostPage>();

        return builder.Build();
	}
}
