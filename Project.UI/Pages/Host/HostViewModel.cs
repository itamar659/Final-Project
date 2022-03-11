using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Project.UI.Pages.Host
{
    internal class HostViewModel : INotifyPropertyChanged
    {
        private string _host;
        public string Host
        {
            get { return _host.ToUpper() + " Bar"; }
        }

        private string _imgUrl;
        public string ImageUrl
        {
            get { return _imgUrl; }
            set => _imgUrl = value;
        }

        private string _genre;
        public string Genre
        {
            get { return _genre.ToString();}
            set => _genre = value;
        }

        private string _activeUsers;
        public string ActiveUsers
        {
            get { return _activeUsers + ": users"; }
            set => _activeUsers = value;
        }

        private DateTime _sessionTime;
        public string SessionTime
        {
            //Todo: Format the DateTime
            get { return "2:54:49 Hours"; }
        }


        private string _song;
        public string Song
        {
            get { return _song; }
            set
            {
                _song = value;
                UpdateSong(value);
            }
        }

        private void UpdateSong(string value)
        {
            RaisePropertyChanged(nameof(Song));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public HostViewModel(string hostName)
        {
            _host = hostName;
        }
    }
}
