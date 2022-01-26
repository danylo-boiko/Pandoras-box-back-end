namespace Auth.Core.Database.Entities.Identity
{
    using Microsoft.AspNetCore.Identity;

    public class ScamUser : IdentityUser<int>
    {
        public string Username { get; set; } = null!;

        public string? AvatarUrl { get; set; }

        public string? Bio { get; set; }

        public virtual List<ScamUserRole> UserRoles { get; set; } = new();
    }
}
