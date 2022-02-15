using Microsoft.EntityFrameworkCore;
using Tags.Core.Database;
using Tags.Core.Models;
using Tags.Core.Repositories.Interfaces;

namespace Tags.Core.Repositories;

public class TagsRepository : ITagsRepository
{
    private readonly TagsDbContext _context;

    public TagsRepository(TagsDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Tag?> GetAsync(int id)
    {
        return await _context.Tags.FirstOrDefaultAsync(t => t.Id.Equals(id));
    }

    public async Task<bool> CreateAsync(Tag tag)
    {
        await _context.Tags.AddAsync(tag);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(Tag tag)
    {
        var existTag = await _context.Tags.FindAsync(tag.Id);
        _context.Entry(existTag).CurrentValues.SetValues(tag);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existTag = await _context.Tags.FindAsync(id);
        _context.Tags.Remove(existTag);
        return await _context.SaveChangesAsync() > 0;
    }
}