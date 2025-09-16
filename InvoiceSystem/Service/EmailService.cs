using System.Net.Mail;
using System.Net;
using InvoiceSystem.Models.Entity;
using InvoiceSystem.Service;
using InvoiceSystem.Resources; 
public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendInvoiceEmailAsync(string toEmail, Invoice invoice)
    {
        // Configure SMTP client
        var smtpClient = new SmtpClient(_config["EmailSettings:SmtpHost"])
        {
            Port = int.Parse(_config["EmailSettings:Port"]),
            Credentials = new NetworkCredential(
                _config["EmailSettings:SmtpUser"],
                _config["EmailSettings:SmtpPass"]),
            EnableSsl = true,
        };

        string subjectTemplate = MailResources.InvoiceMailSubject;
        string bodyTemplate = MailResources.InvoiceMailBody;
        string sender = MailResources.InvoiceMailSender;

        // Replace placeholders dynamically
        string subject = subjectTemplate
            .Replace("{InvoiceId}", invoice.Id.ToString())
            .Replace("{InvoiceDate}", invoice.BillingDate.ToString("dd/MM/yyyy"));

        string body = bodyTemplate
            .Replace("{InvoiceId}", invoice.Id.ToString())
            .Replace("{InvoiceDate}", invoice.BillingDate.ToString("dd/MM/yyyy"))
            .Replace("{InvoiceAmount}", invoice.TotalAmount.ToString("N2"))
            .Replace("{InvoiceDiscount}", invoice.Discount?.Name ?? "N/A")
            .Replace("{InvoiceDueDate}", invoice.BillingDate.AddDays(10).ToString("dd/MM/yyyy"));

        // Create the email message
        var mail = new MailMessage(sender, toEmail, subject, body)
        {
            IsBodyHtml = true // HTML template
        };

        // Send the email
        await smtpClient.SendMailAsync(mail);
    }
}
