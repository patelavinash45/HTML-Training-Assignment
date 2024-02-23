using Repositories.DataModels;
using Repositories.Implementation;
using Repositories.Interface;
using Repositories.Interfaces;
using Repositories.ViewModels;
using Services.Interfaces.Patient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation.Patient
{
    public class ResetPasswordService : IResetPasswordService
    {
        private readonly IAspNetUserRepository _userRepository;

        public ResetPasswordService(IAspNetUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> resetPasswordLinkSend(string email)
        {
            try
            {
                int aspNetUserId = _userRepository.checkUser(email);
                String guiId = Guid.NewGuid().ToString();
                _userRepository.setToken(token: guiId,aspNetUserId: aspNetUserId);
                String link = "token="+guiId + "&&aspNetUserId=" + aspNetUserId + "&&time=" + DateTime.Now.ToString("yyyyMMddHHmmss");
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("tatva.dotnet.avinashpatel@outlook.com"),
                    Subject = "Reset Password Link",
                    Body = "Link for reset password : https://localhost:44392/Patient/Newpassword?"+link,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add("tatva.dotnet.avinashpatel@outlook.com");

                SmtpClient smtpClient = new SmtpClient("smtp.office365.com")
                {
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true,
                    Port = 587,
                    Credentials = new NetworkCredential(userName: "tatva.dotnet.avinashpatel@outlook.com", password: "Avinash@6351"),
                };
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex) { }
            return true;
        }

        public SetNewPassword validatePasswordLink(String token, int aspNetUserId, String time)
        {
            DateTime dateTime = DateTime.ParseExact(time, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            TimeSpan timeDiffrence = DateTime.Now.Subtract(dateTime);
            SetNewPassword setNewPassword = new()
            {
                AspNetUserId= aspNetUserId.ToString(),
                IsValidLink= timeDiffrence.Minutes < 5 ? _userRepository.checkToken(token: token, aspNetUserId: aspNetUserId): false,
            };
            return setNewPassword;
        }

        public Task<bool> changePassword(SetNewPassword model)
        {
            AspNetUser aspNetUser = _userRepository.getUser(int.Parse(model.AspNetUserId));
            return _userRepository.changePassword(aspNetUser:aspNetUser,password:model.Password);
        }
    }
}
