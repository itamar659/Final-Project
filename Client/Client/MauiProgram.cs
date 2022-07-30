using Client.Pages.Users;
using Client.Services;
using Client.ViewModels;

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
        builder.Services.AddTransient<FindHostPageViewModel>();
        builder.Services.AddTransient<HostFrontPageViewModel>();
        builder.Services.AddTransient<HostHomePageViewModel>();
        builder.Services.AddTransient<HostLastPageViewModel>();
        builder.Services.AddTransient<SongsViewModel>();

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<FindHostPage>();
        builder.Services.AddTransient<HostHomePage>();
        builder.Services.AddTransient<HostLastPage>();
        builder.Services.AddTransient<HostNotFoundPage>();
        builder.Services.AddTransient<HostFrontPage>();

        return builder.Build();
	}
}
