using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data; //Dataset

using System.DirectoryServices;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Text;

namespace web_livret_apprentissage_1
{
    public partial class vueTravailEntrepriseParProf : System.Web.UI.Page
    {
        //Table tblcahierText = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            String[] groups = (string[])Session["groupsUser"];

            try // cas ou connecté mais pas le bon ex: tuteur au lieu de prof
            {
                if ((groups[0].Trim() != "Profs-IG.global" && groups[0].Trim() != "Profs-general.global")) //un tuteur ne peut etre admin
                { Response.Redirect("~/Logon.aspx"); } //la redirection ne fait pas s'executer le reste du code

            }
            catch //cas ou il n'est pas du tout connecté
            {
                if (groups == null)
                { Response.Redirect("~/Logon.aspx"); }

            }
            if (!Page.IsPostBack) //si la page n'a pas encore été postée au serveur
            {
                // XmlTextReader reader = new XmlTextReader("Matiere.Xml");

                XmlDocument odocXml = new XmlDocument();
                //string fileName = Path.Combine(Directory.GetCurrentDirectory(), "Matiere.Xml");
                try
                {
                    string fileName = Server.MapPath("~/Etudiants/" + Convert.ToString(Session["annee_scolaire"]) + "/" + "Etudiant.Xml");
                    odocXml.Load(fileName);
                }
                catch
                {

                    lbException.Text = "Absence de fichier xml 'Etudiant'.";
                }

                string nomEtudiant = null, prenomEtudiant = null;
                if (odocXml.HasChildNodes)
                {

                    XmlNodeList olist = odocXml.GetElementsByTagName("etudiant");
                    ddlChoixEtudiant.Items.Add(" ");
                    foreach (XmlNode onode in olist)
                    {
                        foreach (XmlNode onodeChild in onode)
                        {
                            if (onodeChild.Name == "nom_etudiant")
                            {
                                nomEtudiant = onodeChild.InnerText;

                            }
                            if (onodeChild.Name == "prenom_etudiant")
                            {
                                prenomEtudiant = onodeChild.InnerText;
                                ddlChoixEtudiant.Items.Add(nomEtudiant.Trim() + "." + prenomEtudiant.Trim());

                            }
                        }
                    }
                }


            }
            //tblcahierText = new Table();
            //tblcahierText.GridLines = GridLines.Both;
            //PlaceHolder1.Controls.Add(tblcahierText); //je rajoute le table dans le placeHolder -> permet de positionner le table sur la page au lieu de mettre le table directement dans la page asp


        }



