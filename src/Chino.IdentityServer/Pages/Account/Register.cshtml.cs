using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.IdentityServer.Dtos.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chino.IdentityServer.Pages.Account
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public RegisterPageDto RegisterDto { get; set; }

        public void OnGet()
        {
        }




    }
}
