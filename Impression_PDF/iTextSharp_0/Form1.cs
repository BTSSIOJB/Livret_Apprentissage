using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf; // Pour Document
using System.IO; // pour FileStream
using classeMétier;
using System.DirectoryServices;
namespace iTextSharp_0
{
    public partial class Form1 : Form
    {
        Cprofs oprofs = null;
        Cmatieres omatieres = null;
        
        public Form1()
        {
            InitializeComponent();
            DirectoryEntry Ldap = new DirectoryEntry("LDAP://192.168.0.2", "btsljb@administrateur", "caspere69xo");

            StreamReader sr = new StreamReader("FichierMatProf.txt");
            
            cbChoixMatiere.Text = "Dérouler la liste pour choisir votre matière";
            string ligne,caract,matiere = null,profPrenom = null, profNom = null;
            Boolean rencontreBlanc = false;
            int x = 0;

            while ((ligne = sr.ReadLine()) != null)
            {
                while((caract = ligne.Substring(x,1)) != "#")
                {

                    matiere = matiere + caract;
                    x++;
                }

                x++; //pour sauter le #

                while ((caract = ligne.Substring(x, 1)) != "#")
                {
                    if (caract == " ")
                    {
                        rencontreBlanc = true;
                    }
                    if (!rencontreBlanc)
                    {
                        profPrenom = profPrenom + caract;
                    }
                    else
                    {
                        profNom = profNom + caract;

                    }
                    x++;
                }

                oprofs = Cprofs.getInstance();
                omatieres = Cmatieres.getInstance();
                Cmatiere omatiere = new Cmatiere();
                omatiere.designation = matiere.Trim();
                int numMat = Cmatiere.augmentenbTotalMat();
                omatiere.num = numMat;
                omatieres.ocollMatieres.Add(omatiere);
                Cprof oprof = new Cprof();
                oprof.nom = profNom.Trim();
                oprof.prenom = profPrenom.Trim();
                int numprof = Cprof.augmentenbTotalProf();
                oprof.num = numprof;
                oprof.lesMatieres.Add(omatiere);
                oprofs.ocollProfs.Add(oprof);

                cbChoixMatiere.Items.Add(matiere);
                oprof.lesMatieres.Add(omatiere); //association 1 à * entre Cprof et Cmatiere

                /* Réinitialisation pour l'itération suivante du WHILE externe */
                x = 0;
                rencontreBlanc = false;
                matiere = null;
                profNom = null;
                profPrenom = null;
               
            }

            sr.Close();

            cbHeureDebut.Text = "HD";
            cbHeureDebut.Items.Add("8");
            cbHeureDebut.Items.Add("9");
            cbHeureDebut.Items.Add("10");
            cbHeureDebut.Items.Add("11");
            cbHeureDebut.Items.Add("12");
            cbHeureDebut.Items.Add("13");
            cbHeureDebut.Items.Add("14");
            cbHeureDebut.Items.Add("15");
            cbHeureDebut.Items.Add("16");
            cbHeureDebut.Items.Add("17");

            cbHeureFin.Text = "HF";
            cbHeureFin.Items.Add("9");
            cbHeureFin.Items.Add("10");
            cbHeureFin.Items.Add("11");
            cbHeureFin.Items.Add("12");
            cbHeureFin.Items.Add("13");
            cbHeureFin.Items.Add("14");
            cbHeureFin.Items.Add("15");
            cbHeureFin.Items.Add("16");
            cbHeureFin.Items.Add("17");
            cbHeureFin.Items.Add("18");
        }

