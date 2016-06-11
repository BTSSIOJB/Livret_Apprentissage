using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.DirectoryServices;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Text;
using System.Data; // pour dataset
using web_livret_apprentissage_1.mesClasses;

namespace web_livret_apprentissage_1
{
    public partial class Tuteur : System.Web.UI.Page
    {
        //string prenomNomTuteur = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            /************************************************************ authentification ********************************************************************/
            String[] groups = (string[])Session["groupsUser"];

            try // cas ou connecté mais pas le bon ex: prof au lieu de tuteur
            {
                if (groups[0].Trim() != "ggtuteurs" && groups[1].Trim() != "Admins du domaine")
                { Response.Redirect("~/Logon.aspx"); }
                //else { Response.Redirect("~/Default.aspx"); }
            }
            catch //cas ou il n'est pas du tout connecté
            {
                if (groups == null)
                { Response.Redirect("~/Logon.aspx"); }

            }
            
            /**************************************************************************************************************************************************/


            if (!Page.IsPostBack) //si la page n'a pas encore été postée au serveur
            {
                try
                {
                    tbDebutPeriode.Enabled = false;
                    tbFinPeriode.Enabled = false;
                    if ((string)Session["connected"] == "Admin")
                    {
                        try
                        {
                            XmlDocument odocXml = new XmlDocument();
                            string fileName = Server.MapPath("~/Etudiants/" + Convert.ToString(Session["annee_scolaire"]) + "/" + "Etudiant.Xml");
                            odocXml.Load(fileName);
                            XmlNodeList olist = odocXml.SelectNodes("/etudiants/etudiant");
                            ddlChoixEtudiant.Items.Add(" ");
                            foreach (XmlNode onode in olist)
                            {
                                ddlChoixEtudiant.Items.Add(onode["nom_etudiant"].InnerText.Trim() + "." + onode["prenom_etudiant"].InnerText.Trim());
                            }
                        }
                        catch(Exception ex)
                        { //throw new Exception("Absence de fichier xml.");
                            lbValidationSaisie.Text = ex.Message + "\n\n" + "Prévenir un administrateur. Merci !";
                        }
                    }
                    else
                    {
                        // XmlTextReader reader = new XmlTextReader("Matiere.Xml");

                        XmlDocument odocXml = new XmlDocument();
                        //string fileName = Path.Combine(Directory.GetCurrentDirectory(), "Matiere.Xml");
                        try
                        {
                            string fileName = Server.MapPath("~/Etudiants/" + Convert.ToString(Session["annee_scolaire"]) + "/" + "Etudiant.Xml");
                            odocXml.Load(fileName);
                        }
                        catch(Exception ex)
                        {
                            //throw new Exception("Absence de fichier xml.");
                            lbValidationSaisie.Text = ex.Message + "\n\n" + "Prévenir un administrateur. Merci !";
                        }

                        if (odocXml.HasChildNodes) //que l'étudiant du tuteur
                        {

                            //XmlNodeList olistnomEtudiant = odocXml.GetElementsByTagName("nom_etudiant");
                            //XmlNodeList olistprenomEtudiant = odocXml.GetElementsByTagName("prenom_etudiant");
                            XmlNodeList olist = odocXml.SelectNodes("/etudiants/etudiant");
                            ddlChoixEtudiant.Items.Add(" ");
                            /*for (int i = 0; i < olistnomEtudiant.Count; i++)
                            {
                                ddlChoixEtudiant.Items.Add(olistnomEtudiant[i].InnerText.Trim() + "." + olistprenomEtudiant[i].InnerText.Trim());

                            }*/
                            foreach (XmlNode onode in olist) // tourne sur etudiant
                            {
                                string nomPrenomUser = (string)Session["prenomNomUser"]; //recuperation du user de la session défini dans Logon.aspx
                                string nomPrenomUserTrouve = null;
                                XmlDocument _odocXml = new XmlDocument();
                                string _fileName = Server.MapPath("~/Tuteurs/" + Convert.ToString(Session["annee_scolaire"]) + "/" + "Tuteur.Xml");
                                _odocXml.Load(_fileName);
                                XmlNodeList _olist = _odocXml.SelectNodes("/tuteurs/tuteur");

                                foreach (XmlNode _onode in _olist)
                                {
                                  
                                    if (_onode["num_tuteur"].InnerText.Trim() == onode["num_tuteur"].InnerText.Trim())
                                    {
                                        nomPrenomUserTrouve = _onode["prenom_tuteur"].InnerText.Trim() + " " + _onode["nom_tuteur"].InnerText.Trim();
                                        break;
                                    }

                                }

                                if (nomPrenomUserTrouve != null)
                                {
                                    if (nomPrenomUser.Trim().ToLower() == nomPrenomUserTrouve.Trim().ToLower()) // Si l'etudiant correspond au tuteur connecté
                                    {
                                        //ddlChoixMatiere.Items.Add(onode["designation_mat"].InnerText); // Que les matieres du prof connecté
                                        ddlChoixEtudiant.Items.Add(onode["nom_etudiant"].InnerText.Trim() + "." + onode["prenom_etudiant"].InnerText.Trim());

                                    }
                                }
                                nomPrenomUserTrouve = null;

                            } //fin foreach onode
                        }

                    }
                }
                catch(Exception ex)
                {

                    System.Drawing.Color couleur = System.Drawing.Color.Red;
                    lbValidationSaisie.ForeColor = couleur;
                    lbValidationSaisie.Width = 300;
                    lbValidationSaisie.Text = ex.Message + "\n\n" + "Prévenir un administrateur. Merci !";

                    btnValidation.Enabled = false;
                    ddlChoixEtudiant.Enabled = false;
                    tbDebutPeriode.Enabled = false;
                    tbFinPeriode.Enabled = false;
                    tbResume.Enabled = false;
                    
                    
                }
            }
        }

