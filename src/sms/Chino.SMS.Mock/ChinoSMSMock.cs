using System;
using System.Threading.Tasks;
using Chino.SMS.Shared;
using Microsoft.Extensions.Logging;

namespace Chino.SMS.Mock
{
    public class ChinoSMSMock : IChinoSMSService
    {
        private readonly ILogger<ChinoSMSMock> m_Logger;

        public ChinoSMSMock(ILogger<ChinoSMSMock> logger)
        {
            this.m_Logger = logger;
        }

        public Task SendVerificationCode(string verificationCode, string phoneNumber)
        {
            this.m_Logger.LogInformation("Send verification code {0} to phone number {1}", verificationCode, phoneNumber);
            return Task.CompletedTask;
        }
    }
}
