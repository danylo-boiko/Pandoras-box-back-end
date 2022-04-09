using Microsoft.EntityFrameworkCore;
using Tags.Core.Models;

namespace Tags.Core.Database;

public class TagsDbContext : DbContext
{
    public DbSet<Tag> Tags { get; set; }
    
    public TagsDbContext(DbContextOptions<TagsDbContext> options) : base(options)
    {
    }
}