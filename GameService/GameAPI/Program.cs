using System.Text;
using Azure.Identity;
using GameAPI.Messaging;
using GameAPI.Steam;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<SteamApi>();
builder.Configuration.AddJsonFile(builder.Environment.IsProduction()
    ? "appsettings.json"
    : "appsettings.Development.json");

builder.Services.AddHostedService<MessageHost>();
builder.Services.AddSingleton<MessageService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Configure Azure Key Vault integration
var vault = builder.Configuration["AzureKeyVault:Vault"];
var vaultUri = new Uri($"https://{vault}.vault.azure.net/");
builder.Configuration.AddAzureKeyVault(vaultUri, new DefaultAzureCredential());

if (builder.Environment.IsProduction())
{
    var rabbitMqConnString = builder.Configuration["RabbitMQContext"];
    builder.Configuration["ConnectionStrings:RabbitMQContext"] = rabbitMqConnString;
}

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "UserPolicy"
        , policy => { policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
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
