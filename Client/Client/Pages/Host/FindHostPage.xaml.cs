namespace Client;

public partial class FindHostPage : ContentPage
{
    private string _name;

    public string WelcomeMessage
    {
        get { return "Welcome: " + _name; }
        set { _name = value; }
    }

    public FindHostPage(string name)
    {
        InitializeComponent();
        WelcomeMessage = name;

        BindingContext = this;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (entry.Text.ToLower() == "bruni")
        {
            await Navigation.PushAsync(new HostPage("bruni"));
        }
        else
        {
            await Navigation.PushAsync(new HostNotFoundPage());
        }

        //Navigation.InsertPageBefore(new FindHostPage(entry.Text), Navigation.NavigationStack[0]);
        //await Navigation.PopToRootAsync();
        //App.Current.MainPage = new NavigationPage(new FindHostPage());
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();

        //Navigation.PushAsync(new PlayerPage());
    }
}