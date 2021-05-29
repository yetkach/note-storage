using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using NoteStorage.Data.EF;
using NoteStorage.Data.Interfaces;
using NoteStorage.Data.Repositories;
using NoteStorage.Jwt;
using NoteStorage.Jwt.Interfaces;
using NoteStorage.Web.Middleware;
using NoteStorage.Logics.Interfaces;
using NoteStorage.Logics.Services;
using NoteStorage.Jwt.Services;
using NoteStorage.Logics.Mapping;
using NoteStorage.Web.Mapping;

namespace NoteStorage.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connection);
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddAutoMapper(typeof(MapperProfile));
            services.AddAutoMapper(typeof(MapperConfigure));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserIdProvider, UserIdProvider>();

            var authOptions = Configuration.GetSection("Auth").Get<AuthOptions>();
            var authOptionsConfigure = Configuration.GetSection("Auth");
            services.Configure<AuthOptions>(authOptionsConfigure);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = authOptions.Audience,

                        ValidateLifetime = true,

                        IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };
                });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
