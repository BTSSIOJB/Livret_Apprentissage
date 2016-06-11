using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FormsAuth;
using System.IO;

namespace web_livret_apprentissage_1
{
    public partial class Logon : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnconnexion_Click(object sender, EventArgs e)
        {
            string adPath = "LDAP://192.168.0.2"; // à indiquer dans la partie administrative du site
            LdapAuthentication oauth = new LdapAuthentication(adPath);
            Session["oauth"] = oauth;

            try
            {
                if (oauth.IsAuthenticated(tbuser.Text.Trim(), tbpassword.Text.Trim())) // si authentifié
                {
                    Session["prenomNomUser"] = oauth.FilterAttribute; //appelle un accesseur sur l'objet 
                    string[] groups = oauth.GetGroups().Split(new char[] { '|' }); ; //appelle une fonction qui renvoie tous les groupes "Windows Server" d'appartenance

                    Session["groupsUser"] = groups;
                    
                    if (groups[0] == "Profs-IG.global" || groups[0] == "Profs-general.global") //Profs-general.global
                    {
                        Session["connected"] = "Professeur";
                    }
                    if (groups[0] == "gr-SIO2-Alter-ferme" || groups[0] == "gr-sio1-alter-ferme")
                    {
                        Session["connected"] = "Apprenti";
                    }
                    if (groups[0] == "ggtuteurs")
                    {
                        Session["connected"] = "Tuteur";
                    }
                    if (groups[1] == "Admins du domaine") // le groupe admin arrive en 2 car 1 est occupé par Profs-IG.global
                    {
                        Session["connected"] = "Admin";
                    }

                    
                    

                    // loguer les connexions au site
                    string dateJour = System.DateTime.Now.ToShortDateString();
                    dateJour = dateJour.Replace('/', '-');
                    StreamWriter sr = new StreamWriter(Server.MapPath("~/Log/" + dateJour + ".log"),true);
                    sr.WriteLine("Login de : " + Session["prenomNomUser"].ToString() + " ||  Fonction : " + Session["connected"].ToString() + " || Heure : " + System.DateTime.Now.ToLongTimeString());
                    sr.Close();
                    Response.Redirect("~/Default.aspx");

                }
                else
                {
                    throw new Exception("Erreur d'authentification : " + "mot de passe et/ou nom d'utilisateur incorrect !");
                }
            }
            catch(Exception ex)
            {
                lbException.Text = ex.Message; //récupère l'exception lancée depuis la classe LdapAuthentication

            }
        }
    }
}