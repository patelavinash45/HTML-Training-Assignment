using Services.Interfaces.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation.Patient
{
    public class ResetPasswordService : IResetPasswordService
    {
        public bool resetPasswordLinkSend(string email)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("abc@gmail.com");
            mailMessage.Subject = "Reset Password Link";
            mailMessage.To.Add("avinashspatel11@gmail.com");
            mailMessage.Body = "Link";
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gamil.com";
            System.Net.NetworkCredential networkCredential = new System.Net.NetworkCredential();
            networkCredential.UserName = "abc123@gmail.com";
            networkCredential.Password = "";
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = networkCredential;
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);
            return true;
        }
    }
}
