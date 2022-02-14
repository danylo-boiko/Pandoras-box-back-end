namespace Users.Core.Services.User
{
    using Database.Entities.Identity;

    public interface IUserService
    {
        int UserId { get; }

        Task<ScamUser?> GetCurrentUserAsync();
    }
}
