using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Mail
{
    public  interface IMailService
    {
        Task<IResult> SendEmailAsync(MailRequest mailRequest);
    }
}
