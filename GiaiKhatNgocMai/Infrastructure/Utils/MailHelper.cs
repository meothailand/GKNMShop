using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;

namespace GiaiKhatNgocMai.Infrastructure.Utils
{
    public class MailHelper
    {
        private string _email = "giaikhatngocmai@gmail.com";
        private string _password = "gm@ilpass";
        public MailHelper()
        {
           
        }

        public void SendMailNoAttachment(string toEmailAddress, string subject, string content, Encoding encoding, bool isBodyHtml = false)
        {
            MailMessage mail = new MailMessage(_email, toEmailAddress);
            mail.Subject = string.Format(subject);
            mail.IsBodyHtml = isBodyHtml;
            mail.BodyEncoding = encoding ?? Encoding.UTF8;
            mail.SubjectEncoding = mail.BodyEncoding;
            var server = HttpContext.Current.Server;
            if (isBodyHtml)
            {
                LinkedResource img = new LinkedResource(server.MapPath("~/Content/images/order-signature.png"), MediaTypeNames.Image.Jpeg);
                img.ContentId = "signature";
                AlternateView altView = AlternateView.CreateAlternateViewFromString(content, mail.BodyEncoding, MediaTypeNames.Text.Html);
                altView.LinkedResources.Add(img);
                mail.AlternateViews.Add(altView);
            }
            else
            {
                mail.Body = content;
            }
            

            SmtpClient SmtpServer = new SmtpClient();
            SmtpServer.Host = "smtp.gmail.com";
            SmtpServer.EnableSsl = true;
            SmtpServer.Port = 587;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new NetworkCredential(_email, _password);
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpServer.DeliveryFormat = SmtpDeliveryFormat.International;
            try
            {
                SmtpServer.Send(mail);
            }
            finally
            {
                SmtpServer.Dispose();
                mail.Dispose();
            }
        }

        public void SelfSendMailNoAttachment(string subject, string content, Encoding encoding, bool isBodyHtml = false)
        {
            SendMailNoAttachment(_email, subject, content, encoding, isBodyHtml);
        }
    }
}