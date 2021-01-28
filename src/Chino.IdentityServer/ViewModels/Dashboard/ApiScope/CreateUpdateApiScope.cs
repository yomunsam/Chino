using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chino.IdentityServer.ViewModels.Dashboard.ApiScope
{
    public class CreateUpdateApiScope
    {
        public bool Enabled { get; set; }
        public bool Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
    }
}
