using Microsoft.Extensions.Options;
using IdentityModel.Client;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NaviMente.WebApi
{
    public class SwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly ILogger<SwaggerOptions> _logger;

        public SwaggerOptions(ILogger<SwaggerOptions> logger)
        {
            _logger = logger;
        }

        public void Configure(SwaggerGenOptions options)
        {
            try
            {
                var disco = GetDiscoveryDocument();
                var oauthScopes = new Dictionary<string, string>
                {
                    { "api://26e5bf9c-9ca7-47c9-bbbc-13f11b4b41ba/AccessAsUser", "NaviMenteApi" },
                    { "openid", "OpenID information"},
                    { "profile", "User profile information" },
                    { "email", "User email" }
                };
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(disco?.AuthorizeEndpoint ?? throw new ArgumentNullException("disco", "AuthorizeEndpoint not found")),
                            TokenUrl = new Uri(disco?.TokenEndpoint ?? throw new ArgumentNullException("disco", "TokenEndpoint not found")),
                            Scopes = oauthScopes
                        }
                    }
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            }
                        },
                        oauthScopes.Keys.ToArray()
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error loading discovery document for Swagger UI");
            }
        }

        private static DiscoveryDocumentResponse GetDiscoveryDocument()
        {
            var client = new HttpClient();
            var authority = "https://login.microsoftonline.com/9c1dfdd0-7032-44d6-adfe-27ae4e0d2326/v2.0";

            return client.GetDiscoveryDocumentAsync(
                new DiscoveryDocumentRequest()
                {
                    Address = authority,
                    Policy = {
                        ValidateIssuerName = false,
                        ValidateEndpoints = false
                    }
                }
            )
            .GetAwaiter()
            .GetResult();
        }
    }
}
