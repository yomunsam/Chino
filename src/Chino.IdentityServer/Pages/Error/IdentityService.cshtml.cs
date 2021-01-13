using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Nekonya;

namespace Chino.IdentityServer.Pages.Error
{
    /// <summary>
    /// 身份服务 错误处理页面 
    /// 用于处理OIDC的错误信息
    /// </summary>
    public class IdentityServiceModel : PageModel
    {
        private readonly IIdentityServerInteractionService m_InteractionService;
        private readonly IConfiguration m_Configuration;

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public ErrorMessage ErrorMsg { get; set; }

        public bool ShowDetail { get; set; } = false;

        public IdentityServiceModel(IIdentityServerInteractionService interactionService,
            IConfiguration configuration)
        {
            this.m_InteractionService = interactionService;
            this.m_Configuration = configuration;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (Id.IsNullOrEmpty())
            {
                return BadRequest();
            }

            ErrorMsg = await m_InteractionService.GetErrorContextAsync(this.Id);

            //如果使用管理员账号登录就显示细节
            if (User.Identity.IsAuthenticated)
            {
                ShowDetail = User.IsInRole(m_Configuration["Chino:AdminRoleName"]);
            }

            return Page();
        }
    }
}
