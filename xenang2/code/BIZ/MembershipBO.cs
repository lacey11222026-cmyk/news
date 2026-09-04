using System.Text.RegularExpressions;
using System.Web;

using System.Web.Security;

namespace BIZ
{
    public class MembershipBO
    {
        /// <summary>
        /// Checks password complexity requirements for the actual membership provider
        /// </summary>
        /// <param name="password">password to check</param>
        /// <returns>true if the password meets the req. complexity</returns>
        static public bool CheckPasswordComplexity(string password)
        {
            return CheckPasswordComplexity(Membership.Provider, password);
        }


        /// <summary>
        /// Checks password complexity requirements for the given membership provider
        /// </summary>
        /// <param name="membershipProvider">membership provider</param>
        /// <param name="password">password to check</param>
        /// <returns>true if the password meets the req. complexity</returns>
        static public bool CheckPasswordComplexity(MembershipProvider membershipProvider, string password)
        {
            if (string.IsNullOrEmpty(password)) return false;

            if (password.Length < membershipProvider.MinRequiredPasswordLength) return false;

            int nonAlnumCount = 0;
            for (int i = 0; i < password.Length; i++)
            {
                if (!char.IsLetterOrDigit(password, i)) nonAlnumCount++;
            }

            if (nonAlnumCount < membershipProvider.MinRequiredNonAlphanumericCharacters) 
                return false;

            if (!string.IsNullOrEmpty(membershipProvider.PasswordStrengthRegularExpression) &&
                !Regex.IsMatch(password, membershipProvider.PasswordStrengthRegularExpression))
            {
                return false;
            }
            return true;
        }


    }
}
