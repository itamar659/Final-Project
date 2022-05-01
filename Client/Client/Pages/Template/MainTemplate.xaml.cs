namespace Client;

[ContentProperty("Child")]
public partial class MainTemplate : ContentView
{
	public string PageTitle
	{
		get => HeaderLabel.Text;
		set
		{
			HeaderLabel.Text = value;

			if (value != String.Empty)
			{
				HeaderLabel.Margin = new Thickness(0, 20, 0, 20);
			}
		}
	}

	public double TitleSize
	{
		get => HeaderLabel.FontSize;
		set => HeaderLabel.FontSize = value;
	}

	public View PageContent
	{
		get => PageContainer.Content;
		set => PageContainer.Content = value;
	}

	public MainTemplate()
	{
		InitializeComponent();
	}
}