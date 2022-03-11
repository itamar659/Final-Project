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
        }

        private int _players;
        public string Players
        {
            get { return _players + ": players";}
        }

        private int _activePlayers;
        public string ActivePlayers
        {
            get { return _activePlayers + ": active"; }
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
