using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace WebApi.Area.Account.Utility;

public interface ITokenManager
{
    /// <summary>
    /// Generate Json Web Token.
    /// </summary>
    /// <param name="user"><see cref="IdentityUser"/></param>
    /// <returns><see cref="JwtSecurityToken"/></returns>
    Task<JwtSecurityToken> GenerateJsonWebTokenAsync(IdentityUser user);

    /// <summary>
    /// Generate Json Web Token.
    /// </summary>
    /// <param name="user"><see cref="IdentityUser"/></param>
    /// <param name="expireMinute">Set token expiry duration in minute</param>
    /// <returns><see cref="JwtSecurityToken"/></returns>
    Task<JwtSecurityToken> GenerateJsonWebTokenAsync(IdentityUser user, int expireMinute);

    /// <summary>
    /// Wrapper to serializes a <see cref="JwtSecurityToken"/> into a JWT in Compact Serialization Format.
    /// </summary>
    /// <param name="token"><see cref="JwtSecurityToken"/> to serialize.</param>
    /// <returns></returns>
    string SerializeJwtToken(SecurityToken token);

}