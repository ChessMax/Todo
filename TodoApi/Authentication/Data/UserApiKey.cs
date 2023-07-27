using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using TodoApi.User.Domain;

namespace TodoApi.Authentication.Data;

[Index(nameof(Value), IsUnique = true)]
public class UserApiKey
{
    [JsonIgnore]
    public int Id { get; set; }
    
    [Required]
    public required string Value { get; set; }

    [JsonIgnore] [Required] 
    public string UserId { get; set; } = null!;
    
    [JsonIgnore]
    public required TodoUser User { get; set; }
}