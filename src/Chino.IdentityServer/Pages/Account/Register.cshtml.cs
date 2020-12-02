using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.IdentityServer.Configures;
using Chino.IdentityServer.Dtos.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chino.IdentityServer.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly ChinoAccountConfiguration m_AccountConfiguration;

        public RegisterModel(ChinoAccountConfiguration chinoAccountConfiguration)
        {
            m_AccountConfiguration = chinoAccountConfiguration;
        }

        [BindProperty]
        public RegisterPageDto RegisterDto { get; set; }

        public IActionResult OnGetAsync()
        {
            
            if (!m_AccountConfiguration.EnableRegister)
            {
                //TODO: �����ô���ҳ֮�������������ת������ҳ��
                return Redirect("/");
            }

            return Page();
        }



        public async Task<IActionResult> OnPostAsync()
        {
            await Task.Yield();

            return Page();
        }








    }
}
