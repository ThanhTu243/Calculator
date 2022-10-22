using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Ultilities
{
    public class MailHelper
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }



        public async Task<string> Send(string mailTo, string subject, string content, string filePath = "")
        {
            try
            {
                StringBuilder Body = new StringBuilder();

                Body.Append(content);
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(Email);
                mail.To.Add(mailTo);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = Body.ToString();
                if (!string.IsNullOrEmpty(filePath))
                {
                    Attachment attachment = new Attachment(filePath);
                    mail.Attachments.Add(attachment);
                }
                SmtpClient smtp = new SmtpClient();
                smtp.Host = Host;
                smtp.Port = Port;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new System.Net.NetworkCredential(Email, Password);
                smtp.EnableSsl = EnableSsl;
                smtp.Send(mail);
                Console.WriteLine($"{DateTime.Now:hh:mm dd/MM/yyyy} mailhelper send {mailTo}");
                return mailTo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mail helper exception: {ex.Message}");
                return ex.Message;
            }
        }
    }
}
