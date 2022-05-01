namespace Client;

public partial class HostPlayerControl : ContentView
{
    #region Position Bindable Property
    public static readonly BindableProperty PositionProperty = BindableProperty.Create(
        nameof(Position),
        typeof(double),
        typeof(HostPlayerControl),
        propertyChanged: updatePosition);

    public double Position
    {
        get => (double)GetValue(PositionProperty);
        set => SetValue(PositionProperty, value);
    }

    private static void updatePosition(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (HostPlayerControl)bindable;

        control.Position = (double)newValue;
    }
    #endregion

    #region Duration Bindable Property
    public static readonly BindableProperty DurationProperty = BindableProperty.Create(
        nameof(Duration),
        typeof(double),
        typeof(HostPlayerControl),
        propertyChanged: updateDuration);

    private static void updateDuration(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (HostPlayerControl)bindable;

        control.Duration = (double)newValue;
    }

    public double Duration
    {
        get => (double)GetValue(DurationProperty);
        set => SetValue(DurationProperty, value);
    }
    #endregion

    #region Name Bindable Property
    public static readonly BindableProperty NameProperty = BindableProperty.Create(
        nameof(Name),
        typeof(string),
        typeof(HostPlayerControl),
        propertyChanged: updateName);

    public string Name
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }

    private static void updateName(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (HostPlayerControl)bindable;

        control.Name = (string)newValue;
    } 
    #endregion

    public HostPlayerControl()
    {
        InitializeComponent();
    }
}