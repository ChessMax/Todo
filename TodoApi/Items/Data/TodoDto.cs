using System.Diagnostics.CodeAnalysis;
using TodoApi.Items.Domain;

namespace TodoApi;

public class TodoDto
{
    public TodoDto(int id, string title, bool isComplete)
    {
        Id = id;
        Title = title;
        IsComplete = isComplete;
    }

    public int Id { get; }

    public string Title { get; }

    public bool IsComplete { get; }
}

public static class TodoDtoExtension
{
    public static TodoDto ToDto(this TodoItem todo)
    {
        return new TodoDto(todo.Id, todo.Title, todo.IsComplete);
    }
}

public static class TodoExtension
{
    public static TodoItem ToModel(this TodoDto todo, string ownerId)
    {
        return new TodoItem(todo.Id, todo.Title, todo.IsComplete, ownerId);
    }
}