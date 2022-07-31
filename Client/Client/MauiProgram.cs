using Client.Auth0;
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

        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<FindHostPage>();
        builder.Services.AddTransient<HostHomePage>();
        builder.Services.AddTransient<HostLastPage>();
        builder.Services.AddTransient<HostNotFoundPage>();
        builder.Services.AddTransient<HostFrontPage>();

        builder.Services.AddSingleton(new Auth0Client(new()
        {
            Domain = "dev-csharp-project-final.eu.auth0.com",
            ClientId = "fxsNZuJbYFzJS8d8xz0dHBkP0aUyhbSK",
            Scope = "openid profile",
            RedirectUri = "myapp://callback"
        }));

        return builder.Build();
	}
}
