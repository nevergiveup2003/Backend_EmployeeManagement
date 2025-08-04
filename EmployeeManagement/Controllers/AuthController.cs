using EmployeeManagement.Data;
using EmployeeManagement.Entity;
using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRepository<User> userRepo;
        private readonly IConfiguration configuaration;

        public AuthController(IRepository<User> userRepo, IConfiguration configuaration)
        {
            this.userRepo = userRepo;
            this.configuaration = configuaration;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthDto model)
        {
            var user = (await userRepo.GetAll(x => x.Email == model.Email)).FirstOrDefault();
            if(user == null)
            {
                return new BadRequestObjectResult(new { message = "user not found" });
            }
            var passwordHelper = new PasswordHelper();

            if (!passwordHelper.VerifyPassword(user.Password,model.Password))
            {
                return new BadRequestObjectResult(new { message = "email hoac password khong dung" });
            }
            var token = GenerateToken(user.Email,user.Role);
            return Ok(new AuthTokenDto()
            {
                Id = user.Id,
                Email = user.Email,
                Token = token,
                Role = user.Role
            });

        }
        private string GenerateToken(string email,string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuaration["Jwtkey"]!));
            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,email),
                new Claim(ClaimTypes.Role,role),
            };
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
