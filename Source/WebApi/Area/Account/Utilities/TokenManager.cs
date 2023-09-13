using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApi.Area.Account.Utility;

public class TokenManager : ITokenManager
{
    private readonly IDistributedCache _distributedCache;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;
    private static int _expireMinute = 10;

    public TokenManager(
        IConfiguration configuration,
        IDistributedCache distributedCache,
        UserManager<IdentityUser> userManager)
    {
        _distributedCache = distributedCache;
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<JwtSecurityToken> GenerateJsonWebTokenAsync(IdentityUser user)
    {
        return await GenerateJsonWebTokenAsync(user, _expireMinute);
    }

    public async Task<JwtSecurityToken> GenerateJsonWebTokenAsync(IdentityUser user, int expireMinute)
    {
        // JWT Claims
        var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
            };

        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        // JWT secret
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        return new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddMinutes(_expireMinute),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
    }

    public string SerializeJwtToken(SecurityToken token)
    {
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
