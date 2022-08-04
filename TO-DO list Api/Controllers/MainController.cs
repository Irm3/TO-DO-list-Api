using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TO_DO_list_Api.Models;
using TO_DO_list_Api.JWT;
using TO_DO_list_Api.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;
using System.Security.Cryptography;
using MimeKit;
using MailKit.Net.Smtp;

namespace TO_DO_list_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly ToDoListDBContext _context;
        private readonly JWTAuthenticationManager _jwtAuthenticationManager = null!;

        public MainController(ToDoListDBContext context, JWTAuthenticationManager jwtAuthenticationManager)
        {
            _context = context;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody]Login user)
        {
            var token = _jwtAuthenticationManager.Authenticate(user.Email, user.Password);

            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(new { token = token, email = user.Email });
        }

        [AllowAnonymous]
        [HttpPost("verify")]
        public IActionResult Verify(string emailToken)
        {
            var user = _context.Users.FirstOrDefault(u => u.ResetToken == emailToken);

            if (user == null)
            {
                return BadRequest();
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("forgot_password/{email1}")]
        public IActionResult ForgotPassword(string email1)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email1);

            if (user == null)
            {
                return BadRequest();
            }

            var builder = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json");
            IConfiguration configuration = builder.Build();
            string server = configuration.GetConnectionString("EmailServer");
            string email_from = configuration.GetConnectionString("MainEmail"); // using ethereal website for testing email sending

            user.ResetToken = CreateToken();
            user.ResetTokenExpiration = DateTime.Now.AddHours(2);
            _context.SaveChanges();

            string email_to = email1;

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(email_from));
            email.To.Add(MailboxAddress.Parse(email1));
            email.Subject = "Reset Password";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = "Reset Token: " + user.ResetToken + ", API endpoint to reset password: http://localhost:5241/api/Main/reset_password," +
                                                                             "enter in json: `Token`, new `Password` and `ConfirmPassword` " };
            using var smtp = new SmtpClient();
            smtp.Connect(server, 587, MailKit.Security.SecureSocketOptions.StartTls); // using ethereal smtp
            smtp.Authenticate("adolfo.welch31@ethereal.email", "GEeYQtacXajZRyNWXp");
            smtp.Send(email);
            smtp.Disconnect(true);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("reset_password")]
        public IActionResult ResetPassword(ResetPassword request)
        {
            var user = _context.Users.FirstOrDefault(u => u.ResetToken == request.Token);

            if (user == null || user.ResetTokenExpiration < DateTime.Now)
            {
                return BadRequest("Bad token");
            }
            user.Password = request.Password;
            user.ResetToken = null;
            user.ResetTokenExpiration = null;

            _context.SaveChanges();

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] Login userRegister)
        {
            // check email
            if (_context.Users.FirstOrDefault(acc => acc.Email == userRegister.Email) != null)
            {
                return NotFound(new { message = "Email already exists." });
            }

            var user = new User
            {
                Email = userRegister.Email,
                Password = userRegister.Password,
                Role = "role2" // role1 = admin, role2 = user
            
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Created("Success!", user);
        }

        /// <summary>
        /// For testing logged in users
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("testlogin")]
        public IActionResult TestLogin()
        {
            Request.Headers.TryGetValue("Authorization", out StringValues authHeader);
            return Ok(new { token = authHeader });
        }

        /// <summary>
        /// For testing role specific users (admin)
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "role1")]
        [HttpGet("testadmin")]
        public IActionResult TestAdmin()
        {
            Request.Headers.TryGetValue("Authorization", out StringValues authHeader);
            return Ok(new { token = authHeader });
        }

        private string CreateToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}
