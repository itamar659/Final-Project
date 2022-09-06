using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host.Models.Requests;
public class SongUpdateRequest
{
    public string SongName { get; set; }
    public double Duration { get; set; }
    public double Position { get; set; }
    public bool IsPaused { get; set; }
}
