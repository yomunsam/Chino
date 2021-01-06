using System.ComponentModel.DataAnnotations;

namespace Chino.IdentityServer.ViewModels.Dashboard.Client
{
    public class CreateClientViewModel
    {
        [Required]
        public string ClientId { get; set; }

        [Required]
        public string ClientName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
