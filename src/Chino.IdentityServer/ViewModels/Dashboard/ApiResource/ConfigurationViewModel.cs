using System.ComponentModel.DataAnnotations;

namespace Chino.IdentityServer.ViewModels.Dashboard.ApiResource
{
    public class ConfigurationViewModel
    {
        public bool Enabled { get; set; }

        [Required]
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}
