namespace Host.Models;
public class RoomUpdater : BaseViewModel
{

    public static readonly double PROPERTIES_UPDATE_DELAY = TimeSpan.FromSeconds(1).TotalMilliseconds;

    private System.Timers.Timer _updateTimer;
    private Room _room;

    public string LiveTime => _room.LiveTime.ToString(@"hh\:mm\:ss");

    public RoomUpdater(Room room)
    {
        _room = room;

        _updateTimer = new System.Timers.Timer(PROPERTIES_UPDATE_DELAY);
        _updateTimer.Elapsed += _updateTimer_Elapsed;
        _updateTimer.Start();
    }

    private void _updateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        OnPropertyChanged(nameof(LiveTime));
    }
}
