using MasterGameMaster.Application.Spotify;

namespace MasterGameMaster.API.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ISpotifyService, SpotifyService>();

            return services;
        }

        public static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration configuration) 
        {
            var frontendBaseUri = configuration.GetValue<string>("FrontendUrl");

            if (frontendBaseUri is null)
               throw new ArgumentNullException(nameof(frontendBaseUri));

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => {
                    builder.WithOrigins(frontendBaseUri);
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                    builder.AllowCredentials();
                });
            });

            return services;
        }
    }
}
