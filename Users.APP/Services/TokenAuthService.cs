using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CORE.APP.Models.Authentication;
using CORE.APP.Services.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace Users.APP.Services;

/// <summary>
/// Implementation of ITokenAuthService providing JWT token generation and validation.
/// </summary>
public class TokenAuthService : ITokenAuthService
{
    /// <summary>
    /// Extracts claims from a JWT token using the provided security key.
    /// </summary>
    /// <param name="token">The JWT token to extract claims from.</param>
    /// <param name="securityKey">The security key used to validate the token.</param>
    /// <returns>A collection of claims extracted from the token.</returns>
    public IEnumerable<Claim> GetClaims(string token, string securityKey)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(securityKey);
        
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false // We're extracting claims from an expired token
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal.Claims;
        }
        catch
        {
            return Enumerable.Empty<Claim>();
        }
    }

    /// <summary>
    /// Generates a new refresh token.
    /// </summary>
    /// <returns>A new refresh token string.</returns>
    public string GetRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    /// <summary>
    /// Generates a token response containing JWT access token and refresh token.
    /// </summary>
    /// <param name="userId">The user ID to include in the token claims.</param>
    /// <param name="userName">The username to include in the token claims.</param>
    /// <param name="roles">The roles to include in the token claims.</param>
    /// <param name="expiration">The expiration date and time for the JWT token.</param>
    /// <param name="securityKey">The security key used to sign the token.</param>
    /// <param name="issuer">The issuer claim for the token.</param>
    /// <param name="audience">The audience claim for the token.</param>
    /// <param name="refreshToken">The refresh token to include in the response.</param>
    /// <returns>A token response containing the JWT access token and refresh token.</returns>
    public TokenResponse GetTokenResponse(int userId, string userName, string[] roles, DateTime expiration,
        string securityKey, string issuer, string audience, string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(securityKey);

        var claims = new List<Claim>
        {
            new Claim("Id", userId.ToString()),
            new Claim(ClaimTypes.Name, userName)
        };

        // Add role claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiration,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return new TokenResponse
        {
            Token = tokenString,
            RefreshToken = refreshToken
        };
    }
}

