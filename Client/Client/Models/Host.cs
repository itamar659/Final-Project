using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public record Host
    {
        public string SessionKey { get; set; }
        public string Name { get; set; }
        public int OnlineUsers { get; set; }
        public int TotalUsers { get; set; }
        public string SongName { get; set; }
        public string Picture { get; set; }
        public string StatusComment { get; set; }
        public Boolean IsOnline { get; set; }
    }
}
