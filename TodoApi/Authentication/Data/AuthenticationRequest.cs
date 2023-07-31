using System.ComponentModel.DataAnnotations;

namespace TodoApi.Authentication.Data;

public class AuthenticationRequest
{
    [Required] public required string UserName { get; set; }
    [Required] public required string Password { get; set; }
}