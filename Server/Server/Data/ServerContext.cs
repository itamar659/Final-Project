using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Data;

public class ServerContext : DbContext
{
    public ServerContext (DbContextOptions<ServerContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JukeboxHost>(entity => {
            entity.HasIndex(e => e.Username).IsUnique();
        });

        base.OnModelCreating(modelBuilder);
    }


    public DbSet<JukeboxHost> Hosts { get; set; } = default!;

    public DbSet<JukeboxClient> Clients { get; set; } = default!;

    public DbSet<JukeboxRoom> Rooms { get; set; } = default!;

    public DbSet<PollOption> Polls{ get; set; } = default!;
}
