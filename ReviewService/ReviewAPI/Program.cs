using System.Text;
using Azure.Identity;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RepositoryLayer;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile(builder.Environment.IsProduction()
    ? "appsettings.json"
    : "appsettings.Development.json");

// Configure Azure Key Vault integration
var vault = builder.Configuration["AzureKeyVault:Vault"];
var vaultUri = new Uri($"https://{vault}.vault.azure.net/");
builder.Configuration.AddAzureKeyVault(vaultUri, new DefaultAzureCredential());

// Retrieve connection string from Key Vault
var connString = builder.Configuration["reviewdbConn"];

if (builder.Environment.IsProduction())
{
    var rabbitMqConnString = builder.Configuration["RabbitMQContext"];
    builder.Configuration["ConnectionStrings:RabbitMQContext"] = rabbitMqConnString;
}

builder.Services.AddDbContext<ReviewContext>(options =>
{
    options.UseMySql(connString, ServerVersion.AutoDetect(connString));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddTransient<IReviewService, ReviewService>();
builder.Services.AddTransient<IReviewRepository, ReviewRepository>();
builder.Services.AddTransient<IAuthorizationService, AuthorizationService>();

builder.Services.AddSingleton<MessageService>();
builder.Services.AddHostedService<MessageHost>();

builder.Services.AddControllers();
builder.Services.AddSingleton<IMessageService, MessageService>();
builder.Services.AddSwaggerGen(c =>
{
    // Add JWT token authentication
    c.AddSecurityDefinition("Bearer", new()
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Gateway"
        , policy => { policy.WithOrigins("https://arkade-api.germanywestcentral.cloudapp.azure.com/").AllowAnyMethod().AllowAnyHeader(); });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtIssuer"],
        ValidAudience = builder.Configuration["JwtIssuer"],
        IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"] ?? string.Empty))
    };
});

WebApplication app = builder.Build();

app.MapHealthChecks("/health");

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
