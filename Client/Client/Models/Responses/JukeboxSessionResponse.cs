using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models.Responses;

public class JukeboxSessionResponse
{
    public Song currentSong { get; set; }
    public DateTime songTimeToFinish { get; set; } // TODO: think of better way
}
