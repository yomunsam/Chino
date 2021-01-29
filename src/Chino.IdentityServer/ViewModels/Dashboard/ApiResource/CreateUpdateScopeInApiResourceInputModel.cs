using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chino.IdentityServer.ViewModels.Dashboard.ApiResource
{
    /// <summary>
    /// 往Api Resource添加或更新作用域的ViewModel
    /// </summary>
    public class CreateUpdateScopeInApiResourceInputModel
    {

        public bool Enabled { get; set; }

        [Required]
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }

    }
}
