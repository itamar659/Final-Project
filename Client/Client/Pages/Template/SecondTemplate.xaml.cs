namespace Client;

[ContentProperty("Child")]
public partial class SecondTemplate : ContentView
{ 
	public View PageContent
	{
		get => PageContainer.Content;
		set => PageContainer.Content = value;
	}
	public SecondTemplate()
	{
		InitializeComponent();
	}
}