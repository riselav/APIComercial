using Voalaft.API.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Voalaft.API.Servicios;
using Voalaft.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:key").Value)),
        ValidateIssuer = false,
        ValidateAudience = false,
    });
builder.Services.AddCors(options =>
{
    // Allow any origin using "*"
    options.AddPolicy("AllowAll", builder =>
    {
        builder.WithOrigins("*"); // Allow requests from any origin
        builder.AllowAnyMethod();   // Allow any HTTP method
        builder.AllowAnyHeader();   // Allow any header
        builder.AllowAnyOrigin();
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.RegistrarRepositorios();
builder.Services.RegistrarServicios();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();


//app.UsePeticionApiMiddleware();

app.UseHttpsRedirection();
app.MapControllers();




app.Run();
