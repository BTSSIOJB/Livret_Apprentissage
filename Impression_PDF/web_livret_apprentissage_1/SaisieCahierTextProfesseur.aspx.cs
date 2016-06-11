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
//using FormsAuth; // pour classe perso  LDAP
using web_livret_apprentissage_1.mesClasses;

namespace web_livret_apprentissage_1
{
    public partial class professeur : System.Web.UI.Page
    {
   
        protected void Page_Load(object sender, EventArgs e)
        {
            /********************************************************** authentification LDAP **************************************************/
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
            /************************************************************************************************************************************/
            
            if (!Page.IsPostBack) //si la page n'a pas encore été postée au serveur
            {
                try
                {

                    XmlDocument odocXml = new XmlDocument();
                    //string fileName = Path.Combine(Directory.GetCurrentDirectory(), "Matiere.Xml");
                    string fileName = Server.MapPath("~/Professeurs/" + Convert.ToString(Session["annee_scolaire"]) + "/" + "Matiere.Xml");
                    odocXml.Load(fileName);
                    if (odocXml.HasChildNodes)
                    {
                        if ((string)Session["connected"] == "Admin")
                        {
                            XmlNodeList olist = odocXml.SelectNodes("/matieres/matiere"); //selectionne les noeuds matiere et les noeuds enfants ex: num_prof
                            ddlChoixMatiere.Items.Add(" ");
                            foreach (XmlNode onode in olist)
                            {
                                if (onode["num_prof"].InnerText != "") //n'affiche que les matières affectées à un professeur
                                {
                                    ddlChoixMatiere.Items.Add(onode["designation_mat"].InnerText); // Que les matieres du prof connecté

                                }

                            }
                        }
                        else
                        {
                            string nomPrenomUser = (string)Session["prenomNomUser"]; //recuperation du user de la session défini dans Logon.aspx
                            string nomPrenomUserTrouve = null;
                            XmlNodeList olist = odocXml.SelectNodes("/matieres/matiere"); //selectionne les noeuds matiere et les noeuds enfants ex: num_prof
                            ddlChoixMatiere.Items.Add(" ");
                            
                            foreach (XmlNode onode in olist)
                            {
                                //recuperation du nom et prenom du prof titulaire de la matière
                                XmlDocument _odocXml = new XmlDocument();
                                string _fileName = Server.MapPath("~/Professeurs/" + Convert.ToString(Session["annee_scolaire"]) + "/" + "Professeur.Xml");
                                _odocXml.Load(_fileName);
                                XmlNodeList _olist = _odocXml.SelectNodes("/professeurs/professeur");
                                foreach(XmlNode _onode in _olist)
                                {
                                    if(_onode["num_prof"].InnerText.Trim() ==  onode["num_prof"].InnerText.Trim())
                                    {
                                       nomPrenomUserTrouve =  _onode["prenom_prof"].InnerText.Trim() + " " + _onode["nom_prof"].InnerText.Trim();
                                       break;
                                    }

                                }

                                if (nomPrenomUserTrouve != null)
                                {
                                    if (nomPrenomUser.Trim().ToLower() == nomPrenomUserTrouve.Trim().ToLower()) // si le prof connecte correspond a la matière
                                    {
                                        ddlChoixMatiere.Items.Add(onode["designation_mat"].InnerText); // Que les matieres du prof connecté

                                    }
                                }
                                nomPrenomUserTrouve = null;
                                /*
                                if (onode["num_prof"].InnerText != "") //n'affiche que les matières affectées à un professeur
                                {
                                    ddlChoixMatiere.Items.Add(onode["designation_mat"].InnerText); // Que les matieres du prof connecté

                                }*/

                            }

                        }
                    }


                    ddlHeureDebut.Items.Add(" ");
                    ddlHeureDebut.Items.Add("8");
                    ddlHeureDebut.Items.Add("9");
                    ddlHeureDebut.Items.Add("10");
                    ddlHeureDebut.Items.Add("11");
                    ddlHeureDebut.Items.Add("12");
                    ddlHeureDebut.Items.Add("13");
                    ddlHeureDebut.Items.Add("14");
                    ddlHeureDebut.Items.Add("15");
                    ddlHeureDebut.Items.Add("16");
                    ddlHeureDebut.Items.Add("17");

                    ddlHeureFin.Items.Add(" ");
                    ddlHeureFin.Items.Add("9");
                    ddlHeureFin.Items.Add("10");
                    ddlHeureFin.Items.Add("11");
                    ddlHeureFin.Items.Add("12");
                    ddlHeureFin.Items.Add("13");
                    ddlHeureFin.Items.Add("14");
                    ddlHeureFin.Items.Add("15");
                    ddlHeureFin.Items.Add("16");
                    ddlHeureFin.Items.Add("17");
                    ddlHeureFin.Items.Add("18");

                    ddlHeureDebut.Enabled = false;
                    ddlHeureFin.Enabled = false;

                }
                catch(Exception ex)
                {
                    System.Drawing.Color couleur = System.Drawing.Color.Red;
                    lbValidationSaisie.ForeColor = couleur;
                    lbValidationSaisie.Width = 300;
                    lbValidationSaisie.Text = ex.Message + "\n\n" + "Prévenir un administrateur. Merci !";

                    btnValidation.Enabled = false;
                    ddlChoixMatiere.Enabled = false;
                    ddlHeureDebut.Enabled = false;
                    ddlHeureFin.Enabled = false;
                    tbResume.Enabled = false;
                    tbDateCours.Enabled = false;
                   

                }
            }

        }
        protected void trouverProfMatiere(object sender, EventArgs e)
        {
            try
            {
                int indexDdlMAt = ddlChoixMatiere.SelectedIndex;
                indexDdlMAt--;
                string numProf = null;
                XmlDocument odocXml = new XmlDocument();
                //string fileName = Path.Combine(Directory.GetCurrentDirectory(), "Matiere.Xml");
                try
                {
                    string fileName = Server.MapPath("~/Professeurs/" + Convert.ToString(Session["annee_scolaire"]) + "/" + "Matiere.Xml");
                    odocXml.Load(fileName);
                }
                catch
                {
                    lbValidationSaisie.Text = "Absence de fichier xml.";
                }

                if (odocXml.HasChildNodes)
                {
                    XmlNodeList olistdesignMat = odocXml.GetElementsByTagName("designation_mat");
                    XmlNodeList olistnumProf = odocXml.GetElementsByTagName("num_prof");
                    for(int i = 0; i < olistdesignMat.Count;i++)
                    {
                        if(olistdesignMat[i].InnerText.Trim() == ddlChoixMatiere.SelectedValue.Trim())
                        {
                            numProf = olistnumProf[i].InnerText.Trim(); //numprof correspondant à la matière dans le fichier xml
                            break;
                        }

                    }

                }

                XmlDocument odocXml2 = new XmlDocument();
                try
                {
                    string fileName2 = Server.MapPath("~/Professeurs/" + Convert.ToString(Session["annee_scolaire"]) + "/" + "Professeur.Xml");
                    odocXml2.Load(fileName2);
                }
                catch
                {
                    throw new Exception("Abscence de fichier xml.");
                }
                if (odocXml2.HasChildNodes)
                {
                    XmlNodeList olistNumProf = odocXml2.GetElementsByTagName("num_prof");
                    int i;
                    for (i = 0; i < olistNumProf.Count; i++)
                    {
                        if (olistNumProf[i].InnerText.Trim() == numProf)
                        {                           
                            break; //j'ai trouvé la place du numProf dans le fichier de Professeur
                        }

                    }

                    XmlNodeList olistPrenom = odocXml2.GetElementsByTagName("prenom_prof");
                    XmlNodeList olistNom = odocXml2.GetElementsByTagName("nom_prof");
                    //je choisis le point pour ne pas avoir les même séparations : le repérage dans le nom du fichier sera plus facile
                    lbNomProf.Text = "Nom du professeur correspondant : " +  olistPrenom[i].InnerText.Trim() + " " + olistNom[i].InnerText.Trim();
                    Session["prenomNomProf"] = olistPrenom[i].InnerText.Trim() + "." + olistNom[i].InnerText.Trim(); // variable utilisé dans la structure du nom de fichier
                }


            }
            catch(Exception ex)
            {
                System.Drawing.Color couleur = System.Drawing.Color.Red;
                lbValidationSaisie.ForeColor = couleur;
                lbValidationSaisie.Width = 300;
                lbValidationSaisie.Text = ex.Message + "\n\n" + "Prévenir un administrateur. Merci !";

                btnValidation.Enabled = false;
                ddlChoixMatiere.Enabled = false;
                ddlHeureDebut.Enabled = false;
                ddlHeureFin.Enabled = false;
                tbResume.Enabled = false;
                tbDateCours.Enabled = false;
            }


        }

