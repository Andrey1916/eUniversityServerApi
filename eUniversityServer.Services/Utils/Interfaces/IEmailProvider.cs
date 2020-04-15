using System.Threading.Tasks;

namespace eUniversityServer.Services.Utils
{
    public interface IEmailProvider
    {
        bool SendEmailConfirmationMail(string targetEmail, string targetFullName, string callbackUrl);

        bool SendForgotPasswordMail(string targetEmail, string targetFullName, string callbackUrl);
    }
}
