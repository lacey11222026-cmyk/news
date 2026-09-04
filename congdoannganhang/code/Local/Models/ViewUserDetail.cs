using BIZ.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Local.Models
{
    public class ViewUserDetail
    {
        public string[] rolenames { get; set; }
        public string[] user_roles { get; set; }
        public MembershipUser user { get; set; }

        public List<CATEGORY_FULL> lstcate { get; set; }
        public string userCategoryPath { get; set; }
    }
}