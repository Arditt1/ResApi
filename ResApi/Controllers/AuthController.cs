using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ResApi.DTA.Intefaces;
using ResApi.DTO.LoginDTO;
using static IdentityServer4.Models.IdentityResources;

namespace ResApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAuth _auth;

        public AuthController(IConfiguration config,IAuth auth)
        {
            _config = config;
            _auth = auth;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] UserLoginDTO userLogin)
        {
            var user = AuthenticateUser(userLogin);
            if (user == null)
                return Unauthorized();

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        private UserDTO AuthenticateUser(UserLoginDTO userLogin)
        {
            var result = _auth.AuthenticateUser(userLogin);
            return result;

        }

        private string GenerateJwtToken(UserDTO user)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
                var credentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = GenerateClaims(user),
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    SigningCredentials = credentials,
                };

                var token = handler.CreateToken(tokenDescriptor);
                return handler.WriteToken(token);
            }
            catch (Exception ex)
            {
                // Log the exception message
                Console.WriteLine($"Exception occurred: {ex.Message}");
                throw;
            }
        }


        private static ClaimsIdentity GenerateClaims(UserDTO user)
        {
            var claims = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.RoleName) });
        

            //foreach (var role in user.Roles)
            //    claims.AddClaim(new Claim(ClaimTypes.Role, role));

            return claims;
        }
    }
}

