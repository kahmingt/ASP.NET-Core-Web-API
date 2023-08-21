using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using WebApi.Area.Account.Model;
using WebApi.Area.Account.Utility;
using WebApi.Shared.Service.Email;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [Route("api/[Controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ILogger<AccountController> _logger;
        private readonly ITokenManager _tokenManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(
            IConfiguration configuration,
            IEmailService emailService,
            ILogger<AccountController> logger,
            ITokenManager tokenManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _emailService = emailService;
            _logger = logger;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _tokenManager = tokenManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] AccountLoginModel model)
        {
            if (ModelState.IsValid)
            {
                // Search by username or email
                var user = await _userManager.FindByNameAsync(model.Login);
                user ??= await _userManager.FindByEmailAsync(model.Login);

                if (user == null)
                {
                    return Unauthorized("Unauthorized");
                }

                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, true, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var token = await _tokenManager.GenerateJsonWebTokenAsync(user, 1);

                    return Ok(new
                    {
                        token = _tokenManager.SerializeJwtToken(token),
                        expiration = token.ValidTo.ToUniversalTime(),
                    });
                }
                return Unauthorized("Unauthorized");
            }
            return BadRequest("Invalid");
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] AccountRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                };

                if (await Registration(user, model.Password))
                {
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        // Send email verification handler
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: true);
                        return Ok("Registration Success");
                    }
                }
            }
            return BadRequest("Invalid");
        }


        [HttpPost]
        [Route("Logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return NoContent();
        }



        private async Task<bool> Registration(IdentityUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Assign default role
                string defaultRoleName = _configuration["Default:Role"];
                await _roleManager.CreateAsync(new IdentityRole(defaultRoleName));
                IdentityResult RoleAssignment = await _userManager.AddToRoleAsync(user, defaultRoleName);

                if (RoleAssignment.Succeeded)
                {
                    return true;
                }
                else
                {
                    _logger.LogError("__LOGGER__ Failed to assign default role to \"" + user.UserName + "\".");
                }
            }
            else
            {
                _logger.LogError("__LOGGER__ Failed to register new user account \"" + user.UserName + "\".");
            }

            return false;
        }

        private async Task SendEmailAsync(IdentityUser user)
        {
            var uid = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(user.Id));
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var callbackUrl = Url.Page(
                "/Account/Verify",
                pageHandler: null,
                values: new
                {
                    area = "Identity",
                    tid = token,
                    uid = uid,
                    sid = 's',
                },
                protocol: Request.Scheme);

            await _emailService.SendEmailAsync(new Message(
                user.Email,
                "Confirm your account",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.<br/>" +
                "The link will expire after 2 hours."
            ));
        }



    }
}