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

    private async void openSongBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Task.Run(pickFilesAsync);
        }
        catch
        {
            await App.Current.MainPage.DisplayAlert("Failed to open", "Premission to read required.", "OK");
        }
    }

    private async void pickFilesAsync()
    {
        var files = await FilePicker.PickMultipleAsync(getFilePickerOptions());

        await _viewModel.AddSongsAsync(files);

        Dispatcher.Dispatch(async () => await App.Current.MainPage.DisplayAlert("Message", "Update Songs Completed.", "OK"));
    }

    /// <summary>
    /// Returns audio files options for all the platforms.
    /// </summary>
    private PickOptions getFilePickerOptions()
    {
        var audioFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "audio/mpeg" } },
                    { DevicePlatform.WinUI, new[] { "mp3" } },
                    { DevicePlatform.iOS, new[] { "public.mp3" } },
                    { DevicePlatform.MacCatalyst, new[] { "mp3" } },
                });

        var audioOptions = new PickOptions
        {
            PickerTitle = "Please select an audio files",
            FileTypes = audioFileType,
        };

        return audioOptions;
    }
}