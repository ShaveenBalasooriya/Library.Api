using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Library.Api.OpenApi;

internal sealed class AuthentikSecuritySchemeTransformer(IConfiguration configuration) : IOpenApiDocumentTransformer
{
    public static readonly string[] RequiredScopes = ["openid", "profile", "email", "offline_access"];

    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var authorizationUrl = configuration.GetValue<Uri>("Authentik:AuthorizationUrl")
            ?? throw new InvalidOperationException("Configuration 'Authentik:AuthorizationUrl' is not configured.");
        var tokenUrl = configuration.GetValue<Uri>("Authentik:TokenUrl")
            ?? throw new InvalidOperationException("Configuration 'Authentik:TokenUrl' is not configured.");

        var securityScheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = authorizationUrl,
                    TokenUrl = tokenUrl,
                    RefreshUrl = tokenUrl,
                    Scopes = new Dictionary<string, string>
                    {
                        ["openid"] = "OpenID Connect",
                        ["profile"] = "User profile",
                        ["email"] = "User email",
                        ["offline_access"] = "Refresh token"
                    }
                }
            }
        };

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["OAuth2"] = securityScheme;

        document.Security ??= [];
        document.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("OAuth2", document)] = [.. RequiredScopes]
        });

        return Task.CompletedTask;
    }
}
