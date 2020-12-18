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
        public EmailRegisterAndLoginElement Email { get; set; } = new EmailRegisterAndLoginElement(register: true, registerRequire: true, login: true, requireConfirnedEmail: false);
        public PhoneRegisterAndLoginElement Phone { get; set; } = new PhoneRegisterAndLoginElement(register: false, registerRequire: false, login: false, requireConfirnedPhoneNumber: false);

        /// <summary>
        /// 如果需要确认的项目（比如Email、手机号）中有任意一个验证通过，则账号验证成功
        /// </summary>
        public bool ConfirmOneOfThen { get; set; } = false;

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

        public class PhoneRegisterAndLoginElement : RegisterAndLoginElement
        {
            /// <summary>
            /// 需要验证手机号
            /// </summary>
            public bool RequireConfirmedPhoneNumber { get; set; }

            public PhoneRegisterAndLoginElement()
            {
            }

            public PhoneRegisterAndLoginElement(bool register, bool registerRequire, bool login, bool requireConfirnedPhoneNumber) : base(register, registerRequire, login)
            {
                this.RequireConfirmedPhoneNumber = requireConfirnedPhoneNumber;
            }
        }

        public class EmailRegisterAndLoginElement : RegisterAndLoginElement
        {
            /// <summary>
            /// 需要确认电子邮件
            /// </summary>
            public bool RequireConfirmedEmail { get; set; }

            public EmailRegisterAndLoginElement()
            {
            }

            public EmailRegisterAndLoginElement(bool register, bool registerRequire, bool login, bool requireConfirnedEmail) : base(register, registerRequire, login)
            {
                this.RequireConfirmedEmail = requireConfirnedEmail;
            }
        }



        public static ChinoAccountConfiguration GetConfiguration(IConfiguration configuration)
        {
            var conf = new ChinoAccountConfiguration();
            configuration.Bind("Chino:Account", conf);
            return conf;
        }

        

    }
}
