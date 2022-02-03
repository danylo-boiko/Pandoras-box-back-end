namespace Auth.Core.Services.User
{
    using Users.Core.Database.Entities.Identity;
    
    public interface IUserService
    {
        int UserId { get; }

        Task<ScamUser?> GetCurrentUserAsync();
    }
}
