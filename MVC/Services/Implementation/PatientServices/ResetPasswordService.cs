using Repositories.DataModels;
using Repositories.Interface;
using Repositories.ViewModels;
using Services.Interfaces.PatientServices;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace Services.Implementation.PatientServices
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
                _userRepository.setToken(token: guiId, aspNetUserId: aspNetUserId);
                String link = "token=" + guiId + "&&id=" + aspNetUserId + "&&time=" + genrateHash(DateTime.Now.ToString("yyyyMMddHHmm"));
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("tatva.dotnet.avinashpatel@outlook.com"),
                    Subject = "Reset Password Link",
                    IsBodyHtml = true,
                };
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EmailTemplate/resetPasswordEmail.cshtml");
                string body= System.IO.File.ReadAllText(templatePath).Replace("EmailLink", link);
                mailMessage.Body = body;
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
                return true;
            }
            catch (Exception ex) { 
                return false;
            }
        }

        public SetNewPassword validatePasswordLink(String token, int aspNetUserId, String time)
        {
            SetNewPassword setNewPassword = new()
            {
                AspNetUserId = aspNetUserId.ToString(),
                IsValidLink = false,
            };
            for (int i=0;i>=-3;i--)
            {
                if(time == genrateHash(DateTime.Now.AddMinutes(i).ToString("yyyyMMddHHmm")))
                {
                    setNewPassword.IsValidLink = _userRepository.checkToken(token: token, aspNetUserId: aspNetUserId);
                    return setNewPassword;
                }
            }
            return setNewPassword;
        }

        public Task<bool> changePassword(SetNewPassword model)
        {
            using (var sha256 = SHA256.Create())
            {
                AspNetUser aspNetUser = _userRepository.getUser(int.Parse(model.AspNetUserId));
                return _userRepository.changePassword(aspNetUser:aspNetUser,password: genrateHash(model.Password));
            }
        }

        private String genrateHash(String password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashPassword).Replace("-", "").ToLower();
            }
        }
    }
}
