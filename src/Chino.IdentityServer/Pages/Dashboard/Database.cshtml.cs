using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.EntityFramework.Shared.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Chino.IdentityServer.Pages.Dashboard
{
    public class DatabaseModel : PageModel
    {
        private readonly ChinoApplicationDbContext m_AppDbContext;

        public bool ChinoApp_CanConnect { get; set; }

        public IEnumerable<string> ChinoApp_PendingMigrations { get; set; } = new List<string>();


        public DatabaseModel(ChinoApplicationDbContext appDbContext)
        {
            this.m_AppDbContext = appDbContext;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            ChinoApp_CanConnect = await m_AppDbContext.Database.CanConnectAsync();
            if (ChinoApp_CanConnect)
            {
                ChinoApp_PendingMigrations = await m_AppDbContext.Database.GetPendingMigrationsAsync();
            }
            return Page();
        }
    }
}