        protected void peuplerCDT(object sender, EventArgs e)
        {
            //Récupération nom et prenom de la ddl

            string nomPrenomEtu = ddlChoixEtudiant.SelectedValue;
            int indexPoint = nomPrenomEtu.IndexOf(".", 0);
            string nomEtu = null;
            string prenomEtu = null;
            try
            { 
                nomEtu = nomPrenomEtu.Substring(0, indexPoint);
                prenomEtu = nomPrenomEtu.Substring(indexPoint + 1, nomPrenomEtu.Length - (nomEtu.Length + 1));
            }
            catch
            {
                string ex = "Choix apprenti invalide !";
                lbException.Text = ex;
            }
            

            string annee_sco = Convert.ToString(Session["annee_scolaire"]);
            string repertoireCible = Server.MapPath("livrets/parTuteur/" + annee_sco + "/");
            string[] fileEntries = Directory.GetFiles(repertoireCible, "*.Xml");

            foreach(string fileName in fileEntries)
            {
                //bool dejaTitre = false;
                int indexUnderscore = calculPositionUnderscore(fileName);

                string nomPrenom = fileName.Substring(indexUnderscore + 1, fileName.Length - (indexUnderscore + 5));
                
                if( nomPrenom == nomPrenomEtu)
                {
                    TableHeaderRow otbhr = new TableHeaderRow();
                    otbhr.BackColor = System.Drawing.Color.FromArgb(101,75,36);
                    otbhr.ForeColor = System.Drawing.Color.White;

                    TableCell otbc = new TableCell();
                    otbc.RowSpan = 1;
                    otbc.ColumnSpan = 2;
                    otbc.HorizontalAlign = HorizontalAlign.Center;
                    otbc.VerticalAlign = VerticalAlign.Middle;
                    
                    //Recuperation numTuteur
                    string numTuteur = null;
                    XmlDocument oDocEtudiant = new XmlDocument();
                    try
                    {
                        oDocEtudiant.Load(Server.MapPath("~/Etudiants/" + annee_sco + "/Etudiant.Xml"));
                    }
                    catch
                    {
                        lbException.Text = "Absence de fichier xml.";
                    }

                    XmlNodeList xmlnodeListEtudiant = oDocEtudiant.SelectNodes("/etudiants/etudiant");
                    foreach(XmlNode oNode in xmlnodeListEtudiant)
                    {
                        if(oNode["nom_etudiant"].InnerText.Trim() == nomEtu.Trim() && oNode["prenom_etudiant"].InnerText.Trim() == prenomEtu.Trim())
                        {
                            numTuteur = oNode["num_tuteur"].InnerText;
                            break;

                        }
                    }

                    //Recuperation nom et prenom tuteur
                    string nomTuteur = null, prenomTuteur = null;
                    XmlDocument oDocTuteur = new XmlDocument();
                    try
                    {
                        oDocTuteur.Load(Server.MapPath("~/Tuteurs/" + annee_sco + "/Tuteur.Xml"));
                    }
                    catch
                    {
                        lbException.Text = "Absence de fichier xml.";
                    }

                    XmlNodeList xmlnodeListTuteur = oDocTuteur.SelectNodes("/tuteurs/tuteur");

                    foreach(XmlNode oNode in xmlnodeListTuteur)
                    {
                        if( oNode["num_tuteur"].InnerText == numTuteur)
                        {
                            nomTuteur = oNode["nom_tuteur"].InnerText;
                            prenomTuteur = oNode["prenom_tuteur"].InnerText;
                            break;
                        }
                    }

                    otbc.Text = "Désignation du tuteur : " + nomTuteur + " " + prenomTuteur;
                    otbc.HorizontalAlign = HorizontalAlign.Center;
                    otbhr.Cells.Add(otbc);

                    tblcahierText.Rows.Add(otbhr);

                    XmlDocument oDocItems = new XmlDocument();
                    oDocItems.Load(fileName);
                    XmlNodeList xmlnodeListItems = oDocItems.SelectNodes("/items/item");

                    foreach(XmlNode oNode in xmlnodeListItems)
                    {
                        
                        TableHeaderRow otbhr1 = new TableHeaderRow();

                        otbhr1.BackColor = otbhr1.BackColor = System.Drawing.Color.FromArgb(217, 166, 115); //System.Drawing.Color.Orange;
                        
                        TableCell otbc1 = new TableCell();
                        otbc1.Text = "du " + oNode["dateDebut"].InnerText + " au " + oNode["dateFin"].InnerText;
                        
                        otbhr1.Cells.Add(otbc1);

                        

                        TableCell otbc2 = new TableCell();
                        
                        
                        
                        otbc2.Text = oNode["travail_realise"].InnerText;
                        otbhr1.Cells.Add(otbc2);

                        tblcahierText.Rows.Add(otbhr1);
                            
                    }


                }

            }

                       
        }

        private int calculPositionUnderscore(string fileName)
        {
            int longueurFileName = fileName.Length;
            int indexTiret = -1;
            string caract = null;

            while (caract != "_")
            {
                longueurFileName = longueurFileName - 1;
                caract = fileName.Substring(longueurFileName, 1);
                if (caract == "_")
                {

                    indexTiret = longueurFileName;
                }
            }
            return indexTiret;
        }

    }
}