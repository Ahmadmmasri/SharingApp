using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SharingApp.Mail
{
    public class MailHelper : IMailHelper
    {
        public void sendMail(InputEmailMessage model)
        {
           using(SmtpClient mail = new SmtpClient("smtp.gmail.com", 587))
            {
                var msg = new MailMessage();
                msg.IsBodyHtml = true;
                msg.To.Add(model.Email);
                msg.Subject=model.Subject;
                msg.Body = model.Body;
                msg.From = new MailAddress(model.Email, "Someone", System.Text.Encoding.UTF8);

                mail.EnableSsl = true;
                mail.Credentials = new System.Net.NetworkCredential("testassegment25@gmail.com", "Test12345678");
                mail.Send(msg);
            }
        }
    }
}
