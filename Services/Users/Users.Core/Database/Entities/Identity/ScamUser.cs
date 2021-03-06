namespace Users.Core.Database.Entities.Identity
{
    using Microsoft.AspNetCore.Identity;
    using NodaTime;

    public class ScamUser : IdentityUser<int>
    {
        public string? Bio { get; set; }

        public LocalDate? BirthDate { get; set; }

        public string? DisplayName { get; set; }

        public virtual ICollection<ScamUserRole> UserRoles { get; set; }
    }
}
