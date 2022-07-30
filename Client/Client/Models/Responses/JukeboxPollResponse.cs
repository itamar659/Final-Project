using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models.Responses;
public class JukeboxPollResponse
{
    public List<PollOption> Options { get; set; }
}
