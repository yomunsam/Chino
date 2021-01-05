using System.Threading.Tasks;
using Chino.EntityFramework.Shared.Entities.User;
using Chino.IdentityServer.Extensions.User;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

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

        public bool IsAdmin { get; set; } = false;

        private readonly UserManager<ChinoUser> m_UserManager;
        private readonly IConfiguration m_Configuration;

        public IndexModel(UserManager<ChinoUser> userManager,
            IConfiguration configuration)
        {
            m_UserManager = userManager;
            this.m_Configuration = configuration;
        }
        public async Task<IActionResult> OnGet()
        {
            var user_info = await m_UserManager.GetUserAsync(User);
            DisplayName = this.User.GetDisplayName();
            SubjectId = this.User.GetSubjectId();
            UserName = user_info.UserName;
            Email = user_info.Email;
            NickName = this.User.GetNickName();

            ViewData["Title"] = DisplayName ?? "User";

            //判断是否是管理器
            IsAdmin = this.User.IsInRole(m_Configuration["Chino:AdminRoleName"]);

            return Page();
        }
    }
}
