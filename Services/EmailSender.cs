using Microsoft.AspNetCore.Routing.Constraints;
using NajdiSpolubydliciRazor.Services.Interfaces;
using System.Net;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace NajdiSpolubydliciRazor.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration = null!;

        private readonly string EmailServiceUri;

        private readonly int EmailServicePort;

        private readonly string EmailServiceUsername;

        private readonly string EmailServicePassword;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;

            EmailServiceUri = _configuration["EmailService:uri"]
                                ?? throw new ArgumentNullException("email service uri not configurated in appsettings.json");

            EmailServicePort = int.TryParse(_configuration["EmailService:port"], out int portNumber)
                                        ? portNumber
                                        : throw new ArgumentNullException("email service port not configurated in appsettings.json");

            EmailServiceUsername = _configuration["EmailService:username"]
                                            ?? throw new ArgumentNullException("email service username not configurated in appsettings.json");

            EmailServicePassword = _configuration["EmailService:password"]
                                            ?? throw new ArgumentNullException("email service password not configurated in appsettings.json");
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Najdi Spolubydlící TEST", EmailServiceUsername));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(EmailServiceUri, EmailServicePort);
                await client.AuthenticateAsync(EmailServiceUsername, EmailServicePassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
            
        }
    }
}
