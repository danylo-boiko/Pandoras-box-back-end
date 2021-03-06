namespace Users.Core.Services.User
{
    using System.Security.Claims;
    using Database;
    using Database.Entities.Identity;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UsersDbContext _dbContext;

        public UserService(
            IHttpContextAccessor httpContextAccessor,
            UsersDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public int UserId
        {
            get
            {
                var idString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                return idString == null ? 0 : int.Parse(idString);
            }
        } 
        
        public async Task<ScamUser?> GetCurrentUserAsync()
        {
            var userId = UserId;
            var user = await _dbContext
                .Users
                .SingleOrDefaultAsync(e => e.Id == UserId);

            return user;
        }
    }
}
