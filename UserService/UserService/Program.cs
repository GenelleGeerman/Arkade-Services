using System.Text;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using PersistenceLayer.Repositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddSingleton<ILoginRepository, JsonRepository>();
builder.Services.AddSingleton<IRegisterRepository, JsonRepository>();
builder.Services.AddSingleton<ILoginService, LoginService>();
builder.Services.AddSingleton<IRegisterService, RegisterService>();
builder.Services.AddSingleton<IAuthorizationService, AuthorizationService>();
builder.Services.AddSingleton<IProfileService, ProfileService>();
builder.Services.AddSingleton<IProfileRepository, JsonRepository>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name:
        "AllowAllPolicy"
        , configurePolicy: policy => { policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
        };
    });

builder.Services.AddHealthChecks();

WebApplication app = builder.Build();

app.UseHealthChecks("/health");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseHttpsRedirection();
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
