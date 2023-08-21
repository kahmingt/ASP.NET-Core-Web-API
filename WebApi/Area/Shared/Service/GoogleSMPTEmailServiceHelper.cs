using MailKit.Net.Smtp;
using MimeKit;

namespace WebApi.Shared.Service.Email;

public interface IEmailService
{
    /// <summary>
    /// Send an email using Google SMTP service
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task SendEmailAsync(Message message);
}

public class Message
{
    public List<MailboxAddress> To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }

    /// <summary>
    /// Single email recipient
    /// </summary>
    public Message(string to, string subject, string body)
    {
        To = new List<MailboxAddress>() { new MailboxAddress("", to) };
        Subject = subject;
        Body = body;
    }

    /// <summary>
    /// Multiple email recipients
    /// </summary>
    public Message(IEnumerable<string> to, string subject, string body)
    {
        To = new List<MailboxAddress>();

        To.AddRange(to.Select(x => new MailboxAddress("", x)));
        Subject = subject;
        Body = body;
    }

}

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(Message message)
    {
        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(new MailboxAddress("noreply", _configuration["Email:GoogleSTMP:AdminEmail"]));
        mimeMessage.To.AddRange(message.To);
        mimeMessage.Subject = message.Subject;
        mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message.Body
        };

        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync(
                    host: _configuration["Email:GoogleSTMP:OutgoingServer"],
                    port: int.Parse(_configuration["Email:GoogleSTMP:OutgoingPort"]),
                    useSsl: true);

                //client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(
                    userName: _configuration["Email:GoogleSTMP:AdminEmail"],
                    password: _configuration["Email:GoogleSTMP:AdminPassword"]
                );

                await client.SendAsync(mimeMessage);
            }
            catch
            {
                throw new Exception("Unable to send email!");
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}