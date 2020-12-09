using System.Threading.Tasks;
using Chino.IdentityServer.Extensions.User;
using Chino.IdentityServer.Models.User;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chino.IdentityServer.Pages.User
{
    [Authorize]
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Nickname
        /// </summary>
        public string NickName { get; set; }
        public string SubjectId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        private readonly UserManager<ChinoUser> m_UserManager;

        public IndexModel(UserManager<ChinoUser> userManager)
        {
            m_UserManager = userManager;
        }
        public async Task<IActionResult> OnGet()
        {
            var user_info = await m_UserManager.GetUserAsync(User);
            DisplayName = this.User.GetDisplayName();
            ViewData["DisplayName"] = DisplayName;
            SubjectId = this.User.GetSubjectId();
            UserName = user_info.UserName;
            Email = user_info.Email;
            NickName = this.User.GetNickName();

            ViewData["Title"] = DisplayName ?? "User";
            return Page();
        }
    }
}
