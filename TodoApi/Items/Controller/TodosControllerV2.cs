using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Db;
using TodoApi.Items.Domain;
using TodoApi.User.Domain;

namespace TodoApi.Controllers;

[Tags("Todos v2")]
[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/todos")]
public class TodosControllerV2 : ControllerBase
{
    private readonly TodoContext _context;
    private readonly ILogger<TodosControllerV2> _logger;
    private readonly UserManager<TodoUser> _userManager;

    public TodosControllerV2(
        ILogger<TodosControllerV2> logger,
        UserManager<TodoUser> userManager,
        TodoContext context)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme},ApiKey")]
    public async Task<Results<Ok<IEnumerable<TodoDto>>, BadRequest>> GetItems()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return TypedResults.BadRequest();
        var items = await _context.Items.Where(item => item.OwnerId == userId).AsNoTracking().ToListAsync();
        return TypedResults.Ok(items.Select(item => item.ToDto()));
    }

    [Authorize(AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme},ApiKey")]
    [HttpGet("{id:int}")]
    public async Task<Results<Ok<TodoDto>, NotFound>> GetItemById(int id)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return TypedResults.NotFound();

        var item = (await GetItem(id))?.ToDto();
        if (item == null) return TypedResults.NotFound();

        return TypedResults.Ok(item);
    }

    private async Task<TodoItem?> GetItem(int todoId)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return null;

        var item = await _context.Items
            .Where(e => e.Id == todoId && e.OwnerId == userId)
            .FirstOrDefaultAsync();

        return item;
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme},ApiKey")]
    public async Task<Results<Created<TodoDto>, BadRequest>> CreateItem(TodoPayload item)
    {
        // TODO:
        if (!ModelState.IsValid) return TypedResults.BadRequest();
        var userId = _userManager.GetUserId(User);
        if (userId == null) return TypedResults.BadRequest();

        var newItem = item.ToModel(ownerId: userId);
        await _context.Items.AddAsync(newItem);
        await _context.SaveChangesAsync();
        return TypedResults.Created($"/todos/{newItem.Id}", newItem.ToDto());
    }

    [HttpPatch]
    [Authorize(AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme},ApiKey")]
    public async Task<Results<Ok<TodoDto>, BadRequest, NotFound>> UpdateItem(TodoUpdateRequest request)
    {
        // TODO:
        if (!ModelState.IsValid) return TypedResults.BadRequest();
        var userId = _userManager.GetUserId(User);
        if (userId == null) return TypedResults.BadRequest();

        var item = await GetItem(request.Id);
        if (item == null) return TypedResults.NotFound();

        item.Title = request.Title;
        item.IsComplete = request.IsComplete;
        await _context.SaveChangesAsync();
        
        return TypedResults.Ok(item.ToDto());
    }
}