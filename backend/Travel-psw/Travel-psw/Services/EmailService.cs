using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpSettings = _configuration.GetSection("EmailSettings");
        var smtpClient = new SmtpClient(smtpSettings["SmtpServer"])
        {
            Port = int.Parse(smtpSettings["SmtpPort"]),
            Credentials = new NetworkCredential(smtpSettings["SmtpUser"], smtpSettings["SmtpPassword"]),
            EnableSsl = true, // Uveri se da koristiš SSL/TLS
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpSettings["FromEmail"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(toEmail);

        try
        {
            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            // Log or handle exception
            throw new Exception("Error sending email", ex);
        }
    }
}
