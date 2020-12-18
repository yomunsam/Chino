using System;
using System.Threading.Tasks;
using Chino.SMS.Shared;
using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Chino.SMS.Twilio
{
    /// <summary>
    /// 
    /// </summary>
    public class ChinoSMSTwilio : IChinoSMSService
    {
        private readonly IConfiguration m_Configuration;

        private PhoneNumber FromNumber_VerificationCode;

        public ChinoSMSTwilio(IConfiguration configuration)
        {
            m_Configuration = configuration;
            string account_id = configuration["twilio:Account_SID"];
            string auth_token = configuration["twilio:Auth_Token"];

            FromNumber_VerificationCode = new PhoneNumber(configuration["twilio:VerificationCodeFromNumber"]);

            TwilioClient.Init(account_id, auth_token);
        }


        public async Task SendVerificationCode(string verificationCode, string phoneNumber)
        {
            var message = await MessageResource.CreateAsync(to: new PhoneNumber(phoneNumber),
                body: verificationCode
                );
        }



    }
}
