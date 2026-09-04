using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Local.Models
{
    public class UserModel
    {
        public MembershipUserCollection listuser { get; set; }
        
        public string searchtext { get; set; }
    }
}