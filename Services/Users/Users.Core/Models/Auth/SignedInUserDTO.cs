namespace Users.Core.Models.Auth
{
    public class SignedInUserDto
    {
        public int Id { get; set; }

        public string? DisplayName { get; set; }

        public List<string> Roles { get; set; }

        public string Email { get; set; }
    }
}