        protected void btnValidation_Click(object sender, EventArgs e)
        {
            if(btnValidation.Text == "Nouvelle Saisie ?")
            {
                Response.Redirect("SaisieCahierTextProfesseur.aspx");
            }

            string x = (string)Request.Form["testSelect"];

            if(Page.IsValid)
            {
                //enregistrement des données dans les fichiers XML

                // <?xml version="1.0" encoding="utf-8" ?>

                //Recuperation de la matière et de l'annéeBTS (premiere ou seconde)  


                int indexTiret = ddlChoixMatiere.SelectedValue.IndexOf("-", 0);
                
                string anneeBTS = ddlChoixMatiere.SelectedValue.Substring(0, indexTiret);
                
                string matiere = ddlChoixMatiere.SelectedValue.Substring(indexTiret + 1, (ddlChoixMatiere.SelectedValue.Length) - (indexTiret + 1));

                matiere = matiere.ToUpper();

                anneeBTS = anneeBTS.ToUpper(); //les matières générales seront estempillées SIO1 ou SIO2 uniquement

                if(matiere == "SI1" || matiere == "SI2" || matiere == "SI3" || matiere == "SI4" || matiere == "SI5" || matiere == "SI6")
                {
                    anneeBTS = "SIO1COMMUN"; // cela me permet de regrouper les SIx - Ces regroupement me faciliteront la tâche pour la lecture lors de la génération des LIVRETS me semble t-il
                }
                if(matiere == "SLAM1" || matiere == "SLAM2")
                {
                    anneeBTS = "SIO1SLAM"; // Cela me permet de regrouper les SLAM de première année
                }
                if(matiere == "SISR1" || matiere == "SISR2")
                {
                    anneeBTS = "SIO1SISR"; // Cela me permet de regrouper les SISR de première année 
                }
                if (matiere == "SLAM3" || matiere == "SLAM4" || matiere == "SLAM5")
                {

                    anneeBTS = "SIO2SLAM"; // Cela me permet de regrouper les SLAM de deuxième  année
                }
                if (matiere == "SI7")
                {
                    anneeBTS = "SIO2COMMUN"; // enseignement commun de deuxième année
                }
                if (matiere == "SISR3" || matiere == "SISR4" || matiere == "SISR5")
                {

                    anneeBTS = "SIO2SISR"; // Cela me permet de regrouper les SISR de deuxième année
                }

                string annee = anneeBTS.Substring(0, 4);

                /****************************récupération du numéro de matière pour inscription dans le fichier Xml***********************/
                string numMat = null;
                XmlDocument odocmat = new XmlDocument();
                odocmat.Load(Server.MapPath("~/Professeurs/" + Session["annee_scolaire"] + "/Matiere.Xml"));

                //XmlDocument odocprof = new XmlDocument();
                //odocprof.Load(Server.MapPath("~/Professeurs/" + Session["annee_scolaire"] + "/Professeur.Xml"));

                XmlNodeList xnlistmat = odocmat.SelectNodes("/matieres/matiere");

                //XmlNodeList xnlistprof = odocprof.SelectNodes("/professeurs/professeur");

                foreach (XmlNode onodemat in xnlistmat)
                {
                    if(onodemat["designation_mat"].InnerText == ddlChoixMatiere.SelectedValue)
                    {
                        numMat = onodemat["num_mat"].InnerText;
                    }
                }
                /*************************************************************************************************************************/

                string fileNameLivretProf = Server.MapPath("~/livrets/parProf/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomProf"] + "_" + matiere + "-" + anneeBTS + ".Xml");
               
                // je ne met pas les mêmes séparateurs "nom.prenom_matiere-classe.xml" afin de faciliter la recherche dans le nom de fichier
                    if (!File.Exists(fileNameLivretProf))
                    {
                        // StringBuilder sb = new StringBuilder();

                        //le fichier n'existe pas la racine "items" est créée en même temps que le premier "item"
                        XDocument doc = new XDocument(
                            new XElement("items",
                                new XElement("item", new XElement("numMatiere", numMat.Trim()),
                                                     new XElement("date", tbDateCours.Text.Trim()),
                                                     new XElement("heure_debut", ddlHeureDebut.SelectedValue),
                                                     new XElement("heure_fin", ddlHeureFin.SelectedValue),
                                                     new XElement("resume_cours", tbResume.Text)
                                                    )));




                        // TextWriter tr = new StringWriter(sb);
                        // doc.Save(tr);
                        doc.Save(Server.MapPath("~/livrets/parProf/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomProf"] + "_" + matiere + "-" + anneeBTS + ".Xml"));
                        btnValidation.Text = "Nouvelle Saisie ?";
                        // btnValidation.Enabled = false;
                        lbValidationSaisie.Text = "Saisie validée. Merci !";
                    }
                    else // Avec possibilité de modification
                    {
                        // Verifie si il existe un contenu à la même date et à la même heure si oui alors l'ancien contenu sera écrasé
                        DataSet ods = new DataSet();
                        ods.ReadXml(Server.MapPath("~/livrets/parProf/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomProf"] + "_" + matiere + "-" + anneeBTS + ".Xml"));
                        Boolean modifExistant = false;
                        int numenreg = 0; // Permet de connaître le numéro d'enregistrement du datagrid sur lequel on intervient
                        foreach(DataRow orow in ods.Tables["item"].Rows)
                        {
                            if(Convert.ToString(orow["date"]) == tbDateCours.Text.Trim() && Convert.ToString(orow["heure_debut"]) == ddlHeureDebut.SelectedValue && Convert.ToString(orow["heure_fin"]) == ddlHeureFin.SelectedValue)
                            {
                                orow["resume_cours"] = tbResume.Text.Trim();
                                modifExistant = true;
                                break;
                            }
                            numenreg++;

                        }

