using System;
using System.Web;
using System.Web.Security;

namespace UTILS
{
    /// <summary>
    /// Summary description for FormsAuthSessionEnforcement
    /// </summary>
    public class FormsAuthSessionEnforcement: IHttpModule
    {
        public FormsAuthSessionEnforcement ()
        { }

        public void Init ( HttpApplication context )
        {
            context.PostAuthenticateRequest += new EventHandler ( OnPostAuthenticate );
        }

        private void OnPostAuthenticate ( Object sender, EventArgs e )
        {
            HttpApplication a = ( HttpApplication ) sender;
            HttpContext c = a.Context;

            //If the user was authenticated with Forms Authentication
            //Then check the session ID.
            if ( c.User.Identity.IsAuthenticated == true )
            {
                FormsAuthenticationTicket ft =
                    ( ( FormsIdentity ) c.User.Identity ).Ticket;

                Guid g = new Guid ( ft.UserData );

                MembershipUser loginUser = Membership.GetUser ( ft.Name );
                Guid currentSession;
                //If there isn't any session information in Membership at this point
                //then it is likely the user logged out, and an old cookie is
                //being replayed.
                if ( !String.IsNullOrEmpty ( loginUser.Comment ) )
                {
                    string currentSessionString =
                        loginUser.Comment.Split ( "|".ToCharArray () ) [1];
                    currentSession = new Guid ( currentSessionString.Split ( ";".ToCharArray () ) [1] );
                }
                else
                    currentSession = Guid.Empty;

                //If the session in the cookie does not match the current session as stored
                //in the Membership database, then terminate this request
                if ( g != currentSession )
                {
                    FormsAuthentication.SignOut ();
                    FormsAuthentication.RedirectToLoginPage ();
                }

            }
        }

        public void Dispose () { }

    }

}