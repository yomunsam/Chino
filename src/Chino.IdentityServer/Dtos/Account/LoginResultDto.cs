using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Chino.IdentityServer.Dtos.Account
{
    public class LoginResultDto
    {
        [JsonPropertyName("success")]
        public bool IsSuccess { get; set; }

        [JsonPropertyName("userInfo")]
        public UserInfoDto UserInfo { get; set; }
    }
}
