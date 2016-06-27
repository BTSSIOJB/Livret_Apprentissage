using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml.Linq;
using System.Web.UI;
using System.IO;
using System.Security.Principal; //pour authentification : classe GenericIdentity

namespace web_livret_apprentissage_1
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code qui s’exécute au démarrage de l’application
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
			
			// gestion des dates dans l'application
            DateTime odate = System.DateTime.Now;
            int mois = odate.Month;
            string anneePrecedente = (odate.Year - 1).ToString() + "-" + odate.Year.ToString();
            string anneeSuivante = odate.Year.ToString() + "-" + (odate.Year + 1);

            if (mois >= 8 && mois <= 12)
            {
                if (!File.Exists(Server.MapPath("~/Professeurs/" + anneeSuivante + "/Matiere.Xml")) && !File.Exists(Server.MapPath("~/Professeurs/" + anneeSuivante + "/Professeur.Xml")))
                {
                    File.Copy(Server.MapPath("~/Professeurs/" + anneePrecedente + "/Matiere.Xml"), Server.MapPath("~/Professeurs/" + anneeSuivante + "/Matiere.Xml"));
                    File.Copy(Server.MapPath("~/Professeurs/" + anneePrecedente + "/Professeur.Xml"), Server.MapPath("~/Professeurs/" + anneeSuivante + "/Professeur.Xml"));
                }

            }

            ScriptManager.ScriptResourceMapping.AddDefinition("jquery", new ScriptResourceDefinition
            {
                Path = "~/Scripts/jquery_1.11/jquery.js",
               // DebugPath = "~/Scripts/jquery-" + str + ".js",
                LoadSuccessExpression = "window.jQuery"
            });

            ScriptManager.ScriptResourceMapping.AddDefinition("jquery-ui", new ScriptResourceDefinition
            {
                Path = "~/Scripts/jquery_1.11/jquery-ui.min.js"
            });

            ScriptManager.ScriptResourceMapping.AddDefinition("Bootstrap", new ScriptResourceDefinition
            {
                Path = "~/Scripts/bootstrap.min.js",
                DebugPath = "~/Script/bootstrap.js",
                // les css de bootstrap sont gérés dans Content
                //CdnPath = "http://netdna.bootstrapcdn.com/twitter-bootstrap/2.3.2/js/bootstrap.js",
                //CdnDebugPath = "http://netdna.bootstrapcdn.com/twitter-bootstrap/2.3.2/js/bootstrap.min.js"
            });
          
        }

    

        void Session_start(object sender, EventArgs e)
        {
            //Détermination de l'année scolaire
            DateTime odate = System.DateTime.Now;
            int mois = odate.Month;
            if (mois >= 8 && mois <= 12)
            {

                Session["annee_scolaire"] = odate.Year.ToString() + "-" + (odate.Year + 1);

            }
            else
            {
                Session["annee_scolaire"] = (odate.Year - 1).ToString() + "-" + odate.Year.ToString();

            }

            Session["prenomNomProf"] = null;
            Session["prenomNomTuteur"] = null;

        }

        

    }
}