namespace Auth.Core.Database.Entities.Identity
{
    using Microsoft.AspNetCore.Identity;

    public class ScamUserRole : IdentityUserRole<int>
    {
        public virtual ScamUser User { get; set; }

        public virtual ScamRole Role { get; set; }
    }
}
