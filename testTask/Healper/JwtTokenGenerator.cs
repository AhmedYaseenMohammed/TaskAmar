using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace testTask.Healper
{
    public class JwtTokenGenerator: IJwtTokenGenerator
    {
        private readonly JWT _config;
        private readonly HttpContext _context;
        public JwtTokenGenerator(IOptions<JWT> config)
        {
            _config = config.Value;
        }


        public string GenerateToken(int id, Role role)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor desc = new SecurityTokenDescriptor()
            {
                Subject = new System.Security.Claims.ClaimsIdentity(
                    new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                        new Claim(ClaimTypes.Role, role.ToString()),
                    }),
                Expires = DateTime.UtcNow.AddDays(_config.ExpireDays),
                Issuer = _config.Issuer,
                Audience = _config.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Secret)), SecurityAlgorithms.HmacSha256),
            };
            SecurityToken token = handler.CreateToken(desc);
            return handler.WriteToken(token);
        }
    }
}
