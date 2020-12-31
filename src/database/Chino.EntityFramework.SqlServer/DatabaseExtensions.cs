using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chino.EntityFramework.SqlServer
{
    public static class DatabaseExtensions
    {
        public static void RegisterSqlServerChinoDatabase<TAppDbContext>(this IServiceCollection service, IConfiguration configuration) 
            where TAppDbContext : DbContext
        {
            string chinoApp_ConnectionString  = configuration.GetConnectionString("ChinoApp");
            var migrationsAssembly = typeof(DatabaseExtensions).GetTypeInfo().Assembly.GetName().Name;

            service.AddDbContext<TAppDbContext>(options =>
                options.UseSqlServer(chinoApp_ConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));
        }


        public static void RegisterSqlServerIdentityServerConfigurationDatabase(this DbContextOptionsBuilder builder, string connectionString)
        {
            var migrationsAssembly = typeof(DatabaseExtensions).GetTypeInfo().Assembly.GetName().Name;

            builder.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
        }


        public static void RegisterSqlServerIdentityServerOperationalDatabase(this DbContextOptionsBuilder builder, string connectionString)
        {
            var migrationsAssembly = typeof(DatabaseExtensions).GetTypeInfo().Assembly.GetName().Name;
            builder.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
        }

    }
}
