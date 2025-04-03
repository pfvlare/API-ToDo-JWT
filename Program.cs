using Microsoft.EntityFrameworkCore;
using TodoApiNovo.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 🔐 JWT - Chave secreta para gerar tokens
var key = Encoding.ASCII.GetBytes("super-secreta-chave-do-jwt-2024-123456789!");

// 🔄 CORS (permite acesso do front-end)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 🧠 Bancos de dados em memória
builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));

builder.Services.AddDbContext<AuthContext>(opt =>
    opt.UseInMemoryDatabase("Users"));

// 🔐 Autenticação JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// 📦 MVC + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🌐 Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// 🔄 Habilita CORS
app.UseCors("AllowAll");

// 🔐 Middleware de autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// 📍 Rotas da API
app.MapControllers();

// ▶️ Executa
app.Run();