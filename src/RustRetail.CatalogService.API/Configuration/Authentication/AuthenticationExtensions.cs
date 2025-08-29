using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RustRetail.CatalogService.API.Configuration.Authentication
{
    internal static class AuthenticationExtensions
    {
        const string JwtSectionName = "Jwt";
        const string JwtIssuerSectionName = "Issuer";
        const string JwtAudienceSectionName = "Audience";
        const string JwtSecretKeySectionName = "SecretKey";
        const string JwtAuthenticationScheme = "Bearer";

        public static IServiceCollection ConfigureJwtAuthentication(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtAuthenticationScheme)
                .AddJwtBearer(JwtAuthenticationScheme, options =>
                {
                    options.MapInboundClaims = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration.GetSection(JwtSectionName)[JwtIssuerSectionName],
                        ValidAudience = configuration.GetSection(JwtSectionName)[JwtAudienceSectionName],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection(JwtSectionName)[JwtSecretKeySectionName]!)),
                        ClockSkew = TimeSpan.Zero,
                        // Change default role claim type to match identity service role claim type
                        RoleClaimType = "roles"
                    };
                });
            return services;
        }
    }
}
