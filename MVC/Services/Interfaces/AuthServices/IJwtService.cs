using Services.ViewModels;
using System.IdentityModel.Tokens.Jwt;

namespace Services.Interfaces.AuthServices
{
    public interface IJwtService
    {
        string GenerateJwtToken(UserDataModel user);

        bool ValidateToken(String token, out JwtSecurityToken jwtToken);
    }
}
