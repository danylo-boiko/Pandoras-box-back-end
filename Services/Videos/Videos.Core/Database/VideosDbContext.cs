using Microsoft.EntityFrameworkCore;
using Videos.Core.Database.Entities;

namespace Videos.Core.Database;

public class VideosDbContext : DbContext
{
    public virtual DbSet<Video> Videos { get; set; }
    public virtual DbSet<VideoTag> VideoTags { get; set; }

    public VideosDbContext(DbContextOptions<VideosDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VideosDbContext).Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder
            .Properties<string>()
            .HaveMaxLength(250);
    }
}