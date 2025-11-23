using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProyectoIdeasApi.CONTRACT.JwtDto;
using ProyectoIdeasApi.INFRASTRUCTURE.Data;
using ProyectoIdeasApi.INFRASTRUCTURE.Jwt;
using ProyectoIdeasApi.INTERFACES.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);




//Entity Framework Core - Postgres

// Configurar el DbContext con PostgreSQL
// Connection string (appsettings o env var)
var cs = builder.Configuration.GetConnectionString("DefaultConnection")
         ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

// EF Core + Npgsql
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseNpgsql(cs, x =>
    {
        // x.MigrationsHistoryTable("__EFMigrationsHistory", "public"); // opcional
    })
    .UseSnakeCaseNamingConvention(); // opcional, prolijo en Postgres
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// JWT Options (config)
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));



// Registrar servicio de token
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// Configurar autenticación JWT
var key = builder.Configuration["Jwt:Key"];
var issuer = builder.Configuration["Jwt:Issuer"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
