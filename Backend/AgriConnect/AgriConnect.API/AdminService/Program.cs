
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// STEP 1: Register API Controllers
builder.Services.AddControllers();

// STEP 2: OpenAPI and JWT Swagger Configuration
builder.Services.AddOpenApi(options =>
{
    // Define JWT Bearer security scheme
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

    // Mark protected API operations in Swagger
    options.AddOperationTransformer(
        (operation, context, cancellationToken) =>
        {
            var metadata =
                context.Description.ActionDescriptor
                    .EndpointMetadata;

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

// STEP 3: CORS for AgriConnect Web UI
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

// STEP 4: Read JWT Settings
var jwtIssuer =
    builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException(
        "JWT Issuer is missing.");

var jwtAudience =
    builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException(
        "JWT Audience is missing.");

var jwtKey =
    builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "JWT Key is missing.");

// STEP 5: Decode Base64 Secret Key
var keyBytes = Convert.FromBase64String(jwtKey);

if (keyBytes.Length < 32)
{
    throw new InvalidOperationException(
        "JWT key must contain at least 32 bytes.");
}

// STEP 6: JWT Authentication
builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
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

// STEP 7: Authorization
builder.Services.AddAuthorization();

// STEP 8: Build Application
var app = builder.Build();

// STEP 9: Swagger UI - Development Only
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "AdminService API v1");
    });
}

// STEP 10: HTTPS
app.UseHttpsRedirection();

// STEP 11: Middleware
app.UseRouting();

app.UseCors("AgriConnectUI");

app.UseAuthentication();

app.UseAuthorization();

// STEP 12: Map Controllers
app.MapControllers();

// STEP 13: Run API
app.Run();