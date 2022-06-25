using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class HostStatus
    {
        public string Picture { get; set; }
        public string Name { get; set; }
        public string StatusComment { get; set; }
        public Boolean IsOnline { get; set; }
    }
}
