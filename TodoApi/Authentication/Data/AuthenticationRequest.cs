using System.ComponentModel.DataAnnotations;

namespace TodoApi;

public class AuthenticationRequest
{
    [Required] public required string UserName { get; set; }
    [Required] public required string Password { get; set; }
}