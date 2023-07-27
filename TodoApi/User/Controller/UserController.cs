using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Authentication.Data;
using TodoApi.Services;
using TodoApi.User.Data;
using TodoApi.User.Domain;

namespace TodoApi.User.Controller;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly UserManager<TodoUser> _userManager;
    private readonly ITokenCreationService _jwtService;
    private readonly IApiKeyCreationService _apiKeyCreationService;

    public UserController(
        UserManager<TodoUser> userManager,
        ITokenCreationService jwtService,
        ILogger<UserController> logger, 
        IApiKeyCreationService apiKeyCreationService)
    {
        _logger = logger;
        _apiKeyCreationService = apiKeyCreationService;
        _jwtService = jwtService;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> PostUser(UserPayload payload)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = payload.ToModel();
        var result = await _userManager.CreateAsync(
            user,
            password: payload.Password
        );

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return user.ToDto();
    }

    [HttpGet("{userName}")]
    public async Task<ActionResult<UserDto>> GetUser(string userName)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var user = await _userManager.FindByNameAsync(userName: userName);
        if (user == null) return NotFound();

        return user.ToDto();
    }
    
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetUsers()
    {
        var users = await _userManager.Users.Select(user => user.ToDto()).ToListAsync();
        return Ok(users);
    }
    
    [HttpDelete("{userName}")]
    public async Task<ActionResult> DeleteUser(string userName)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var user = await _userManager.FindByNameAsync(userName: userName);
        if (user == null) return NotFound();

        user.IsDeleted = true;
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return Ok();
        }

        return BadRequest("Deleting failed");
    }

    [HttpPost("bearer-token")]
    public async Task<ActionResult<AuthenticationResponse>> CreateUserToken(
        AuthenticationRequest request)
    {
        if (!ModelState.IsValid) return BadRequest("Bad credentials");

        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user == null)
        {
            return BadRequest("Bad credentials");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid) return BadRequest("Bad credentials");

        var token = _jwtService.CreateToken(user);

        return Ok(token);
    }

    [HttpPost("api-key")]
    public async Task<ActionResult<UserApiKey>> CreateApiKey(AuthenticationRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user == null) return BadRequest("Bad credentials");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid) return BadRequest("Bad credentials");

        var token = await _apiKeyCreationService.CreateApiKey(user);

        return Ok(token);
    }
}