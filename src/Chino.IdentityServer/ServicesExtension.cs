using System;
using Chino.EntityFramework.Mysql;
using Chino.EntityFramework.Sqlite;
using Chino.EntityFramework.SqlServer;
using Chino.IdentityServer.Configures;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nekonya;

namespace Chino.IdentityServer
{
    public static class ServicesExtension
    {
        public static void AddChinoDatabase<TAppDbContext>(this IServiceCollection services, IConfiguration configuration)
            where TAppDbContext : DbContext
        {
            string connectionString = configuration.GetConnectionString("ChinoApp");
            string providerType = configuration["Database:ProviderType:Chino:App"] ?? "sqlite";
            string providerStr = configuration["OverrideDbProvider"];
            if (!providerStr.IsNullOrEmpty())
            {
                providerType = providerStr;
            }
            switch (providerType.ToLower())
            {
                case "mysql":
                case "mariadb":
                    services.RegisterMysqlChinoDatabase<TAppDbContext>(configuration);
                    break;
                case "sqlite":
                case "sqlite3":
                    services.RegisterSqliteChinoDatabase<TAppDbContext>(configuration);
                    break;
                case "sqlserver":
                case "mssql":
                    services.RegisterSqlServerChinoDatabase<TAppDbContext>(configuration);
                    break;
                default:
                    throw new Exception($"Unknow database provider type: {providerType} - Chino Application");
            }

        }

        public static void AddIdentityServerConfigurationDatabase(this Microsoft.EntityFrameworkCore.DbContextOptionsBuilder builder, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("IdentityServerConfiguration");
            string providerType = configuration["Database:ProviderType:IdentityServer:Configuration"] ?? "sqlite";
            string providerStr = configuration["OverrideDbProvider"];
            if (!providerStr.IsNullOrEmpty())
            {
                providerType = providerStr;
            }
            switch (providerType.ToLower())
            {
                case "mysql":
                case "mariadb":
                    builder.RegisterMysqlIdentityServerConfigurationDatabase(connectionString);
                    break;
                case "sqlite":
                case "sqlite3":
                    builder.RegisterSqliteIdentityServerConfigurationDatabase(connectionString);
                    break;
                case "sqlserver":
                case "mssql":
                    builder.RegisterSqlServerIdentityServerConfigurationDatabase(connectionString);
                    break;
                default:
                    throw new Exception($"Unknow database provider type: {providerType} - IdentityServer Configuration");
            }
        }

        public static void AddIdentityServerOperationalDatabase(this Microsoft.EntityFrameworkCore.DbContextOptionsBuilder builder, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("IdentityServerOperational");
            string providerType = configuration["Database:ProviderType:IdentityServer:Operational"] ?? "sqlite";
            string providerStr = configuration["OverrideDbProvider"];
            if (!providerStr.IsNullOrEmpty())
            {
                providerType = providerStr;
            }

            switch (providerType.ToLower())
            {
                case "mysql":
                case "mariadb":
                    builder.RegisterMysqlIdentityServerOperationalDatabase(connectionString);
                    break;
                case "sqlite":
                case "sqlite3":
                    builder.RegisteSqliteIdentityServerOperationalDatabase(connectionString);
                    break;
                case "sqlserver":
                case "mssql":
                    builder.RegisterSqlServerIdentityServerOperationalDatabase(connectionString);
                    break;
                default:
                    throw new Exception($"Unknow database provider type: {providerType} - IdentityServer Operational");
            }
        }

        public static void AddChinoConfigurations(this IServiceCollection services, IConfiguration configuration, out ChinoAccountConfiguration chinoAccountConfiguration)
        {
            //Chino Account
            chinoAccountConfiguration = ChinoAccountConfiguration.GetConfiguration(configuration);
            services.AddSingleton(chinoAccountConfiguration);

            services.AddSingleton(CountryCodeConfiguration.GetConfiguration());
        }

        /// <summary>
        /// 根据ChinoAccountConfiguration的配置，对Asp .net core Identity 的 identityOptions进行一些设置
        /// </summary>
        /// <param name="options"></param>
        /// <param name="chinoAccountConfiguration"></param>
        public static void ApplyIdentityOptionsByChiniAccountConfiguration(this IdentityOptions options, ChinoAccountConfiguration chinoAccountConfiguration)
        {
            if(chinoAccountConfiguration.Phone.Register && chinoAccountConfiguration.Phone.RequireConfirmedPhoneNumber)
            {
                options.SignIn.RequireConfirmedPhoneNumber = true;
            }
        }

        /// <summary>
        /// 添加外部身份验证提供程序
        /// </summary>
        /// <param name="builder"></param>
        public static void AddExternalAuthProviders(this AuthenticationBuilder builder, IConfiguration configuration)
        {
            //Github
            if (configuration.GetValue<bool>("ExternalAuthProviders:Github:Enable"))
            {
                builder.AddGitHub(options =>
                {
                    options.ClientId = configuration["ExternalAuthProviders:Github:ClientId"];
                    options.ClientSecret = configuration["ExternalAuthProviders:Github:ClientSecret"];
                    options.Scope.Add("user:email");
                });
            }
        }

    }
}
