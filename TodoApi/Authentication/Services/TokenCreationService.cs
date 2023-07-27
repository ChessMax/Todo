using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TodoApi.User.Domain;

namespace TodoApi.Services;

public class TokenCreationService : ITokenCreationService
{
    private const int EXPIRATION_MINUTES = 20;

    private readonly IConfiguration _configuration;

    public TokenCreationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AuthenticationResponse CreateToken(TodoUser user)
    {
        var expiration = DateTime.UtcNow.AddMinutes(EXPIRATION_MINUTES);

        var token = CreateJwtToken(
            CreateClaims(user),
            CreateSigningCredentials(),
            expiration
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return new AuthenticationResponse
        {
            Token = tokenHandler.WriteToken(token),
            Expiration = expiration,
        };
    }

    private JwtSecurityToken CreateJwtToken(
        Claim[] claims,
        SigningCredentials credentials,
        DateTime expiration)
    {
        return new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: expiration,
            signingCredentials: credentials
        );
    }

    private Claim[] CreateClaims(TodoUser user)
    {
        return new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            // TODO: do we really need email here?
            new Claim(ClaimTypes.Email, user.Email),
        };
    }

    private SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:key"])),
            SecurityAlgorithms.HmacSha256
        );
    }
}