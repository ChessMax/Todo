using TodoApi.Authentication.Data;
using TodoApi.Db;
using TodoApi.User.Domain;

namespace TodoApi.Services;

public class ApiKeyCreationService : IApiKeyCreationService
{
    private readonly TodoContext _context;

    public ApiKeyCreationService(TodoContext context)
    {
        _context = context;
    }

    public async Task<UserApiKey> CreateApiKey(TodoUser user) 
    {
        var newApiKey = new UserApiKey()
        {
            User = user,
            Value = GenerateApiKeyValue()
        };

        _context.UserApiKeys.Add(newApiKey);
        await _context.SaveChangesAsync();
        return newApiKey;
    }

    private string GenerateApiKeyValue()
    {
        // TODO: better api key generator 
        // https://jonathancrozier.com/blog/how-to-generate-a-cryptographically-secure-random-string-in-dot-net-with-c-sharp
        return $"{Guid.NewGuid().ToString()}-{Guid.NewGuid().ToString()}";
    }
}