using Repositories.DataModels;
using Repositories.Interface;
using Services.Interfaces;
using Services.Interfaces.AuthServices;
using Services.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Services.Implementation
{
    public class ResetPasswordService : IResetPasswordService
    {
        private readonly IAspNetUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public ResetPasswordService(IAspNetUserRepository userRepository,IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<bool> resetPasswordLinkSend(string email)
        {
            try
            {
                int aspNetUserId = _userRepository.checkUser(email);
                List<Claim> claims = new List<Claim>()
                {
                    new Claim("aspNetUserId", aspNetUserId.ToString()),
                };
                String token = _jwtService.genrateJwtTokenForSendMail(claims, DateTime.Now.AddDays(1));
                await _userRepository.setToken(token: token, aspNetUserId: aspNetUserId);
                string link = "token=" + token;
                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("tatva.dotnet.avinashpatel@outlook.com"),
                    Subject = "Reset Password Link",
                    IsBodyHtml = true,
                };
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EmailTemplate/resetPasswordEmail.cshtml");
                string body = File.ReadAllText(templatePath).Replace("EmailLink", link);
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
                smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public SetNewPassword validatePasswordLink(string token)
        {
            SetNewPassword setNewPassword = new()
            {
                IsValidLink = false,
            };
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(token);
            if (_jwtService.validateToken(token, out jwtSecurityToken))
            {
                int aspNetUserId = int.Parse(jwtSecurityToken.Claims.FirstOrDefault(a => a.Type == "aspNetUserId").Value);
                setNewPassword.IsValidLink = _userRepository.checkToken(token: token, aspNetUserId: aspNetUserId);
                setNewPassword.AspNetUserId = aspNetUserId.ToString();
                return setNewPassword;
            }
            return setNewPassword;
        }

        public Task<bool> changePassword(SetNewPassword model)
        {
            using (var sha256 = SHA256.Create())
            {
                AspNetUser aspNetUser = _userRepository.getUser(int.Parse(model.AspNetUserId));
                return _userRepository.changePassword(aspNetUser: aspNetUser, password: genrateHash(model.Password));
            }
        }

        private string genrateHash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashPassword).Replace("-", "").ToLower();
            }
        }
    }
}
