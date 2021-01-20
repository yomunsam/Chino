using System;
using System.Threading.Tasks;
using Chino.SMS.Shared;

namespace Chino.SMS.Aliyun
{
    public class ChinoSMSAliyun : IChinoSMSService
    {
        public Task SendVerificationCode(string verificationCode, string phoneNumber)
        {
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }
    }
}
