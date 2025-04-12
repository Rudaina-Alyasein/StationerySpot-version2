using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendEmail(string toEmail, string subject, string body)
    {
        var smtpHost = _configuration["MailSettings:SmtpHost"];
        var smtpPort = int.Parse(_configuration["MailSettings:SmtpPort"]);
        var smtpUsername = _configuration["MailSettings:SmtpUsername"];
        var smtpPassword = _configuration["MailSettings:SmtpPassword"];
        var fromEmail = _configuration["MailSettings:FromEmail"];

        var client = new SmtpClient(smtpHost, smtpPort)
        {
            Credentials = new NetworkCredential(smtpUsername, smtpPassword),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(fromEmail),
            Subject = subject,
            Body = body
        };

        mailMessage.To.Add(toEmail);

        client.Send(mailMessage);
        Console.WriteLine("Sent");
    }
}
