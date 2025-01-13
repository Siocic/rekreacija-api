using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eRekreacija.Services.Services
{
    public class GenerateJWTToken
    {
        public static string JWTTokenGenerate(string email, string firstName, string lastName, string role)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email,email),
                new Claim("FirstName",firstName),
                new Claim("LastName",lastName),
                new Claim("Role",role),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my_super_secret_key_for_my_application_work_for_subject_on_collegue"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "mydomain.com",
                audience: "mydomain.com",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(20),
                signingCredentials: creds
             );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
