using System.Text;
using InvoiceProcessor.Application.Auth;
using InvoiceProcessor.Application.Users;
using InvoiceProcessor.Application.Vendors;
using InvoiceProcessor.Infrastructure.Auth;
using InvoiceProcessor.Infrastructure.Persistence;
using InvoiceProcessor.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// Repositories
builder.Services.AddScoped<IVendorRepository, VendorRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

//Services
builder.Services.AddScoped<IVendorService, VendorService>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthService>(sp =>
{
    var jwtSettings = sp.GetRequiredService<IOptions<JwtSettings>>().Value;
    return new AuthService(
        sp.GetRequiredService<IUserRepository>(),
        sp.GetRequiredService<IRefreshTokenRepository>(),
        sp.GetRequiredService<IPasswordHasher>(),
        sp.GetRequiredService<IJwtTokenService>(),
        jwtSettings.RefreshTokenExpirationDays);
});

builder.Services.AddScoped<CreateVendorRequestValidator>();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

var jwtSettingsSection = builder.Configuration.GetSection("Jwt");
var jwtSigningKey = jwtSettingsSection["SigningKey"]
                    ?? throw new InvalidOperationException("Jwt:SigningKey is not configured.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettingsSection["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettingsSection["Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSigningKey))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();