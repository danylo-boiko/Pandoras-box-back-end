using Tags.Core.Models;

namespace Tags.Core.Repositories.Interfaces;

public interface ITagsRepository
{
    public Task<Tag?> GetAsync(int id);
    public Task<bool> CreateAsync(Tag tag);
    public Task<bool> UpdateAsync(Tag tag);
    public Task<bool> DeleteAsync(int id);
}