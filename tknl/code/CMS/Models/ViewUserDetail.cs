using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CMS.Models
{
    public class ViewUserDetail
    {
        public string[] rolenames { get; set; }
        public string[] user_roles { get; set; }
        public MembershipUser user { get; set; }
    }
}