namespace Auth.Core.Services.TwoFactorDigitCodeProvider
{
    using Microsoft.AspNetCore.Identity;
    using Users.Core.Database.Entities.Identity;
    public class TwoFactorDigitCodeProvider : TotpSecurityStampBasedTokenProvider<ScamUser>
    {
        public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<ScamUser> manager, ScamUser user)
        {
            return Task.FromResult(!user.EmailConfirmed);
        }
    }
}
