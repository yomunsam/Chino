using Microsoft.AspNetCore.Identity;

namespace Chino.EntityFramework.Shared.Entities.User
{
    public class ChinoUser : IdentityUser
    {
        public string PhoneDialingCode { get; set; }

    }
}
