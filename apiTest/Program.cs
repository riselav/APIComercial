using Voalaft.API.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Voalaft.API.Servicios;
using Voalaft.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// === JWT ===
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:key").Value)
        ),
        ValidateIssuer = false,
        ValidateAudience = false,
    });

// === CORS desde configuración ===
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
if (allowedOrigins is null || allowedOrigins.Length == 0)
{
    throw new InvalidOperationException(
        "CORS: 'AllowedOrigins' no configurado en appsettings*.json");
}

// (Opcional) log para confirmar lo cargado
Console.WriteLine("CORS origins: " + string.Join(", ", allowedOrigins));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(allowedOrigins)      // requiere coincidencia exacta esquema+host+puerto
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();              // déjalo solo si usas cookies/credentials
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegistrarRepositorios();
builder.Services.RegistrarServicios();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Si estás probando en HTTP sin cert confiable, deja esta línea comentada.
// Cuando tengas HTTPS real, descoméntala.
// app.UseHttpsRedirection();

// === MUY IMPORTANTE: CORS debe ir lo más arriba posible ===
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.UsePeticionApiMiddleware();

// Exigir la política también en endpoints (doble cinturón de seguridad)
app.MapControllers().RequireCors("AllowFrontend");

app.Run();