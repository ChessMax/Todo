namespace TodoApi;

public class AuthenticationResponse
{
    public required string Token { get; set; }
    public required DateTime Expiration { get; set; }
}