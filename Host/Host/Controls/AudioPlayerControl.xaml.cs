namespace Host;

public partial class AudioPlayerControl : ContentView
{
    private AudioPlayerViewModel _viewModel;

    public AudioPlayerControl()
    {
        InitializeComponent();
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        IAudioService audioPlayer = Handler.MauiContext.Services.GetService<IAudioService>();
        _viewModel = new AudioPlayerViewModel(audioPlayer);
        BindingContext = _viewModel;


    }

    [Obsolete]
    private async void openSongBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Task.Run(async () =>
            {
                var audioFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "audio/mpeg" } },
                    { DevicePlatform.UWP, new[] { "mp3" } },
                    { DevicePlatform.iOS, new[] { "public.mp3" } },
                    { DevicePlatform.MacCatalyst, new[] { "mp3" } },
                });

                var audioOptions = new PickOptions
                {
                    PickerTitle = "Please select an audio files",
                    FileTypes = audioFileType,
                };

                var files = await FilePicker.PickMultipleAsync(audioOptions);

                await _viewModel.AddSongsAsync(files);
            });
        }
        catch
        {
            await App.Current.MainPage.DisplayAlert("Failed to open", "Premission to read required.", "OK");
        }
    }
}