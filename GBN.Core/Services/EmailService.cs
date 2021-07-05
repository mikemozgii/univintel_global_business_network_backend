using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace UnivIntel.GBN.Core.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendSimpleEmail(string to, string subject, string content)
        {
            var client = new SmtpClient("smtp.gmail.com");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential("univinteldev@gmail.com", "Temp3232");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("univinteldev@gmail.com");
            mailMessage.To.Add(to);
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = content;
            mailMessage.Subject = subject;
            await client.SendMailAsync(mailMessage);
        }
    }
}
