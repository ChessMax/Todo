using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TodoApi;
using TodoApi.Db;
using TodoApi.User.Domain;


class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private const string API_KEY_HEADER = "Api-Key";
    private readonly TodoContext _context;
    
    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options, 
        ILoggerFactory logger, 
        UrlEncoder encoder, 
        ISystemClock clock, 
        TodoContext context) : base(options, logger, encoder, clock)
    {
        _context = context;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(API_KEY_HEADER, out var apiKeyToValidate))
        {
            return AuthenticateResult.Fail("Not found");
        }

        var apiKey = await _context.UserApiKeys.Include(u => u.User)
            .SingleOrDefaultAsync(u => u.Value == apiKeyToValidate.ToString());

        if (apiKey == null)
        {
            return AuthenticateResult.Fail("Invalid key.");
        }
        return AuthenticateResult.Success(CreateTicket(apiKey.User));
    }

    private AuthenticationTicket CreateTicket(TodoUser user)
    {
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return ticket;
    }
}