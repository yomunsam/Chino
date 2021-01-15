using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.IdentityServer.Services.Clients;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Nekonya;

namespace Chino.IdentityServer.Pages.Dashboard.Client
{
    public class RedirectUrisModel : PageModel
    {
        private readonly IClientService m_ClientService;
        private readonly ILogger<RedirectUrisModel> m_Logger;

        /// <summary>
        /// 客户端的实体ID
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        public IdentityServer4.EntityFramework.Entities.Client ClientEntity { get; set; }

        public RedirectUrisModel(IClientService clientService,
            ILogger<RedirectUrisModel> logger)
        {
            this.m_ClientService = clientService;
            this.m_Logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            this.ClientEntity = await m_ClientService.GetClientAndRedirectUris(this.Id);
            if (ClientEntity == null)
                return NotFound();
            return Page();
        }

        /// <summary>
        /// 直接Post用来添加允许的重定向Uri。
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(string redirectUri)
        {
            if (redirectUri.IsNullOrEmpty())
            {
                this.ModelState.AddModelError(string.Empty, "RedirectUri can't be empty.");
                this.ClientEntity = await m_ClientService.GetClientAndRedirectUris(this.Id);
                if (ClientEntity == null)
                    return NotFound();
                return Page();
            }

            this.ClientEntity = await m_ClientService.GetClientAndRedirectUris(this.Id);
            if (ClientEntity == null)
                return NotFound();

            if (ClientEntity.RedirectUris.Any(ru => ru.RedirectUri.Equals(redirectUri)))
                return Page(); //已经存在了，不处理

            //处理添加
            ClientEntity.RedirectUris.Add(new IdentityServer4.EntityFramework.Entities.ClientRedirectUri
            {
                RedirectUri = redirectUri
            });
            await m_ClientService.Update(ClientEntity);

            m_Logger.LogInformation("Client \"{0}\" (clientID:{1}, description:{2}) add RedirectUri \"{3}\" by user \"{4}\"({5})",
                    ClientEntity.ClientName,
                    ClientEntity.ClientId,
                    ClientEntity.Description,
                    redirectUri,
                    this.User.GetSubjectId(),
                    this.User.GetDisplayName());

            return Page();
        }


        /// <summary>
        /// 删除某个子项的话，调用这里
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetDeletItemAsync(int redirectUriId)
        {
            this.ClientEntity = await m_ClientService.GetClientAndRedirectUris(this.Id);
            if (ClientEntity == null)
                return NotFound();
            var redirectUri = ClientEntity.RedirectUris.FirstOrDefault(ru => ru.Id == redirectUriId);
            if (redirectUri != null)
            {
                ClientEntity.RedirectUris.Remove(redirectUri);
                await m_ClientService.Update(this.ClientEntity);

                m_Logger.LogInformation("Client \"{0}\" (clientID:{1}, description:{2}) 's RedirectUri \"{3}\" was deleted by user \"{4}\"({5})",
                    ClientEntity.ClientName,
                    ClientEntity.ClientId,
                    ClientEntity.Description,
                    redirectUri.RedirectUri,
                    this.User.GetSubjectId(),
                    this.User.GetDisplayName());
            }

            return Page();
        }


    }
}
