using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chino.IdentityServer.Pages.Account.Confirmation
{
    public class PhoneModel : PageModel
    {

        [BindProperty(SupportsGet = true)]
        public string UserId { get; set; }


        public void OnGet()
        {
        }
    }
}
