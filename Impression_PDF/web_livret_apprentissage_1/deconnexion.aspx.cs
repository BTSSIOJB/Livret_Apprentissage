using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace web_livret_apprentissage_1
{
    public partial class deconnexion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Connected"] == null)
            { Response.Redirect("~/Logon.aspx"); }
            else {
                Session["groupsUser"] = null;
                Session["prenomNomUser"] = null;
                Session["Connected"] = null;
                Response.Redirect("~/Default.aspx");
            }
           
        }
    }
}