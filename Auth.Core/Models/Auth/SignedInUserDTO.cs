namespace Auth.Core.Models.Auth
{
    public class SignedInUserDTO
    {
        public int Id { get; set; }

        public string? Username { get; set; }

        public List<string> Roles { get; set; }

        public string Email { get; set; }
    }
}
