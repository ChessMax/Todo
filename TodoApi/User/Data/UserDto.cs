using System.ComponentModel.DataAnnotations;
using TodoApi.User.Domain;

namespace TodoApi.User.Data;

public class UserDto
{
    [Required] public required string UserId { get; set; }

    [Required] public required string UserName { get; set; }
}

public static class UserDtoExt
{
    public static UserDto ToDto(this TodoUser user)
    {
        return new UserDto { UserId = user.Id, UserName = user.UserName };
    }
}