                        if (modifExistant)
                        {
                            if (tbResume.Text == string.Empty) //Si on a saisi un 'resumé de cours' vide il faut effacer l'item puis réécrire le fichier xml
                            {
                                ods.Tables["item"].Rows[numenreg].Delete();
                                if (ods.Tables["item"].Rows.Count == 0) // Cas ou le dataTable n'a plus d'enregistrement sinon erreur cas le fichier sera vide (en-tête seule)
                                {
                                    // Necessite d'accorder la permission NTFS "suppression" sur le serveur à l'utilisateur sous le nom duquel tourne le process serveur Web
                                    File.Delete(Server.MapPath("~/livrets/parProf/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomProf"] + "_" + matiere + "-" + anneeBTS + ".Xml"));
                                }
                                else
                                {
                                    ods.WriteXml(Server.MapPath("~/livrets/parProf/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomProf"] + "_" + matiere + "-" + anneeBTS + ".Xml"));
                                }
                                lbValidationSaisie.Text = "Saisie validée, l'enregistrement a été effacé. Merci !";
                            }
                            else //on réécrit le fichier xml à partir du datatable modifié dans le ForEach ci-dessus
                            {
                                ods.WriteXml(Server.MapPath("~/livrets/parProf/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomProf"] + "_" + matiere + "-" + anneeBTS + ".Xml"));
                                lbValidationSaisie.Text = "Saisie validée, l'ancien contenu a été modifié. Merci !";
                            }
                        }
                        else
                        {
                            // Le fichier existe on ne crée que les "item" dans la racine "items" existante
                            // la méthode Load est abstraite
                            XDocument odoc = XDocument.Load(Server.MapPath("~/livrets/parProf/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomProf"] + "_" + matiere + "-" + anneeBTS + ".Xml"), LoadOptions.None);
                            //"Element" permet d'obtenir le premier niveau de noeud "items" pour pouvoir rajouter le noeud "item"
                            odoc.Element("items").Add(new XElement("item", new XElement("numMatiere", numMat.Trim()),
                                                                           new XElement("date", tbDateCours.Text.Trim()),
                                                                           new XElement("heure_debut", ddlHeureDebut.SelectedValue),
                                                                           new XElement("heure_fin", ddlHeureFin.SelectedValue),
                                                                           new XElement("resume_cours", tbResume.Text)));
                            odoc.Save(Server.MapPath("~/livrets/parProf/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomProf"] + "_" + matiere + "-" + anneeBTS + ".Xml"));
                            lbValidationSaisie.Text = "Saisie validée. Merci ! En cas d'erreur de saisie, accepter une nouvelle saisie, votre modification sera prise en compte après validation.";
                        }

                        triFichierXml(matiere, anneeBTS); // La méthode est juste celle qui suit

                        btnValidation.Text = "Nouvelle Saisie ?";
                        // btnValidation.Enabled = false;
                        
                        
                        


                    }
                    
                   
            
            }
        }

        

        protected void triFichierXml(string smatiere, string sanneeBTS)
        {
            /* Mettre le contenu du fichier XML dans une collection et le trier */
            if (File.Exists(Server.MapPath("~/livrets/parProf/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomProf"] + "_" + smatiere + "-" + sanneeBTS + ".Xml")))
            {
                XDocument odocument = XDocument.Load(Server.MapPath("~/livrets/parProf/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomProf"] + "_" + smatiere + "-" + sanneeBTS + ".Xml"), LoadOptions.None);
                IEnumerable<XElement> ocollitems = odocument.Elements("items");
                List<CitemCDT> ocollItemCDT = new List<CitemCDT>();

                foreach (XElement oelementItems in ocollitems)
                {
                    IEnumerable<XElement> ocollitem = oelementItems.Elements("item");
                    foreach (XElement oelementItem in ocollitem)
                    {
                        CitemCDT oitemCDT = new CitemCDT();

                        IEnumerable<XElement> ocollchampItem1 = oelementItem.Elements("numMatiere"); //Elements retourne une collection de 1 elt enfant de oelementItem                           
                        oitemCDT.NumMatiere = Convert.ToString(ocollchampItem1.ElementAt(0).Value);
                        IEnumerable<XElement> ocollchampItem2 = oelementItem.Elements("date");
                        oitemCDT.Date = Convert.ToDateTime(ocollchampItem2.ElementAt(0).Value);
                        IEnumerable<XElement> ocollchampItem3 = oelementItem.Elements("heure_debut");
                        oitemCDT.Heure_debut = Convert.ToString(ocollchampItem3.ElementAt(0).Value);
                        IEnumerable<XElement> ocollchampItem4 = oelementItem.Elements("heure_fin");
                        oitemCDT.Heure_fin = Convert.ToString(ocollchampItem4.ElementAt(0).Value);
                        IEnumerable<XElement> ocollchampItem5 = oelementItem.Elements("resume_cours");
                        oitemCDT.Resume_cours = Convert.ToString(ocollchampItem5.ElementAt(0).Value);

                        ocollItemCDT.Add(oitemCDT); // mis dans la collection
                    }
                }

                // obtenir une collection triée
                var ocollItemTrie = from oitem in ocollItemCDT
                                    orderby oitem.Date ascending
                                    select oitem;
                // je réécris le fichier XML correspondant trié
                bool premier = true;
                XDocument mondoc = new XDocument();

                foreach (CitemCDT oitem in ocollItemTrie)
                {
                    if (premier)
                    {
                        mondoc.Add(
                            new XElement("items",
                                new XElement("item", new XElement("numMatiere", oitem.NumMatiere),
                                                     new XElement("date", oitem.Date.ToShortDateString()),
                                                     new XElement("heure_debut", oitem.Heure_debut),
                                                     new XElement("heure_fin", oitem.Heure_fin),
                                                     new XElement("resume_cours", oitem.Resume_cours)
                                                    )));
                        premier = false;
                    }
                    else
                    {
                        mondoc.Element("items").Add(new XElement("item", new XElement("numMatiere", oitem.NumMatiere),
                                                                           new XElement("date", oitem.Date.ToShortDateString()),
                                                                           new XElement("heure_debut", oitem.Heure_debut),
                                                                           new XElement("heure_fin", oitem.Heure_fin),
                                                                           new XElement("resume_cours", oitem.Resume_cours)));
                    }

                }

                mondoc.Save(Server.MapPath("~/livrets/parProf/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomProf"] + "_" + smatiere + "-" + sanneeBTS + ".Xml"));
            }
        }

        protected void VerifHeure(object source, ServerValidateEventArgs args) // méthode appelée par le customValidator qui invalid la page si la dateDebut est superieure ou egale à la dateFin
        {
            try
            {

                if (Convert.ToInt16(ddlHeureDebut.SelectedValue.Trim()) >= Convert.ToInt16(ddlHeureFin.SelectedValue.Trim()))
                {
                    args.IsValid = false;
                }
                else
                {
                    args.IsValid = true;
                }
            }
            catch
            { }
        }

 
        /*
        protected void imgBtnRappel_Click(object sender, ImageClickEventArgs e)
        {
            int indexTiret = ddlChoixMatiere.SelectedValue.IndexOf("-", 0);

            string anneeBTS = ddlChoixMatiere.SelectedValue.Substring(0, indexTiret);

            string matiere = ddlChoixMatiere.SelectedValue.Substring(indexTiret + 1, (ddlChoixMatiere.SelectedValue.Length) - (indexTiret + 1));

            matiere = matiere.ToUpper();

            if (matiere == "SI1" || matiere == "SI2" || matiere == "SI3" || matiere == "SI4" || matiere == "SI5" || matiere == "SI6")
            {
                anneeBTS = "SIO1COMMUN"; // cela me permet de regrouper les SIx - Ces regroupement me faciliteront la tâche pour la lecture lors de la génération des LIVRETS me semble t-il
            }
            if (matiere == "SLAM1" || matiere == "SLAM2")
            {
                anneeBTS = "SIO1SLAM"; // Cela me permet de regrouper les SLAM de première année
            }
            if (matiere == "SISR1" || matiere == "SISR2")
            {
                anneeBTS = "SIO1SISR"; // Cela me permet de regrouper les SISR de première année 
            }
            if (matiere == "SLAM3" || matiere == "SLAM4" || matiere == "SLAM5")
            {

                anneeBTS = "SIO2SLAM"; // Cela me permet de regrouper les SLAM de deuxième  année
            }
            if (matiere == "SI7")
            {
                anneeBTS = "SIO2COMMUN"; // enseignement commun de deuxième année
            }
            if (matiere == "SISR3" || matiere == "SISR4" || matiere == "SISR5")
            {

                anneeBTS = "SIO2SISR"; // Cela me permet de regrouper les SISR de deuxième année
            }

            matiere = matiere.ToUpper();
            string fileNameLivretProf = Server.MapPath("~/livrets/parProf/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomProf"] + "_" + matiere + "-" + anneeBTS + ".Xml");

            DataSet ods = new DataSet();
            ods.ReadXml(fileNameLivretProf);


            foreach (DataRow orow in ods.Tables["item"].Rows)
            {
                if (Convert.ToString(orow["date"]) == tbDateCours.Text.Trim() && Convert.ToString(orow["heure_debut"]) == ddlHeureDebut.SelectedValue && Convert.ToString(orow["heure_fin"]) == ddlHeureFin.SelectedValue)
                {
                    tbResume.Text = (string)orow["resume_cours"];
                    break;
                }
            }
        } */



        //methode permettant de rechercher l'item du CDT pour un horaire et une date donnée afin de faire l'affichage si il existe
        protected void rechercheItemParDateHeure(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                int indexTiret = ddlChoixMatiere.SelectedValue.IndexOf("-", 0);

                string anneeBTS = ddlChoixMatiere.SelectedValue.Substring(0, indexTiret);

                string matiere = ddlChoixMatiere.SelectedValue.Substring(indexTiret + 1, (ddlChoixMatiere.SelectedValue.Length) - (indexTiret + 1));

                matiere = matiere.ToUpper();

                if (matiere == "SI1" || matiere == "SI2" || matiere == "SI3" || matiere == "SI4" || matiere == "SI5" || matiere == "SI6")
                {
                    anneeBTS = "SIO1COMMUN"; // cela me permet de regrouper les SIx - Ces regroupement me faciliteront la tâche pour la lecture lors de la génération des LIVRETS me semble t-il
                }
                if (matiere == "SLAM1" || matiere == "SLAM2")
                {
                    anneeBTS = "SIO1SLAM"; // Cela me permet de regrouper les SLAM de première année
                }
                if (matiere == "SISR1" || matiere == "SISR2")
                {
                    anneeBTS = "SIO1SISR"; // Cela me permet de regrouper les SISR de première année 
                }
                if (matiere == "SLAM3" || matiere == "SLAM4" || matiere == "SLAM5")
                {

                    anneeBTS = "SIO2SLAM"; // Cela me permet de regrouper les SLAM de deuxième  année
                }
                if (matiere == "SI7")
                {
                    anneeBTS = "SIO2COMMUN"; // enseignement commun de deuxième année
                }
                if (matiere == "SISR3" || matiere == "SISR4" || matiere == "SISR5")
                {

                    anneeBTS = "SIO2SISR"; // Cela me permet de regrouper les SISR de deuxième année
                }

                matiere = matiere.ToUpper();

                if (File.Exists(Server.MapPath("~/livrets/parProf/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomProf"] + "_" + matiere + "-" + anneeBTS + ".Xml")))
                {
                    XDocument odocument = XDocument.Load(Server.MapPath("~/livrets/parProf/" + Convert.ToString(Session["annee_scolaire"]) + "/" + Session["prenomNomProf"] + "_" + matiere + "-" + anneeBTS + ".Xml"), LoadOptions.None);
                    IEnumerable<XElement> ocollitems = odocument.Elements("items");
                    List<CitemCDT> ocollItemCDT = new List<CitemCDT>();

                    foreach (XElement oelementItems in ocollitems)
                    {
                        IEnumerable<XElement> ocollitem = oelementItems.Elements("item");
                        foreach (XElement oelementItem in ocollitem)
                        {
                            CitemCDT oitemCDT = new CitemCDT();

                            IEnumerable<XElement> ocollchampItem1 = oelementItem.Elements("numMatiere"); //Elements retourne une collection de 1 elt enfant de oelementItem                           
                            oitemCDT.NumMatiere = Convert.ToString(ocollchampItem1.ElementAt(0).Value);
                            IEnumerable<XElement> ocollchampItem2 = oelementItem.Elements("date");
                            oitemCDT.Date = Convert.ToDateTime(ocollchampItem2.ElementAt(0).Value);
                            IEnumerable<XElement> ocollchampItem3 = oelementItem.Elements("heure_debut");
                            oitemCDT.Heure_debut = Convert.ToString(ocollchampItem3.ElementAt(0).Value);
                            IEnumerable<XElement> ocollchampItem4 = oelementItem.Elements("heure_fin");
                            oitemCDT.Heure_fin = Convert.ToString(ocollchampItem4.ElementAt(0).Value);
                            IEnumerable<XElement> ocollchampItem5 = oelementItem.Elements("resume_cours");
                            oitemCDT.Resume_cours = Convert.ToString(ocollchampItem5.ElementAt(0).Value);

                            ocollItemCDT.Add(oitemCDT); // mis dans la collection
                        }
                    }

                    var ocoll = from oitem in ocollItemCDT
                                where oitem.Date == Convert.ToDateTime(tbDateCours.Text.Trim()) && oitem.Heure_debut == ddlHeureDebut.Text.Trim() && oitem.Heure_fin == ddlHeureFin.Text.Trim()
                                select oitem;

                    if (ocoll.Count() > 0) //Si la collection a un element
                    {
                        tbResume.Text = ((CitemCDT)ocoll.ElementAt(0)).Resume_cours.Trim();
                        lbItemTrouve.Text = "un enregistrement a été trouvé pour cette date et cet horaire. Vous pouvez le modifier ou l'annuler en laissant la zone vide ! Merci.";
                    }

                }

            }


        }

        protected void enableDdlHD(object sender, EventArgs e)
        {
            ddlHeureDebut.Enabled = true;
        }

        protected void enableDdlHF(object sender, EventArgs e)
        {
            ddlHeureFin.Enabled = true;
        }

        
    }
}