        private void btnGenererPDF_Click(object sender, EventArgs e)
        {
            Document newDoc = new Document();
            FileStream ofs = new FileStream("File_" + lbNomProf.Text + "_" + cbChoixMatiere.Text + ".pdf", FileMode.Append);
            
            PdfWriter opdfW = PdfWriter.GetInstance(newDoc,ofs);
            // opdfW.Pause();
            newDoc.Open();
            Phrase op = new Phrase("MATIERE : " + cbChoixMatiere.Text);
            op.Add(new Phrase("\n\r" + "Nom du Professeur : " + lbNomProf.Text + "\n\n"));
            
            Paragraph opara = new Paragraph(op);
            // Phrase op1 = new Phrase(tbTextPdf.Text + "\n\n");
            // opara.Add(op1);
            opara.IndentationLeft = 30f;     
            newDoc.Add(opara);

            float[] largeursTab = { 20, 80};
            PdfPTable tableau = new PdfPTable(largeursTab);
            //tableau2.TotalWidth = 560;
            //tableau2.LockedWidth = true;

            tableau.AddCell("DATE - HORAIRE");
            tableau.AddCell("TRAVAIL REALISE");
            
       

            tableau.AddCell(mcDate.SelectionStart.ToShortDateString() + "\n\n" + cbHeureDebut.SelectedItem.ToString() + "-" + cbHeureFin.SelectedItem.ToString() + " Heure");

            tableau.AddCell(tbTextPdf.Text); ;
           

            newDoc.Add(tableau);
            newDoc.Add(new Phrase("\n\nImprimé par le Professeur : " + lbNomProf.Text + " le " + DateTime.Today.ToShortDateString()));
            newDoc.Close();

            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("File_" + lbNomProf.Text + "_" + cbChoixMatiere.Text + ".pdf");

            System.Diagnostics.Process.Start(psi);

        }

        private void cbChoixMatiere_SelectedValueChanged(object sender, EventArgs e)
        {
            Cprofs oprofs = Cprofs.getInstance();
            Cmatieres omats = Cmatieres.getInstance();
            string test = cbChoixMatiere.Text;
            foreach(Cprof oprof in oprofs.ocollProfs)
            {
                
                if(oprof.lesMatieres[0].designation.ToString().Trim() == cbChoixMatiere.Text.Trim())
                {
                    lbNomProf.Text = oprof.prenom + " " + oprof.nom;
                    break;

                }

            }
            
        }

        private void btnEnregistrer_Click(object sender, EventArgs e)
        {
          DialogResult odr =  MessageBox.Show("Voulez vous vraiment valider la saisie ?", "Attention", MessageBoxButtons.YesNo);

          if (odr == DialogResult.Yes)
          {
              if (File.Exists("File_" + lbNomProf.Text + "_" + cbChoixMatiere.Text + ".txt"))
              {
                  StreamWriter sw = new StreamWriter("File_" + lbNomProf.Text + "_" + cbChoixMatiere.Text + ".txt", true);
                  sw.WriteLine("#" + mcDate.SelectionStart.ToShortDateString() + "#");
                  sw.WriteLine("#" + cbHeureDebut.Text + "-" + cbHeureFin.Text + " Heure" + "#");
                  sw.WriteLine("#" + tbTextPdf.Text + "#");
                  sw.WriteLine(" ");
                  sw.Close();
              }
              else
              {
                  StreamWriter sw = new StreamWriter("File_" + lbNomProf.Text + "_" + cbChoixMatiere.Text + ".txt", true);
                  sw.WriteLine("#" + cbChoixMatiere.Text + "#");
                  sw.WriteLine("#" + lbNomProf.Text + "#");
                  sw.WriteLine(" ");
                  sw.WriteLine("#" + mcDate.SelectionStart.ToShortDateString() + "#");
                  sw.WriteLine("#" + cbHeureDebut.Text + "-" + cbHeureFin.Text + " Heure" + "#");
                  sw.WriteLine("#" + tbTextPdf.Text + "#");
                  sw.WriteLine(" ");
                  sw.Close();

              }

              MessageBox.Show("Enregistrement effectué !");
          }
          else
          {
              MessageBox.Show("Aucun enregistrement effectué !");

          }
        }

        private void cbHeureFin_SelectedValueChanged(object sender, EventArgs e)
        {
            if(Convert.ToInt16(cbHeureDebut.Text) > Convert.ToInt16(cbHeureFin.Text))
            {
                MessageBox.Show("Attention heure de fin supérieure à heure début !", "Attention");

            }
        }
    
    }
}
