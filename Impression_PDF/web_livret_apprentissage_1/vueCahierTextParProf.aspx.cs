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
    public partial class vueCahierTextParProf : System.Web.UI.Page
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

            /*if (!Page.IsPostBack) //si la page n'a pas encore été postée au serveur
            {
                ddlChoixEtudiant.Items.Add(" ");
                ddlChoixEtudiant.Items.Add("BTS SIO1");
                ddlChoixEtudiant.Items.Add("BTS SIO2");
               
          
            }*/
            //tblcahierText = new Table();
            //tblcahierText.GridLines = GridLines.Both;
            //PlaceHolder1.Controls.Add(tblcahierText); //je rajoute le table dans le placeHolder -> permet de positionner le table sur la page au lieu de mettre le table directement dans la page asp

            peuplerCDT();
           
        }

        

        protected void peuplerCDT()
        {

            //string anneeEtudiant = ddlChoixEtudiant.SelectedValue.Trim();

            string annee_sco = Convert.ToString(Session["annee_scolaire"]);
            string repertoireCible = Server.MapPath("livrets/parProf/" + annee_sco + "/");
            string[] fileEntries = Directory.GetFiles(repertoireCible, "*.Xml");

            //tri tableau ordre inverse
            IComparer myComparer = new myReverserClass();
            Array.Sort(fileEntries, myComparer);

            string prenomNomProf = (string)Session["prenomNomUser"];
            prenomNomProf = prenomNomProf.Replace(" ", ".");
            bool cdtExiste = false;

            //en fonction de l'année (Classe) selectionnée
        
               
                    foreach (string fileName in fileEntries) //parcours l'ensemble des fichiers
                    {
                        // ProcessFile(fileName);
                        //int lg = fileName.Length;
                        //int indexUs0 = fileName.IndexOf("-", 130);

                        bool dejaTitre = false;

                        //int indexTiret0 = calculPositionTiret(fileName); //méthode en fin de fichier
                        int indexUnderscore0 = calculPositionUnderscore(fileName); //méthode en fin de fichier

                        string prenomNomExtrait = fileName.Substring(indexUnderscore0 - prenomNomProf.Trim().Length , prenomNomProf.Trim().Length);


                        //string test = fileName.Substring(indexUs0 + 1, fileName.Length - (indexUs0 + 1));
                        if (prenomNomExtrait.Trim().ToLower() ==  prenomNomProf.Trim().ToLower()) //verifie que le fichier est un fichier avec une extension Xml
                        {
                            //XDocument odoc1 = XDocument.Load(Server.MapPath("livrets/parProf/" + ddlAnneeScolaire.SelectedValue + "/" + ddlChoixType.SelectedValue + "_" + matiere + "-" + anneeBTS + ".Xml"));

                            cdtExiste = true;
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
                  
                if(!cdtExiste)
                {
                    //Si aucun fichier XML avec le nom du prof alors avertir le professeur
                    lbException.Text = "Aucun cahier de texte n'a été saisi de votre part !";
                }
               

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