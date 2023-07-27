using System.ComponentModel.DataAnnotations;
using TodoApi.Items.Domain;

namespace TodoApi;

public class TodoPayload
{
    [Required]
    public string Title { get; set; } = "";
    public bool IsComplete { get; set; }
}

public class TodoUpdateRequest : TodoPayload
{
    [Required]
    public int Id { get; set; }
}

public static class TodoPayloadExt
{
    public static TodoItem ToModel(this TodoPayload payload, string ownerId)
    {
        return new TodoItem(id: 0, title: payload.Title, isComplete: payload.IsComplete, ownerId: ownerId);
    }
}