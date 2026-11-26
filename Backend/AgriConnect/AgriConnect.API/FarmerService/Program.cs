
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Controllers
builder.Services.AddControllers();

// 2. OpenAPI + Swagger JWT Authorization
builder.Services.AddOpenApi(options =>
{
    // Register Bearer security scheme
    options.AddDocumentTransformer(
        (document, context, cancellationToken) =>
        {
            document.Components ??= new OpenApiComponents();

            document.Components.SecuritySchemes =
                new Dictionary<string, IOpenApiSecurityScheme>
                {
                    ["Bearer"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header
                    }
                };

            return Task.CompletedTask;
        });

    // Mark endpoints that require JWT
    options.AddOperationTransformer(
        (operation, context, cancellationToken) =>
        {
            var metadata =
                context.Description.ActionDescriptor.EndpointMetadata;

            bool requiresAuth =
                metadata.OfType<IAuthorizeData>().Any();

            bool allowsAnonymous =
                metadata.OfType<IAllowAnonymous>().Any();

            if (requiresAuth && !allowsAnonymous)
            {
                operation.Security ??=
                    new List<OpenApiSecurityRequirement>();

                operation.Security.Add(
                    new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference(
                            "Bearer", context.Document)] =
                            new List<string>()
                    });
            }

            return Task.CompletedTask;
        });
});

// 3. CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AgriConnectUI", policy =>
    {
        policy.WithOrigins(
                "https://localhost:44332",
                "http://localhost:10132"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// 4. JWT Settings
var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException(
        "JWT Issuer is missing.");

var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException(
        "JWT Audience is missing.");

var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "JWT Key is missing.");

var keyBytes = Convert.FromBase64String(jwtKey);

if (keyBytes.Length < 32)
{
    throw new InvalidOperationException(
        "JWT key must contain at least 32 bytes.");
}

// 5. JWT Authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,

                ValidateAudience = true,
                ValidAudience = jwtAudience,

                ValidateLifetime = true,
                RequireExpirationTime = true,

                ValidateIssuerSigningKey = true,
                RequireSignedTokens = true,

                IssuerSigningKey =
                    new SymmetricSecurityKey(keyBytes),

                ValidAlgorithms = new[]
                {
                    SecurityAlgorithms.HmacSha256
                },

                RoleClaimType = "role",
                NameClaimType = "sub",

                ClockSkew = TimeSpan.FromSeconds(30)
            };
    });

// 6. Authorization
builder.Services.AddAuthorization();

// 7. Build
var app = builder.Build();

// 8. Swagger UI
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "FarmerService API v1");
    });
}

// 9. Middleware
app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AgriConnectUI");

app.UseAuthentication();

app.UseAuthorization();

// 10. Controllers
app.MapControllers();

app.Run();