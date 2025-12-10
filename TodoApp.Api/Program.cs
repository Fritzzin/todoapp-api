using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Dtos;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Services;
using TodoApp.Domain.Interfaces;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Puxar connectionString do secrets.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ========== SERVICOS ==========

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// ===== SWAGGER UI =====
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ===== INJECAO DE DEPENDENCIAS =====
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITodoService, TodoService>();

// ===== Configurando DB Context =====
builder.Services.AddDbContext<AppDbContext>(options => { options.UseSqlite(connectionString); });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/todos", async (CreateTodoRequestDto request, ITodoService todoService) =>
{
    var newTodoId = await todoService.CreateTodoAsync(request.Title, request.Description);

    return Results.Created($"/todos/{newTodoId}", newTodoId);
});

app.MapGet("/todos", async (ITodoService todoService) => await todoService.GetAllAsync());

app.MapGet("/todos/{id}", async (Guid id, ITodoService todoService) =>
{
    var result = await todoService.GetByIdAsync(id);

    return result == null ? Results.NotFound() : Results.Json(result);
});

app.MapPut("/todos/{id}", async (Guid id, UpdateTodoRequestDto request, ITodoService todoService) =>
{
    var isUpdated = await todoService.UpdateTodoAsync(id, request.title, request.description);

    return isUpdated ? Results.NoContent() : Results.NotFound();
});

app.MapDelete("/todos/{id}", async (Guid id, ITodoService todoService) =>
{
    var isDeleted = await todoService.DeleteTodoAsync(id);

    return isDeleted ? Results.NoContent() : Results.NotFound();
});

app.UseHttpsRedirection();

app.Run();