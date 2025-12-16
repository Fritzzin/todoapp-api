using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TodoApp.Application.Interfaces;

namespace TodoApp.Application.Services;

public class TokenService : ITokenService
{
    private readonly IUserService _service;
    private readonly IConfiguration _configuration;

    public TokenService(IUserService service, IConfiguration configuration)
    {
        _service = service;
        _configuration = configuration;
    }

    public async Task<string?> AuthenticateAsync(string email, string password)
    {
        var user = await _service.GetUserByEmailAsync(email);

        if (user is null || !user.VerifyPassword(password))
        {
            return null;
        }

        // Criar chave
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]);
        var securityKey = new SymmetricSecurityKey(key);

        // Descriptor (detalhes) do token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            // Criar Payload
            Subject = new ClaimsIdentity([
                // Informacoes que identificariam o usuario (sem dados sensiveis)
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            ]),
            Expires = DateTime.UtcNow.AddHours(8),
            Audience = _configuration["JwtSettings:Audience"],
            Issuer = _configuration["JwtSettings:Issuer"],

            // Assinatura: Recebe a securityKey e criptografa com sha256
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
        };

        // Criacao do token utilizando parametros a cima
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }
}