using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chino.IdentityServer.ViewModels.Dashboard.Client
{
    public class ConfigurationClientViewModel
    {
        [Required]
        public string ClientId { get; set; }

        [Required]
        public string ClientName { get; set; }

        /// <summary>
        /// 客户端是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 需要验证客户端密码
        /// </summary>
        public bool RequireClientSecret { get; set; }

        public bool RequirePkce { get; set; }
        public bool AllowPlainTextPkce { get; set; }

        public bool AllowOfflineAccess { get; set; }

        public bool AllowAccessTokensViaBrowser { get; set; }

        public List<string> AllowedScopes { get; set; }
    }
}
