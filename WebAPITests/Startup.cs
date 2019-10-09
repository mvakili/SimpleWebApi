using AutoMapper;
using Business.Configurations;
using Business.Services;
using DataAccess;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Model.Mappers;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using Xunit.DependencyInjection;

[assembly: TestFramework("WebAPITests.Startup", "WebAPITests")]
namespace WebAPITests
{
    public class Startup : DependencyInjectionTestFramework
    {
        public Startup(IMessageSink messageSink) : base(messageSink) { }

        protected override void ConfigureServices(IServiceCollection services)
        {
            #region Configure DbContext
            

            services.AddScoped(provider =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                var option = new DbContextOptionsBuilder<WebApiDbContext>()
                    .UseSqlite(connection).Options;
                var context = new WebApiDbContext(option);
                context.Database.Migrate();
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                return context;

            });
            #endregion

            #region Configure AutoMapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserMapProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            #region Configure JWT
            // configure strongly typed settings objects
            var appSettings = new JWTSettings
            {
                Secret = "RandomLongerThan16CharacterSecret"
            };
            services.AddSingleton(appSettings);

            // configure jwt authentication
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            #endregion

            #region Add Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            #endregion

            #region Add Services
            services.AddScoped<IUserService, UserService>();
            #endregion
        }

        protected override IHostBuilder CreateHostBuilder(AssemblyName assemblyName) =>
            base.CreateHostBuilder(assemblyName)
                .ConfigureHostConfiguration(builder => builder.AddInMemoryCollection(new Dictionary<string, string> { { HostDefaults.ApplicationKey, assemblyName.Name } }));

    }
}
