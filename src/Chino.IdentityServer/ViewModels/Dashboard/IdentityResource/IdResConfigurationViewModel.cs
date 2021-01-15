using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chino.IdentityServer.ViewModels.Dashboard.IdentityResource
{
    /// <summary>
    /// 身份资源配置ViewModel
    /// </summary>
    public class IdResConfigurationViewModel
    {
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
    }
}
