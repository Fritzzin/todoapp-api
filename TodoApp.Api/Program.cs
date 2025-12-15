using System.Text;
using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Endpoints;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Services;
using TodoApp.Domain.Interfaces;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Data.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TodoApp.Api.Middleware;
using TodoApp.Api.Validators;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Defina um nome para a política de CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// ========== SERVICOS ==========

// Adicione o serviço de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            // Para desenvolvimento, podemos ser permissivos.
            // Permitir qualquer origem, qualquer método, qualquer cabeçalho.
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});


// Puxar connectionString do secrets.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ========== SERVICOS ==========

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// ===== SWAGGER UI =====
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // 1. Definir o esquema de segurança
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Insira o token JWT desta maneira: \nBearer {seu token}"
    });

    // 2. Adicionar o requisito de segurança
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// ===== Injecao de Dependencias =====
builder.Services.AddScoped<DbContext, AppDbContext>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// ===== Configurando DB Context =====
builder.Services.AddDbContext<AppDbContext>(options => { options.UseSqlite(connectionString); });

// ===== Validadores =====
// Fluent Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateTodoRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateTodoRequestValidator>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Ler configuracao jwt
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];

        // Configurar parametros de validacao
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();


// ===== Middlewares =====
// Middleware de Excecoes
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

// Ele deve vir antes de UseAuthentication e UseAuthorization.
app.UseCors(MyAllowSpecificOrigins);


// Middlewares de Autenticacao
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// ===== SEED DOS DADOS =====
// Inserir dados no banco caso estiver vazio ou for recem criado
// Obtenha o escopo de serviço para resolver o DbContext
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DataSeeder.Seed(dbContext); // Chame seu método
}


// ===== Endpoints =====
app.AddEndpointsTodos();
app.AddAuthEndpoint();


app.Run();