using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Chino.EntityFramework.Sqlite
{
    public static class DatabaseExtensions
    {
        public static void RegisterSqliteChinoDatabase<TAppDbContext>(this IServiceCollection service, IConfiguration configuration) 
            where TAppDbContext : DbContext
        {
            string chinoApp_ConnectionString  = configuration.GetConnectionString("ChinoApp");
            var migrationsAssembly = typeof(DatabaseExtensions).GetTypeInfo().Assembly.GetName().Name;


            service.AddDbContext<TAppDbContext>(options =>
                options.UseSqlite(chinoApp_ConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));
        }

        public static void RegisterSqliteIdentityServerConfigurationDatabase(this DbContextOptionsBuilder builder, string connectionString)
        {
            var migrationsAssembly = typeof(DatabaseExtensions).GetTypeInfo().Assembly.GetName().Name;

            builder.UseSqlite(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
        }


        public static void RegisteSqliteIdentityServerOperationalDatabase(this DbContextOptionsBuilder builder, string connectionString)
        {
            var migrationsAssembly = typeof(DatabaseExtensions).GetTypeInfo().Assembly.GetName().Name;
            builder.UseSqlite(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
        }
    }
}
