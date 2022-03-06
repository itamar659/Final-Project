using Project.UI.Platforms;

namespace Project.UI;

public partial class PlayerPage : ContentPage
{
    private IAudioPlayer _audioPlayer;

    public double Duration { get; set; }
    public double Position { get; set; }
    public bool IsPlaying { get; set; }

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
        Duration = _audioPlayer.Duration;
        Position = _audioPlayer.CurrentPosition;
        IsPlaying = _audioPlayer.IsPlaying;

        OnPropertyChanged(nameof(Duration));
        OnPropertyChanged(nameof(Position));
        OnPropertyChanged(nameof(IsPlaying));

        return true;
    }

    private void PlayBtn_Clicked(object sender, EventArgs e)
    {
        _audioPlayer.SetSong("https://soundbible.com/mp3/heavy-rain-daniel_simon.mp3");

        _audioPlayer.Play();
    }

    private void StopBtn_Clicked(object sender, EventArgs e)
    {
        _audioPlayer.Stop();
    }
}