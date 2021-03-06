namespace Users.Core.Services.TwoFactorDigitCodeProvider
{
    using Database.Entities.Identity;
    using Microsoft.AspNetCore.Identity;

    public class TwoFactorDigitCodeProvider : TotpSecurityStampBasedTokenProvider<ScamUser>
    {
        public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<ScamUser> manager, ScamUser user)
        {
            return Task.FromResult(!user.EmailConfirmed);
        }
    }
}
