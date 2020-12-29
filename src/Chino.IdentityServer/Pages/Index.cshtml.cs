using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.IdentityServer.Extensions.User;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Nekonya;

namespace Chino.IdentityServer.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IStringLocalizer<IndexModel> mLocalizer;

        public string UserDisplayName { get; set; }

        public IndexModel(IStringLocalizer<IndexModel> localizer)
        {
            mLocalizer = localizer;
        }

        public void OnGet()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                UserDisplayName = this.User.GetNickName();
                if (UserDisplayName.IsNullOrEmpty())
                    UserDisplayName = this.User.GetDisplayName();
            }
        }


        public IActionResult OnPostSetCulture(string returnUrl, string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
            return LocalRedirect(returnUrl);
        }

    }
}
