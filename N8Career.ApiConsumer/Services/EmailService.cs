using System;
using System.Configuration;
using System.Net.Mail;
using System.Text;

namespace N8Career.ApiConsumer.Services
{
    public interface IEmailService
    {
        bool SendEmail(string emailBody, string subject, string to);
    }

    public class EmailService : IEmailService
    {
        private readonly string _emailUser;
        private readonly string _emailPass;
        public EmailService()
        {
            _emailUser = ConfigurationManager.AppSettings["emailUser"];
            _emailPass = ConfigurationManager.AppSettings["emailPass"];
        }
        public bool SendEmail(string emailBody, string subject, string to)
        {
            var response = false;
            try
            {
                using (var client = new SmtpClient())
                {
                    client.Port = 587;
                    client.Host = "smtp.gmail.com";
                    client.EnableSsl = true;
                    client.Timeout = 10000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential(_emailUser, _emailPass);

                    var mail = new MailMessage(_emailUser, to, subject, emailBody)
                    {
                        BodyEncoding = Encoding.UTF8,
                        IsBodyHtml = true,
                        DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                    };

                    client.Send(mail);
                }
                response = true;
            }
            catch (Exception e)
            {
                response = false;
                Console.WriteLine(e);
            }

            return response;
        }
    }
}
