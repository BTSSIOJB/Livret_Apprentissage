using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf; // Pour Classe Document
using System.Xml.Linq;
using System.IO;

namespace web_livret_apprentissage_1
{
    public partial class Administration : System.Web.UI.Page
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

            if(!IsPostBack)
            {

                ddlAnneeScolaire.Enabled = false;
                ddlChoixType.Enabled = false;
                lbChoixType.Visible = false;
                ddlChoixType.Visible = false;
                lbChoixMatière.Visible = false;
                ddlChoixMatiere.Visible = false;

                ddlChoixGeneration.Items.Add(" ");
                ddlChoixGeneration.Items.Add("Professeur"); //génération PDF cahier de texte
                ddlChoixGeneration.Items.Add("Etudiant"); //génération PDF livret complet (partie entrepreise + école
                ddlChoixGeneration.Items.Add("Tuteur"); //Génération PDF travail en entreprise

                ddlAnneeScolaire.Items.Add(" ");
                ddlAnneeScolaire.Items.Add("2014-2015");
                ddlAnneeScolaire.Items.Add("2015-2016");
                ddlAnneeScolaire.Items.Add("2016-2017");
                ddlAnneeScolaire.Items.Add("2017-2018");
                ddlAnneeScolaire.Items.Add("2018-2019");
                ddlAnneeScolaire.Items.Add("2019-2020");
                ddlAnneeScolaire.Items.Add("2020-2021");
                ddlAnneeScolaire.Items.Add("2021-2022");
                ddlAnneeScolaire.Items.Add("2022-2023");
                ddlAnneeScolaire.Items.Add("2023-2024");
                ddlAnneeScolaire.Items.Add("2024-2025");

               
                

            }
        }

        protected void InitDDLChoixType(object sender, EventArgs e)
        {
            
            
                lbChoixType.Visible = true;
                ddlChoixType.Visible = true;
                lbChoixType.Text = "Choisir " + ddlChoixGeneration.SelectedValue + " : ";
                ddlChoixType.Enabled = true;
                ddlAnneeScolaire.Enabled = true;
                ddlChoixMatiere.Visible = false;
                lbChoixMatière.Visible = false;
                ddlChoixType.Items.Clear();

                try
                {
                    XmlDocument odocXml = new XmlDocument();

                    string fileName = Server.MapPath(ddlChoixGeneration.SelectedValue + "s" + "/" + Convert.ToString(Session["annee_scolaire"]) + "/" + ddlChoixGeneration.SelectedValue + ".Xml");

                    odocXml.Load(fileName);
                    if (odocXml.HasChildNodes)
                    {

                        //XmlNodeList olist = odocXml.GetElementsByTagName("*");
                        ddlChoixType.Items.Add(" ");

                        switch (ddlChoixGeneration.SelectedValue)
                        {
                            case "Professeur":

                                XmlNodeList olistProfesseur = odocXml.GetElementsByTagName("professeur");
                                string nomProf = null, prenomProf = null;
                                foreach (XmlNode onodeProf in olistProfesseur)
                                {
                                    foreach (XmlNode onodeChamp in onodeProf) //evite getEnumerator
                                    {
                                        if (onodeChamp.Name == "nom_prof")
                                        {
                                            nomProf = onodeChamp.InnerText;
                                        }

                                        if (onodeChamp.Name == "prenom_prof")
                                        {
                                            prenomProf = onodeChamp.InnerText;
                                        }


                                    }
                                    //ne depend plus de la place du champ dans le fichier XML
                                    ddlChoixType.Items.Add(prenomProf + "." + nomProf); //Prenom.Nom
                                }
                                break;
                            case "Tuteur":
                                XmlNodeList olistTuteur = odocXml.GetElementsByTagName("tuteur");
                                string nomTuteur = null, prenomTuteur = null;
                                foreach (XmlNode onodeTuteur in olistTuteur)
                                {
                                    foreach (XmlNode onodeChamp in onodeTuteur) //evite getEnumerator
                                    {
                                        if (onodeChamp.Name == "nom_tuteur")
                                        {
                                            nomTuteur = onodeChamp.InnerText;
                                        }

                                        if (onodeChamp.Name == "prenom_tuteur")
                                        {
                                            prenomTuteur = onodeChamp.InnerText;
                                        }


                                    }
                                    ddlChoixType.Items.Add(prenomTuteur + "_" + nomTuteur); //Prenom_Nom
                                }
                                break;
                            case "Etudiant":
                                XmlNodeList olistEtudiant = odocXml.GetElementsByTagName("etudiant");
                                string nomEtudiant = null, prenomEtudiant = null;
                                foreach (XmlNode onodeEtudiant in olistEtudiant)
                                {
                                    foreach (XmlNode onodeChamp in onodeEtudiant) //evite getEnumerator
                                    {
                                        if (onodeChamp.Name == "nom_etudiant")
                                        {
                                            nomEtudiant = onodeChamp.InnerText;
                                        }

                                        if (onodeChamp.Name == "prenom_etudiant")
                                        {
                                            prenomEtudiant = onodeChamp.InnerText;
                                        }


                                    }
                                    ddlChoixType.Items.Add(prenomEtudiant + "_" + nomEtudiant); //Prenom_Nom
                                }
                                break;
                        }
                    }
                } // fin try
                catch(Exception ex)
                {
                    System.Drawing.Color couleur = System.Drawing.Color.Red;
                    lbGenerationPDF.ForeColor = couleur;
                    lbGenerationPDF.Text = ex.Message;

                }
           
        }

        protected void InitDDLChoixMat(object sender, EventArgs e)
        {
            
            ddlChoixMatiere.Visible = true;
            lbChoixMatière.Visible = true;

            ddlChoixMatiere.Items.Clear();

            try
            {
                switch (ddlChoixGeneration.SelectedValue)
                {
                    case "Professeur":

                        //Récupération du fichier XML "Matiere.Xml"

                        XDocument odoc = XDocument.Load(Server.MapPath(ddlChoixGeneration.SelectedValue + "s" + "/" + Convert.ToString(Session["annee_scolaire"]) + "/" + "Matiere.Xml"));
                        IEnumerable<XElement> ocollElement = odoc.Element("matieres").Elements();
                        List<string> olistMatiereProfEnCours = new List<string>(); /* Contiendra l'ensemble des matières du prof */
                        lbChoixMatière.Text = "Choix matière :"; //le label affiche choix matière
                        foreach (XElement oelement in ocollElement)
                        {

                            IEnumerable<XElement> ocollElement2 = oelement.Elements(); //Prend les elements enfants de oelement ici "matiere"


                            foreach (XElement oelement2 in ocollElement2)
                            {
                                if (oelement2.Name == "designation_mat")
                                {
                                    olistMatiereProfEnCours.Add(oelement2.Value); //ajoute temporairement
                                }
                                if (oelement2.Name == "num_prof")
                                {
                                    if (oelement2.Value != trouverNumProfParNom(ddlChoixType.SelectedValue)) //valide l'ajout si le numéro de prof correspond à celui indiqué dans matière
                                    {
                                        olistMatiereProfEnCours.RemoveAt(olistMatiereProfEnCours.Count - 1); //enlève de la collection
                                    }

                                }

                            }



                        }
                        ddlChoixMatiere.Items.Add(" ");
                        foreach (string omatiereProfEnCours in olistMatiereProfEnCours)
                        {
                            ddlChoixMatiere.Items.Add(omatiereProfEnCours.Trim());

                        }
                        ddlChoixMatiere.Items.Add("Toutes les matières".Trim());
                        break;

                    case "Tuteur":
                        // Récupération du fichier XML "Etudiant.Xml"

                        XDocument odocTuteur = XDocument.Load(Server.MapPath("Etudiants/" + Convert.ToString(Session["annee_scolaire"]) + "/" + "Etudiant.Xml"));
                        IEnumerable<XElement> ocollElementEtudiant = odocTuteur.Element("etudiants").Elements();
                        List<string> olistEtudiantTuteurEnCours = new List<string>(); /* Contiendra l'ensemble des apprentis du tuteur */
                        lbChoixMatière.Text = "Choix apprenti :"; //Le label affiche choix apprenti
                        string nomEtudiant = null, prenomEtudiant = null;

                        foreach (XElement oelement in ocollElementEtudiant)
                        {

                            IEnumerable<XElement> ocollElementEtudiant2 = oelement.Elements();


                            foreach (XElement oelement2 in ocollElementEtudiant2)
                            {
                                if (oelement2.Name == "nom_etudiant") //il faut le nom suivi du prenom de l'étudiant pour la structure du nom du fichier XML
                                {
                                    nomEtudiant = oelement2.Value.Trim(); //list de chaines
                                }
                                if (oelement2.Name == "prenom_etudiant")
                                {
                                    prenomEtudiant = oelement2.Value.Trim();
                                    olistEtudiantTuteurEnCours.Add(nomEtudiant + "." + prenomEtudiant); //list de chaines
                                }
                                if (oelement2.Name == "num_tuteur")
                                {
                                    if (oelement2.Value.Trim() != trouverNumTuteurParNom(ddlChoixType.SelectedValue.Trim()))
                                    {
                                        olistEtudiantTuteurEnCours.RemoveAt(olistEtudiantTuteurEnCours.Count - 1);
                                    }

                                }

                            }

                        }
                        ddlChoixMatiere.Items.Add(" ");
                        foreach (string etudiantTuteurEnCours in olistEtudiantTuteurEnCours)
                        {
                            ddlChoixMatiere.Items.Add(etudiantTuteurEnCours.Trim()); //la DDL reste choixMatiere mais elle comportera des étudiants

                        }
                        break;
                    case "Etudiant":
                        ddlChoixMatiere.Visible = false;
                        lbChoixMatière.Visible = false;
                        rfvmatiere.Visible = false;
                        break;
                } // fin Switch
            } //fin try
            catch(Exception ex)
            {
                System.Drawing.Color couleur = System.Drawing.Color.Red;
                lbGenerationPDF.ForeColor = couleur;
                lbGenerationPDF.Text = ex.Message;

            }
        }


        

        protected void btnGenerationPDF_Click(object sender, EventArgs e)
        {
            if (btnGenerationPDF.Text != "Générer un autre PDF ?")
            {
                switch (ddlChoixGeneration.SelectedValue)
                {
                    case "Professeur":

                        try
                        {
                            switch (ddlChoixMatiere.SelectedValue)
                            { 
                                case "Toutes les matières": // Cahier de texte professeur toutes les matières // La recherche des fichiers xml se fait sur le nom du prof

                                    string annee_sco = Convert.ToString(Session["annee_scolaire"]);
                                    string repertoireCible = Server.MapPath("livrets/parProf/" + annee_sco + "/");
                                    string[] tabFileByGetFiles = Directory.GetFiles(repertoireCible,ddlChoixType.SelectedValue + "*.Xml"); //je choisis les fichiers Xml du prof choisis dans la ddl


                                    Document oDocPdfTm = new Document(); //Tm pour Toutes matieres

                                    if (tabFileByGetFiles.Count() > 0) // si au moins un fichier XML trouvé
                                    {

                                        //Document newDocTm = new Document(); //Tm pour Toutes matieres
                                        FileStream ofsTm = new FileStream(Server.MapPath("livrets/parProf/" + ddlAnneeScolaire.SelectedValue + "/" + ddlChoixType.SelectedValue + ".pdf"), FileMode.Create);
                                        PdfWriter opdfWTm = PdfWriter.GetInstance(oDocPdfTm, ofsTm);
                                        //attribution des événement au writer pour la numérotation  de page
                                        opdfWTm.PageEvent = new PageEventHelper();
                                        oDocPdfTm.Open();

                                        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/logoJB.png"));
                                        logo.ScaleToFit(100f, 100f);
                                        Paragraph oParaLogo = new Paragraph();
                                        oParaLogo.Add(logo);
                                        oParaLogo.IndentationLeft = 50;

                                        string phraseTitre = "Cahier de texte toutes matières";
                                        Phrase titre = new Phrase(phraseTitre,
                                            FontFactory.GetFont(
                                            FontFactory.HELVETICA,
                                            20, Font.BOLD));
                                        Paragraph optitre = new Paragraph();
                                        optitre.IndentationLeft = 150;
                                        optitre.Add(titre);

                                        oDocPdfTm.Add(oParaLogo);



                                        Chunk ochk = new Chunk("\r\n");
                                        oDocPdfTm.Add(ochk);

                                        oDocPdfTm.Add(optitre);

                                        oDocPdfTm.Add(ochk);

                                        Phrase op1 = new Phrase("PROFESSEUR : " +  ddlChoixType.SelectedValue);
                                        op1.Add(new Phrase("\r\n" + "ANNÉE SCOLAIRE : " + ddlAnneeScolaire.SelectedValue));
                                        Paragraph opara1 = new Paragraph(op1);
                                        opara1.IndentationLeft = 50f;
                                        oDocPdfTm.Add(opara1);
                                    }
                                    else 
                                    {
                                        throw new Exception("Absence de fichier XML pour la génération toutes matières pour professeur : " + ddlChoixType.SelectedValue + " !"); //cas pas de fichier XML existant
                                    
                                    }

                                    foreach (string fileName in tabFileByGetFiles) //parcours l'ensemble des fichiers
                                    {                                  
                                        int longueurFileName = fileName.Length;
                                       
                                        int indexUs1, indexTiretTm = -1;
                                        
                                        // appel fonction perso pour trouver index a partir fin de chaine pour ne pas dependre de la syntaxe du chemin
                                        indexUs1 = findIndexFromEnd("_", fileName);
                                        indexTiretTm = findIndexFromEnd("-", fileName);

                                        
                                        
                                                XDocument odocTm = XDocument.Load(fileName);
                                                IEnumerable<XElement> ocollDesItems = odocTm.Elements("items");
                                                string matiereTm = fileName.Substring(indexUs1 + 1, indexTiretTm - indexUs1 - 1);
                                                string annee = fileName.Substring(indexTiretTm + 1, 4);
                                                string intitule = null;

                                                XmlDocument odocmatiere = new XmlDocument();
                                                odocmatiere.Load(Server.MapPath("~/Professeurs/" + Session["annee_scolaire"] + "/Matiere.Xml"));

                                                XmlNodeList xnlist = odocmatiere.SelectNodes("/matieres/matiere");

                                                foreach (XmlNode onode in xnlist)
                                                {
                                                    if (onode["designation_mat"].InnerText == annee.ToLower() + "-" + matiereTm.ToLower())
                                                    {
                                                        intitule = onode["intitule_mat"].InnerText;
                                                        break;
                                                    }
                                                }
                                                


                                                Phrase op1 = new Phrase(matiereTm.ToUpper() + " - " + intitule + "\n\n");
                                                float[] largeursTabScol = { 100 };
                                                PdfPTable otableauTitre = new PdfPTable(largeursTabScol); //tableau qui va contenir les tableaux de matières

                                                PdfPCell ocellTitre = new PdfPCell(op1);
                                                ocellTitre.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                                ocellTitre.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                                                BaseColor couleurCell = new BaseColor(237, 237, 237);
                                                ocellTitre.BackgroundColor = couleurCell;
                                                otableauTitre.AddCell(ocellTitre);
                                                
                                                /*Paragraph opara1 = new Paragraph(op1);
                                                opara1.IndentationLeft = 30f;*/

                                                Chunk ochk = new Chunk("\r\n");
                                                oDocPdfTm.Add(ochk);
                                                //oDocPdfTm.Add(otbTitre);
                                                oDocPdfTm.Add(ochk);

                                                float[] largeursTableau = { 20, 80 };
                                                PdfPTable otableauUneMatDansTM = new PdfPTable(largeursTableau); //Tableau 1 matiere dans toutes Matieres
                                                //tableau2.TotalWidth = 560;
                                                //tableau2.LockedWidth = true;

                                                


                                                Phrase phraseHoraire = new Phrase("PÉRIODE",
                                                FontFactory.GetFont(
                                                FontFactory.HELVETICA,
                                                11, Font.BOLD, new BaseColor(126, 51, 0)));

                                                PdfPCell ocellHoraire = new PdfPCell(phraseHoraire);
                                                ocellHoraire.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                                                Phrase phraseTravail = new Phrase("COURS OU TRAVAUX PRATIQUES",
                                                FontFactory.GetFont(
                                                FontFactory.HELVETICA,
                                                11, Font.BOLD, new BaseColor(126, 51, 0)));

                                                PdfPCell ocellTravail = new PdfPCell(phraseTravail);
                                                ocellTravail.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                                                otableauUneMatDansTM.AddCell(ocellHoraire);
                                                otableauUneMatDansTM.AddCell(ocellTravail);

                                                foreach (XElement el in ocollDesItems)
                                                {
                                                    IEnumerable<XElement> ocollitem = el.Elements("item");
                                                    foreach (XElement el2 in ocollitem)
                                                    {
                                                        IEnumerable<XElement> ocollchampItem1 = el2.Elements("numMatiere"); //Elements retourne une collection 
                                                        IEnumerable<XElement> ocollchampItem2 = el2.Elements("date");
                                                        IEnumerable<XElement> ocollchampItem3 = el2.Elements("heure_debut");
                                                        IEnumerable<XElement> ocollchampItem4 = el2.Elements("heure_fin");
                                                        IEnumerable<XElement> ocollchampItem5 = el2.Elements("resume_cours");

                                                        IEnumerator<XElement> iterator = ocollchampItem2.GetEnumerator(); //Pour se positinner sur le seul élément de la collection. - Pédagogique -  ocollchampItem2[0] plus simple
                                                        iterator.MoveNext();
                                                        XElement element = iterator.Current;
                                                        IEnumerator<XElement> iterator1 = ocollchampItem3.GetEnumerator();
                                                        iterator1.MoveNext();
                                                        XElement element1 = iterator1.Current;
                                                        IEnumerator<XElement> iterator2 = ocollchampItem4.GetEnumerator();
                                                        iterator2.MoveNext();
                                                        XElement element2 = iterator2.Current;
                                                        IEnumerator<XElement> iterator3 = ocollchampItem5.GetEnumerator();
                                                        iterator3.MoveNext();
                                                        XElement element3 = iterator3.Current;
                                                        Phrase phraseHeure = new Phrase(element.Value + "\n\n" + element1.Value + "h" + "-" + element2.Value + "h",
                                                                                        FontFactory.GetFont(
                                                                                        FontFactory.HELVETICA,
                                                                                        10, Font.BOLD));
                                                        otableauUneMatDansTM.AddCell(phraseHeure);
                                                        Phrase phraseResumeCours = new Phrase(element3.Value,
                                                                                              FontFactory.GetFont(
                                                                                              FontFactory.HELVETICA,
                                                                                              11, Font.NORMAL));

                                                        otableauUneMatDansTM.AddCell(phraseResumeCours); ;



                                                    }

                                                }
                                                otableauTitre.AddCell(otableauUneMatDansTM);
                                                otableauTitre.KeepTogether = true;
                                                otableauTitre.SplitRows = true;
                                                otableauTitre.SplitLate = false;
                                                oDocPdfTm.Add(otableauTitre);

                                           // } //fin if
                                       // }

                                    } // fin foreach n°1

                                    oDocPdfTm.Add(new Phrase("\n\nImprimé le : " + DateTime.Today.ToShortDateString()));
                                    oDocPdfTm.Close();

                                    //System.Diagnostics.ProcessStartInfo psiTm = new System.Diagnostics.ProcessStartInfo(Server.MapPath("livrets/parProf/" + ddlAnneeScolaire.SelectedValue + "/" + ddlChoixType.SelectedValue + ".pdf"));

                                    //System.Diagnostics.Process.Start(psiTm);

                                    System.Drawing.Color couleurTm = System.Drawing.Color.Green;
                                    lbGenerationPDF.ForeColor = couleurTm;
                                    lbGenerationPDF.Text = "Génération réussie !";
                                    btnGenerationPDF.Text = "Générer un autre PDF ?";

                                    //appel du pdf généré prof toutes matières côté client
                                    //Response.Redirect("https://btssio-jbrel.hd.free.fr/" + "livrets/parProf/" + ddlAnneeScolaire.SelectedValue + "/" + ddlChoixType.SelectedValue + ".pdf");
                                    ResponseHelper.Redirect("https://btssio-jbrel.hd.free.fr/" + "livrets/parProf/" + ddlAnneeScolaire.SelectedValue + "/" + ddlChoixType.SelectedValue + ".pdf", "_blank", "menubar=0,width=1200,height=2940");
                                    //ResponseHelper.Redirect("http://localhost:1514/" + "livrets/parProf/" + ddlAnneeScolaire.SelectedValue + "/" + ddlChoixType.SelectedValue + ".pdf", "_blank", "menubar=0,width=1200,height=2940");
                                    break;

                                default : // Cahier de texte par matière au format PDF

                                    int indexTiret = ddlChoixMatiere.SelectedValue.IndexOf("-", 0);

                                    string anneeBTS = ddlChoixMatiere.SelectedValue.Substring(0, indexTiret);

                                    string matiere = ddlChoixMatiere.SelectedValue.Substring(indexTiret + 1, (ddlChoixMatiere.SelectedValue.Length) - (indexTiret + 1));

                                    matiere = matiere.ToUpper();
                                    anneeBTS = anneeBTS.ToUpper(); //les matières générales seront estempillées SIO1 ou SIO2 uniquement

                                     if(matiere == "SI1" || matiere == "SI2" || matiere == "SI3" || matiere == "SI4" || matiere == "SI5" || matiere == "SI6")
                                     {
                                       anneeBTS = "SIO1COMMUN";
                                     }
                                     if(matiere == "SLAM1" || matiere == "SLAM2")
                                     {
                                       anneeBTS = "SIO1SLAM";
                                     }
                                     if(matiere == "SISR1" || matiere == "SISR2")
                                     {
                                       anneeBTS = "SIO1SISR";
                                     }
                                     if (matiere == "SLAM3" || matiere == "SLAM4" || matiere == "SLAM5")
                                     {

                                       anneeBTS = "SIO2SLAM";
                                     }
                                     if (matiere == "SI7")
                                     {
                                        anneeBTS = "SIO2COMMUN";
                                     }
                                     if (matiere == "SISR3" || matiere == "SISR4" || matiere == "SISR5")
                                     {

                                         anneeBTS = "SIO2SISR";
                                     }                                      

                                    /* A mettre ici car si le fichier XML n'existe pas on va dans le catch et cela ne genere pas un pdf erroné */
                                    XDocument odoc = XDocument.Load(Server.MapPath("livrets/parProf/" + ddlAnneeScolaire.SelectedValue + "/" + ddlChoixType.SelectedValue + "_" + matiere + "-" + anneeBTS +  ".Xml"));
                                    
                                    
                                    
                                    IEnumerable<XElement> ocollitems = odoc.Elements("items");

                                    Document oDocPdf = new Document();
                                    FileStream ofs = new FileStream(Server.MapPath("livrets/parProf/" + ddlAnneeScolaire.SelectedValue + "/" + ddlChoixType.SelectedValue + "_" + ddlChoixMatiere.SelectedValue + ".pdf"), FileMode.Create);

                                    PdfWriter opdfW = PdfWriter.GetInstance(oDocPdf, ofs);

                                    //attribution des événements au writer pour la numérotation de page
                                    opdfW.PageEvent = new PageEventHelper();
                                    oDocPdf.Open();


                                    iTextSharp.text.Image logo2 = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/logoJB.png"));
                                    logo2.ScaleToFit(100f, 100f);
                                    Paragraph oParaLogo2 = new Paragraph();
                                    oParaLogo2.Add(logo2);
                                    oParaLogo2.IndentationLeft = 50;

                                    string phraseTitre2 = "Cahier de texte d'une matière";
                                    Phrase titre2 = new Phrase(phraseTitre2, 
                                    FontFactory.GetFont(
                                    FontFactory.HELVETICA,
                                    20, Font.BOLD));
                                    Paragraph optitre2 = new Paragraph();
                                    optitre2.IndentationLeft = 150;
                                    optitre2.Add(titre2);

                                    oDocPdf.Add(oParaLogo2);

                                    Chunk ochk2 = new Chunk("\r\n");

                                    oDocPdf.Add(ochk2);

                                    oDocPdf.Add(optitre2);

                                    oDocPdf.Add(ochk2);

                                    string intitule2 = null;

                                    XmlDocument odocmatiere2 = new XmlDocument();
                                    odocmatiere2.Load(Server.MapPath("~/Professeurs/" + Session["annee_scolaire"] + "/Matiere.Xml"));

                                    XmlNodeList xnlist2 = odocmatiere2.SelectNodes("/matieres/matiere");

                                    foreach (XmlNode onode in xnlist2)
                                    {
                                        if (onode["designation_mat"].InnerText == anneeBTS.Substring(0,4).ToLower() + "-" + matiere.ToLower())
                                        {
                                            intitule2 = onode["intitule_mat"].InnerText;
                                        }
                                    }

                                    string[] anneeMat = ddlChoixMatiere.SelectedValue.Trim().Split('-');
                                    Phrase op = new Phrase("MATIÈRE : " + anneeMat[1].ToUpper() + " - " + intitule2);
                                    op.Add(new Phrase("\r\n" + "Nom du Professeur : " + ddlChoixType.SelectedValue));
                                    op.Add(new Phrase("\r\n" + "Annee scolaire : " + ddlAnneeScolaire.SelectedValue));

                                    Paragraph opara = new Paragraph(op);
                                    // Phrase op1 = new Phrase(tbTextPdf.Text + "\n\n");
                                    // opara.Add(op1);
                                    opara.IndentationLeft = 50f;
                                    oDocPdf.Add(opara);

                                    oDocPdf.Add(ochk2);


                                    float[] largeursTab = { 20, 80 };
                                    PdfPTable otableauUneMat = new PdfPTable(largeursTab);
                                    //tableau2.TotalWidth = 560;
                                    //tableau2.LockedWidth = true;

                                    

                                    Phrase phraseHoraire2 = new Phrase("PÉRIODE",
                                    FontFactory.GetFont(
                                    FontFactory.HELVETICA,
                                    11, Font.BOLD, new BaseColor(126, 51, 0)));
                                    

                                    Phrase phraseTravail2 = new Phrase("COURS OU TRAVAUX PRATIQUES",
                                    FontFactory.GetFont(
                                    FontFactory.HELVETICA,
                                    11, Font.BOLD, new BaseColor(126, 51, 0)));
                                    

                                    PdfPCell ocell1 = new PdfPCell(phraseHoraire2);
                                    ocell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    PdfPCell ocell2 = new PdfPCell(phraseTravail2);
                                    ocell2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                                    otableauUneMat.AddCell(ocell1);
                                    otableauUneMat.AddCell(ocell2);



                                    foreach (XElement el in ocollitems)
                                    {
                                        IEnumerable<XElement> ocollitem = el.Elements("item");
                                        foreach (XElement el2 in ocollitem)
                                        {
                                            IEnumerable<XElement> ocollchampItem1 = el2.Elements("numMatiere"); //Elements retourne une collection 
                                            IEnumerable<XElement> ocollchampItem2 = el2.Elements("date");
                                            IEnumerable<XElement> ocollchampItem3 = el2.Elements("heure_debut");
                                            IEnumerable<XElement> ocollchampItem4 = el2.Elements("heure_fin");
                                            IEnumerable<XElement> ocollchampItem5 = el2.Elements("resume_cours");

                                            IEnumerator<XElement> iterator = ocollchampItem2.GetEnumerator(); //Pour se positinner sur le seul élément de la collection. - Pédagogique -  ocollchampItem2[0] plus simple
                                            iterator.MoveNext();
                                            XElement element = iterator.Current;
                                            IEnumerator<XElement> iterator1 = ocollchampItem3.GetEnumerator();
                                            iterator1.MoveNext();
                                            XElement element1 = iterator1.Current;
                                            IEnumerator<XElement> iterator2 = ocollchampItem4.GetEnumerator();
                                            iterator2.MoveNext();
                                            XElement element2 = iterator2.Current;
                                            IEnumerator<XElement> iterator3 = ocollchampItem5.GetEnumerator();
                                            iterator3.MoveNext();
                                            XElement element3 = iterator3.Current;
                                            Phrase phraseHeure = new Phrase(element.Value + "\n\n" + element1.Value + "h" + "-" + element2.Value + "h",
                                                                                        FontFactory.GetFont(
                                                                                        FontFactory.HELVETICA,
                                                                                        10, Font.BOLD));
                                            otableauUneMat.AddCell(phraseHeure);
                                            Phrase phraseResumeCours = new Phrase(element3.Value,
                                                                                  FontFactory.GetFont(
                                                                                  FontFactory.HELVETICA,
                                                                                  11, Font.NORMAL));

                                            otableauUneMat.AddCell(phraseResumeCours); ;



                                        }

                                    }
                                    otableauUneMat.KeepTogether = true;
                                    oDocPdf.Add(otableauUneMat);
                                    oDocPdf.Add(new Phrase("\n\nImprimé le : " + DateTime.Today.ToShortDateString()));
                                    oDocPdf.Close();

                                    //System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(Server.MapPath("livrets/parProf/" + ddlAnneeScolaire.SelectedValue + "/" + ddlChoixType.SelectedValue + "_" + ddlChoixMatiere.SelectedValue + ".pdf"));

                                    //System.Diagnostics.Process.Start(psi);

                                    System.Drawing.Color couleur = System.Drawing.Color.Green;
                                    lbGenerationPDF.ForeColor = couleur;
                                    lbGenerationPDF.Text = "Génération réussie !";
                                    btnGenerationPDF.Text = "Générer un autre PDF ?";
                                    //appel du pdf 'prof pour une matière' généré côté client
                                    //Response.Redirect("https://btssio-jbrel.hd.free.fr/" + "livrets/parProf/" + ddlAnneeScolaire.SelectedValue + "/" + ddlChoixType.SelectedValue + "_" + ddlChoixMatiere.SelectedValue + ".pdf");
                                    ResponseHelper.Redirect("https://btssio-jbrel.hd.free.fr/" + "livrets/parProf/" + ddlAnneeScolaire.SelectedValue + "/" + ddlChoixType.SelectedValue + "_" + ddlChoixMatiere.SelectedValue + ".pdf", "_blank", "menubar=0,width=1200,height=2940");
                                    //ResponseHelper.Redirect("http://localhost:1514/" + "livrets/parProf/" + ddlAnneeScolaire.SelectedValue + "/" + ddlChoixType.SelectedValue + "_" + ddlChoixMatiere.SelectedValue + ".pdf", "_blank", "menubar=0,width=1200,height=2940");
                                    break;

                                 


                            } //fin switch

                        } //fin bloc try professeur
                        catch(Exception ex)
                        {
                            System.Drawing.Color couleur = System.Drawing.Color.Red;
                            lbGenerationPDF.ForeColor = couleur;
                            lbGenerationPDF.Text = "Génération non réalisée ! \n\n" + "Cause possible : " + ex.Message;
                            btnGenerationPDF.Text = "Générer un autre PDF ?";
                        }

                        break;

                    case "Tuteur":
                        try
                        {
                            string nomApprenti = ddlChoixMatiere.SelectedValue; //le choix de la matière devient choix apprenti
                            /* A mettre ici car si le fichier XML n'existe pas on va dans le catch et cela ne genere pas un pdf erroné */
                            XDocument odocTuteurXml = XDocument.Load(Server.MapPath("livrets/parTuteur/" + ddlAnneeScolaire.SelectedValue + "/" + ddlChoixType.SelectedValue + "_" + nomApprenti + ".Xml"));
                            IEnumerable<XElement> ocollitems = odocTuteurXml.Elements("items");

                            Document oDocTuteurPdf = new Document();
                            FileStream ofs = new FileStream(Server.MapPath("livrets/parTuteur/" + ddlAnneeScolaire.SelectedValue + "/" + ddlChoixType.SelectedValue + "_" + ddlChoixMatiere.SelectedValue + ".pdf"), FileMode.Create);

                            PdfWriter opdfWriter = PdfWriter.GetInstance(oDocTuteurPdf, ofs);

                            //attribution des événement au writer pour la numérotation de page
                            opdfWriter.PageEvent = new PageEventHelper();
                            
                            // opdfW.Pause();
                            oDocTuteurPdf.Open();

                            Chunk ochk = new Chunk("\r\n");

                            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/logoJB.png"));
                            logo.ScaleToFit(100f, 100f);
                            Paragraph oParaLogo = new Paragraph();
                            oParaLogo.Add(logo);
                            oParaLogo.IndentationLeft = 50;

                            

                            string phraseTitre = "Travaux réalisés en entreprise";
                            Phrase titre = new Phrase(phraseTitre, 
                                FontFactory.GetFont(
                                FontFactory.HELVETICA,
                                20, Font.BOLD));
                            Paragraph optitre = new Paragraph();
                            optitre.IndentationLeft = 150;
                            optitre.Add(titre);

                            oDocTuteurPdf.Add(oParaLogo);

                            oDocTuteurPdf.Add(ochk);

                            oDocTuteurPdf.Add(optitre);

                            

                            oDocTuteurPdf.Add(ochk);

                            string nomEntreprise = null;
                            string[] tabTuteur = ddlChoixType.SelectedValue.Split('_');
                            XmlDocument odoctut = new XmlDocument();
                            odoctut.Load(Server.MapPath("~/Tuteurs/" + Session["annee_scolaire"] + "/Tuteur.Xml"));

                            XmlNodeList xnlisttuteur = odoctut.SelectNodes("/tuteurs/tuteur");

                            foreach(XmlNode onode in xnlisttuteur)
                            {
                                if (onode["nom_tuteur"].InnerText == tabTuteur[1] && onode["prenom_tuteur"].InnerText == tabTuteur[0])
                                {
                                    nomEntreprise = onode["entreprise_tuteur"].InnerText;
                                    break;
                                }
                            }

                            Phrase ophrase = new Phrase("Nom de l'apprenti : " + ddlChoixMatiere.SelectedValue);
                            ophrase.Add(new Phrase("\r\n" + "Nom du Tuteur : " + ddlChoixType.SelectedValue));
                            ophrase.Add(new Phrase("\r\n" + "Entreprise : " + nomEntreprise));
                            ophrase.Add(new Phrase("\r\n" + "Année scolaire: " + ddlAnneeScolaire.SelectedValue));

                            Paragraph oparagrah = new Paragraph(ophrase);
                            // Phrase op1 = new Phrase(tbTextPdf.Text + "\n\n");
                            // opara.Add(op1);
                            oparagrah.IndentationLeft = 50f;
                            oDocTuteurPdf.Add(oparagrah);
                            oDocTuteurPdf.Add(ochk);

                            float[] largeursTabTuteur = { 20, 80 };
                           
                            PdfPTable otableauTravailEISE = new PdfPTable(largeursTabTuteur);
                            //tableau2.TotalWidth = 560;
                            //tableau2.LockedWidth = true;

                            Phrase phraseHoraire3 = new Phrase("DATE DÉBUT et FIN DE PÉRIODE",
                            FontFactory.GetFont(
                            FontFactory.HELVETICA,
                            8, Font.BOLD, new BaseColor(126, 51, 0)));

                            PdfPCell ocell3 = new PdfPCell(phraseHoraire3);
                            ocell3.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                            Phrase phraseTravail3 = new Phrase("ACTIVITÉS REALISÉES",
                            FontFactory.GetFont(
                            FontFactory.HELVETICA,
                            11, Font.BOLD, new BaseColor(126, 51, 0)));

                            PdfPCell ocell4 = new PdfPCell(phraseTravail3);
                            ocell4.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                            otableauTravailEISE.AddCell(ocell3);
                            otableauTravailEISE.AddCell(ocell4);

                            



                            foreach (XElement el in ocollitems)
                            {
                                IEnumerable<XElement> ocollitem = el.Elements("item");
                                foreach (XElement el2 in ocollitem)
                                {
                                    IEnumerable<XElement> ocollchampItem1 = el2.Elements("numMatiere");
                                    IEnumerable<XElement> ocollchampItem2 = el2.Elements("dateDebut");
                                    IEnumerable<XElement> ocollchampItem3 = el2.Elements("dateFin");
                                    IEnumerable<XElement> ocollchampItem4 = el2.Elements("travail_realise");

                                    IEnumerator<XElement> iterator = ocollchampItem2.GetEnumerator();
                                    iterator.MoveNext();
                                    XElement element = iterator.Current;
                                    IEnumerator<XElement> iterator1 = ocollchampItem3.GetEnumerator();
                                    iterator1.MoveNext();
                                    XElement element1 = iterator1.Current;
                                    IEnumerator<XElement> iterator2 = ocollchampItem4.GetEnumerator();
                                    iterator2.MoveNext();
                                    XElement element2 = iterator2.Current;

                                    Phrase phraseHeure = new Phrase("Du " + element.Value + "\n\n" + "au" + "\n\n" + element1.Value,
                                                                                        FontFactory.GetFont(
                                                                                        FontFactory.HELVETICA,
                                                                                        10, Font.BOLD));
                                    otableauTravailEISE.AddCell(phraseHeure);
                                    Phrase phraseResumeCours = new Phrase(element2.Value,
                                                                          FontFactory.GetFont(
                                                                          FontFactory.HELVETICA,
                                                                          11, Font.NORMAL));

                                    otableauTravailEISE.AddCell(phraseResumeCours);
                                    
                                }

                            }
                            otableauTravailEISE.KeepTogether = true;
                            oDocTuteurPdf.Add(otableauTravailEISE);
                            oDocTuteurPdf.Add(new Phrase("\n\nImprimé par le : " + DateTime.Today.ToShortDateString()));

                            

                            oDocTuteurPdf.Close();

                            
                            //System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(Server.MapPath("livrets/parTuteur/" + ddlAnneeScolaire.SelectedValue + "/" + ddlChoixType.SelectedValue + "_" + ddlChoixMatiere.SelectedValue + ".pdf"));

                            //System.Diagnostics.Process.Start(psi);
                            
                            
                            System.Drawing.Color couleur = System.Drawing.Color.Green;
                            lbGenerationPDF.ForeColor = couleur;
                            lbGenerationPDF.Text = "Génération réussie !";
                            btnGenerationPDF.Text = "Générer un autre PDF ?";

                            //appel pdf généré côté client
                            //Response.Redirect("https://btssio-jbrel.hd.free.fr/" + "livrets/parTuteur/" + ddlAnneeScolaire.SelectedValue + "/" + ddlChoixType.SelectedValue + "_" + ddlChoixMatiere.SelectedValue + ".pdf");
                            ResponseHelper.Redirect("https://btssio-jbrel.hd.free.fr/" + "livrets/parTuteur/" + ddlAnneeScolaire.SelectedValue + "/" + ddlChoixType.SelectedValue + "_" + ddlChoixMatiere.SelectedValue + ".pdf", "_blank", "menubar=0,width=1200,height=2940");
                            //ResponseHelper.Redirect("http://localhost:1514/" + "livrets/parTuteur/" + ddlAnneeScolaire.SelectedValue + "/" + ddlChoixType.SelectedValue + "_" + ddlChoixMatiere.SelectedValue + ".pdf", "_blank", "menubar=0,width=1200,height=2940");

                        } //fin bloc try tuteur
                        catch(Exception ex)
                        {
                            System.Drawing.Color couleur = System.Drawing.Color.Red;
                            lbGenerationPDF.ForeColor = couleur;
                            lbGenerationPDF.Text = "Génération non réalisée ! Cause possible : " + ex.Message;
                            btnGenerationPDF.Text = "Générer un autre PDF ?";
                        }

                        break;

                    case "Etudiant": /*----------------------------------------------------------------------------------------------------------------------------------------------------------------*/
                        /*Travail Jean-yves SAM génération du LIVRET d'apprentissage étudiant par étudiant
                         * Ce LIVRET devra comprendre :
                         *  - Le logo du Lycée Jacques Brel Puis le titre "LIVRET D'APPRENTISSAGE"
                         *  - Nom et prenom de l'apprenti (Exemple APPRENTI : Dupont François"
                         *  - La partie entreprise provenant de la lecture fichier XML de son tuteur (PdfTable) précédé du Nom prenom du Tuteur (Exemple TUTEUR : Dupont Xavier)
                         *  - La partie lycée provenant de la lecture de l'ensemble des fichiers cahier de texte XML de ses professeurs(1 pdfTable par matière)
                                -> Chaque tableau est précédé de la désignation de la matière (Exemple: MATIERE : SLAM5) puis du nom prenom PROFESSEUR (Exemple PROFESSEUR : Pilla Philippe
                                -> A la fin "Imprimé le :"
                         
                         * Dans le répertoire "Professeurs" "Tuteurs" et "Etudiants" se trouve les fichiers XML concerant ces trois type
                         * Les cahiers de texte, travaux entreprise et livrets d'apprentissage (les PDF et les XML) sont dans les sous dossiers de Livrets
                         * 
                         * Les fichiers cahier de texte ont leur nom terminant par exemple par SIO1COMMUN il s'agit ici des matières communes SI1 à SI6 du premier semestre SIO1
                         * J'ai choisi de faire comme cela afin de les tester plus rapidement si je cherche des SIx par exemple - Voir SaisieCahierTextProfesseur.aspx.cs
                         * Je n'ai pas rentré l'ensemble des matières dans le fichier XML correspondant qui se trouve dans le répertoire "Professeurs"
                         * Bon travail : Un temps d'appropriation du code sera peut être nécessaire
                         *----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
                        try
                        {

                            XmlDocument oXmlEtu = new XmlDocument();
                            oXmlEtu.Load(Server.MapPath("~/Etudiants/" + ddlAnneeScolaire.SelectedValue + "/Etudiant.xml"));
                            XmlNodeList oXmlnodeListEtu = oXmlEtu.SelectNodes("/etudiants/etudiant");

                            string nomEtu = null;
                            string prenomEtu = null;
                            string anneeEtu = null;
                            string speEtu = null;


                            string numTuteur = null;
                            string prenomTuteur = null;
                            string nomTuteur = null;
                            string entreprise = null;

                            /*Les couleurs des titres de tableau et texte*/
                            BaseColor MarronTclairSurligneTitre = new BaseColor(232, 214, 195);//(237,237,237);
                            BaseColor MarronClairText = new BaseColor(126, 51, 0);
                            BaseColor NoirText = new BaseColor(0, 0, 0);

                            foreach (XmlNode oNode in oXmlnodeListEtu)//On recupere le numéro du tuteur, l'année de l'étudiant et sa spécialité
                            {
                                if ((oNode["prenom_etudiant"].InnerText + "_" + oNode["nom_etudiant"].InnerText) == ddlChoixType.SelectedValue)
                                {
                                    nomEtu = oNode["nom_etudiant"].InnerText;
                                    prenomEtu = oNode["prenom_etudiant"].InnerText;
                                    numTuteur = oNode["num_tuteur"].InnerText;
                                    string[] anneeSpe = new string[2];
                                    anneeSpe = oNode["annee_etudiant"].InnerText.Split('-');
                                    anneeEtu = anneeSpe[0];
                                    speEtu = anneeSpe[1];

                                }
                            }

                            XmlDocument oXmlTuteur = new XmlDocument();
                            oXmlTuteur.Load(Server.MapPath("~/tuteurs/" + ddlAnneeScolaire.SelectedValue + "/" + "Tuteur.xml"));
                            XmlNodeList oXmlnodeListTuteur = oXmlTuteur.SelectNodes("/tuteurs/tuteur");

                            foreach (XmlNode oNode in oXmlnodeListTuteur)
                            {
                                if (oNode["num_tuteur"].InnerText.Trim() == numTuteur.Trim())
                                {
                                    prenomTuteur = oNode["prenom_tuteur"].InnerText.Trim();
                                    nomTuteur = oNode["nom_tuteur"].InnerText.Trim();
                                    entreprise = oNode["entreprise_tuteur"].InnerText.Trim();
                                    break;
                                }
                            }

                            //création du PDF
                            string nomPDF = nomEtu + "_" + prenomEtu + ".pdf";

                            //endroit sur le disque serveur
                            FileStream fsEtu = new FileStream(Server.MapPath("~/livrets/parEtudiant/" + ddlAnneeScolaire.SelectedValue + "/" + nomPDF), FileMode.Create, FileAccess.Write, FileShare.None);
                            Document oDocPdf = new Document();
                            PdfWriter oPdfW = PdfWriter.GetInstance(oDocPdf, fsEtu);

                            //attribution des événement au writer pour la numérotation de page
                            oPdfW.PageEvent = new PageEventHelper();
                            oDocPdf.Open();

                            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/logoJB.png"));
                            logo.ScaleToFit(100f, 100f);
                            
                            Paragraph oParaLogo = new Paragraph();
                            oParaLogo.Add(logo);
                            oParaLogo.IndentationLeft = 50;

                            Phrase TitrePdf = new Phrase("\r\n LIVRET D'APPRENTISSAGE",
                                FontFactory.GetFont(
                                FontFactory.HELVETICA,
                                20, Font.BOLD,MarronClairText));
                            
                            

                            Paragraph oParaTitrePdf = new Paragraph();
                            oParaTitrePdf.Add(TitrePdf);
                            oParaTitrePdf.IndentationLeft = 150;
                            
 
                            oDocPdf.Add(oParaLogo);
                            Chunk espace = new Chunk("\r\n");
                            oDocPdf.Add(espace);
                            oDocPdf.Add(espace);
                            oDocPdf.Add(oParaTitrePdf);
                            

                            string chaineEtudiant = "\r\n" +"Apprenti : " + prenomEtu + " " + nomEtu + "\r\n" +
                                "Année : "+ddlAnneeScolaire.SelectedValue+"\r\n"+
                                "Classe : BTS "+anneeEtu.ToUpper() + "\r\n \r\n";
                            Phrase phraseEtudiant = new Phrase(chaineEtudiant,
                                FontFactory.GetFont(
                                FontFactory.HELVETICA,
                                11, Font.NORMAL,MarronClairText));

                            string Laius = "Ce livret d'apprentissage est composé de deux parties, la liste des activités réalisées en entreprise d'une part et l'ensemble des cahiers de texte des cours dispensés dans le cadre du centre de formation d'autre part. \r\n \r\n";
                            Phrase phraseLaius = new Phrase(Laius,FontFactory.GetFont(
                                FontFactory.HELVETICA,11 ,Font.ITALIC,NoirText));
                            
                            Phrase TitreEntreprise = new Phrase("LES TRAVAUX RÉALISÉS EN ENTREPRISE",
                                FontFactory.GetFont(
                                FontFactory.HELVETICA,
                                14, Font.BOLD,MarronClairText));



                            float[] largeursTabEnt = { 100 };
                            PdfPTable oTableTitreEntreprise = new PdfPTable(largeursTabEnt);

                            PdfPCell oCellTitreEntreprise = new PdfPCell(TitreEntreprise);
                            oCellTitreEntreprise.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            oCellTitreEntreprise.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                            
                            oCellTitreEntreprise.BackgroundColor = MarronTclairSurligneTitre;

                            oTableTitreEntreprise.AddCell(oCellTitreEntreprise);
                                                        
                            //Phrase phrase = new Phrase("Etudiant : " + nomEtu + " " + prenomEtu + "\r\n" + "\r\n" + "\r\n");
                            Phrase phrase2 = new Phrase("\r\nTuteur : " + prenomTuteur + " " + nomTuteur + "\r\n", FontFactory.GetFont(
                                FontFactory.HELVETICA,
                                12, Font.BOLD,
                                MarronClairText));
                            Phrase phrase3 = new Phrase("Entreprise : " + entreprise + "\r\n", FontFactory.GetFont(
                                FontFactory.HELVETICA,
                                12, Font.BOLD,
                                MarronClairText));
                            Paragraph paragrah = new Paragraph(phraseEtudiant); /***************************/
                            paragrah.IndentationLeft = 50;
                            paragrah.Add(espace);
                            paragrah.Add(espace);
                            paragrah.Add(phraseLaius); /****************************************************/
                            
                            oDocPdf.Add(paragrah); //paragraph contient la presentation de etudiant + Laius
                            
                            oDocPdf.Add(espace);
                            oDocPdf.Add(espace);
                            //oDocPdf.Add(espace);
                            //oDocPdf.Add(espace);
                            //oDocPdf.Add(espace);
                            //oDocPdf.Add(espace);

                            //oDocPdf.Add(oTableTitreEntreprise);

                            Paragraph ParagraphTitreEntreprise = new Paragraph();
                            ParagraphTitreEntreprise.Add(phrase2);
                            ParagraphTitreEntreprise.Add(phrase3);
                            
                            
                            ParagraphTitreEntreprise.Add(espace);
                            // Phrase op1 = new Phrase(tbTextPdf.Text + "\n\n");
                            // opara.Add(op1);
                            ParagraphTitreEntreprise.IndentationLeft = 50;
                            PdfPCell oCellPresTuteur = new PdfPCell(ParagraphTitreEntreprise);
                            oCellPresTuteur.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            oTableTitreEntreprise.AddCell(oCellPresTuteur);
                            //oDocPdf.Add(ParagraphTitreEntreprise);
                            //oDocPdf.Add(oTableTitreEntreprise);                                                /*modif du 24/09*/

                            float[] largeursTabTut = { 20, 80 };
                            PdfPTable tableauPdf = new PdfPTable(largeursTabTut);
                            //tableau2.TotalWidth = 560;
                            //tableau2.LockedWidth = true;


                            Phrase phraseTitrePeriode = new Phrase("PÉRIODE",
                                FontFactory.GetFont(
                                FontFactory.HELVETICA,
                                11, Font.BOLD));
                                //new BaseColor(126,51,0)));
                            PdfPCell oCellPeriodeTabEnt = new PdfPCell(phraseTitrePeriode);
                            oCellPeriodeTabEnt.BackgroundColor = MarronTclairSurligneTitre;
                            oCellPeriodeTabEnt.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            oCellPeriodeTabEnt.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;


                            Phrase phraseTitreTravailEntreprise = new Phrase("ACTIVITÉS RÉALISÉES",
                                FontFactory.GetFont(
                                FontFactory.HELVETICA,
                                11, Font.BOLD));
                                //new BaseColor(126,51,0)));
                            PdfPCell oCellTitreTabEnt = new PdfPCell(phraseTitreTravailEntreprise);
                            oCellTitreTabEnt.BackgroundColor = MarronTclairSurligneTitre;
                            oCellTitreTabEnt.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            oCellTitreTabEnt.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;

                            tableauPdf.AddCell(oCellPeriodeTabEnt);
                            tableauPdf.AddCell(oCellTitreTabEnt);


                            XmlDocument oXmlItemsTuteur = new XmlDocument();
                            try
                            {
                                oXmlItemsTuteur.Load(Server.MapPath("~/livrets/parTuteur" + "/" + ddlAnneeScolaire.SelectedValue + "/" + prenomTuteur + "_" + nomTuteur + "_" + nomEtu + "." + prenomEtu + ".xml"));
                            }
                            catch
                            {
                                throw new Exception("Le tuteur n'a pas encore saisi ses données !"); //cas pas de fichier XML Tuteur existant
                                
                            }
                            XmlNodeList oXmlnodeListItemsTuteur = oXmlItemsTuteur.SelectNodes("/items/item");

                            foreach (XmlNode oNode in oXmlnodeListItemsTuteur)
                            {                            

                                string chainePeriode = "Du " + oNode["dateDebut"].InnerText + "\n\n" + "au \n\n" + oNode["dateFin"].InnerText;
                                Phrase phrasePeriode = new Phrase(chainePeriode,
                                    FontFactory.GetFont(
                                    FontFactory.HELVETICA,
                                    9, Font.BOLD));
                                PdfPCell cellphrasePeriode = new PdfPCell(phrasePeriode);
                                cellphrasePeriode.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                tableauPdf.AddCell(cellphrasePeriode);
                                Phrase phraseTravailRealise = new Phrase(oNode["travail_realise"].InnerText,
                                    FontFactory.GetFont(
                                    FontFactory.HELVETICA,
                                    10,Font.NORMAL));
                                PdfPCell cellphraseTravailRealise = new PdfPCell(phraseTravailRealise);
                                cellphraseTravailRealise.HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED;
                                tableauPdf.AddCell(cellphraseTravailRealise);
                            }
                            //tableauPdf.KeepTogether = true;
                            tableauPdf.SplitRows = true;
                            tableauPdf.SplitLate = false;
                            oTableTitreEntreprise.AddCell(tableauPdf);
                            oTableTitreEntreprise.KeepTogether = true;
                            oTableTitreEntreprise.SplitRows = true;
                            oTableTitreEntreprise.SplitLate = false;
                            //oDocPdf.Add(tableauPdf);
                            //oTableTitreEntreprise.KeepTogether = true;
                            //oTableTitreEntreprise.KeepRowsTogether(100);
                            oDocPdf.Add(oTableTitreEntreprise);
                            oDocPdf.Add(espace);
                            oDocPdf.Add(espace);
                            oDocPdf.Add(espace);
                            oDocPdf.Add(espace);
                            oDocPdf.Add(espace);
                            oDocPdf.Add(espace);
                            oDocPdf.Add(espace);
                            oDocPdf.Add(espace);


                            //Partie professeur ---------------------------------------------------------------------------------------------------------------------

                            string annee_scol = Convert.ToString(Session["annee_scolaire"]);
                            string repertoire = Server.MapPath("livrets/parProf/" + annee_scol + "/");
                            string[] tabFichierByGetFiles = Directory.GetFiles(repertoire, "*.Xml");

                           

                            Phrase TitreScolaire = new Phrase("LES TRAVAUX RÉALISÉS DANS LE CADRE SCOLAIRE",
                                FontFactory.GetFont(
                                FontFactory.HELVETICA,
                                14, Font.BOLD,MarronClairText));

                            float[] largeursTabScol = { 100 };
                            PdfPTable oTableTitreScolaire = new PdfPTable(largeursTabScol);

                            PdfPCell oCellTitreScolaire = new PdfPCell(TitreScolaire);
                            oCellTitreScolaire.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            oCellTitreScolaire.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                            //BaseColor grisEtain = new BaseColor(237, 237, 237);
                            oCellTitreScolaire.BackgroundColor = MarronTclairSurligneTitre;

                            oTableTitreScolaire.AddCell(oCellTitreScolaire);
                            //oTableTitreScolaire.KeepTogether = true;
                          
                            
                            //oDocPdf.Add(oTableTitreScolaire);
                            //oDocPdf.Add(espace);

                            int cptEntreeForeach = 0; //Pour ne mettre le titre qu'une seule fois sur le premier tableau dans le PDF
                            int cptMatTechnique = 0;
                            foreach (string fileName in tabFichierByGetFiles) //Traitement des fichiers xml matières techniques
                            {

                                cptMatTechnique++;
                                int longueurFileName = fileName.Length;
                                string caract = null;
                                int indexUs1, indexPointTm = -1;
                                while (caract != "_")
                                {
                                    longueurFileName = longueurFileName - 1;
                                    caract = fileName.Substring(longueurFileName, 1);
                                    if (caract == ".")
                                    {
                                        indexPointTm = longueurFileName;

                                    }


                                }

                                int Longeur = longueurFileName;
                                indexUs1 = longueurFileName;
                                XDocument odocTm = XDocument.Load(fileName);
                                IEnumerable<XElement> ocollDesItems = odocTm.Elements("items");
                                string matiereTm = fileName.Substring(indexUs1 + 1, indexPointTm - indexUs1 - 1);
                                string[] matAnneeSpe = matiereTm.Split('-');
                                string Matiere = matAnneeSpe[0];
                                string AnneeSpe = matAnneeSpe[1];

                                if (AnneeSpe.Length > 4) // les matière techniques
                                {
                                    
                                    string Annee = AnneeSpe.Substring(0, 4);
                                    string Spe = null;
                                    if (AnneeSpe.Length == 8)
                                    {
                                        Spe = AnneeSpe.Substring(4, 4); //cas matiere tech specialisée
                                    }
                                    else
                                    {
                                        Spe = AnneeSpe.Substring(4, 6); //cas matiere tech commune
                                    }

                                    if (Annee.Trim().ToLower() == anneeEtu.Trim().ToLower() && (Spe.Trim().ToLower() == speEtu.Trim().ToLower() || Spe.Trim().ToLower() == "commun" ))
                                    {
                                        cptEntreeForeach++; 
                                        XmlDocument oXmlItemsMat = new XmlDocument();
                                        oXmlItemsMat.Load(fileName);
                                        XmlNodeList oXmlnodeListItemsMat = oXmlItemsMat.SelectNodes("/items/item");
                                        
                                        float[] largeursTabProf = { 20, 80 };
                                        PdfPTable tableauPdfProf = new PdfPTable(largeursTabProf);

                                        string nomProf = null;
                                        string prenomProf = null;
                                        string numProf = null;
                                        string numMat = null;
                                        string intituleMat = null;
                                        foreach (XmlNode oNode in oXmlnodeListItemsMat)
                                        {
                                            numMat = oNode["numMatiere"].InnerText;
                                            break;
                                        }
                                        XmlDocument oXmlMat = new XmlDocument();

                                        oXmlMat.Load(Server.MapPath("~/Professeurs/" + annee_scol + "/" + "Matiere.Xml"));
                                        XmlNodeList oXmlNodesListMat = oXmlMat.SelectNodes("/matieres/matiere");
                                        foreach (XmlNode oNode in oXmlNodesListMat)
                                        {
                                            if (numMat == oNode["num_mat"].InnerText)
                                            {
                                                intituleMat = oNode["intitule_mat"].InnerText;
                                                numProf = oNode["num_prof"].InnerText;
                                            }
                                        }



                                        XmlDocument oXmlProf = new XmlDocument();

                                        oXmlProf.Load(Server.MapPath("~/Professeurs/" + annee_scol + "/" + "Professeur.Xml"));
                                        XmlNodeList oXmlNodesListProf = oXmlProf.SelectNodes("/professeurs/professeur");
                                        foreach (XmlNode oNode in oXmlNodesListProf)
                                        {
                                            if (numProf == oNode["num_prof"].InnerText)
                                            {
                                                nomProf = oNode["nom_prof"].InnerText;
                                                prenomProf = oNode["prenom_prof"].InnerText;
                                                break;
                                            }
                                        }


                                        string chaineEnseignantMatiere = "Enseignant : " + prenomProf + " " + nomProf + "\r\n" + "Matiere : " + Matiere.ToUpper() + " - " + intituleMat + "\r\n";
                                        Phrase phraseEnseignantMatiere = new Phrase(chaineEnseignantMatiere,
                                            FontFactory.GetFont(
                                            FontFactory.HELVETICA,
                                            11, Font.NORMAL,MarronClairText));
                                        //Phrase phraseProf = new Phrase("Enseignant : " + nomProf + " " + prenomProf + "\r\n" + "Matiere : " + Matiere.ToUpper() +" - "+ intituleMat + "\r\n");

                                        Paragraph paragraphProf = new Paragraph();
                                        paragraphProf.Add(phraseEnseignantMatiere);
                                        Chunk espaceProf = new Chunk("\r\n");
                                        paragraphProf.Add(espaceProf);
                                        paragraphProf.IndentationLeft = 50;

                                        Phrase phraseTitreDateProf = new Phrase("DATE - HORAIRE",
                                        FontFactory.GetFont(
                                        FontFactory.HELVETICA,
                                            11, Font.BOLD));

                                        PdfPCell oCellDateTabProf = new PdfPCell(phraseTitreDateProf);
                                        oCellDateTabProf.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        oCellDateTabProf.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                                        oCellDateTabProf.BackgroundColor = MarronTclairSurligneTitre;

                                        Phrase phraseTitreTravailProf = new Phrase("COURS OU TRAVAUX PRATIQUES",
                                        FontFactory.GetFont(
                                        FontFactory.HELVETICA,
                                            11, Font.BOLD));

                                        PdfPCell oCellTitreTabProf = new PdfPCell(phraseTitreTravailProf);
                                        oCellTitreTabProf.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        oCellTitreTabProf.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                                        oCellTitreTabProf.BackgroundColor = MarronTclairSurligneTitre;

                                        tableauPdfProf.AddCell(oCellDateTabProf);
                                        tableauPdfProf.AddCell(oCellTitreTabProf);



                                        foreach (XmlNode oNode in oXmlnodeListItemsMat)
                                        {                                         
                                            string chainePeriode = oNode["date"].InnerText + "\r\n" + oNode["heure_debut"].InnerText + "h" + " - " + oNode["heure_fin"].InnerText + "h";
                                            Phrase phrasePeriode = new Phrase(chainePeriode,
                                                FontFactory.GetFont(
                                                FontFactory.HELVETICA,
                                                9, Font.BOLD));
                                            PdfPCell cellphrasePeriode = new PdfPCell(phrasePeriode);
                                            cellphrasePeriode.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                            tableauPdfProf.AddCell(cellphrasePeriode);
                                            Phrase phraseResumeCours = new Phrase(oNode["resume_cours"].InnerText,FontFactory.GetFont(
                                                FontFactory.HELVETICA,
                                                10, Font.NORMAL));
                                            PdfPCell cellphraseResumeCours = new PdfPCell(phraseResumeCours);
                                            cellphraseResumeCours.HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED;
                                            tableauPdfProf.AddCell(cellphraseResumeCours);
                                        }

                                        
                                        //oDocPdf.Add(paragrahProf);
                                        PdfPCell ocellTitreProf = new PdfPCell(paragraphProf);

                                        if (cptEntreeForeach == 1) //Pour ne mettre le titre qu'une seule fois sur le premier tableau dans le PDF
                                        {
                                            
                                            oTableTitreScolaire.AddCell(ocellTitreProf); //oTableTitreEntreprise contient le titre, le tableauPdfProf est inséré dans dans une cellule du premier tableau
                                            oTableTitreScolaire.AddCell(tableauPdfProf);
                                            oTableTitreScolaire.KeepTogether = true;
                                            oTableTitreScolaire.SplitLate = false;
                                            oTableTitreScolaire.SplitRows = true;
                                            oDocPdf.Add(oTableTitreScolaire);
                                        }
                                        else
                                        {
                                            PdfPTable otableCours = new PdfPTable(largeursTabScol);
                                            otableCours.AddCell(ocellTitreProf);
                                            otableCours.AddCell(tableauPdfProf);
                                            otableCours.KeepTogether = true;
                                            otableCours.SplitLate = false;
                                            otableCours.SplitRows = true;
                                            oDocPdf.Add(otableCours);

                                        }
                                        //oDocPdf.Add(espaceProf);
                                        //oDocPdf.Add(espaceProf);
                                        //oDocPdf.Add(espaceProf);
                                        //oDocPdf.Add(espaceProf);



                                    }
                                }
                                

                            } // fin foreach sur fichier xml cahier de texte matières techniques

                            int cptMatgene = 0; //compteur des fichier mat gene
                            foreach (string fileName in tabFichierByGetFiles) //Traitement des fichiers xml matières générales
                            {

                                cptMatgene++;
                                int longueurFileName = fileName.Length;
                                string caract = null;
                                int indexUs1, indexPointTm = -1;
                                while (caract != "_")
                                {
                                    longueurFileName = longueurFileName - 1;
                                    caract = fileName.Substring(longueurFileName, 1);
                                    if (caract == ".")
                                    {
                                        indexPointTm = longueurFileName;

                                    }


                                }

                                int Longeur = longueurFileName;
                                indexUs1 = longueurFileName;
                                XDocument odocTm = XDocument.Load(fileName);
                                IEnumerable<XElement> ocollDesItems = odocTm.Elements("items");
                                string matiereTm = fileName.Substring(indexUs1 + 1, indexPointTm - indexUs1 - 1);
                                string[] matAnneeSpe = matiereTm.Split('-');
                                string Matiere = matAnneeSpe[0];
                                string AnneeSpe = matAnneeSpe[1];

                               
                                if (AnneeSpe.Length <= 4)
                                {
                                    if (AnneeSpe.Trim().ToLower() == anneeEtu.Trim().ToLower())
                                    {
                                        XmlDocument oXmlItemsMat = new XmlDocument();
                                        oXmlItemsMat.Load(fileName);
                                        XmlNodeList oXmlnodeListItemsMat = oXmlItemsMat.SelectNodes("/items/item");

                                        float[] largeursTabProf = { 20, 80};
                                        PdfPTable tableauPdfProf = new PdfPTable(largeursTabProf);

                                        string nomProf = null;
                                        string prenomProf = null;
                                        string numProf = null;
                                        string numMat = null;
                                        string intituleMat = null;
                                        foreach (XmlNode oNode in oXmlnodeListItemsMat)
                                        {
                                            numMat = oNode["numMatiere"].InnerText;

                                            break;
                                        }

                                        XmlDocument oXmlMat = new XmlDocument();

                                        oXmlMat.Load(Server.MapPath("~/Professeurs/" + annee_scol + "/" + "Matiere.Xml"));
                                        XmlNodeList oXmlNodesListMat = oXmlMat.SelectNodes("/matieres/matiere");
                                        foreach (XmlNode oNode in oXmlNodesListMat)
                                        {
                                            if (numMat == oNode["num_mat"].InnerText)
                                            {
                                                numProf = oNode["num_prof"].InnerText;
                                                intituleMat = oNode["intitule_mat"].InnerText;
                                                break;
                                            }
                                        }



                                        XmlDocument oXmlProf = new XmlDocument();

                                        oXmlProf.Load(Server.MapPath("~/Professeurs/" + annee_scol + "/" + "Professeur.Xml"));
                                        XmlNodeList oXmlNodesListProf = oXmlProf.SelectNodes("/professeurs/professeur");
                                        foreach (XmlNode oNode in oXmlNodesListProf)
                                        {
                                            if (numProf == oNode["num_prof"].InnerText)
                                            {
                                                nomProf = oNode["nom_prof"].InnerText;
                                                prenomProf = oNode["prenom_prof"].InnerText;
                                                break;
                                            }
                                        }
                                        string chaineEnseignantMatiere = "Enseignant : " + prenomProf + " " + nomProf + "\r\n" + "Matiere : " + Matiere.ToUpper() + " - " + intituleMat + "\r\n";
                                        Phrase phraseEnseignantMatiere = new Phrase(chaineEnseignantMatiere,
                                            FontFactory.GetFont(
                                            FontFactory.HELVETICA,
                                            11, Font.NORMAL, MarronClairText));
                                        //Phrase phraseProf = new Phrase("Enseignant : " + nomProf + " " + prenomProf + "\r\n" + "Matiere : " + Matiere.ToUpper() +" - "+ intituleMat + "\r\n");

                                        Paragraph paragraphProf = new Paragraph();
                                        paragraphProf.Add(phraseEnseignantMatiere);
                                        Chunk espaceProf = new Chunk("\r\n");
                                        paragraphProf.Add(espaceProf);
                                        paragraphProf.IndentationLeft = 50;

                                        Phrase phraseTitreDateProf = new Phrase("DATE - HORAIRE",
                                        FontFactory.GetFont(
                                        FontFactory.HELVETICA,
                                            11, Font.BOLD));


                                        PdfPCell oCellDateTabProf = new PdfPCell(phraseTitreDateProf);
                                        oCellDateTabProf.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        oCellDateTabProf.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                                        oCellDateTabProf.BackgroundColor = MarronTclairSurligneTitre;

                                        Phrase phraseTitreTravailProf = new Phrase("COURS OU TRAVAUX PRATIQUES",
                                        FontFactory.GetFont(
                                        FontFactory.HELVETICA,
                                            11, Font.BOLD));

                                        PdfPCell oCellTitreTabProf = new PdfPCell(phraseTitreTravailProf);
                                        oCellTitreTabProf.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        oCellTitreTabProf.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                                        oCellTitreTabProf.BackgroundColor = MarronTclairSurligneTitre;

                                        tableauPdfProf.AddCell(oCellDateTabProf);
                                        tableauPdfProf.AddCell(oCellTitreTabProf);


                                        foreach (XmlNode oNode in oXmlnodeListItemsMat)
                                        {

                                            string chainePeriode = oNode["date"].InnerText + "\r\n" + oNode["heure_debut"].InnerText + "h" + " - " + oNode["heure_fin"].InnerText + "h";
                                            Phrase phrasePeriode = new Phrase(chainePeriode,
                                                FontFactory.GetFont(
                                                FontFactory.HELVETICA,
                                                9, Font.BOLD));
                                            PdfPCell cellphrasePeriode = new PdfPCell(phrasePeriode);
                                            cellphrasePeriode.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                            tableauPdfProf.AddCell(cellphrasePeriode);
                                            Phrase phraseResumeCours = new Phrase(oNode["resume_cours"].InnerText, FontFactory.GetFont(
                                                FontFactory.HELVETICA,
                                                10, Font.NORMAL));
                                            PdfPCell cellphraseResumeCours = new PdfPCell(phraseResumeCours);
                                            cellphraseResumeCours.HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED;
                                            tableauPdfProf.AddCell(cellphraseResumeCours);
                                        }


                                        //oDocPdf.Add(paragrahProf);
                                        PdfPCell ocellTitreProf = new PdfPCell(paragraphProf);

                                        if (cptEntreeForeach == 1) //Pour ne mettre le titre qu'une seule fois sur le premier tableau dans le PDF
                                        {

                                            oTableTitreScolaire.AddCell(ocellTitreProf); //oTableTitreEntreprise contient le titre, le tableauPdfProf est inséré dans dans une cellule du premier tableau
                                            oTableTitreScolaire.AddCell(tableauPdfProf);
                                            oTableTitreScolaire.KeepTogether = true;
                                            oTableTitreScolaire.SplitLate = false;
                                            oTableTitreScolaire.SplitRows = true;
                                            oDocPdf.Add(oTableTitreScolaire);
                                        }
                                        else
                                        {
                                            PdfPTable otableCours = new PdfPTable(largeursTabScol);
                                            otableCours.AddCell(ocellTitreProf);
                                            otableCours.AddCell(tableauPdfProf);
                                            otableCours.KeepTogether = true;
                                            otableCours.SplitLate = false;
                                            otableCours.SplitRows = true;
                                            oDocPdf.Add(otableCours);

                                        }
                                        //oDocPdf.Add(espaceProf);
                                        //oDocPdf.Add(espaceProf);
                                        //oDocPdf.Add(espaceProf);
                                        //oDocPdf.Add(espaceProf);
                                    }
                                } // fin du else "traitement matières générale - le if => traitement matieres techniques

                            }// fin foreach sur fichier xml cahier de texte

                            if(cptMatTechnique == 0 && cptMatgene == 0)
                            {
                                throw new Exception("Aucun fichier Xml Cahier de texte généré !");

                            }

                            string chaineImprimeLe = "\n\nImprimé le : " + DateTime.Today.ToShortDateString();
                            Phrase phraseImprimeLe = new Phrase(chaineImprimeLe,
                                FontFactory.GetFont(
                                FontFactory.HELVETICA,
                                11, Font.ITALIC));


                            oDocPdf.Add(phraseImprimeLe);
                            oDocPdf.Close();
                            System.Drawing.Color couleurT = System.Drawing.Color.Green;
                            lbGenerationPDF.ForeColor = couleurT;
                            //System.Diagnostics.ProcessStartInfo psiTm = new System.Diagnostics.ProcessStartInfo(Server.MapPath("~/livrets/parEtudiant/" + ddlAnneeScolaire.SelectedValue + "/" + nomPDF));
                            //System.Diagnostics.Process.Start(psiTm);
                            System.Drawing.Color couleurTm = System.Drawing.Color.Green;
                            //Response.Redirect("https://btssio-jbrel.hd.free.fr/" + "livrets/parEtudiant/" + ddlAnneeScolaire.SelectedValue + "/" + nomPDF); //nomPDF => nom + prenom .pdf
                            //Response.Redirect("http://localhost:1514/" + "livrets/parEtudiant/" + ddlAnneeScolaire.SelectedValue + "/" + nomPDF,false); //nomPDF => nom + prenom .pdf       
                            
                            //ResponseHelper.Redirect("http://localhost:1514/" + "livrets/parEtudiant/" + ddlAnneeScolaire.SelectedValue + "/" + nomPDF, "_blank", "menubar=0,width=1200,height=2940");
                            
                            lbGenerationPDF.Text = "Génération réussie !";
                            btnGenerationPDF.Text = "Générer un autre PDF ?";
							ResponseHelper.Redirect("https://btssio-jbrel.hd.free.fr/" + "livrets/parEtudiant/" + ddlAnneeScolaire.SelectedValue + "/" + nomPDF,"_blank","menubar=0,width=1200,height=2940");
                            //ResponseHelper.Redirect("http://localhost:1514/" + "livrets/parEtudiant/" + ddlAnneeScolaire.SelectedValue + "/" + nomPDF, "_blank", "menubar=0,width=1200,height=2940");
                            //appel pdf 'livret apprentissage étudiant' généré côté client
                            //Response.Redirect("https://btssio-jbrel.hd.free.fr/" + "livrets/parEtudiant/" + ddlAnneeScolaire.SelectedValue + "/" + nomPDF); //nomPDF => nom + prenom .pdf
                        }
                        catch(Exception ex)
                        {
                            System.Drawing.Color couleur = System.Drawing.Color.Red;
                            lbGenerationPDF.ForeColor = couleur;                 
                            lbGenerationPDF.Text = ex.Message;                 
                            //lbGenerationPDF.Text = "Génération non réalisée ! Le fichier prof XML non généré.";
                            btnGenerationPDF.Text = "Générer un autre PDF ?";
                        }
                        
                        
                        break;
                        
                } // fin Swtich
            } //fin if btnGenerationPDF.Text == "Générer un autre PDF ?";
            else
            {
                Response.Redirect("GenerationDesPDF.aspx");
            }
        }

        /* fonction permettant de trouver l'index de la premiere occurence d'un caratere dans une chaine à partir de la fin de la chaine 
           Cela permet de ne pas dépendre de la syntaxe du chemin differente d'une machine à l'autre exemple : pour fileName*/
        private int findIndexFromEnd(string scaractere,string schaine )
        {
            string caract = null;
            int longueurChaine = schaine.Length;
            int indexTrouve = -1;
            while (caract != scaractere) //je cherche la place de scaractere
            {
                longueurChaine = longueurChaine - 1;
                caract = schaine.Substring(longueurChaine, 1);
                
              
            }
            indexTrouve = longueurChaine;
            return indexTrouve;

        }

        private string trouverNumProfParNom(string sprenomNomProf)
        {
                    XDocument odoc = XDocument.Load(Server.MapPath(ddlChoixGeneration.SelectedValue + "s" + "/" + Convert.ToString(Session["annee_scolaire"]) + "/" + "Professeur.Xml"));
                    IEnumerable<XElement> olistProfesseur = odoc.Element("professeurs").Elements();
                    foreach(XElement oNodeProfesseur in olistProfesseur)
                    {
                        IEnumerable<XElement> olistChampsProfresseur = oNodeProfesseur.Elements();
                        string num =null , nom = null , prenom = null;

                        foreach(XElement oNodeChampProfesseur in olistChampsProfresseur)
                        {
                            switch(oNodeChampProfesseur.Name.ToString())
                            {
                                case "num_prof":
                                     num = oNodeChampProfesseur.Value;
                                    break;
                                case "nom_prof":
                                    nom = oNodeChampProfesseur.Value;
                                    break;
                                case "prenom_prof":
                                    prenom = oNodeChampProfesseur.Value;
                                    
                                    break;

                            }

                            
                        }

                        /*mettre ce traitement ici permet de ne pas dépendre de l'ordre des champs dans le fichier en cas de changement */
                        String prenomNom = prenom + "." + nom; 
                        if (prenomNom.Trim() == sprenomNomProf.Trim())
                        {
                            return num;

                        }


                    }
                return null;
          
           
        }
        private string trouverNumTuteurParNom(string sprenomNomTuteur)
        {
                XDocument odoc = XDocument.Load(Server.MapPath(ddlChoixGeneration.SelectedValue + "s" + "/" + Convert.ToString(Session["annee_scolaire"]) + "/" + "Tuteur.Xml"));
                IEnumerable<XElement> olistTuteur = odoc.Element("tuteurs").Elements();
                foreach (XElement oNodeProfesseur in olistTuteur)
                {
                    IEnumerable<XElement> olistChampsProfresseur = oNodeProfesseur.Elements();
                    string num = null, nom = null, prenom = null;

                    foreach (XElement oNodeChampProfesseur in olistChampsProfresseur)
                    {
                        switch (oNodeChampProfesseur.Name.ToString())
                        {
                            case "num_tuteur":
                                num = oNodeChampProfesseur.Value;
                                break;
                            case "nom_tuteur":
                                nom = oNodeChampProfesseur.Value;
                                break;
                            case "prenom_tuteur":
                                prenom = oNodeChampProfesseur.Value;
                                
                                break;

                        }
                    }
                    String prenomNom = prenom + "_" + nom;
                    if (prenomNom.Trim() == sprenomNomTuteur.Trim())
                    {
                        return num;

                    }
                }
                return null;
        }      
    }


   
    public class PageEventHelper : PdfPageEventHelper //pour la numérotation des pages du PDF
    {
        PdfContentByte cb;
        PdfTemplate template;


        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            cb = writer.DirectContent;
            template = cb.CreateTemplate(25, 25);
        }

        public override void OnEndPage(PdfWriter writer, Document doc)
        {

            BaseColor grey = new BaseColor(128, 128, 128);
            iTextSharp.text.Font font = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL, grey);
            
            PdfPTable footerTbl = new PdfPTable(1);
            footerTbl.TotalWidth = doc.PageSize.Width;
           
           
            //numérotation de la page

            Chunk myFooter = new Chunk("Page n° " + (doc.PageNumber), FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 8, grey));
            PdfPCell footer = new PdfPCell(new Phrase(myFooter));
            footer.Border = iTextSharp.text.Rectangle.NO_BORDER; //NO_BORDER
            footer.HorizontalAlignment = Element.ALIGN_CENTER;
            footerTbl.AddCell(footer);

            //Phrase ophrase = new Phrase(myFooter);
            //footerTbl.AddCell(ophrase);
            //doc.Add(ophrase);

            // 20 pour 20 pixels du Bottom pour la place du numéro de page
            footerTbl.WriteSelectedRows(0, -1, 0, (doc.BottomMargin + 5), writer.DirectContent); //coordonnées de l'écriture du PdfTable
        }




        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

        }


    }



    public static class ResponseHelper //affiche le PDF dans un popup coté client - Response.redirect ne pouvant pas affiché dans un nouvel onglet ou dans un popup
    {
        public static void Redirect(string url, string target, string windowFeatures)
        {
            HttpContext context = HttpContext.Current;

            if ((String.IsNullOrEmpty(target) || target.Equals("_self", StringComparison.OrdinalIgnoreCase)) && String.IsNullOrEmpty(windowFeatures))
            {
                context.Response.Redirect(url);
            }
            else
            {
                Page page = (Page)context.Handler;

                if (page == null)
                {
                    throw new InvalidOperationException("Cannot redirect to new window outside Page context.");
                }

                url = page.ResolveClientUrl(url);

                string script;

                if (!String.IsNullOrEmpty(windowFeatures))
                {
                    script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                }
                else
                {
                    script = @"window.open(""{0}"", ""{1}"");";
                }

                script = String.Format(script, url, target, windowFeatures);

               ScriptManager.RegisterStartupScript(page,typeof(Page),"Redirect",script,true);

            }

        }

    }
}