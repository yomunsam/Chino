using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace Chino.IdentityServer.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IStringLocalizer<IndexModel> mLocalizer;

        public IndexModel(IStringLocalizer<IndexModel> localizer)
        {
            mLocalizer = localizer;
            //foreach(var item in localizer.GetAllStrings())
            //{
            //    Console.WriteLine("item");
            //}
        }

        public void OnGet()
        {
            //this.ViewData["Title"] = mLocalizer["Home"];
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
