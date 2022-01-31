namespace Auth.Core.Services.Email
{
    using Configurations;
    using MailKit.Net.Smtp;
    using Microsoft.Extensions.Options;
    using MimeKit;
    using MimeKit.Text;

    public class EmailService : IEmailService
    {
        private readonly IOptions<GoogleSmtpCredentials> _smtpCredentialsOptions;

        public EmailService(IOptions<GoogleSmtpCredentials> smtpCredentials)
        {
            _smtpCredentialsOptions = smtpCredentials;
        }

        public async Task SendMimeMessageAsync(string receiverEmail, string subject, string textBody)
        {
            var mimeMsg = new MimeMessage();

            mimeMsg.From.Add(new MailboxAddress("Pandora's Box", "unitedfamilyofficial@gmail.com"));
            mimeMsg.To.Add(MailboxAddress.Parse(receiverEmail));
            mimeMsg.Subject = subject;
            mimeMsg.Body = new TextPart(TextFormat.Text)
            {
                Text = textBody
            };

            await SendEmail(mimeMsg);
        }

        private async Task SendEmail(MimeMessage message)
        {
            var smtp = new SmtpClient();
            var smtpCredentials = _smtpCredentialsOptions.Value;

            try
            {
                 await smtp.ConnectAsync("smtp.gmail.com", 465, true);
                 await smtp.AuthenticateAsync(smtpCredentials.Login, smtpCredentials.Password);
                 await smtp.SendAsync(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                await smtp.DisconnectAsync(true);
                smtp.Dispose();
            }
        }
    }
}
