using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Tarea6.Models;

namespace Tarea6.Encriptor
{
    public class Utilidad
    {
        private readonly IConfiguration _configuration;
        public Utilidad(IConfiguration configuration)
        {

            _configuration = configuration;

        }


        public string encriptar(string text)
        {


            using (SHA256 sha256has = SHA256.Create())
            {


                byte[] bytes = sha256has.ComputeHash(Encoding.UTF8.GetBytes(text));


                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {


                    builder.Append(bytes[i].ToString("x2"));

                }

                return builder.ToString();
            }
        }

        public string GeneratJTW(RegiUs regiUs)
        {

            var userclaims = new[]
            {

               new Claim(ClaimTypes.NameIdentifier, regiUs.IdR.ToString()),
               new Claim(ClaimTypes.Email, regiUs.Username!)
           };

            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
            var credential = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256Signature);



            var jwrconfi = new JwtSecurityToken(
                claims: userclaims,
                expires: DateTime.UtcNow.AddMinutes(40),
                 signingCredentials: credential

                             );
            return new JwtSecurityTokenHandler().WriteToken(jwrconfi);








        }


    }
}
