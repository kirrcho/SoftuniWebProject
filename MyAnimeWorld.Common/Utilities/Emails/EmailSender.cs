using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace MyAnimeWorld.Common.Utilities.Emails
{
    public class EmailSender : IEmailSender
    {
        public IConfiguration Configation { get; set; }

        public EmailSender(IConfiguration configuration)
        {
            this.Configation = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var apiKey = this.Configation["Secrets:SendGridId"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("kirrcho6@gmail.com", "Kiril Koev");
            var to = new EmailAddress(email, email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, htmlMessage, htmlMessage);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
