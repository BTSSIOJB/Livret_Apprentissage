using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Security.Principal;

namespace WebApplication1
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            string cookieName = FormsAuthentication.FormsCookieName;

            //Obtient l'objet HttpRequest pour la requête HTTP actuelle
            HttpCookie authCookie = Context.Request.Cookies[cookieName];

            if (null == authCookie)
            {
                //There is no authentication cookie.
                return;
            }
            FormsAuthenticationTicket authTicket = null;
            try
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch (Exception ex)
            {
                //Write the exception to the Event Log.
                return;
            }
            if (null == authTicket)
            {
                //Cookie failed to decrypt.
                return;
            }
            //les userdata du ticket contenu dans le cookie correpondent au groupes qui est une chaine délimitée par des pipes "|"
            string[] groups = authTicket.UserData.Split(new char[] { '|' });
            //Create an Identity.
            GenericIdentity id = new GenericIdentity(authTicket.Name, "LdapAuthentication");
           
            //This principal flows throughout the request.
            GenericPrincipal principal = new GenericPrincipal(id, groups);
            
            Context.User = principal;
            

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}