        protected void trouverTuteurApprenti(object sender, EventArgs e)
        {
            try
            {
                tbDebutPeriode.Enabled = true;
                int indexDdlEtudiant = ddlChoixEtudiant.SelectedIndex;
                indexDdlEtudiant--;
                string numTuteur = null;
                XmlDocument odocXml = new XmlDocument();
                //string fileName = Path.Combine(Directory.GetCurrentDirectory(), "Matiere.Xml";
                try
                {
                    string fileName = Server.MapPath("~/Etudiants/" + Convert.ToString(Session["annee_scolaire"]) + "/" + "Etudiant.Xml");
                    odocXml.Load(fileName);
                }
                catch
                {
                    throw new Exception("Absence de fichier xml.");
                }

                if (odocXml.HasChildNodes)
                {
                    XmlNodeList olistnomEtudiant = odocXml.GetElementsByTagName("nom_etudiant");
                    XmlNodeList olistprenomEtudiant = odocXml.GetElementsByTagName("prenom_etudiant");
                    int i; //pour pouvoir l'utiliser apres la boucle
                    
                    for (i = 0; i < olistnomEtudiant.Count; i++)
                    {
                       string nomPrenom = olistnomEtudiant[i].InnerText.Trim() + "." + olistprenomEtudiant[i].InnerText.Trim();
                       if(nomPrenom.Trim() == ddlChoixEtudiant.SelectedValue.Trim())
                       {
                           //sort si le nomPrenom correspond a celui choisi dans la DDL
                           break;
                       }
                       

                    }
                    ///le ième étudiant trouvé correspondra au ième numTuteur dans Etudiant.Xml
                    XmlNodeList olistnumTuteur = odocXml.GetElementsByTagName("num_tuteur");
                    numTuteur = olistnumTuteur[i].InnerText.Trim();

                }

                XmlDocument odocXml2 = new XmlDocument();

                try
                {
                    string fileName2 = Server.MapPath("~/Tuteurs/" + Convert.ToString(Session["annee_scolaire"]) + "/" + "Tuteur.Xml");
                    odocXml2.Load(fileName2);
                }
                catch
                {
                    throw new Exception("Absence de fichier xml.");
                }

                if (odocXml2.HasChildNodes)
                {

                    XmlNodeList olistNumTuteur = odocXml2.GetElementsByTagName("num_tuteur");
                    int x;
                   
                    //Permet d'avoir les numéros de tuteur pas forcément dans l'ordre dans le fichier XML
                    for(x = 0; x < olistNumTuteur.Count;x++)
                    {
                        if(olistNumTuteur[x].InnerText.Trim() == numTuteur.Trim())
                        {
                            break;
                        }

                    }
                    XmlNodeList olistPrenom = odocXml2.GetElementsByTagName("prenom_tuteur");
                    XmlNodeList olistNom = odocXml2.GetElementsByTagName("nom_tuteur");

                    //le numéro de tuteur trouvé est à la xième place dans le fichier Xml donc son nom et son prenom aussi
                    lbNomTuteur.Text = "Nom du tuteur correspondant : " + olistPrenom[x].InnerText + " " + olistNom[x].InnerText;
                    Session["prenomNomTuteur"] = olistPrenom[x].InnerText.Trim() + "_" + olistNom[x].InnerText.Trim();

                }
           
            }
            catch(Exception ex)
            {
                System.Drawing.Color couleur = System.Drawing.Color.Red;
                lbValidationSaisie.ForeColor = couleur;
                lbValidationSaisie.Width = 300;
                lbValidationSaisie.Text = ex.Message + "\n\n" + "Prévenir un administrateur. Merci !";

                
                btnValidation.Enabled = false;
                ddlChoixEtudiant.Enabled = false;
                tbDebutPeriode.Enabled = false;
                tbFinPeriode.Enabled = false;
                tbResume.Enabled = false; 

            }


        }

