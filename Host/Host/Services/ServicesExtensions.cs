using Host.Services;

namespace Host;

public static class ServicesExtensions
{
    public static MauiAppBuilder AddCustomServices(this MauiAppBuilder builder)
    {
#if WINDOWS
        builder.Services.AddSingleton<IAudioService, Platforms.Windows.AudioService>();
#elif ANDROID
        builder.Services.AddSingleton<IAudioService, Platforms.Android.AudioService>();
#elif MACCATALYST
        builder.Services.AddSingleton<IAudioService, Platforms.MacCatalyst.AudioService>();
#elif IOS
        builder.Services.AddSingleton<IAudioService, Platforms.iOS.AudioService>();
#endif
        return builder;
    }
}
