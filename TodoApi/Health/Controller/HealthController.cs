using Microsoft.AspNetCore.Mvc;
using TodoApi.Health.Data;

namespace TodoApi.Health;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
public class HealthController : ControllerBase
{
    [HttpGet("check")]
    public Task<HealthResponse> Check()
    {
        return Task.FromResult(new HealthResponse {Success = true});
    } 
}