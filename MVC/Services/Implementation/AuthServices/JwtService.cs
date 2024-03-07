using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces.AuthServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Implementation.AuthServices
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(String role, int aspNetUserId)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, role),
                new Claim("aspNetUserId", aspNetUserId.ToString()),
            };
            SymmetricSecurityKey Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            SigningCredentials creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            var expries = DateTime.UtcNow.AddMinutes(20);
            JwtSecurityToken token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expries,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateToken(String token, out JwtSecurityToken jwtToken)
        {
            jwtToken = null;
            if(token != null)
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                SymmetricSecurityKey Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
                try
                {
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = Key,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken securityToken);
                    jwtToken = (JwtSecurityToken)securityToken;
                    if (jwtToken != null)
                    {
                        return true;
                    }
                }
                catch (Exception ex) { }
            }
            return false;
        }
    }
}
