using Client.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    public class SongsViewModel
    {

        private string _name = "";
        public string name
        {
            get => _name;
            set
            {
                _name = "Welcome " + value;
            }
        }


        public ObservableCollection<Song> Songs { get; set; }

        public SongsViewModel(UserSingleton user)
        {
            if (user != null)
            {
                name = user.username;
            }
            else
            {
                name = "";
            }

            Songs = new ObservableCollection<Song>()
            {
                new Song { Name = "Kendrick Lamar: ADHD" , Path = ""},
                new Song { Name = "50 Cent: CandyShop" , Path = ""},
                new Song { Name = "Noa Killer: Kill me" , Path = ""},
                new Song { Name = "Noa Killer: Kill me" , Path = ""},
                new Song { Name = "Noa Killer: Kill me" , Path = ""},
            };
        }
    }

}
