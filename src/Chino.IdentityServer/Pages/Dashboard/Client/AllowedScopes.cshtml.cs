using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using Chino.IdentityServer.Services.Clients;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nekonya;
using System.Linq;
using Microsoft.Extensions.Logging;
using IdentityServer4.Extensions;
using System.Collections.Generic;

namespace Chino.IdentityServer.Pages.Dashboard.Client
{
    public class AllowedScopesModel : PageModel
    {
        private readonly IClientService m_ClientService;
        private readonly IMapper m_Mapper;
        private readonly ILogger<AllowedScopesModel> m_Logger;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        public IdentityServer4.EntityFramework.Entities.Client ClientEntity { get; set; }

        public List<string> SuggestedScopes;

        public AllowedScopesModel(IClientService clientService,
            IMapper mapper,
            ILogger<AllowedScopesModel> logger)
        {
            this.m_ClientService = clientService;
            this.m_Mapper = mapper;
            this.m_Logger = logger;


            SuggestedScopes = new List<string>
            {
                "openid",
                "profile",
                "email",
                "address",
                "phone",
                "roles"
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ClientEntity = await m_ClientService.GetClientAndAllowedScopes(this.Id);
            if (ClientEntity == null)
                return NotFound();

            foreach (var item in ClientEntity.AllowedScopes)
            {
                if (SuggestedScopes.Contains(item.Scope))
                    SuggestedScopes.Remove(item.Scope);
            }

            return Page();
        }

        /// <summary>
        /// 直接Post用来添加允许的作用域。
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(string scope)
        {
            if (scope.IsNullOrEmpty())
            {
                this.ModelState.AddModelError(string.Empty, "RedirectUri can't be empty.");
                this.ClientEntity = await m_ClientService.GetClientAndAllowedScopes(this.Id);
                if (ClientEntity == null)
                    return NotFound();
                return Page();
            }

            this.ClientEntity = await m_ClientService.GetClientAndAllowedScopes(this.Id);
            if (ClientEntity == null)
                return NotFound();

            if (ClientEntity.AllowedScopes.Any(acs => acs.Scope.Equals(scope)))
                return Page(); //已经存在了，不处理

            //处理添加
            ClientEntity.AllowedScopes.Add(new ClientScope
            {
                Scope = scope
            });
            await m_ClientService.Update(ClientEntity);

            m_Logger.LogInformation("Client \"{0}\" (clientID:{1}, description:{2}) add AllowedScope \"{3}\" by user \"{4}\"({5})",
                    ClientEntity.ClientName,
                    ClientEntity.ClientId,
                    ClientEntity.Description,
                    scope,
                    this.User.GetSubjectId(),
                    this.User.GetDisplayName());

            foreach (var item in ClientEntity.AllowedScopes)
            {
                if (SuggestedScopes.Contains(item.Scope))
                    SuggestedScopes.Remove(item.Scope);
            }

            return Page();
        }


        /// <summary>
        /// 删除某个子项的话，调用这里
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetDeletItemAsync(int scopeId)
        {
            this.ClientEntity = await m_ClientService.GetClientAndAllowedScopes(this.Id);
            if (ClientEntity == null)
                return NotFound();
            var scope = ClientEntity.AllowedScopes.FirstOrDefault(acs => acs.Id == scopeId);
            if (scope != null)
            {
                ClientEntity.AllowedScopes.Remove(scope);
                await m_ClientService.Update(this.ClientEntity);

                m_Logger.LogInformation("Client \"{0}\" (clientID:{1}, description:{2}) 's AllowedScope \"{3}\" was deleted by user \"{4}\"({5})",
                    ClientEntity.ClientName,
                    ClientEntity.ClientId,
                    ClientEntity.Description,
                    scope.Scope,
                    this.User.GetSubjectId(),
                    this.User.GetDisplayName());
            }

            foreach (var item in ClientEntity.AllowedScopes)
            {
                if (SuggestedScopes.Contains(item.Scope))
                    SuggestedScopes.Remove(item.Scope);
            }
            return Page();
        }

    }
}
