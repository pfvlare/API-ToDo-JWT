using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoApiNovo.Data;
using TodoApiNovo.Models;

namespace TodoApiNovo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthContext _context;
        private readonly IConfiguration _config;

        public AuthController(AuthContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // 🔐 Registro de novo usuário
        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            if (_context.Users.Any(u => u.Username == user.Username))
                return BadRequest("Usuário já existe.");

            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok("Usuário registrado com sucesso!");
        }

        // 🔑 Login de usuário com geração de token JWT
        [HttpPost("login")]
        public IActionResult Login([FromBody] User login)
        {
            try
            {
                // Verifica se usuário existe no banco
                var user = _context.Users.FirstOrDefault(u =>
                    u.Username == login.Username && u.Password == login.Password);

                if (user == null)
                    return Unauthorized("Credenciais inválidas");

                // Geração do token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("super-secreta-chave-do-jwt-2024-123456789!");

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username)
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature
                    )
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new { token = tokenHandler.WriteToken(token) });
            }
            catch (Exception ex)
            {
                // 🧯 Captura qualquer erro inesperado e retorna para o Postman
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}