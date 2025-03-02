using Azure;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;

using System.Text;
using Tarea6.Models;
using System.Net.Http;
using System.Drawing;

namespace Tarea6.Encriptor
{
    public class Utilidad
    {
        public static RegiUs res = new RegiUs();

        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Utilidad(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {

            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

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


        public Refresh refrestoken()
        {
            var refresh = new Refresh
            {
                refreshtoken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
                
            };
            return refresh;
            
        }

        public void SetRefreshToken(Refresh refreshh)
        {
            var cookies = new CookieOptions
            {
                HttpOnly = true,
                Expires= refreshh.Expires,
               
            };


            _httpContextAccessor.HttpContext?.Response.Cookies.Append("refresh", refreshh.refreshtoken, cookies);

            res.refreshtoken1= refreshh.refreshtoken;
            res.TokenExpired= refreshh.Expires;
            res.TokenCreated = refreshh.Created;
                     
            
        }


    }
}
