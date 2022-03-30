
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities.User
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }




    }
}
