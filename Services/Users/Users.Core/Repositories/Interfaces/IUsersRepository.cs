using Users.Core.Database.Entities.Identity;

namespace Users.Core.Repositories.Interfaces;

public interface IUsersRepository
{
    public Task<ScamUser?> GetAsync(int id);
}