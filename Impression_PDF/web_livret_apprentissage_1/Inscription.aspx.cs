using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Xml.Linq;
using System.IO;

namespace Web_Livret_Apprentissage_1
{
    public partial class Inscription : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String[] groups = (string[])Session["groupsUser"];

            try // cas ou connecté mais pas le bon ex: prof au lieu de tuteur
            {
                if (groups[1].Trim() != "Admins du domaine")
                { Response.Redirect("~/Logon.aspx"); }
                //else { Response.Redirect("~/Default.aspx"); }
            }
            catch //cas ou il n'est pas du tout connecté
            {
                if (groups == null)
                { Response.Redirect("~/Logon.aspx"); }

            }
        }

        protected void buttonValiderInscription_Click(object sender, EventArgs e)
        {
            string type = ddlChoixType.SelectedValue;


            
            switch (type)
            {
                case "Professeur":
                    {
                        string nomProf = tbNomInscription.Text;
                        string prenomProf = tbPrenomInscription.Text;
                        int cptProf = 0;

                        string fileNameProf = "~/Professeurs/" + Session["annee_scolaire"] + "/Professeur.Xml";

                        try
                        {
                            if (File.Exists(Server.MapPath(fileNameProf)))
                            {
                                XmlDocument odocnumprof = new XmlDocument();
                                odocnumprof.Load(Server.MapPath(fileNameProf));

                                XmlNodeList xnlist = odocnumprof.SelectNodes("/professeurs/professeur");

                                bool dejaInscrit = false;
                                foreach (XmlNode onode in xnlist)
                                {
                                    if (onode["nom_prof"].InnerText == nomProf && onode["prenom_prof"].InnerText == prenomProf)
                                    {
                                        dejaInscrit = true;
                                        break;
                                    }
                                    else
                                    {
                                        cptProf++;

                                    }

                                }
                                if(dejaInscrit)
                                {
                                    throw new Exception("Professeur est déjà inscrit !");
                                }
                                XDocument odocprof = XDocument.Load(Server.MapPath(fileNameProf));

                                XElement oprof = new XElement("professeur",
                                    new XElement("num_prof", cptProf + 1),
                                    new XElement("nom_prof", nomProf),
                                    new XElement("prenom_prof", prenomProf));

                                odocprof.Element("professeurs").Add(oprof);
                                odocprof.Save(Server.MapPath(fileNameProf));


                            }
                            else
                            {
                                XDocument odocprof = new XDocument();

                                XElement oprof = new XElement("professeurs",
                                    new XElement("professeur",
                                    new XElement("num_prof", '1'),
                                    new XElement("nom_prof", nomProf),
                                    new XElement("prenom_prof", prenomProf)));

                                odocprof.Add(oprof);
                                odocprof.Save(Server.MapPath(fileNameProf));
                            }

                            lbException.Text = "Enregistrement effectué.";
                            lbException.ForeColor = System.Drawing.Color.Green;
                            tbNomInscription.Text = "";
                            tbPrenomInscription.Text = "";
                            ddlChoixType.SelectedIndex = 0;

                            
                        }
                        catch (Exception ex)
                        {
                            lbException.Text = ex.Message;
                        }

                        break;
                    }
                    
                case "Tuteur":
                    {
                        string nomTuteur = tbNomInscription.Text;
                        string prenomTuteur = tbPrenomInscription.Text;
                        string entrepriseTuteur = tbNomEntreprise.Text;
                        int cptTuteur = 0;

                        string fileNameTut = "~/Tuteurs/" + Session["annee_scolaire"] + "/Tuteur.Xml";
                        try
                        {
                            if (File.Exists(Server.MapPath(fileNameTut)))
                            {

                                XmlDocument odocnumtut = new XmlDocument();
                                odocnumtut.Load(Server.MapPath(fileNameTut));

                                XmlNodeList xnlist = odocnumtut.SelectNodes("/tuteurs/tuteur");

                                bool dejaInscrit = false;
                                foreach (XmlNode onode in xnlist)
                                {
                                    if (onode["nom_tuteur"].InnerText == nomTuteur && onode["prenom_tuteur"].InnerText == prenomTuteur)
                                    {
                                        dejaInscrit = true;
                                        break;
                                    }
                                    else
                                    {
                                        cptTuteur++;
                                    }

                                }

                                if(dejaInscrit)
                                {
                                    throw new Exception("Ce tuteur est déjà inscrit.");
                                }

                                XDocument odoctut = XDocument.Load(Server.MapPath(fileNameTut));

                                XElement otut = new XElement("tuteur",
                                    new XElement("num_tuteur", cptTuteur + 1),
                                    new XElement("nom_tuteur", nomTuteur),
                                    new XElement("prenom_tuteur", prenomTuteur),
                                    new XElement("entreprise_tuteur", entrepriseTuteur));
                                odoctut.Element("tuteurs").Add(otut);
                                odoctut.Save(Server.MapPath(fileNameTut));
                            }
                            else
                            {
                                XDocument odoctut = new XDocument();

                                XElement otut = new XElement("tuteurs",
                                    new XElement("tuteur",
                                    new XElement("num_tuteur", '1'),
                                    new XElement("nom_tuteur", nomTuteur),
                                    new XElement("prenom_tuteur", prenomTuteur),
                                    new XElement("entreprise_tuteur", entrepriseTuteur)));

                                odoctut.Add(otut);
                                odoctut.Save(Server.MapPath(fileNameTut));
                            }

                            lbException.Text = "Enregistrement effectué.";
                            lbException.ForeColor = System.Drawing.Color.Green;
                            tbNomInscription.Text = "";
                            tbPrenomInscription.Text = "";
                            tbNomEntreprise.Text = "";
                            ddlChoixType.SelectedIndex = 0;
                        }
                        catch(Exception ex)
                        {
                            lbException.Text = ex.Message;
                        }

                        break;
                    }
                case "Etudiant":
                    {
                        try
                        {
                            XmlDocument odoctuteur = new XmlDocument();

                            odoctuteur.Load(Server.MapPath("~/Tuteurs/" + Session["annee_scolaire"] + "/Tuteur.Xml"));

                            XmlNodeList xnlisttuteur = odoctuteur.SelectNodes("/tuteurs/tuteur");

                            string nomEtudiant = tbNomInscription.Text;
                            string prenomEtudiant = tbPrenomInscription.Text;
                            string nomPrenomTuteur = ddlNomTuteur.SelectedValue;
                            string anneeEtudiant = ddlChoixClasse.Text;
                            int cptEtudiant = 0;
                            string numTuteur = null;

                            foreach (XmlNode onode in xnlisttuteur)
                            {
                                if (nomPrenomTuteur == onode.SelectSingleNode("prenom_tuteur").InnerText + " " + onode.SelectSingleNode("nom_tuteur").InnerText)
                                {
                                    numTuteur = onode.SelectSingleNode("num_tuteur").InnerText;
                                    break;
                                }

                            }

                            string fileNameEtu = "~/Etudiants/" + Session["annee_scolaire"] + "/Etudiant.Xml";

                            if (File.Exists(Server.MapPath(fileNameEtu)))
                            {
                                XmlDocument odocnumetu = new XmlDocument();
                                odocnumetu.Load(Server.MapPath(fileNameEtu));

                                XmlNodeList xnlist = odocnumetu.SelectNodes("/etudiants/etudiant");

                                bool dejaInscrit = false;
                                // bool dejaTuteur = false; A été enlevé car un tuteur peut avoir plusieurs étudiant
                                foreach (XmlNode onode in xnlist)
                                {
                                    if (onode["nom_etudiant"].InnerText == nomEtudiant && onode["prenom_etudiant"].InnerText == prenomEtudiant)
                                    {
                                        dejaInscrit = true;
                                        
                                    }

                                    /*if(onode["num_tuteur"].InnerText == numTuteur)
                                    {
                                        dejaTuteur = true;
                                    }*/

                                     cptEtudiant++;
                                    
                                }

                                

                                if(dejaInscrit)
                                {
                                    throw new Exception("Cet étudiant est déjà inscrit.");
                                }
                                /*if(dejaTuteur)
                                {
                                    throw new Exception("Ce tuteur a déjà un apprenti.");
                                }*/

                                XDocument odocetu = XDocument.Load(Server.MapPath(fileNameEtu));

                                XElement oetu = new XElement("etudiant",
                                    new XElement("num_etudiant", cptEtudiant + 1),
                                    new XElement("nom_etudiant", nomEtudiant),
                                    new XElement("prenom_etudiant", prenomEtudiant),
                                    new XElement("num_tuteur", numTuteur),
                                    new XElement("annee_etudiant", anneeEtudiant));

                                odocetu.Element("etudiants").Add(oetu);
                                odocetu.Save(Server.MapPath(fileNameEtu));
                            }
                            else
                            {
                                XDocument odocetu = new XDocument();

                                XElement oetu = new XElement("etudiants",
                                    new XElement("etudiant",
                                    new XElement("num_etudiant", '1'),
                                    new XElement("nom_etudiant", nomEtudiant),
                                    new XElement("prenom_etudiant", prenomEtudiant),
                                    new XElement("num_tuteur", numTuteur),
                                    new XElement("annee_etudiant", anneeEtudiant)));

                                odocetu.Add(oetu);
                                odocetu.Save(Server.MapPath(fileNameEtu));
                            }

                            lbException.Text = "Enregistrement effectué.";
                            lbException.ForeColor = System.Drawing.Color.Green;
                            tbNomInscription.Text = "";
                            tbPrenomInscription.Text = "";
                            ddlChoixType.SelectedIndex = 0;
                        }
                        catch(Exception ex)
                        {
                            System.Drawing.Color couleur = System.Drawing.Color.Red;
                            lbException.ForeColor = couleur;
                            lbException.Text = ex.Message;
                        }
                        break;
                    }
                case "Matiere":
                    {

                        XmlDocument odocprofesseur = new XmlDocument();
                        odocprofesseur.Load(Server.MapPath("~/Professeurs/" + Session["annee_scolaire"] + "/Professeur.Xml"));

                        XmlNodeList xnlistprofesseur = odocprofesseur.SelectNodes("/professeurs/professeur");

                        string nomProfesseur = ddlChoixProf.SelectedValue;
                        
                        /*string[] tabProf = new string[2];
                        tabProf = null;
                        tabProf = nomProfesseur.Split('.');
                        */
                        string numProfesseur = null;

                        foreach (XmlNode onode in xnlistprofesseur)
                        {
                            if (nomProfesseur == (onode["prenom_prof"].InnerText + " " + onode["nom_prof"].InnerText))
                            {
                                numProfesseur = onode["num_prof"].InnerText;
                                break;
                            }

                        }

                        XmlDocument odocmatiere = new XmlDocument();
                        odocmatiere.Load(Server.MapPath("~/Professeurs/" + Session["annee_scolaire"] + "/Matiere.Xml"));

                        XmlNodeList xnlistmatiere = odocmatiere.SelectNodes("/matieres/matiere");

                        string designationMatiere = ddlChoixMatiere.SelectedValue;
                        string numMatiere = null;

                        foreach (XmlNode onode in xnlistmatiere)
                        {
                            if (onode.SelectSingleNode("designation_mat").InnerText == designationMatiere)
                            {
                                numMatiere = onode.SelectSingleNode("num_mat").InnerText;
                                break;
                            }
                        }

                        string fileNameMat = "~/Professeurs/" + Session["annee_scolaire"] + "/Matiere.Xml";

                        if (File.Exists(Server.MapPath(fileNameMat)))
                        {
                            foreach (XmlNode onodemat in xnlistmatiere)
                            {
                                if (onodemat.SelectSingleNode("num_mat").InnerText == numMatiere)
                                {
                                    onodemat.SelectSingleNode("num_prof").InnerText = numProfesseur;
                                    break;
                                }
                            }

                            odocmatiere.Save(Server.MapPath(fileNameMat));
                        }

                        lbException.Text = "Enregistrement effectué.";
                        lbException.ForeColor = System.Drawing.Color.Green;
                        tbNomInscription.Text = "";
                        tbPrenomInscription.Text = "";
                        ddlChoixType.SelectedIndex = 0;

                        break;
                    }
            }
        }

        protected void ddlChoixType_TextChanged(object sender, EventArgs e)
        {
            string type = ddlChoixType.SelectedValue;
            lbException.Text = "";
            switch (type)
            {
                case "":
                    {
                        lbChoixMatiere.Visible = false;
                        ddlChoixMatiere.Visible = false;
                        lbNomInscription.Visible = false;
                        tbNomInscription.Visible = false;
                        lbPrenomInscription.Visible = false;
                        tbPrenomInscription.Visible = false;
                        lbNomEntreprise.Visible = false;
                        tbNomEntreprise.Visible = false;
                        lbNomTuteur.Visible = false;
                        ddlNomTuteur.Visible = false;
                        lbClasseEtudiant.Visible = false;
                        ddlChoixClasse.Visible = false;
                        lbChoixProf.Visible = false;
                        ddlChoixProf.Visible = false;
                        rfvChoixMatiere.Enabled = false;
                        rfvChoixProfesseur.Enabled = false;
                        rfvChoixTuteur.Enabled = false;
                        rfvClasseEtudiant.Enabled = false;
                        rfvNomEntreprise.Enabled = false;
                        rfvNomInscription.Enabled = false;
                        rfvPrenomInscription.Enabled = false;
                        break;
                    }
                case "Professeur":
                    {
                        lbChoixMatiere.Visible = false;
                        ddlChoixMatiere.Visible = false;
                        lbNomInscription.Visible = true;
                        lbNomInscription.Text = "Nom du professeur :";
                        tbNomInscription.Visible = true;
                        tbNomInscription.Text = "";
                        lbPrenomInscription.Visible = true;
                        lbPrenomInscription.Text = "Prénom du professeur :";
                        tbPrenomInscription.Visible = true;
                        tbPrenomInscription.Text = "";
                        lbNomEntreprise.Visible = false;
                        tbNomEntreprise.Visible = false;
                        lbNomTuteur.Visible = false;
                        ddlNomTuteur.Visible = false;
                        lbClasseEtudiant.Visible = false;
                        ddlChoixClasse.Visible = false;
                        lbChoixProf.Visible = false;
                        ddlChoixProf.Visible = false;
                        rfvChoixMatiere.Enabled = false;
                        rfvChoixProfesseur.Enabled = false;
                        rfvChoixTuteur.Enabled = false;
                        rfvClasseEtudiant.Enabled = false;
                        rfvNomEntreprise.Enabled = false;
                        rfvNomInscription.Enabled = true;
                        rfvPrenomInscription.Enabled = true;
                        break;
                    }
                case "Tuteur":
                    {
                        lbChoixMatiere.Visible = false;
                        ddlChoixMatiere.Visible = false;
                        lbNomInscription.Visible = true;
                        lbNomInscription.Text = "Nom du tuteur :";
                        tbNomInscription.Visible = true;
                        tbNomInscription.Text = "";
                        lbPrenomInscription.Visible = true;
                        lbPrenomInscription.Text = "Prénom du tuteur :";
                        tbPrenomInscription.Visible = true;
                        tbPrenomInscription.Text = "";
                        lbNomEntreprise.Visible = true;
                        tbNomEntreprise.Visible = true;
                        lbNomTuteur.Visible = false;
                        ddlNomTuteur.Visible = false;
                        lbClasseEtudiant.Visible = false;
                        ddlChoixClasse.Visible = false;
                        lbChoixProf.Visible = false;
                        ddlChoixProf.Visible = false;
                        rfvChoixMatiere.Enabled = false;
                        rfvChoixProfesseur.Enabled = false;
                        rfvChoixTuteur.Enabled = false;
                        rfvClasseEtudiant.Enabled = false;
                        rfvNomEntreprise.Enabled = true;
                        rfvNomInscription.Enabled = true;
                        rfvPrenomInscription.Enabled = true;

                        break;
                    }
                case "Etudiant":
                    {
                        lbChoixMatiere.Visible = false;
                        ddlChoixMatiere.Visible = false;
                        lbNomInscription.Visible = true;
                        lbNomInscription.Text = "Nom de l'étudiant :";
                        tbNomInscription.Visible = true;
                        tbNomInscription.Text = "";
                        lbPrenomInscription.Visible = true;
                        lbPrenomInscription.Text = "Prénom de l'étudiant :";
                        tbPrenomInscription.Visible = true;
                        tbPrenomInscription.Text = "";
                        lbNomEntreprise.Visible = false;
                        tbNomEntreprise.Visible = false;
                        lbNomTuteur.Visible = true;
                        ddlNomTuteur.Visible = true;
                        lbClasseEtudiant.Visible = true;
                        ddlChoixClasse.Visible = true;
                        lbChoixProf.Visible = false;
                        ddlChoixProf.Visible = false;
                        rfvChoixMatiere.Enabled = false;
                        rfvChoixProfesseur.Enabled = false;
                        rfvChoixTuteur.Enabled = true;
                        rfvClasseEtudiant.Enabled = true;
                        rfvNomEntreprise.Enabled = false;
                        rfvNomInscription.Enabled = true;
                        rfvPrenomInscription.Enabled = true;

                        XmlDocument odoctuteur = new XmlDocument();

                        try
                        {

                            odoctuteur.Load(Server.MapPath("~/Tuteurs/" + Session["annee_scolaire"] + "/Tuteur.Xml"));

                            XmlNodeList xnlisttuteur = odoctuteur.SelectNodes("/tuteurs/tuteur");

                            ddlNomTuteur.Items.Clear();

                            foreach (XmlNode onode in xnlisttuteur)
                            {
                                ddlNomTuteur.Items.Add(onode.SelectSingleNode("prenom_tuteur").InnerText + " " + onode.SelectSingleNode("nom_tuteur").InnerText);
                            }

                        }
                        catch(Exception ex)
                        {
                            lbException.Text = ex.Message;
                        }
                    
                        break;
                    }

                case "Matiere":
                    {
                        lbChoixMatiere.Visible = true;
                        ddlChoixMatiere.Visible = true;
                        lbNomInscription.Visible = false;
                        tbNomInscription.Visible = false;
                        tbNomInscription.Text = "";
                        lbPrenomInscription.Visible = false;
                        tbPrenomInscription.Visible = false;
                        tbPrenomInscription.Text = "";
                        lbNomEntreprise.Visible = false;
                        tbNomEntreprise.Visible = false;
                        lbNomTuteur.Visible = false;
                        ddlNomTuteur.Visible = false;
                        lbClasseEtudiant.Visible = false;
                        ddlChoixClasse.Visible = false;
                        lbChoixProf.Visible = true;
                        ddlChoixProf.Visible = true;
                        rfvChoixMatiere.Enabled = true;
                        rfvChoixProfesseur.Enabled = true;
                        rfvChoixTuteur.Enabled = false;
                        rfvClasseEtudiant.Enabled = false;
                        rfvNomEntreprise.Enabled = false;
                        rfvNomInscription.Enabled = false;
                        rfvPrenomInscription.Enabled = false;

                        try
                        {
                            XmlDocument odocmatiere = new XmlDocument();
                            odocmatiere.Load(Server.MapPath("~/Professeurs/" + Session["annee_scolaire"] + "/Matiere.Xml"));

                            XmlNodeList xnlistmatiere = odocmatiere.SelectNodes("/matieres/matiere");

                            ddlChoixMatiere.Items.Clear();

                            foreach (XmlNode onode in xnlistmatiere)
                            {
                                ddlChoixMatiere.Items.Add(onode.SelectSingleNode("designation_mat").InnerText);
                            }

                            XmlDocument odocprofesseur = new XmlDocument();
                            odocprofesseur.Load(Server.MapPath("~/Professeurs/" + Session["annee_scolaire"] + "/Professeur.Xml"));

                            XmlNodeList xnlistprofesseur = odocprofesseur.SelectNodes("/professeurs/professeur");

                            ddlChoixProf.Items.Clear();

                            foreach (XmlNode onode in xnlistprofesseur)
                            {
                                ddlChoixProf.Items.Add(onode.SelectSingleNode("prenom_prof").InnerText + " " + onode.SelectSingleNode("nom_prof").InnerText);
                            }
                        }
                        catch (Exception ex)
                        {
                            lbException.Text = ex.Message;
                        }
                        break;
                    }
            }

        }

    }
}

