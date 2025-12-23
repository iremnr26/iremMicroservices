using CORE.APP.Services.Authentication;
using CORE.APP.Services.HTTP;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Users.APP.Domain;

var builder = WebApplication.CreateBuilder(args);

// Uygulama için varsayılan servisler (health check vb.)
builder.AddServiceDefaults();


// --------------------------------------------------
// Dependency Injection (IoC Container)
// --------------------------------------------------

// DbContext kaydı
// UsersDb kullanılır, SQLite veritabanına bağlanır
builder.Services.AddDbContext<DbContext, UsersDb>(
    options => options.UseSqlite(
        builder.Configuration.GetConnectionString("UsersDb"))
);

// MediatR kayıtları
// Yüklü olan tüm assembly’lerdeki handler’ları otomatik bulur
foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
{
    builder.Services.AddMediatR(config =>
        config.RegisterServicesFromAssemblies(assembly));
}

// JWT üretme servisi
// Stateless olduğu için Singleton kullanıldı
builder.Services.AddSingleton<ITokenAuthService, TokenAuthService>();


// --------------------------------------------------
// Authentication (JWT)
// --------------------------------------------------

// JWT için kullanılacak gizli anahtar
builder.Configuration["SecurityKey"] =
    "users_microservices_security_key_2025=";

// JWT Bearer Authentication ayarları
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(config =>
    {
        config.TokenValidationParameters = new TokenValidationParameters
        {
            // Token imzası bu key ile doğrulanır
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["SecurityKey"] ?? string.Empty)),

            ValidIssuer = builder.Configuration["Issuer"],
            ValidAudience = builder.Configuration["Audience"],

            // Token doğrulama kontrolleri
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true
        };
    });


// --------------------------------------------------
// HttpContext erişimi (Controller dışı sınıflar için)
// --------------------------------------------------
builder.Services.AddHttpContextAccessor();


// --------------------------------------------------
// HttpClient (External API çağrıları için)
// --------------------------------------------------
builder.Services.AddHttpClient();


// HttpServiceBase → HttpService
// Scoped: Her HTTP request için ayrı instance
builder.Services.AddScoped<HttpServiceBase, HttpService>();


// --------------------------------------------------
// Controllers & Swagger
// --------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


// Swagger ayarları + JWT desteği
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API",
        Version = "v1"
    });

    // Swagger’da JWT girebilmek için
    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Bearer {JWT}"
        });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            Array.Empty<string>()
        }
    });
});


// --------------------------------------------------
// CORS
// --------------------------------------------------
// Şu an herkese açık (development için uygun)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});



var app = builder.Build();

// Health check vb. default endpoint’ler
app.MapDefaultEndpoints();


// Swagger (prod + dev açık)
app.UseSwagger();
app.UseSwaggerUI();

// HTTPS zorunlu
app.UseHttpsRedirection();

// Authentication middleware
app.UseAuthentication();

// Authorization middleware
app.UseAuthorization();

// Controller endpoint’leri
app.MapControllers();

// CORS aktif
app.UseCors();

// Uygulamayı başlat
app.Run();
