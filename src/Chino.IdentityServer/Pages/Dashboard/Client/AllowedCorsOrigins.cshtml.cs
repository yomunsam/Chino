using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.IdentityServer.Services.Clients;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Nekonya;

namespace Chino.IdentityServer.Pages.Dashboard.Client
{
    public class AllowedCorsOriginsModel : PageModel
    {
        private readonly IClientService m_ClientService;
        private readonly ILogger<AllowedCorsOriginsModel> m_Logger;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        public IdentityServer4.EntityFramework.Entities.Client ClientEntity { get; set; }

        public AllowedCorsOriginsModel(IClientService clientService, 
            ILogger<AllowedCorsOriginsModel> logger)
        {
            this.m_ClientService = clientService;
            this.m_Logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            this.ClientEntity = await m_ClientService.GetClientAndAllowedCorsOrigins(this.Id);
            if (ClientEntity == null)
                return NotFound();
            return Page();
        }

        /// <summary>
        /// ֱ��Post�����������Ŀ�����Դ��
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(string origin)
        {
            if (origin.IsNullOrEmpty())
            {
                this.ModelState.AddModelError(string.Empty, "Cors origin can't be empty.");
                this.ClientEntity = await m_ClientService.GetClientAndAllowedCorsOrigins(this.Id);
                if (ClientEntity == null)
                    return NotFound();
                return Page();
            }

            this.ClientEntity = await m_ClientService.GetClientAndAllowedCorsOrigins(this.Id);
            if (ClientEntity == null)
                return NotFound();

            if (ClientEntity.AllowedCorsOrigins.Any(co => co.Origin.Equals(origin)))
                return Page(); //�Ѿ������ˣ�������

            //�������
            ClientEntity.AllowedCorsOrigins.Add(new IdentityServer4.EntityFramework.Entities.ClientCorsOrigin
            {
                Origin = origin
            });
            await m_ClientService.Update(ClientEntity);

            return Page();
        }


        /// <summary>
        /// ɾ��ĳ������Ļ�����������
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetDeletItemAsync(int corsId)
        {
            this.ClientEntity = await m_ClientService.GetClientAndAllowedCorsOrigins(this.Id);
            if (ClientEntity == null)
                return NotFound();
            var cor = ClientEntity.AllowedCorsOrigins.FirstOrDefault(aco => aco.Id == corsId);
            if(cor != null)
            {
                ClientEntity.AllowedCorsOrigins.Remove(cor);
                await m_ClientService.Update(this.ClientEntity);

                m_Logger.LogInformation("Client \"{0}\" (clientID:{1}, description:{2}) 's AllowedCorsOrigins \"{3}\" was deleted by user \"{4}\"({5})",
                    ClientEntity.ClientName,
                    ClientEntity.ClientId,
                    ClientEntity.Description,
                    cor.Origin,
                    this.User.GetSubjectId(),
                    this.User.GetDisplayName());
            }

            return Page();
        }

    }
}
