using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chino.EntityFramework.Mysql
{
    public static class DatabaseExtensions
    {
        public static void RegisterMysqlChinoDatabase<TAppDbContext>(this IServiceCollection service, IConfiguration configuration) 
            where TAppDbContext : DbContext
        {
            string chinoApp_ConnectionString  = configuration.GetConnectionString("ChinoApp");
            var migrationsAssembly = typeof(DatabaseExtensions).GetTypeInfo().Assembly.GetName().Name;

            service.AddDbContext<TAppDbContext>(options =>
                options.UseMySql(chinoApp_ConnectionString, ServerVersion.AutoDetect(chinoApp_ConnectionString), sql => sql.MigrationsAssembly(migrationsAssembly)));
        }

        public static void RegisterMysqlIdentityServerConfigurationDatabase(this DbContextOptionsBuilder builder, string connectionString)
        {
            var migrationsAssembly = typeof(DatabaseExtensions).GetTypeInfo().Assembly.GetName().Name;

            builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), sql => sql.MigrationsAssembly(migrationsAssembly));
        }


        public static void RegisterMysqlIdentityServerOperationalDatabase(this DbContextOptionsBuilder builder, string connectionString)
        {
            var migrationsAssembly = typeof(DatabaseExtensions).GetTypeInfo().Assembly.GetName().Name;
            builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), sql => sql.MigrationsAssembly(migrationsAssembly));
        }
    }
}
