using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.IdentityServer.Services.IdentityResources;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Nekonya;

namespace Chino.IdentityServer.Pages.Dashboard.IdentityResource
{
    public class UserClaimsModel : PageModel
    {
        private readonly IIdentityResouceService m_IdentityResouceService;
        private readonly ILogger<UserClaimsModel> m_Logger;

        /// <summary>
        /// IdentityResource Id
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public IdentityServer4.EntityFramework.Entities.IdentityResource IdentityResourceEntity { get; set; }


        public UserClaimsModel(IIdentityResouceService identityResouceService,
            ILogger<UserClaimsModel> logger)
        {
            this.m_IdentityResouceService = identityResouceService;
            this.m_Logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            this.IdentityResourceEntity = await m_IdentityResouceService.GetWithUserClaimsAsync(this.Id);
            if (IdentityResourceEntity is null)
                return NotFound();

            return Page();
        }


        /// <summary>
        /// 直接Post用来添加允许的用户声明。
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(string userClaim)
        {
            if (userClaim.IsNullOrEmpty())
            {
                this.ModelState.AddModelError(string.Empty, "User claim can't be empty.");
                this.IdentityResourceEntity = await m_IdentityResouceService.GetWithUserClaimsAsync(this.Id);
                if (IdentityResourceEntity == null)
                    return NotFound();
                return Page();
            }

            this.IdentityResourceEntity = await m_IdentityResouceService.GetWithUserClaimsAsync(this.Id);
            if (IdentityResourceEntity == null)
                return NotFound();

            if (IdentityResourceEntity.UserClaims.Any(uc => uc.Type.Equals(userClaim)))
                return Page(); //已经存在了，不处理

            //处理添加
            IdentityResourceEntity.UserClaims.Add(new IdentityServer4.EntityFramework.Entities.IdentityResourceClaim
            {
                Type = userClaim
            });
            await m_IdentityResouceService.UpdateAsync(IdentityResourceEntity);

            return Page();
        }


        /// <summary>
        /// 删除某个子项的话，调用这里
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetDeletItemAsync(int claimId)
        {
            this.IdentityResourceEntity = await m_IdentityResouceService.GetWithUserClaimsAsync(this.Id);
            if (IdentityResourceEntity == null)
                return NotFound();
            var userClaim = IdentityResourceEntity.UserClaims.FirstOrDefault(uc => uc.Id == claimId);
            if (userClaim != null)
            {
                IdentityResourceEntity.UserClaims.Remove(userClaim);
                await m_IdentityResouceService.UpdateAsync(this.IdentityResourceEntity);

                m_Logger.LogInformation("IdentityResource \"{0}\" (Name:{1}, description:{2}) 's UserClaim \"{3}\" was deleted by user \"{4}\"({5})",
                    IdentityResourceEntity.DisplayName,
                    IdentityResourceEntity.Name,
                    IdentityResourceEntity.Description,
                    userClaim.Type,
                    this.User.GetSubjectId(),
                    this.User.GetDisplayName());
            }

            return Page();
        }

    }
}
