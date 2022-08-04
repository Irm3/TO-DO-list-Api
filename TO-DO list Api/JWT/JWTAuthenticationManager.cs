using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TO_DO_list_Api.Models;
using System.Data;
using System.Data.SqlClient;
using TO_DO_list_Api.Database;

namespace TO_DO_list_Api.JWT
{
    public class JWTAuthenticationManager
    {
        private readonly ToDoListDBContext _context;
        private readonly string key = null!;


        public JWTAuthenticationManager(string key)
        {
            _context = new ToDoListDBContext();
            this.key = key;
        }

        public string? Authenticate(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                //return "Nėra tokios paskyros registruotu šiuo el. paštu.";
                return null;
            }

            if (user.Password != password)
            {
                //return "Blogas slaptažodis.";
                return null;
            }

            var role = user.Role;

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}
