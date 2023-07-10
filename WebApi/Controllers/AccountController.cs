using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using API.Models;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Owin.Security;
//using Microsoft.Owin.Security.Cookies;
//using Microsoft.Owin.Security.OAuth;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;


namespace WebApi.Controllers
{
    [Route("api/[Controller]")]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(
            IConfiguration configuration,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        private async Task<JwtSecurityToken> GenerateJsonWebToken(IdentityUser user, int expiresMinutes = 10)
        {
            // JWT Claims
            // - GUID
            // - UserName
            // - Roles
            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            // JWT secret
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            // JWT expiration
            expiresMinutes = (expiresMinutes < 1) ? 10 : expiresMinutes;

            return new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(expiresMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest LoginRequest)
        {
            if (ModelState.IsValid)
            {
                // Search by username or email
                var user = await _userManager.FindByNameAsync(LoginRequest.Login);
                user ??= await _userManager.FindByEmailAsync(LoginRequest.Login);

                if (user == null)
                {
                    return Unauthorized();
                }

                var result = await _signInManager.PasswordSignInAsync(user.UserName, LoginRequest.Password, true, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var token = await GenerateJsonWebToken(user);

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
                return Unauthorized("Unauthorized");
            }
            return BadRequest("Invalid");
        }
    }

    public class Response
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }
}