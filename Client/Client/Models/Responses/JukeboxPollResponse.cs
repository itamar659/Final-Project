using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models.Responses;
public class JukeboxPollResponse
{
    public class PollOption
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Votes { get; set; }
    }

    public List<PollOption> Options { get; set; }
}
