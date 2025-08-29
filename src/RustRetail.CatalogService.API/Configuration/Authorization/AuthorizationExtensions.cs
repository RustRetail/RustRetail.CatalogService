namespace RustRetail.CatalogService.API.Configuration.Authorization
{
    internal static class AuthorizationExtensions
    {
        // Policies
        public const string AdministratorPolicy = nameof(AdministratorPolicy);
        public const string UserPolicy = nameof(UserPolicy);
        public const string AuthenticatedUserPolicy = nameof(AuthenticatedUserPolicy);
        // Roles
        const string Administrator = nameof(Administrator);
        const string User = nameof(User);

        public static IServiceCollection ConfigureAuthorization(
            this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                // Administrator
                options.AddPolicy(AdministratorPolicy, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(Administrator);
                });
                // User
                options.AddPolicy(UserPolicy, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole(User);
                });
                // Authenticated user
                options.AddPolicy(AuthenticatedUserPolicy, policy =>
                {
                    policy.RequireAuthenticatedUser();
                });
            });
            return services;
        }
    }
}
