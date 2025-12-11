using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Endpoints;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Services;
using TodoApp.Domain.Interfaces;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Data.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using TodoApp.Api.Validators;

var builder = WebApplication.CreateBuilder(args);

// Puxar connectionString do secrets.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ========== SERVICOS ==========

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// ===== SWAGGER UI =====
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ===== Injecao de Dependencias =====
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<DbContext, AppDbContext>();

// ===== Configurando DB Context =====
builder.Services.AddDbContext<AppDbContext>(options => { options.UseSqlite(connectionString); });

// ===== Validadores =====
// Fluent Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateTodoRequestValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ===== Endpoints =====
app.AddEndpointsTodos();

app.UseHttpsRedirection();

app.Run();