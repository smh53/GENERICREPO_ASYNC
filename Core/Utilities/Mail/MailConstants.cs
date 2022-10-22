using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Mail
{
    public static class MailConstants
    {
        public const string RegistrationSubject = "Complete Registration";
        public const string ResetPasswordSubject = "Password Reset";
        public const string RegistrationBodyText = "Click the link for the activation: ";
        public const string ResetPasswordBodyText = "Click the link for the password reset: ";
    }
}
