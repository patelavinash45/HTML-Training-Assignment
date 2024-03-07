using System.IdentityModel.Tokens.Jwt;

namespace Services.Interfaces.AuthServices
{
    public interface IJwtService
    {
        string GenerateJwtToken(String role, int aspNetUserId);

        bool ValidateToken(String token, out JwtSecurityToken jwtToken);
    }
}
