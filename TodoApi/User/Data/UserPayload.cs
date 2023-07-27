using System.ComponentModel.DataAnnotations;
using TodoApi.User.Domain;

namespace TodoApi.User.Data;

public class UserPayload
{
    private UserPayload()
    {
    }

    public UserPayload(string userName, string password, string email)
    {
        Email = email;
        UserName = userName;
        Password = password;
    }

    [Required] public string Email { get; set; } = null!;

    [Required] public string UserName { get; set; } = null!;

    [Required] public string Password { get; set; } = null!;
}

public static class UserPayloadExt
{
    public static TodoUser ToModel(this UserPayload payload)
    {
        return new TodoUser { Email = payload.Email, UserName = payload.UserName };
    }
}