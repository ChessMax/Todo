using TodoApi.User.Domain;

namespace TodoApi.Services;

public interface ITokenCreationService
{
    public AuthenticationResponse CreateToken(TodoUser user);
}