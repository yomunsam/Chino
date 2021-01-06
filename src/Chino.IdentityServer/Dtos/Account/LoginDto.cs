using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Chino.IdentityServer.Dtos.Account
{
    /// <summary>
    /// Login
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// Username or email address
        /// </summary>
        [JsonPropertyName("identity")]
        public string IdentityString { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("rememberMe")]
        public bool RememberMe { get; set; }
    }
}
