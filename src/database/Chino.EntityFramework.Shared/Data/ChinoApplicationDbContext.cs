using Chino.EntityFramework.Shared.Entities.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chino.EntityFramework.Shared.Data
{
    public class ChinoApplicationDbContext : IdentityDbContext<ChinoUser>
    {
        public ChinoApplicationDbContext(DbContextOptions<ChinoApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
