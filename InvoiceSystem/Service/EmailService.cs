using System.Net.Mail;
using System.Net;
using InvoiceSystem.Models.Entity;
using InvoiceSystem.Service;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendInvoiceEmailAsync(string toEmail, Invoice invoice)
    {
        var smtpClient = new SmtpClient(_config["EmailSettings:SmtpHost"])
        {
            Port = int.Parse(_config["EmailSettings:Port"]),
            Credentials = new NetworkCredential(
        _config["EmailSettings:SmtpUser"],
        _config["EmailSettings:SmtpPass"]),
            EnableSsl = true,
        };

        string subject = $"Fatura #{invoice.Id} – {invoice.BillingDate:dd/MM/yyyy}";
        string body = $@"
        Pershendetje,

         Ju sapo keni marre nje fature te re per abonimin tuaj:

        • Numri i Fatures: {invoice.Id}
        • Data e Fatures: {invoice.BillingDate:dd/MM/yyyy}
        • Shuma Totale: {invoice.TotalAmount:N2} LEKË
        • Zbritje: {invoice.Discount?.Name ?? "N/A"}
        • Afati i Pagesës: {invoice.BillingDate.AddDays(10):dd/MM/yyyy}

        Faleminderit,
        Sistemi i Faturimit";

        var mail = new MailMessage("no-reply@invoice.com", toEmail, subject, body);
        await smtpClient.SendMailAsync(mail);
    }
}
