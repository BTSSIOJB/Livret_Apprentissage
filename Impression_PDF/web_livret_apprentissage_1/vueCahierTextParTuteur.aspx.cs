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
using System.Collections; //Icomparer
using web_livret_apprentissage_1.mesClasses; //pour mesclasses

namespace web_livret_apprentissage_1
{
    public partial class vueCahierTextParTuteur : System.Web.UI.Page
    {
        //Table tblcahierText = null;
        protected void Page_Load(object sender, EventArgs e)
        {

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

            if (!Page.IsPostBack) //si la page n'a pas encore été postée au serveur
            {
                // XmlTextReader reader = new XmlTextReader("Matiere.Xml");
                /*
                XmlDocument odocXml = new XmlDocument();
                //string fileName = Path.Combine(Directory.GetCurrentDirectory(), "Matiere.Xml");
                try
                {
                    string fileName = Server.MapPath("~/Etudiants/" + Convert.ToString(Session["annee_scolaire"]) + "/" + "Etudiant.Xml");
                    odocXml.Load(fileName);
                }
                catch
                {
                   lbException.Text = "Absence de fichier xml.";
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
                }*/

              
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
                        catch
                        {
                            lbException.Text = "Absence de fichier xml 'Etudiant'.";
                           // throw new Exception("Absence de fichier xml 'Etudiant'."); 
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
                        catch
                        {
                            lbException.Text = "Absence de fichier xml 'Etudiant'.";
                            //throw new Exception("Absence du fichier xml 'Etudiant'.");
                        }

                        if (odocXml.HasChildNodes) //que l'étudiant du tuteur
                        {

                            //XmlNodeList olistnomEtudiant = odocXml.GetElementsByTagName("nom_etudiant");
                            //XmlNodeList olistprenomEtudiant = odocXml.GetElementsByTagName("prenom_etudiant");
                            XmlNodeList olist = odocXml.SelectNodes("/etudiants/etudiant");
                            ddlChoixEtudiant.Items.Add(" ");
                           
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
                                    if (_onode["num_tuteur"].InnerText.Trim().ToLower() == onode["num_tuteur"].InnerText.Trim().ToLower())
                                    {
                                        nomPrenomUserTrouve = _onode["prenom_tuteur"].InnerText.Trim() + " " + _onode["nom_tuteur"].InnerText.Trim();
                                        break;
                                    }

                                }

                                //met tous les etudiants du tuteur dans la liste déroulante
                                if (nomPrenomUser.Trim().ToLower() == nomPrenomUserTrouve.Trim().ToLower()) // si le tuteur logué est le tuteur de l'étudiant on rajoute l'étudiant à la liste déroulante
                                {
                                   
                                    ddlChoixEtudiant.Items.Add(onode["nom_etudiant"].InnerText.Trim() + "." + onode["prenom_etudiant"].InnerText.Trim());

                                }
                                nomPrenomUserTrouve = null;

                            } //fin foreach onode
                        }

                    }
          
            }
            //tblcahierText = new Table();
            //tblcahierText.GridLines = GridLines.Both;
            //PlaceHolder1.Controls.Add(tblcahierText); //je rajoute le table dans le placeHolder -> permet de positionner le table sur la page au lieu de mettre le table directement dans la page asp

           
        }

        

        protected void peuplerCDT(object sender, EventArgs e)
        {
            /*<asp:TableHeaderRow><asp:TableCell>ANGLAIS</asp:TableCell></asp:TableHeaderRow>
            <asp:TableHeaderRow><asp:TableCell>DATE</asp:TableCell><asp:TableCell>HEURE</asp:TableCell><asp:TableCell>RESUME COURS</asp:TableCell></asp:TableHeaderRow>
            <asp:TableRow><asp:TableCell>10/05/2015</asp:TableCell><asp:TableCell>10-12 heure</asp:TableCell><asp:TableCell>b laballalalalalalala babababababababababababab</asp:TableCell></asp:TableRow>
            <asp:TableHeaderRow><asp:TableCell>SLAM</asp:TableCell></asp:TableHeaderRow>
            <asp:TableHeaderRow><asp:TableCell>DATE</asp:TableCell><asp:TableCell>HEURE</asp:TableCell><asp:TableCell>RESUME COURS</asp:TableCell></asp:TableHeaderRow>
            <asp:TableRow><asp:TableCell>10/05/2015</asp:TableCell><asp:TableCell>10-12 heure</asp:TableCell><asp:TableCell>blablabla blablablabla djjdjdddjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllEOF</asp:TableCell></asp:TableRow> */

            //Récupération du nom et prenom de la ddl

            string nomPrenom = ddlChoixEtudiant.SelectedValue;
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
                            
                        }
                        if(onodeChild.Name == "annee_etudiant" && (nomEtudiant.Trim().ToLower() + "." + prenomEtudiant.Trim().ToLower()) == ddlChoixEtudiant.SelectedValue.Trim().ToLower() )
                        {

                            anneeEtudiant = onodeChild.InnerText;
                        }
                    }

                    
                }
            }


            string annee_sco = Convert.ToString(Session["annee_scolaire"]);
            string repertoireCible = Server.MapPath("livrets/parProf/" + annee_sco + "/");
            string[] fileEntries = Directory.GetFiles(repertoireCible,"*.Xml");

            //tri tableau ordre inverse
            IComparer myComparer = new myReverserClass();
            Array.Sort(fileEntries, myComparer);

            //en fonction de l'année (Classe) selectionnée
            switch (anneeEtudiant)
            {
                case "sio2-slam" :
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