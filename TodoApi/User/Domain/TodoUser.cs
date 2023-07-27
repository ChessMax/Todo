using Microsoft.AspNetCore.Identity;

namespace TodoApi.User.Domain;

// TODO: check IdentityUser<T>
public class TodoUser : IdentityUser
{
    [ProtectedPersonalData] 
    public override string Email { get; set; } = null!;
    
    [ProtectedPersonalData]
    public override string UserName { get; set; } = null!;
    
    public bool IsDeleted { get; set; }
    
    // public TodoUser() {}
    // public TodoUser(string email, string userName)
    // {
    //     Email = email;
    //     UserName = userName;
    // }
}