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
    public class AllowedGrantTypesModel : PageModel
    {
        private readonly IClientService m_ClientService;
        private readonly ILogger<AllowedGrantTypesModel> m_Logger;


        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        public IdentityServer4.EntityFramework.Entities.Client ClientEntity { get; set; }

        public List<string> SuggestedItem;

        public AllowedGrantTypesModel(IClientService clientService,
            ILogger<AllowedGrantTypesModel> logger)
        {
            this.m_ClientService = clientService;
            this.m_Logger = logger;

            SuggestedItem = new List<string>
            {
                "authorization_code",       //授权码
                "client_credentials",
                "refresh_token",
                "implicit",                 //隐式
                "password",
                "urn:ietf:params:oauth:grant-type:device_code" //设备代码流
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            this.ClientEntity = await m_ClientService.GetClientAndAllowedGrantTypes(this.Id);
            if (ClientEntity == null)
                return NotFound();

            foreach(var item in ClientEntity.AllowedGrantTypes)
            {
                if (SuggestedItem.Contains(item.GrantType))
                    SuggestedItem.Remove(item.GrantType);
            }
            return Page();
        }

        /// <summary>
        /// 直接Post用来添加允许的授权类型。
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(string grantType)
        {
            if (grantType.IsNullOrEmpty())
            {
                this.ModelState.AddModelError(string.Empty, "Grant type name can't be empty.");
                this.ClientEntity = await m_ClientService.GetClientAndAllowedGrantTypes(this.Id);
                if (ClientEntity == null)
                    return NotFound();
                return Page();
            }

            this.ClientEntity = await m_ClientService.GetClientAndAllowedGrantTypes(this.Id);
            if (ClientEntity == null)
                return NotFound();

            if (ClientEntity.AllowedGrantTypes.Any(gt => gt.GrantType.Equals(grantType)))
                return Page(); //已经存在了，不处理

            //处理添加
            ClientEntity.AllowedGrantTypes.Add(new IdentityServer4.EntityFramework.Entities.ClientGrantType
            {
                GrantType = grantType
            });
            await m_ClientService.Update(ClientEntity);

            foreach (var item in ClientEntity.AllowedGrantTypes)
            {
                if (SuggestedItem.Contains(item.GrantType))
                    SuggestedItem.Remove(item.GrantType);
            }
            return Page();
        }

        /// <summary>
        /// 删除某个子项的话，调用这里
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetDeletItemAsync(int gtId)
        {
            this.ClientEntity = await m_ClientService.GetClientAndAllowedGrantTypes(this.Id);
            if (ClientEntity == null)
                return NotFound();
            var grantType = ClientEntity.AllowedGrantTypes.FirstOrDefault(gt => gt.Id == gtId);
            if (grantType != null)
            {
                ClientEntity.AllowedGrantTypes.Remove(grantType);
                await m_ClientService.Update(this.ClientEntity);

                m_Logger.LogInformation("Client \"{0}\" (clientID:{1}, description:{2}) 's AllowedGrantType \"{3}\" was deleted by user \"{4}\"({5})",
                    ClientEntity.ClientName,
                    ClientEntity.ClientId,
                    ClientEntity.Description,
                    grantType.GrantType,
                    this.User.GetSubjectId(),
                    this.User.GetDisplayName());
            }


            foreach (var item in ClientEntity.AllowedGrantTypes)
            {
                if (SuggestedItem.Contains(item.GrantType))
                    SuggestedItem.Remove(item.GrantType);
            }
            return Page();
        }
    }
}
