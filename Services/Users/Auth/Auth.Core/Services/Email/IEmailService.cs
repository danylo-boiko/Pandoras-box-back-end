namespace Auth.Core.Services.Email
{
    public interface IEmailService
    {
        public Task SendMimeMessageAsync(string receiverEmail, string subject, string textBody);
    }
}
