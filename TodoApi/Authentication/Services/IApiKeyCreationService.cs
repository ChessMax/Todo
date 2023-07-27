using TodoApi.Authentication.Data;
using TodoApi.User.Domain;

namespace TodoApi.Services;

public interface IApiKeyCreationService
{
    public Task<UserApiKey> CreateApiKey(TodoUser user);
}