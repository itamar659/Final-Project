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
        throw new NotImplementedException();
#elif IOS
        throw new NotImplementedException();
#endif
        return builder;
    }
}
