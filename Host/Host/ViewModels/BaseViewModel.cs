using System.ComponentModel;

namespace Host;
public class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged = (s, e) => { };

    protected virtual void OnPropertyChanged(string nameofProperty)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameofProperty));
    }
}
