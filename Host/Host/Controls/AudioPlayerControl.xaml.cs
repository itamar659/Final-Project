using Host.Models;
using Host.Services;
using System.ComponentModel;

namespace Host;

public partial class AudioPlayerControl : ContentView
{

    private AudioPlayer _playerService;

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
            _playerService = Handler.MauiContext.Services.GetService<AudioPlayer>();
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
        if (files != null)
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