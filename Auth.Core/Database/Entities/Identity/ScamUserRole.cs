namespace Auth.Core.Database.Entities.Identity
{
    using Microsoft.AspNetCore.Identity;

    public class ScamUserRole : IdentityUserRole<int>
    {
        public ScamUser User { get; set; } = null!;

        public ScamRole Role { get; set; } = null!;
    }
}