        protected void btnValidation_Click(object sender, EventArgs e)
        {
            if (btnValidation.Text == "Nouvelle Saisie ?")
            {
                Response.Redirect("SaisieTravailTuteur.aspx");
            }

            if (Page.IsValid)
            {
                //enregistrement des données dans les fichiers XML

                // <?xml version="1.0" encoding="utf-8" ?>

                string numTuteur = null;
                //string nomTuteur = null;
                //string prenomTuteur = null;
                XmlDocument odocEtu = new XmlDocument();
                odocEtu.Load(Server.MapPath("~/Etudiants/" + Session["annee_scolaire"] + "/Etudiant.Xml"));

                /*XmlDocument odocTut = new XmlDocument();
                odocTut.Load(Server.MapPath("~/Tuteurs/" + Session["annee_scolaire"] + "/Tuteur.Xml"));*/

                XmlNodeList xnlistEtu = odocEtu.SelectNodes("/etudiants/etudiant");

                //XmlNodeList xnlistTut = odocTut.SelectNodes("/tuteurs/tuteur");

                foreach (XmlNode onode in xnlistEtu)
                {
                    if (onode["nom_etudiant"].InnerText.ToLower() + "." + onode["prenom_etudiant"].InnerText.ToLower() == ddlChoixEtudiant.SelectedValue.ToLower())
                    {
                        numTuteur = onode["num_tuteur"].InnerText;
                    }
                }


                if (!File.Exists(Server.MapPath("~/livrets/parTuteur/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomTuteur"] + "_" + ddlChoixEtudiant.SelectedValue + ".Xml")))
                {
                    // StringBuilder sb = new StringBuilder();

                    //le fichier n'existe pas la racine "items" est créée en même temps que le premier "item"
                    XDocument doc = new XDocument(
                        new XElement("items",
                            new XElement("item", new XElement("numEtudiant", recherNumEtudiant()),
                                                 new XElement("dateDebut", tbDebutPeriode.Text.Trim()),
                                                 new XElement("dateFin", tbFinPeriode.Text.Trim()),
                                                 new XElement("travail_realise", tbResume.Text))));




                    // TextWriter tr = new StringWriter(sb);
                    // doc.Save(tr);
                    doc.Save(Server.MapPath("~/livrets/parTuteur/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomTuteur"] + "_" + ddlChoixEtudiant.SelectedValue + ".Xml"));
                    btnValidation.Text = "Nouvelle Saisie ?";
                    // btnValidation.Enabled = false;
                    lbValidationSaisie.Text = "Saisie validée. Merci !";
                }
                else
                {
                    // Verifie si il existe un contenu à la même date et à la même heure si oui alors l'ancien contenu sera écrasé
                    DataSet ods = new DataSet();
                    ods.ReadXml(Server.MapPath("~/livrets/parTuteur/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomTuteur"] + "_" + ddlChoixEtudiant.SelectedValue + ".Xml"));
                    Boolean modifExistant = false;
                    int numenreg = 0; // Permet de connaître le numéro d'enregistrement du datagrid sur lequel on intervient
                    foreach (DataRow orow in ods.Tables["item"].Rows)
                    {
                        if (Convert.ToString(orow["dateDebut"]) == tbDebutPeriode.Text.Trim() && Convert.ToString(orow["dateFin"]) == tbFinPeriode.Text.Trim())
                        {
                            orow["travail_realise"] = tbResume.Text.Trim();
                            modifExistant = true;
                            break;
                        }
                        numenreg++;

                    }

