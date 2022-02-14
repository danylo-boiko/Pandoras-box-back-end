namespace Users.Core.Database.Entities.Identity
{
    using Microsoft.AspNetCore.Identity;

    public class ScamRole : IdentityRole<int>
    {
        public virtual ICollection<ScamUserRole> UserRoles { get; set; }
    }
}
