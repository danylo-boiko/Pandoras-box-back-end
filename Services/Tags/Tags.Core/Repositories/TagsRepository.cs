using Microsoft.EntityFrameworkCore;
using Tags.Core.Database;
using Tags.Core.Database.Entities;
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

    public async Task<Tag?> GetAsync(string tagContent)
    {
        return await _context.Tags.FirstOrDefaultAsync(t=>t.Content.Equals(tagContent));
    }

    public async Task<IList<Tag>> GetAsync(string pattern, PaginationFilter paginationFilter)
    {
        var tags = _context.Tags.Where(t => t.Content.Contains(pattern));
        
        if (paginationFilter.Limit == 0)
        {
            return await tags.Skip(paginationFilter.Offset).ToListAsync();
        }
            
        return await tags.Skip(paginationFilter.Offset).Take(paginationFilter.Limit).ToListAsync();
    }

    public async Task<bool> CreateAsync(Tag tag)
    {
        await _context.Tags.AddAsync(tag);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(Tag tag)
    {
        var existTag = await _context.Tags.FindAsync(tag.Id);
        existTag.Content = tag.Content;
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existTag = await _context.Tags.FindAsync(id);
        _context.Tags.Remove(existTag);
        return await _context.SaveChangesAsync() > 0;
    }
}