using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces.AuthServices
{
    public interface IJwtService
    {
        string GenerateJwtToken(String role, int aspNetUserId);

        bool ValidateToken(String token, out JwtSecurityToken jwtToken);
    }
}
