using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chino.IdentityServer.ViewModels.Dashboard.ApiResource
{
    public class CreateApiResourceInputModel
    {
        public bool Enabled { get; set; }

        [Required]
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}
