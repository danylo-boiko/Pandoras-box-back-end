using Tags.Core.Models;

namespace Tags.Core.Repositories.Interfaces;

public interface ITagsRepository
{
    public Task<Tag?> GetAsync(int id);
    public Task<Tag?> GetAsync(string tagContent);
    public Task<IList<Tag>> GetAsync(string pattern, PaginationFilter paginationFilter);
    public Task<bool> CreateAsync(Tag tag);
    public Task<bool> UpdateAsync(Tag tag);
    public Task<bool> DeleteAsync(int id);
}