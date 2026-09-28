using System.Security.Claims;
using Application;
using Carter;
using Infrastructure;
using Infrastructure.Persistence;
using Library.Api.Middleware;
using Library.Api.OpenApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration), writeToProviders: true);

builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeFormattedMessage = true;
    logging.IncludeScopes = true;
});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // MetadataAddress is where signing keys are fetched; ValidIssuer is the "iss" the token carries.
        // They differ in Docker (server:9000 vs localhost:9000), so both are set explicitly.
        options.MetadataAddress = builder.Configuration["Authentication:MetadataAddress"]!;
        options.TokenValidationParameters.ValidIssuer = builder.Configuration["Authentication:ValidIssuer"];
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment(); // This needs to be false if dev and true if in prod or stag.
        options.Audience = builder.Configuration["Authentik:ClientId"];
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<AuthentikSecuritySchemeTransformer>();
});
builder.Services.AddCarter();

builder.AddServiceDefaults();

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.Services.ApplyMigration();
    app.MapOpenApi();
    app.MapScalarApiReference("/docs", options =>
    {
        options
            .AddPreferredSecuritySchemes("OAuth2")
            .AddAuthorizationCodeFlow("OAuth2", flow =>
            {
                flow.ClientId = builder.Configuration["Authentik:ClientId"];
                flow.Pkce = Pkce.Sha256;
                flow.SelectedScopes = AuthentikSecuritySchemeTransformer.RequiredScopes;
            });
    });
}

app.UseExceptionHandler();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapDefaultEndpoints();

app.MapCarter();

app.MapGet("user/me", (ClaimsPrincipal claimsPrincipal) =>
{
    // Grouped because Authentik can repeat a claim type (e.g. one "groups" claim per group).
    return claimsPrincipal.Claims
        .GroupBy(c => c.Type)
        .ToDictionary(g => g.Key, g => g.Select(c => c.Value).ToArray());
})
.WithTags("User")
.RequireAuthorization();

app.Run();
