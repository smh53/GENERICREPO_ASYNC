using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
    public static class ResultMessages
    {
       public class AuthorizationMessages
        {
            public static string SuccessChangePassword = "Password changed successfully";
            public static string ErrorChangePassword = "This user does not exist or you entered wrong password";
        }
    }
}
