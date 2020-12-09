using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chino.IdentityServer.Services.Users.User
{
    public class DeleteModel : PageModel
    {
        [BindProperty(SupportsGet =true)]
        public string Id { get; set; }
        public void OnGet()
        {
            
        }
    }
}
