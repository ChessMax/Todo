using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TodoApi.User.Domain;

namespace TodoApi.Items.Domain;

public class TodoItem
{
    public int Id { get; set; }
    
    [Required]
    public required string Title { get; set; }
    
    public bool IsComplete { get; set; }
    
    [Required]
    public required string OwnerId { get; set; }
    
    public TodoUser? Owner { get; set; }

    public TodoItem()
    {
    }

    [SetsRequiredMembers]
    public TodoItem(int id, string title, bool isComplete, string ownerId)
    {
        Id = id;
        Title = title;
        OwnerId = ownerId;
        IsComplete = isComplete;
    }
}