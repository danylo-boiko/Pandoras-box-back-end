using Microsoft.EntityFrameworkCore;
using Tags.Core.Database.Entities;

namespace Tags.Core.Database;

public class TagsDbContext : DbContext
{
    public DbSet<Tag> Tags { get; set; }
    
    public TagsDbContext(DbContextOptions<TagsDbContext> options) : base(options)
    {
    }
}