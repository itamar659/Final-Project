using HostUserShare;

namespace Project.UI;

public partial class PlayerPage : ContentPage
{
    public double Duration { get; set; }
    public double Position { get; set; }
    public bool IsPlaying { get; set; }

    public PlayerPage()
	{
		InitializeComponent();

        BindingContext = this;

        FirebaseUserService.FirebaseClient
            .Child("PlayingSong")
            .AsObservable<Song>()
            .Subscribe((dbevent) =>
            {
                if (dbevent.Object != null)
                {
                    Duration = dbevent.Object.Duration;
                    Position = dbevent.Object.Position;
                    IsPlaying = dbevent.Object.IsPlaying;

                    OnPropertyChanged(nameof(Duration));
                    OnPropertyChanged(nameof(Position));
                    OnPropertyChanged(nameof(IsPlaying));
                }
            });
    }
}