using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Chino.IdentityServer.ViewModels.User
{
    public class ChangePasswordInputModel
    {

        /// <summary>
        /// 现有密码
        /// </summary>
        [Required(ErrorMessage = "current_password_required")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "new_password_required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "new_confirm_password_required")]
        [Compare("NewPassword", ErrorMessage = "invalid_new_confirm_password")]
        public string ConfirmNewPassword { get; set; }
    }
}