                    if (modifExistant)
                    {
                        if (tbResume.Text == string.Empty) //Si on a rentré un 'travail réalisé' vide il faut effacer l'item puis réécrire le fichier xml
                        {
                            ods.Tables["item"].Rows[numenreg].Delete();
                            if (ods.Tables["item"].Rows.Count == 0) // Cas ou le dataTable n'a plus d'enregistrement sinon erreur cas le fichier sera vide (en-tête seule)
                            {
                                // Necessite d'accorder la permission NTFS "suppression" sur le serveur à l'utilisateur sous le nom duquel tourne le process serveur Web
                                File.Delete(Server.MapPath("~/livrets/parTuteur/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomTuteur"] + "_" + ddlChoixEtudiant.SelectedValue + ".Xml"));
                            }
                            else
                            {
                                ods.WriteXml(Server.MapPath("~/livrets/parTuteur/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomTuteur"] + "_" + ddlChoixEtudiant.SelectedValue + ".Xml"));
                            }
                            lbValidationSaisie.Text = "Saisie validée, l'enregistrement a été effacé. Merci !";
                        }
                        else //on réécrit le fichier xml à partir du datatable modifié dans le ForEach ci-dessus
                        {
                            ods.WriteXml(Server.MapPath("~/livrets/parTuteur/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomTuteur"] + "_" + ddlChoixEtudiant.SelectedValue + ".Xml"));
                            lbValidationSaisie.Text = "Saisie validée, l'ancien contenu a été modifié. Merci !";
                        }
                    }
                    else
                    {

                        /********************/
                        // Le fichier existe on ne crée que les "item" dans la racine "items" existante
                        // la méthode Load est abstraite
                        XDocument odoc = XDocument.Load(Server.MapPath("~/livrets/parTuteur/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomTuteur"] + "_" + ddlChoixEtudiant.SelectedValue + ".Xml"), LoadOptions.None);
                        //"Element" permet d'obtenir le premier niveau de noeud "items" pour pouvoir rajouter le noeud "item"
                        odoc.Element("items").Add(new XElement("item", new XElement("numEtudiant", recherNumEtudiant()),
                                                                       new XElement("dateDebut", tbDebutPeriode.Text.Trim()),
                                                                       new XElement("dateFin", tbFinPeriode.Text.Trim()),
                                                                       new XElement("travail_realise", tbResume.Text)));
                        odoc.Save(Server.MapPath("~/livrets/parTuteur/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomTuteur"] + "_" + ddlChoixEtudiant.SelectedValue + ".Xml"));
                        lbValidationSaisie.Text = "Saisie validée. Merci ! En cas d'erreur recommencer la procédure pour la même période et le même apprenti.";
                    }

                    triFichierXml();
                    btnValidation.Text = "Nouvelle Saisie ?";
                   
                }


            }
            else
            {
                lbValidationSaisie.Text = "Saisie non validée. Corriger l'erreur indiquée !";
            }
        }

        protected void triFichierXml()
        {
            /* Mettre le contenu du fichier XML dans une collection et le trier */
            if (File.Exists(Server.MapPath("~/livrets/parTuteur/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomTuteur"] + "_" + ddlChoixEtudiant.SelectedValue + ".Xml")))
            {
                XDocument odocument = XDocument.Load(Server.MapPath("~/livrets/parTuteur/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomTuteur"] + "_" + ddlChoixEtudiant.SelectedValue + ".Xml"), LoadOptions.None);
                IEnumerable<XElement> ocollitems = odocument.Elements("items");
                List<CitemTE> ocollItemTE = new List<CitemTE>();

                foreach (XElement oelementItems in ocollitems)
                {
                    IEnumerable<XElement> ocollitem = oelementItems.Elements("item");
                    foreach (XElement oelementItem in ocollitem)
                    {
                        CitemTE oitemTE = new CitemTE();

                        IEnumerable<XElement> ocollchampItem1 = oelementItem.Elements("numEtudiant"); //Elements retourne une collection de 1 elt enfant de oelementItem                           
                        oitemTE.NumEtudiant = Convert.ToString(ocollchampItem1.ElementAt(0).Value);
                        IEnumerable<XElement> ocollchampItem2 = oelementItem.Elements("dateDebut");
                        oitemTE.DateDebut = Convert.ToDateTime(ocollchampItem2.ElementAt(0).Value);
                        IEnumerable<XElement> ocollchampItem3 = oelementItem.Elements("dateFin");
                        oitemTE.DateFin = Convert.ToDateTime(ocollchampItem3.ElementAt(0).Value);
                        IEnumerable<XElement> ocollchampItem4 = oelementItem.Elements("travail_realise");
                        oitemTE.Travail_realise = Convert.ToString(ocollchampItem4.ElementAt(0).Value);
                        

                        ocollItemTE.Add(oitemTE); // mis dans la collection
                    }
                }

                // obtenir une collection triée avec Linq To Object
                var ocollItemTrie = from oitem in ocollItemTE
                                    orderby oitem.DateDebut ascending
                                    select oitem;

                // je réécris le fichier XML correspondant trié
                bool premier = true;
                XDocument mondoc = new XDocument();

                foreach (CitemTE oitem in ocollItemTrie)
                {
                    if (premier)
                    {
                        mondoc.Add(
                            new XElement("items",
                                new XElement("item", new XElement("numEtudiant", oitem.NumEtudiant.Trim()),
                                                     new XElement("dateDebut", oitem.DateDebut.ToShortDateString().Trim()),
                                                     new XElement("dateFin", oitem.DateFin.ToShortDateString().Trim()),
                                                     new XElement("travail_realise", oitem.Travail_realise.Trim())
                                                    )));
                        premier = false;
                    }
                    else
                    {
                        mondoc.Element("items").Add(new XElement("item", new XElement("numEtudiant", oitem.NumEtudiant.Trim()),
                                                                         new XElement("dateDebut", oitem.DateDebut.ToShortDateString().Trim()),
                                                                         new XElement("dateFin", oitem.DateFin.ToShortDateString().Trim()),
                                                                         new XElement("travail_realise", oitem.Travail_realise.Trim())
                                                                         ));
                    }

                }

                mondoc.Save(Server.MapPath("~/livrets/parTuteur/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomTuteur"] + "_" + ddlChoixEtudiant.SelectedValue + ".Xml"));
            }
        }

        private string recherNumEtudiant()
        {
            string nomPrenom = ddlChoixEtudiant.SelectedValue.ToLower();

            int indexPoint = nomPrenom.IndexOf(".",0);

            string nom = nomPrenom.Substring(0, indexPoint);
            string prenom   = nomPrenom.Substring(indexPoint + 1, nomPrenom.Length - (indexPoint + 1));

            XmlDocument odocXml = new XmlDocument();
            string fileName = Server.MapPath("~/Etudiants/" + Convert.ToString(Session["annee_scolaire"]) + "/" + "Etudiant.Xml");
            odocXml.Load(fileName);

            XmlNodeList olist = odocXml.SelectNodes("/etudiants/etudiant");
            string numEtudiant = null; 
            foreach(XmlNode onode in olist)
            {
                if(onode["nom_etudiant"].InnerText.ToLower() == nom.Trim().ToLower() && onode["prenom_etudiant"].InnerText.ToLower() == prenom.Trim().ToLower() )
                {
                    return numEtudiant = onode["num_etudiant"].InnerText;
                    
                }

            }
            return null;

        }
        /*
        protected void imgBtnRappel_Click(object sender, ImageClickEventArgs e)
        {
            
           string fileNameLivretTuteur = Server.MapPath("~/livrets/parTuteur/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomTuteur"] + "_" + ddlChoixEtudiant.SelectedValue + ".Xml");
           DataSet ods = new DataSet();
           ods.ReadXml(fileNameLivretTuteur);


           foreach (DataRow orow in ods.Tables["item"].Rows)
           {
               if (Convert.ToString(orow["dateDebut"]) == tbDebutPeriode.Text.Trim() && Convert.ToString(orow["dateFin"]) == tbFinPeriode.Text.Trim() && Convert.ToString(orow["numEtudiant"]) == recherNumEtudiant())
               {
                   tbResume.Text = (string)orow["travail_realise"];
                   break;
               }
           }

           
        } */

        protected void rappelSaisieAnte(object sender, EventArgs e)
        {
            if (File.Exists(Server.MapPath("~/livrets/parTuteur/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomTuteur"] + "_" + ddlChoixEtudiant.SelectedValue + ".Xml")))
            {
                string fileNameLivretTuteur = Server.MapPath("~/livrets/parTuteur/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomTuteur"] + "_" + ddlChoixEtudiant.SelectedValue + ".Xml");
                DataSet ods = new DataSet();
                ods.ReadXml(fileNameLivretTuteur);


                foreach (DataRow orow in ods.Tables["item"].Rows)
                {
                    if (Convert.ToString(orow["dateDebut"]) == tbDebutPeriode.Text.Trim() && Convert.ToString(orow["dateFin"]) == tbFinPeriode.Text.Trim() && Convert.ToString(orow["numEtudiant"]) == recherNumEtudiant())
                    {
                        tbResume.Text = (string)orow["travail_realise"];
                        break;
                    }
                }
            }
        }

        protected void enabledTbFinPeriode(object sender, EventArgs e)
        {
            tbFinPeriode.Enabled = true;
        }
       
    }
}