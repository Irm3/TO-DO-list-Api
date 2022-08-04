using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TO_DO_list_Api.Models;
using System.Data;
using System.Data.SqlClient;

namespace TO_DO_list_Api.JWT
{
    public class JWTAuthenticationManager
    {
        private readonly string key = null!;


        public JWTAuthenticationManager(string key)
        {
            this.key = key;
        }

        public string? Authenticate(string email, string password)
        {
            return null;
        }

    }
}
