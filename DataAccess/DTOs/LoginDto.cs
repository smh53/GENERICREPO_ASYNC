using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class LoginDto :  IDto
    {
     
        public string? Password { get; set; }
        public string? UserName { get; set; }
    }

    public class LoginResponseDto
    {
        public string? Token { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
