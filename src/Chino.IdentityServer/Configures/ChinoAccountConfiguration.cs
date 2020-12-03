using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Chino.IdentityServer.Configures
{
    public class ChinoAccountConfiguration
    {
        public bool EnableRegister { get; set; } = true;

        public RegisterAndLoginElement UserName { get; set; } = new RegisterAndLoginElement(register: true, registerRequire: true, login: true);
        public RegisterAndLoginElement Email { get; set; } = new RegisterAndLoginElement(register: true, registerRequire: true, login: true);
        public RegisterAndLoginElement Phone { get; set; } = new RegisterAndLoginElement(register: false, registerRequire: false, login: false);


        public class RegisterAndLoginElement
        {
            public RegisterAndLoginElement() { }
            public RegisterAndLoginElement(bool register, bool registerRequire, bool login)
            {
                this.Register = register;
                this.RegisterRequire = registerRequire;
                this.Login = login;
            }

            public bool Register { get; set; }
            public bool RegisterRequire { get; set; }

            public bool Login { get; set; }
        }



        public static ChinoAccountConfiguration GetConfiguration(IConfiguration configuration)
        {
            var conf = new ChinoAccountConfiguration();
            configuration.Bind("Chino:Account", conf);
            return conf;
        }

        

    }
}
