using BIZ.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace WebMVC4.Models
{
    public class ViewUserDetail
    {
        public string[] rolenames { get; set; }
        public string[] user_roles { get; set; }
        public MembershipUser user { get; set; }

        public List<CATEGORY_FULL> lstcate { get; set; }
        public string userCategoryPath { get; set; }
        public AuthorProfile AuthorProfile { get; set; }
        
    }
    public class AuthorProfile
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Avatar { get; set; }
        public string FullName { get; set; }

        public string UserName { get; set; }

        public int? Order { get; set; }
    }
}