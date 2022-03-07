using Firebase.Database.Query;
using HostApp.Platforms;

namespace HostApp;

public class Song
{
    public double Duration { get; set; }
    public double Position { get; set; }
    public bool IsPlaying { get; set; }

}

public partial class PlayerPage : ContentPage
{
    private IAudioPlayer _audioPlayer;

    public double Duration => _audioPlayer?.Duration ?? 0;
    public double Position => _audioPlayer?.CurrentPosition ?? 0;
    public bool IsPlaying => _audioPlayer?.IsPlaying ?? false;

    public PlayerPage()
    {
        InitializeComponent();

        BindingContext = this;
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        _audioPlayer = Handler.MauiContext.Services.GetService<IAudioPlayer>();
        Device.StartTimer(TimeSpan.FromMilliseconds(1000), updateUI);
    }

    private bool updateUI()
    {
        OnPropertyChanged(nameof(Duration));
        OnPropertyChanged(nameof(Position));
        OnPropertyChanged(nameof(IsPlaying));

        FirebaseUserService.FirebaseClient
            .Child("PlayingSong")
            .DeleteAsync();

        FirebaseUserService.FirebaseClient
            .Child("PlayingSong")
            .PostAsync(new
            {
                Duration = Duration,
                Position = Position,
                IsPlaying = IsPlaying,
            });

        return true;
    }

    private void PlayBtn_Clicked(object sender, EventArgs e)
    {
        _audioPlayer.Play();
    }

    private void StopBtn_Clicked(object sender, EventArgs e)
    {
        _audioPlayer.Stop();
    }

    private async void openBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            var file = await FilePicker.PickAsync(
            new PickOptions
            {
                PickerTitle = "Music files",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "mp3" } },
                    { DevicePlatform.UWP, new[] { "mp3" } },
                    { DevicePlatform.iOS, new[] { "mp3" } },
                    { DevicePlatform.MacCatalyst, new[] { "mp3" } },
                })
            });
            if (file == null)
                return;

            _audioPlayer.SetSong(file.FullPath);
        }
        catch
        {
            await App.Current.MainPage.DisplayAlert("Failed to open", "Premission to read required.", "OK");
        }
    }
}