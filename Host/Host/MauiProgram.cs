﻿using Host.Models;
using Host.Services;

namespace Host;

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
			})

			.AddCustomServices();

		builder.Services.AddSingleton<IServerApi, ServerlessApi>();
		builder.Services.AddSingleton<AudioPlayer>();

		builder.Services.AddTransient<MainPageViewModel>();
		builder.Services.AddTransient<LoginViewModel>();

		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddTransient<LoginPage>();

		return builder.Build();
	}
}
