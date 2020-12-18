using System;
using System.Threading.Tasks;

namespace Chino.SMS.Shared
{
    public interface IChinoSMSService
    {
        Task SendVerificationCode(string verificationCode, string phoneNumber);
    }
}
