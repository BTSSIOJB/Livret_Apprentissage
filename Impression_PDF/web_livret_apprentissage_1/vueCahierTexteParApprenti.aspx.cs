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
using System.Collections;
using web_livret_apprentissage_1.mesClasses;

namespace web_livret_apprentissage_1
{
    public partial class vueCahierTexteParApprenti : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            String[] groups = (string[])Session["groupsUser"];

            try // cas ou connecté mais pas le bon ex: prof au lieu de tuteur
            {
                if (groups[0].Trim() != "gr-SIO2-Alter-ferme" && groups[0].Trim() != "gr-sio1-alter-ferme" )
                { Response.Redirect("~/Default.aspx"); }
                //else { Response.Redirect("~/Default.aspx"); }
            }
            catch //cas ou il n'est pas du tout connecté => groups est null donc une exception est levée
            {
                if (groups == null)
                { Response.Redirect("~/Logon.aspx"); }

            }

            peuplerCDT();

        }

        protected void peuplerCDT()
        {

            string nomPrenomApprenti = Convert.ToString(Session["prenomNomUser"]).Trim();
            int indexEspace = nomPrenomApprenti.IndexOf(" ", 0);
            string nomPrenom = nomPrenomApprenti.Substring(indexEspace + 1, nomPrenomApprenti.Length - (indexEspace + 1)) + "." + nomPrenomApprenti.Substring(0,indexEspace);
            int indexPoint = nomPrenom.IndexOf(".", 0);
            string nom = null;
            string prenom = null;
            lbException.Text = "";
            try
            {
                nom = nomPrenom.Substring(0, indexPoint);
                prenom = nomPrenom.Substring(indexPoint + 1, nomPrenom.Length - (nom.Length + 1));
            }
            catch
            {
                string ex = "Choix apprenti invalide !";
                lbException.Text = ex;
            }


            //Détermination de l'année de l'étudiant selectionné
            XmlDocument odocXml = new XmlDocument();
            try
            {
                string fileNameEtudiant = Server.MapPath("~/Etudiants/" + Convert.ToString(Session["annee_scolaire"]) + "/" + "Etudiant.Xml");
                odocXml.Load(fileNameEtudiant);
            }
            catch
            {
                lbException.Text = "Absence de fichier xml 'Etudiant'.";
            }

            string nomEtudiant = null, prenomEtudiant = null, anneeEtudiant = null;

            if (odocXml.HasChildNodes)
            {

                XmlNodeList olist = odocXml.GetElementsByTagName("etudiant");

                foreach (XmlNode onode in olist) // tourne sur étudiant
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

                        }
                        if (onodeChild.Name == "annee_etudiant" && (nomEtudiant.ToLower().Trim() + "." + prenomEtudiant.ToLower().Trim()).Trim() == nomPrenom.ToLower().Trim())
                        {

                            anneeEtudiant = onodeChild.InnerText;
                        }
                    }


                }
            }


            string annee_sco = Convert.ToString(Session["annee_scolaire"]);
            string repertoireCible = Server.MapPath("livrets/parProf/" + annee_sco + "/");
            string[] fileEntries = Directory.GetFiles(repertoireCible, "*.Xml");

            //tri tableau ordre inverse
            IComparer myComparer = new myReverserClass();
            Array.Sort(fileEntries, myComparer);

            //en fonction de l'année (Classe) selectionnée
            switch (anneeEtudiant)
            {
                case "sio2-slam":
                    foreach (string fileName in fileEntries) //parcours l'ensemble des fichiers
                    {
                        // ProcessFile(fileName);
                        //int lg = fileName.Length;
                        //int indexUs0 = fileName.IndexOf("-", 130);

                        bool dejaTitre = false;

                        int indexTiret0 = calculPositionTiret(fileName); //méthode en fin de fichier
                        int indexUnderscore0 = calculPositionUnderscore(fileName);



                        //string test = fileName.Substring(indexUs0 + 1, fileName.Length - (indexUs0 + 1));
                        if (fileName.Substring(indexTiret0 + 1, fileName.Length - (indexTiret0 + 1)) == "SIO2COMMUN.Xml" || fileName.Substring(indexTiret0 + 1, fileName.Length - (indexTiret0 + 1)) == "SIO2.Xml" || fileName.Substring(indexTiret0 + 1, fileName.Length - (indexTiret0 + 1)) == "SIO2SLAM.Xml") //verifie que le fichier est un fichier avec une extension Xml
                        {
                            //XDocument odoc1 = XDocument.Load(Server.MapPath("livrets/parProf/" + ddlAnneeScolaire.SelectedValue + "/" + ddlChoixType.SelectedValue + "_" + matiere + "-" + anneeBTS + ".Xml"));

                            XDocument odoc = XDocument.Load(fileName);
                            IEnumerable<XElement> ocollitems = odoc.Elements("items");

                            foreach (XElement oelementItems in ocollitems)
                            {


                                /***************************************************************************************/
                                IEnumerable<XElement> ocollitem = oelementItems.Elements("item");
                                foreach (XElement oelementItem in ocollitem)
                                {

                                    IEnumerable<XElement> ocollchampItem1 = oelementItem.Elements("numMatiere"); //Elements retourne une collection de 1 elt enfant de oelementItem
                                    IEnumerable<XElement> ocollchampItem2 = oelementItem.Elements("date");
                                    IEnumerable<XElement> ocollchampItem3 = oelementItem.Elements("heure_debut");
                                    IEnumerable<XElement> ocollchampItem4 = oelementItem.Elements("heure_fin");
                                    IEnumerable<XElement> ocollchampItem5 = oelementItem.Elements("resume_cours");


                                    TableHeaderRow otbhr = null;
                                    if (!dejaTitre) //pour que le titre de la matière n'apparaisse qu'une fois et non à chaque ligne du cahier de texte de la matière
                                    {
                                        dejaTitre = true;
                                        otbhr = new TableHeaderRow();
                                        otbhr.Width = 100;
                                        otbhr.BackColor = System.Drawing.Color.FromArgb(101, 75, 36); //correspond a #654B24 en HEX
                                        otbhr.ForeColor = System.Drawing.Color.White;
                                        otbhr.HorizontalAlign = HorizontalAlign.Center;
                                        TableCell otbc = new TableCell();
                                        otbc.RowSpan = 1;
                                        otbc.ColumnSpan = 3;

                                        int indexUs = fileName.IndexOf("_", 0);
                                        int indexTiret = fileName.IndexOf("-", 0);


                                        string nomMat = null, intituleMat = null;

                                        /*Récupération du nom de la matière à partir de matiere.Xml*/
                                        XDocument odocMatiere = XDocument.Load(Server.MapPath("Professeurs" + "/" + Convert.ToString(Session["annee_scolaire"]) + "/" + "Matiere.Xml"));
                                        IEnumerable<XElement> ocollElement = odocMatiere.Element("matieres").Elements();
                                        Boolean sortieBoucleExterne = false;
                                        for (int x = 0; x < ocollElement.Count(); x++) // parcours matiere
                                        {
                                            IEnumerable<XElement> ocollElementMatiere = ocollElement.ElementAt(x).Elements();

                                            for (int i = 0; i < ocollElementMatiere.Count(); i++)
                                            {
                                                //IEnumerable<XElement> ocollchampNumMat = ocollElementMatiere.Elements("num_mat");
                                                if (ocollElementMatiere.ElementAt(i).Name == "num_mat" && ocollElementMatiere.ElementAt(i).Value == ocollchampItem1.ElementAt(0).Value)
                                                {


                                                    nomMat = ocollElementMatiere.ElementAt(i + 1).Value.Trim();
                                                    intituleMat = ocollElementMatiere.ElementAt(i + 3).Value.Trim();
                                                    sortieBoucleExterne = true;
                                                    break;

                                                }
                                            }

                                            if (sortieBoucleExterne)
                                            {
                                                break;
                                            }
                                            //if(ocollElement[x])

                                        }


                                        otbc.Text = nomMat.ToUpper() + " - " + intituleMat.ToUpper();

                                        otbc.HorizontalAlign = HorizontalAlign.Center;
                                        otbhr.Cells.Add(otbc);
                                        otbhr.Width = 1000;
                                        tblcahierText.Rows.Add(otbhr);

                                    } // fin si  dejaTitre

                                    TableRow otbhr1 = new TableRow();
                                    otbhr1.BackColor = System.Drawing.Color.FromArgb(217, 166, 115);//System.Drawing.Color.Orange;


                                    TableCell otbc1 = new TableCell();
                                    otbc1.Text = ocollchampItem2.ElementAt(0).ToString();
                                    otbc1.HorizontalAlign = HorizontalAlign.Center;

                                    otbhr1.Cells.Add(otbc1);

                                    TableCell otbc2 = new TableCell();

                                    otbc2.Text = ocollchampItem3.ElementAt(0).ToString() + "-" + ocollchampItem4.ElementAt(0).ToString() + " H";
                                    otbc2.HorizontalAlign = HorizontalAlign.Center;
                                    otbhr1.Cells.Add(otbc2);

                                    TableCell otbc3 = new TableCell();
                                    otbc3.Text = ocollchampItem5.ElementAt(0).ToString();
                                    //otbc3.HorizontalAlign = HorizontalAlign.Center;
                                    otbhr1.Cells.Add(otbc3);

                                    tblcahierText.CellPadding = 3;
                                    tblcahierText.Rows.Add(otbhr1);


                                }

                            }
                        }
                    } // foreach
                    break;
                case "sio2-sisr":
                    foreach (string fileName in fileEntries) //parcours l'ensemble des fichiers
                    {
                        // ProcessFile(fileName);
                        //int lg = fileName.Length;
                        //int indexUs0 = fileName.IndexOf("-", 130);
                        bool dejaTitre = false;

                        int indexTiret0 = calculPositionTiret(fileName); //méthode en fin de fichier

                        if (fileName.Substring(indexTiret0 + 1, fileName.Length - (indexTiret0 + 1)) == "SIO2COMMUN.Xml" || fileName.Substring(indexTiret0 + 1, fileName.Length - (indexTiret0 + 1)) == "SIO2.Xml" || fileName.Substring(indexTiret0 + 1, fileName.Length - (indexTiret0 + 1)) == "SIO2SISR.Xml") //verifie que le fichier est un fichier avec une extension Xml
                        {
                            //XDocument odoc1 = XDocument.Load(Server.MapPath("livrets/parProf/" + ddlAnneeScolaire.SelectedValue + "/" + ddlChoixType.SelectedValue + "_" + matiere + "-" + anneeBTS + ".Xml"));
                            XDocument odoc = XDocument.Load(fileName);
                            IEnumerable<XElement> ocollitems = odoc.Elements("items");

                            foreach (XElement oelementItems in ocollitems)
                            {
                                IEnumerable<XElement> ocollitem = oelementItems.Elements("item");
                                foreach (XElement oelementItem in ocollitem)
                                {
                                    IEnumerable<XElement> ocollchampItem1 = oelementItem.Elements("numMatiere"); //Elements retourne une collection de 1 elt enfant de oelementItem
                                    IEnumerable<XElement> ocollchampItem2 = oelementItem.Elements("date");
                                    IEnumerable<XElement> ocollchampItem3 = oelementItem.Elements("heure_debut");
                                    IEnumerable<XElement> ocollchampItem4 = oelementItem.Elements("heure_fin");
                                    IEnumerable<XElement> ocollchampItem5 = oelementItem.Elements("resume_cours");

                                    if (!dejaTitre) //pour que le titre de la matière n'apparaisse qu'une fois et non à chaque ligne du cahier de texte de la matière
                                    {
                                        dejaTitre = true;

                                        TableHeaderRow otbhr = new TableHeaderRow();
                                        otbhr.Width = 100;
                                        otbhr.BackColor = System.Drawing.Color.FromArgb(101, 75, 36); //marron
                                        otbhr.ForeColor = System.Drawing.Color.White;
                                        otbhr.HorizontalAlign = HorizontalAlign.Center;
                                        TableCell otbc = new TableCell();
                                        otbc.RowSpan = 1;
                                        otbc.ColumnSpan = 3;
                                        int indexUs = fileName.IndexOf("_", 0);
                                        int indexTiret = fileName.IndexOf("-", 0);
                                        string nomMat = null, intituleMat = null;


                                        /*Récupération du nom de la matière à partir de matiere.Xml*/
                                        XDocument odocMatiere = XDocument.Load(Server.MapPath("Professeurs" + "/" + Convert.ToString(Session["annee_scolaire"]) + "/" + "Matiere.Xml"));
                                        IEnumerable<XElement> ocollElement = odocMatiere.Element("matieres").Elements();
                                        Boolean sortieBoucleExterne = false;
                                        for (int x = 0; x < ocollElement.Count(); x++) // parcours matiere
                                        {
                                            IEnumerable<XElement> ocollElementMatiere = ocollElement.ElementAt(x).Elements();

                                            for (int i = 0; i < ocollElementMatiere.Count(); i++)
                                            {
                                                //IEnumerable<XElement> ocollchampNumMat = ocollElementMatiere.Elements("num_mat");
                                                if (ocollElementMatiere.ElementAt(i).Name == "num_mat" && ocollElementMatiere.ElementAt(i).Value == ocollchampItem1.ElementAt(0).Value)
                                                {


                                                    nomMat = ocollElementMatiere.ElementAt(i + 1).Value.Trim();
                                                    intituleMat = ocollElementMatiere.ElementAt(i + 3).Value.Trim();
                                                    sortieBoucleExterne = true;
                                                    break;

                                                }
                                            }

                                            if (sortieBoucleExterne)
                                            {
                                                break;
                                            }

                                        }

                                        //nomMat = "ANGLAIS"; //fileName.Substring(indexUs + 1, (indexTiret - 1) - indexUs);
                                        otbc.Text = nomMat.ToUpper() + " - " + intituleMat.ToUpper();

                                        otbc.HorizontalAlign = HorizontalAlign.Center;
                                        otbhr.Cells.Add(otbc);
                                        tblcahierText.Rows.Add(otbhr);

                                    } //fin si dejaTitre
                                    TableHeaderRow otbhr1 = new TableHeaderRow();
                                    otbhr1.BackColor = otbhr1.BackColor = System.Drawing.Color.FromArgb(217, 166, 115);//System.Drawing.Color.Orange;
                                    otbhr1.Width = 100;

                                    TableCell otbc1 = new TableCell();
                                    otbc1.Text = ocollchampItem2.ElementAt(0).ToString();
                                    otbc1.HorizontalAlign = HorizontalAlign.Center;
                                    otbhr1.Cells.Add(otbc1);

                                    TableCell otbc2 = new TableCell();
                                    otbc2.Text = ocollchampItem3.ElementAt(0).ToString() + "-" + ocollchampItem4.ElementAt(0).ToString() + " H";
                                    otbc2.HorizontalAlign = HorizontalAlign.Center;
                                    otbhr1.Cells.Add(otbc2);

                                    TableCell otbc3 = new TableCell();
                                    //otbc3.Wrap = false;


                                    otbc3.Text = ocollchampItem5.ElementAt(0).ToString();
                                    //otbc3.HorizontalAlign = HorizontalAlign.Center;
                                    otbhr1.Cells.Add(otbc3);

                                    tblcahierText.Rows.Add(otbhr1);
                                }

                            }
                        }
                    } // foreach
                    break;
                case "sio1-slam":
                    foreach (string fileName in fileEntries) //parcours l'ensemble des fichiers
                    {
                        // ProcessFile(fileName);
                        //int lg = fileName.Length;
                        //int indexUs0 = fileName.IndexOf("-", 130);
                        bool dejaTitre = false;

                        int indexTiret0 = calculPositionTiret(fileName); //méthode en fin de fichier

                        if (fileName.Substring(indexTiret0 + 1, fileName.Length - (indexTiret0 + 1)) == "SIO1COMMUN.Xml" || fileName.Substring(indexTiret0 + 1, fileName.Length - (indexTiret0 + 1)) == "SIO1.Xml" || fileName.Substring(indexTiret0 + 1, fileName.Length - (indexTiret0 + 1)) == "SIO1SLAM.Xml") //verifie que le fichier est un fichier avec une extension Xml
                        {
                            //XDocument odoc1 = XDocument.Load(Server.MapPath("livrets/parProf/" + ddlAnneeScolaire.SelectedValue + "/" + ddlChoixType.SelectedValue + "_" + matiere + "-" + anneeBTS + ".Xml"));
                            XDocument odoc = XDocument.Load(fileName);
                            IEnumerable<XElement> ocollitems = odoc.Elements("items");

                            foreach (XElement oelementItems in ocollitems)
                            {
                                IEnumerable<XElement> ocollitem = oelementItems.Elements("item");
                                foreach (XElement oelementItem in ocollitem)
                                {
                                    IEnumerable<XElement> ocollchampItem1 = oelementItem.Elements("numMatiere"); //Elements retourne une collection de 1 elt enfant de oelementItem
                                    IEnumerable<XElement> ocollchampItem2 = oelementItem.Elements("date");
                                    IEnumerable<XElement> ocollchampItem3 = oelementItem.Elements("heure_debut");
                                    IEnumerable<XElement> ocollchampItem4 = oelementItem.Elements("heure_fin");
                                    IEnumerable<XElement> ocollchampItem5 = oelementItem.Elements("resume_cours");

                                    if (!dejaTitre) //pour que le titre de la matière n'apparaisse qu'une fois et non à chaque ligne du cahier de texte de la matière
                                    {
                                        dejaTitre = true;

                                        TableHeaderRow otbhr = new TableHeaderRow();
                                        otbhr.Width = 100;
                                        otbhr.BackColor = System.Drawing.Color.FromArgb(101, 75, 36); //marron
                                        otbhr.ForeColor = System.Drawing.Color.White;
                                        otbhr.HorizontalAlign = HorizontalAlign.Center;
                                        TableCell otbc = new TableCell();
                                        otbc.RowSpan = 1;
                                        otbc.ColumnSpan = 3;
                                        int indexUs = fileName.IndexOf("_", 0);
                                        int indexTiret = fileName.IndexOf("-", 0);
                                        string nomMat = null, intituleMat = null; ;


                                        /*Récupération du nom de la matière à partir de matiere.Xml*/
                                        XDocument odocMatiere = XDocument.Load(Server.MapPath("Professeurs" + "/" + Convert.ToString(Session["annee_scolaire"]) + "/" + "Matiere.Xml"));
                                        IEnumerable<XElement> ocollElement = odocMatiere.Element("matieres").Elements();
                                        Boolean sortieBoucleExterne = false;
                                        for (int x = 0; x < ocollElement.Count(); x++) // parcours matiere
                                        {
                                            IEnumerable<XElement> ocollElementMatiere = ocollElement.ElementAt(x).Elements();

                                            for (int i = 0; i < ocollElementMatiere.Count(); i++)
                                            {
                                                //IEnumerable<XElement> ocollchampNumMat = ocollElementMatiere.Elements("num_mat");
                                                if (ocollElementMatiere.ElementAt(i).Name == "num_mat" && ocollElementMatiere.ElementAt(i).Value == ocollchampItem1.ElementAt(0).Value)
                                                {


                                                    nomMat = ocollElementMatiere.ElementAt(i + 1).Value.Trim();
                                                    intituleMat = ocollElementMatiere.ElementAt(i + 3).Value.Trim();
                                                    sortieBoucleExterne = true;
                                                    break;

                                                }
                                            }

                                            if (sortieBoucleExterne)
                                            {
                                                break;
                                            }

                                        }

                                        //nomMat = "ANGLAIS"; //fileName.Substring(indexUs + 1, (indexTiret - 1) - indexUs);
                                        otbc.Text = nomMat.ToUpper() + " - " + intituleMat.ToUpper();

                                        otbc.HorizontalAlign = HorizontalAlign.Center;
                                        otbhr.Cells.Add(otbc);
                                        tblcahierText.Rows.Add(otbhr);

                                    } // fin si dejaTitre

                                    TableHeaderRow otbhr1 = new TableHeaderRow();
                                    otbhr1.BackColor = otbhr1.BackColor = System.Drawing.Color.FromArgb(217, 166, 115);//System.Drawing.Color.Orange;
                                    otbhr1.Width = 100;

                                    TableCell otbc1 = new TableCell();
                                    otbc1.Text = ocollchampItem2.ElementAt(0).ToString();
                                    otbc1.HorizontalAlign = HorizontalAlign.Center;
                                    otbhr1.Cells.Add(otbc1);

                                    TableCell otbc2 = new TableCell();
                                    otbc2.Text = ocollchampItem3.ElementAt(0).ToString() + "h" + "-" + ocollchampItem4.ElementAt(0).ToString() + "h";
                                    otbc2.HorizontalAlign = HorizontalAlign.Center;
                                    otbhr1.Cells.Add(otbc2);

                                    TableCell otbc3 = new TableCell();
                                    //otbc3.Wrap = false;


                                    otbc3.Text = ocollchampItem5.ElementAt(0).ToString();
                                    //otbc3.HorizontalAlign = HorizontalAlign.Center;
                                    otbhr1.Cells.Add(otbc3);

                                    tblcahierText.Rows.Add(otbhr1);
                                }

                            }
                        }
                    } // foreach
                    break;
                case "sio1-sisr":
                    foreach (string fileName in fileEntries) //parcours l'ensemble des fichiers
                    {
                        // ProcessFile(fileName);
                        //int lg = fileName.Length;
                        //int indexUs0 = fileName.IndexOf("-", 130);

                        bool dejaTitre = false;

                        int indexTiret0 = calculPositionTiret(fileName); //méthode en fin de fichier


                        if (fileName.Substring(indexTiret0 + 1, fileName.Length - (indexTiret0 + 1)) == "SIO1COMMUN.Xml" || fileName.Substring(indexTiret0 + 1, fileName.Length - (indexTiret0 + 1)) == "SIO1.Xml" || fileName.Substring(indexTiret0 + 1, fileName.Length - (indexTiret0 + 1)) == "SIO1SISR.Xml") //verifie que le fichier est un fichier avec une extension Xml
                        {
                            //XDocument odoc1 = XDocument.Load(Server.MapPath("livrets/parProf/" + ddlAnneeScolaire.SelectedValue + "/" + ddlChoixType.SelectedValue + "_" + matiere + "-" + anneeBTS + ".Xml"));
                            XDocument odoc = XDocument.Load(fileName);
                            IEnumerable<XElement> ocollitems = odoc.Elements("items");

                            foreach (XElement oelementItems in ocollitems)
                            {
                                IEnumerable<XElement> ocollitem = oelementItems.Elements("item");
                                foreach (XElement oelementItem in ocollitem)
                                {
                                    IEnumerable<XElement> ocollchampItem1 = oelementItem.Elements("numMatiere"); //Elements retourne une collection de 1 elt enfant de oelementItem
                                    IEnumerable<XElement> ocollchampItem2 = oelementItem.Elements("date");
                                    IEnumerable<XElement> ocollchampItem3 = oelementItem.Elements("heure_debut");
                                    IEnumerable<XElement> ocollchampItem4 = oelementItem.Elements("heure_fin");
                                    IEnumerable<XElement> ocollchampItem5 = oelementItem.Elements("resume_cours");

                                    if (!dejaTitre) //pour que le titre de la matière n'apparaisse qu'une fois et non à chaque ligne du cahier de texte de la matière
                                    {
                                        dejaTitre = true;

                                        TableHeaderRow otbhr = new TableHeaderRow();
                                        otbhr.Width = 100;
                                        otbhr.BackColor = System.Drawing.Color.FromArgb(101, 75, 36); //marron
                                        otbhr.ForeColor = System.Drawing.Color.White;
                                        otbhr.HorizontalAlign = HorizontalAlign.Center;
                                        TableCell otbc = new TableCell();
                                        otbc.RowSpan = 1;
                                        otbc.ColumnSpan = 3;
                                        int indexUs = fileName.IndexOf("_", 0);
                                        int indexTiret = fileName.IndexOf("-", 0);
                                        string nomMat = null, intituleMat = null;


                                        /*Récupération du nom de la matière à partir de matiere.Xml*/
                                        XDocument odocMatiere = XDocument.Load(Server.MapPath("Professeurs" + "/" + Convert.ToString(Session["annee_scolaire"]) + "/" + "Matiere.Xml"));
                                        IEnumerable<XElement> ocollElement = odocMatiere.Element("matieres").Elements();
                                        Boolean sortieBoucleExterne = false;
                                        for (int x = 0; x < ocollElement.Count(); x++) // parcours matiere
                                        {
                                            IEnumerable<XElement> ocollElementMatiere = ocollElement.ElementAt(x).Elements();

                                            for (int i = 0; i < ocollElementMatiere.Count(); i++)
                                            {
                                                //IEnumerable<XElement> ocollchampNumMat = ocollElementMatiere.Elements("num_mat");
                                                if (ocollElementMatiere.ElementAt(i).Name == "num_mat" && ocollElementMatiere.ElementAt(i).Value == ocollchampItem1.ElementAt(0).Value)
                                                {


                                                    nomMat = ocollElementMatiere.ElementAt(i + 1).Value.Trim();
                                                    intituleMat = ocollElementMatiere.ElementAt(i + 3).Value.Trim();
                                                    sortieBoucleExterne = true;
                                                    break;

                                                }
                                            }

                                            if (sortieBoucleExterne)
                                            {
                                                break;
                                            }

                                        }

                                        //nomMat = "ANGLAIS"; //fileName.Substring(indexUs + 1, (indexTiret - 1) - indexUs);
                                        otbc.Text = nomMat.ToUpper() + " - " + intituleMat.ToUpper();

                                        otbc.HorizontalAlign = HorizontalAlign.Center;
                                        otbhr.Cells.Add(otbc);
                                        tblcahierText.Rows.Add(otbhr);

                                    } // fin si dejaTitre

                                    TableHeaderRow otbhr1 = new TableHeaderRow();
                                    otbhr1.Width = 100;
                                    otbhr1.BackColor = otbhr1.BackColor = System.Drawing.Color.FromArgb(217, 166, 115);//System.Drawing.Color.Orange;

                                    TableCell otbc1 = new TableCell();
                                    otbc1.Text = ocollchampItem2.ElementAt(0).ToString();
                                    otbc1.HorizontalAlign = HorizontalAlign.Center;
                                    otbhr1.Cells.Add(otbc1);

                                    TableCell otbc2 = new TableCell();
                                    otbc2.Text = ocollchampItem3.ElementAt(0).ToString() + "h" + "-" + ocollchampItem4.ElementAt(0).ToString() + "h";
                                    otbc2.HorizontalAlign = HorizontalAlign.Center;
                                    otbhr1.Cells.Add(otbc2);

                                    TableCell otbc3 = new TableCell();
                                    //otbc3.Wrap = false;

                                    //string strAAfficher = strEncode.Replace("\r\n", "<br/>");


                                    otbc3.Text = ocollchampItem5.ElementAt(0).ToString();
                                    //otbc3.HorizontalAlign = HorizontalAlign.Center;
                                    otbhr1.Cells.Add(otbc3);

                                    tblcahierText.Rows.Add(otbhr1);
                                }

                            }
                        }
                    } // foreach
                    break;

            } //fin switch

        }

        private int calculPositionTiret(string fileName)
        {
            int longueurFileName = fileName.Length;
            int indexTiret = -1;
            string caract = null;

            while (caract != "-")
            {
                longueurFileName = longueurFileName - 1;
                caract = fileName.Substring(longueurFileName, 1);
                if (caract == "-")
                {

                    indexTiret = longueurFileName;
                }
            }
            return indexTiret;
        }

        private int calculPositionUnderscore(string fileName)
        {
            int longueurFileName = fileName.Length;
            int indexUnderscore = -1;
            string caract = null;

            while (caract != "_")
            {
                longueurFileName = longueurFileName - 1;
                caract = fileName.Substring(longueurFileName, 1);
                if (caract == "_")
                {

                    indexUnderscore = longueurFileName;
                }
            }
            return indexUnderscore;
        }
     }
    
}