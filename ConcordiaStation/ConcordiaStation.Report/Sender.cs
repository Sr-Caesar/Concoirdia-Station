using ConcordiaStation.Report.Interfaces;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace ConcordiaStation.Report
{
    public class Sender : ISender
    {
        private readonly IConfiguration _configuration;
        private readonly MailKit.Net.Smtp.SmtpClient _smtpClient;
        private readonly string _subject;
        private readonly string _body;

        public Sender(MailKit.Net.Smtp.SmtpClient smtpClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _smtpClient = smtpClient;
            _subject = "Interim report";
            _body = "Hi,\r\n\r\n" +
                "Commencing data transfer. Enclosed within this communication, you will find the Interim Report.\r\n" +
                "It encompasses the necessary information and updates of the work in Concordia Station.\r\n\r\n" +
                "Remember to not share any of this data with anyone!\r\n\r\n" +
                "Thanks for your collaboration!";
        }

        public void SendEmail(params byte[][] attachments)
        {
            var from = _configuration.GetSection("EmailSettings:From").Value;
            var to = _configuration.GetSection("EmailSettings:To").Value;
            var password = _configuration.GetSection("EmailSettings:Password").Value;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("", from));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = $"{_subject} of {DateTime.Today}";

            var bodyBuilder = new BodyBuilder
            {
                TextBody = _body
            };

            foreach (var attachment in attachments)
            {
                bodyBuilder.Attachments.Add("Report.pdf", attachment);
            }

            message.Body = bodyBuilder.ToMessageBody();

            try
            {
                _smtpClient.Connect("smtp.office365.com", 587);
                _smtpClient.Authenticate(from, password);
                _smtpClient.Send(message);
                _smtpClient.Disconnect(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}