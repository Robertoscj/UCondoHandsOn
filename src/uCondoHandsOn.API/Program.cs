using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using uCondo.Planos.API.Settings;
using uCondo.Planos.API.Middleware;
using System.Threading.RateLimiting;
using Microsoft.EntityFrameworkCore;
using uCondoHandsOn.Infra.Context;
using uCondoHandsOn.Domain.Interfaces;
using uCondoHandsOn.Infrastructure.Repositories;
using uCondoHandsOn.Domain.Services;
using uCondoHandsOn.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "uCondoHandsOn API",
        Version = "v1",
        Description = "API for managing account plans in the uCondo system. This API provides endpoints for creating, reading, and deleting accounts, with support for hierarchical structures and validation rules.",
        Contact = new OpenApiContact
        {
            Name = "uCondo Support",
            Url = new Uri("https://suporte.ucondo.com.br")
        }
    });

    // Add XML comments
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() 
    ?? throw new InvalidOperationException("JwtSettings configuration is missing");
var oauth2Settings = builder.Configuration.GetSection("OAuth2").Get<OAuth2Settings>()
    ?? throw new InvalidOperationException("OAuth2 configuration is missing");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("JWT", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
        };
    })
    .AddJwtBearer("OAuth2", options =>
    {
        options.Authority = oauth2Settings.Authority;
        options.Audience = oauth2Settings.Audience;
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AccountManagement", policy =>
        policy.RequireClaim("scope", "accounts.manage"));
    
    options.AddPolicy("AccountRead", policy =>
        policy.RequireClaim("scope", "accounts.read"));
});

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));
});

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Repository and Service
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<IAccountsService, AccountService>();
builder.Services.AddTransient<ExceptionMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "uCondo API V1");
        c.RoutePrefix = string.Empty; 
        c.DocumentTitle = "uCondoHandsOn API";
    });
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseRateLimiter();

app.Run();
