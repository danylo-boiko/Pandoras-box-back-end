namespace Users.Core.Models.Request.Profile
{
    using Microsoft.AspNetCore.Http;

    public class SetAvatarRequest
    {
        public IFormFile Avatar { get; init; }
    }
}
