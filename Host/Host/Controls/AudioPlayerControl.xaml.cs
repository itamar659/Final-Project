namespace Host;

public partial class AudioPlayerControl : ContentView
{
    private AudioPlayer _playerService;

    //public static readonly BindableProperty SongNameProperty =
    //    BindableProperty.Create(
    //        nameof(SongName),
    //        typeof(string),
    //        typeof(AudioPlayerControl),
    //        defaultValue: string.Empty,
    //        propertyChanged: (b, o, n) => ((AudioPlayerControl)b).SongName = n.ToString());

    //public static readonly BindableProperty DurationProperty =
    //    BindableProperty.Create(
    //        nameof(Duration),
    //        typeof(double),
    //        typeof(AudioPlayerControl),
    //        defaultValue: double.Epsilon,
    //        propertyChanged: (b, o, n) => ((AudioPlayerControl)b).Duration = (double)n);

    //public static readonly BindableProperty PositionProperty =
    //    BindableProperty.Create(
    //        nameof(Position),
    //        typeof(double),
    //        typeof(AudioPlayerControl),
    //        defaultValue: (double)0,
    //        propertyChanged: (b, o, n) => ((AudioPlayerControl)b).Position = (double)n);

    //public static readonly BindableProperty IsPlayingProperty =
    //    BindableProperty.Create(
    //        nameof(IsPlaying),
    //        typeof(bool),
    //        typeof(AudioPlayerControl),
    //        defaultValue: false,
    //        propertyChanged: (b, o, n) => {
    //            ((AudioPlayerControl)b).IsPlaying = (bool)n;
    //            ((AudioPlayerControl)b).playStopBtn.Text = (bool)n ? "Stop" : "Play";
    //        });

    //public bool IsPlaying
    //{
    //    get { return (bool)GetValue(IsPlayingProperty); }
    //    set { SetValue(IsPlayingProperty, value); }
    //}


    //public string SongName
    //{
    //    get { return (string)GetValue(SongNameProperty); }
    //    set { SetValue(SongNameProperty, value); }
    //}

    //public double Duration
    //{
    //    get { return (double)GetValue(DurationProperty); }
    //    set { SetValue(DurationProperty, value); }
    //}

    //public double Position
    //{
    //    get { return (double)GetValue(PositionProperty); }
    //    set { SetValue(PositionProperty, value); }
    //}

    public AudioPlayerControl()
    {
        InitializeComponent();

        BindingContext = this;

        timeSlider.Maximum = double.Epsilon;
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (_playerService == null)
        {
            IAudioService audioPlayer = Handler.MauiContext.Services.GetService<IAudioService>();
            _playerService = new AudioPlayer(audioPlayer);
            _playerService.SongStateChanged += _playerService_SongStateChanged;

            songsCollection.ItemsSource = _playerService.Songs;
        }
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

        await addSongsAsync(files);

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

    /// <summary>
    /// Add a list of song to the playlist asynchronous.
    /// </summary>
    /// <param name="files">list of paths</param>
    /// <returns></returns>
    private Task addSongsAsync(IEnumerable<FileResult> files)
    {
        foreach (var file in files)
            _playerService.AddToPlaylist(file.FullPath);

        return Task.CompletedTask;
    }

    private async void skipBtn_Clicked(object sender, EventArgs e)
    {
        await _playerService.NextAsync();
    }

    private async void playStopBtn_Clicked(object sender, EventArgs e)
    {
        if (_playerService.IsPlaying)
        {
            await _playerService.StopAsync();
        }
        else
        {
            await _playerService.PlayAsync();
            Dispatcher.Dispatch(notifyChange);
        }
    }

    private void _playerService_SongStateChanged(object sender, EventArgs e)
    {
        Dispatcher.Dispatch(songStateChanged);
    }

    /// <summary>
    /// Updates some of the player values when the state of the player has changed
    /// </summary>
    private void songStateChanged()
    {
        songName.Text = _playerService.SongName;
        playStopBtn.Text = _playerService.IsPlaying ? "Stop" : "Play";

        if (_playerService.State == AudioPlayer.PlayerState.Stop)
            timeSlider.Value = 0;
    }
    
    /// <summary>
    /// Update the duration / Position as long as song is plays.
    /// </summary>
    private void notifyChange()
    {
        // TODO: From playerService, Duration should send a notify when its async change completed. and set it outside of this function.
        timeSlider.Value = _playerService.Position;
        timeSlider.Maximum = _playerService.Duration;

        if (_playerService.IsPlaying)
            Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(500), notifyChange);
    }
}