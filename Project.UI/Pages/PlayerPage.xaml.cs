using Project.UI.Platforms;

namespace Project.UI;

public partial class PlayerPage : ContentPage
{
    private IAudioPlayer _audioPlayer;

    public PlayerPage()
	{
		InitializeComponent();
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        _audioPlayer = Handler.MauiContext.Services.GetService<IAudioPlayer>();
        BindingContext = _audioPlayer;
    }

    private void PlayBtn_Clicked(object sender, EventArgs e)
    {
        _audioPlayer.Play();
    }

    private void StopBtn_Clicked(object sender, EventArgs e)
    {
        _audioPlayer.Stop();
    }
}