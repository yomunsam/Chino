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
    /// ��ݷ��� ������ҳ�� 
    /// ���ڴ���OIDC�Ĵ�����Ϣ
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

            //���ʹ�ù���Ա�˺ŵ�¼����ʾϸ��
            if (User.Identity.IsAuthenticated)
            {
                ShowDetail = User.IsInRole(m_Configuration["Chino:AdminRoleName"]);
            }

            return Page();
        }
    }
}
