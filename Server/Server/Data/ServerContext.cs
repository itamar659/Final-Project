#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Data;

public class ServerContext : DbContext
{
    public ServerContext (DbContextOptions<ServerContext> options)
        : base(options)
    {
    }

    public DbSet<Server.Models.JukeboxHost> JukeboxHost { get; set; }

    public DbSet<Server.Models.JukeboxSession> JukeboxSession { get; set; }

    public DbSet<Server.Models.JukeboxClient> JukeboxClient { get; set; }

    public DbSet<Server.Models.PollOption> PollOption { get; set; }
}
