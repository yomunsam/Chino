using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Chino.IdentityServer.Configures
{
    public class ChinoAccountConfiguration
    {
        public bool EnableRegister { get; set; }

        public static ChinoAccountConfiguration GetConfiguration(IConfiguration configuration)
        {
            var conf = new ChinoAccountConfiguration();
            configuration.Bind("Chino:Account", conf);
            return conf;
        }
    }
}
