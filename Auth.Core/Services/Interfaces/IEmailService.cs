namespace Auth.Core.Services.Interfaces
{
    using MimeKit;

    public interface IEmailService
    {
        public Task SendMimeMessageAsync(string receiverEmail, string subject, string textBody);
    }
}
