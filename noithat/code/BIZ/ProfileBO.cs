using System.Web.Profile;
using System.Web.Security;

namespace BIZ
{
    public class ProfileBO : ProfileBase
    {
        public static ProfileBO GetUserProfile(string username)
        {
            return Create(username) as ProfileBO;
        }

        public static ProfileBO GetUserProfile()
        {
            return Create(Membership.GetUser().UserName)
                as ProfileBO;
        }

        [SettingsAllowAnonymous(false)]
        public string FullName
        {
            get { return base["FullName"] as string; }
            set { base["FullName"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        public string HomePhone
        {
            get { return base["HomePhone"] as string; }
            set { base["HomePhone"] = value; }
        }


        [SettingsAllowAnonymous(false)]
        public string MobilePhone
        {
            get { return base["MobilePhone"] as string; }
            set { base["MobilePhone"] = value; }
        }


        [SettingsAllowAnonymous(false)]
        public string Address
        {
            get { return base["Address"] as string; }
            set { base["Address"] = value; }
        }



    }
}
