namespace Auth.Core.Database.Entities.Identity
{
    using Microsoft.AspNetCore.Identity;

    public class ScamUser : IdentityUser<int>
    {

        public string? AvatarUrl { get; set; }

        public string? Bio { get; set; }

        public virtual ICollection<ScamUserRole> UserRoles { get; set; }
    }
}
