namespace iTextSharp_0
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnGenerer = new System.Windows.Forms.Button();
            this.tbTextPdf = new System.Windows.Forms.RichTextBox();
            this.mcDate = new System.Windows.Forms.MonthCalendar();
            this.cbChoixMatiere = new System.Windows.Forms.ComboBox();
            this.lbChoixMatiere = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lbResume = new System.Windows.Forms.Label();
            this.lbDateHeure = new System.Windows.Forms.Label();
            this.lbHeureDebut = new System.Windows.Forms.Label();
            this.lbHeureFin = new System.Windows.Forms.Label();
            this.cbHeureDebut = new System.Windows.Forms.ComboBox();
            this.cbHeureFin = new System.Windows.Forms.ComboBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lbTitreNomProf = new System.Windows.Forms.Label();
            this.lbNomProf = new System.Windows.Forms.Label();
            this.btnEnregistrer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGenerer
            // 
            this.btnGenerer.Location = new System.Drawing.Point(716, 485);
            this.btnGenerer.Name = "btnGenerer";
            this.btnGenerer.Size = new System.Drawing.Size(109, 56);
            this.btnGenerer.TabIndex = 0;
            this.btnGenerer.Text = "Générer un PDF récapitulatif";
            this.btnGenerer.UseVisualStyleBackColor = true;
            this.btnGenerer.Click += new System.EventHandler(this.btnGenererPDF_Click);
            // 
            // tbTextPdf
            // 
            this.tbTextPdf.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTextPdf.Location = new System.Drawing.Point(18, 321);
            this.tbTextPdf.Name = "tbTextPdf";
            this.tbTextPdf.Size = new System.Drawing.Size(807, 158);
            this.tbTextPdf.TabIndex = 1;
            this.tbTextPdf.Text = "";
            // 
            // mcDate
            // 
            this.mcDate.Location = new System.Drawing.Point(15, 129);
            this.mcDate.Name = "mcDate";
            this.mcDate.TabIndex = 2;
            // 
            // cbChoixMatiere
            // 
            this.cbChoixMatiere.FormattingEnabled = true;
            this.cbChoixMatiere.Location = new System.Drawing.Point(170, 10);
            this.cbChoixMatiere.Name = "cbChoixMatiere";
            this.cbChoixMatiere.Size = new System.Drawing.Size(278, 21);
            this.cbChoixMatiere.TabIndex = 3;
            this.cbChoixMatiere.SelectedValueChanged += new System.EventHandler(this.cbChoixMatiere_SelectedValueChanged);
            // 
            // lbChoixMatiere
            // 
            this.lbChoixMatiere.AutoSize = true;
            this.lbChoixMatiere.Font = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbChoixMatiere.Location = new System.Drawing.Point(12, 13);
            this.lbChoixMatiere.Name = "lbChoixMatiere";
            this.lbChoixMatiere.Size = new System.Drawing.Size(152, 18);
            this.lbChoixMatiere.TabIndex = 4;
            this.lbChoixMatiere.Text = "Choisir votre matière :";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(545, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(311, 278);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // lbResume
            // 
            this.lbResume.AutoSize = true;
            this.lbResume.Font = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbResume.Location = new System.Drawing.Point(15, 300);
            this.lbResume.Name = "lbResume";
            this.lbResume.Size = new System.Drawing.Size(125, 18);
            this.lbResume.TabIndex = 6;
            this.lbResume.Text = "Résumé du cours :";
            // 
            // lbDateHeure
            // 
            this.lbDateHeure.AutoSize = true;
            this.lbDateHeure.Font = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDateHeure.Location = new System.Drawing.Point(15, 107);
            this.lbDateHeure.Name = "lbDateHeure";
            this.lbDateHeure.Size = new System.Drawing.Size(155, 18);
            this.lbDateHeure.TabIndex = 7;
            this.lbDateHeure.Text = "Choisir Date et heure :";
            // 
            // lbHeureDebut
            // 
            this.lbHeureDebut.AutoSize = true;
            this.lbHeureDebut.Font = new System.Drawing.Font("Modern No. 20", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbHeureDebut.Location = new System.Drawing.Point(263, 129);
            this.lbHeureDebut.Name = "lbHeureDebut";
            this.lbHeureDebut.Size = new System.Drawing.Size(82, 15);
            this.lbHeureDebut.TabIndex = 8;
            this.lbHeureDebut.Text = "Heure Début :";
            // 
            // lbHeureFin
            // 
            this.lbHeureFin.AutoSize = true;
            this.lbHeureFin.Font = new System.Drawing.Font("Modern No. 20", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbHeureFin.Location = new System.Drawing.Point(263, 189);
            this.lbHeureFin.Name = "lbHeureFin";
            this.lbHeureFin.Size = new System.Drawing.Size(74, 15);
            this.lbHeureFin.TabIndex = 9;
            this.lbHeureFin.Text = "Heure Fin : ";
            // 
            // cbHeureDebut
            // 
            this.cbHeureDebut.FormattingEnabled = true;
            this.cbHeureDebut.Location = new System.Drawing.Point(358, 129);
            this.cbHeureDebut.Name = "cbHeureDebut";
            this.cbHeureDebut.Size = new System.Drawing.Size(41, 21);
            this.cbHeureDebut.TabIndex = 10;
            // 
            // cbHeureFin
            // 
            this.cbHeureFin.FormattingEnabled = true;
            this.cbHeureFin.Location = new System.Drawing.Point(358, 189);
            this.cbHeureFin.Name = "cbHeureFin";
            this.cbHeureFin.Size = new System.Drawing.Size(41, 21);
            this.cbHeureFin.TabIndex = 11;
            this.cbHeureFin.SelectedValueChanged += new System.EventHandler(this.cbHeureFin_SelectedValueChanged);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // lbTitreNomProf
            // 
            this.lbTitreNomProf.AutoSize = true;
            this.lbTitreNomProf.Font = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitreNomProf.Location = new System.Drawing.Point(12, 56);
            this.lbTitreNomProf.Name = "lbTitreNomProf";
            this.lbTitreNomProf.Size = new System.Drawing.Size(145, 18);
            this.lbTitreNomProf.TabIndex = 12;
            this.lbTitreNomProf.Text = "Nom du Professeur : ";
            // 
            // lbNomProf
            // 
            this.lbNomProf.AutoSize = true;
            this.lbNomProf.Location = new System.Drawing.Point(167, 61);
            this.lbNomProf.Name = "lbNomProf";
            this.lbNomProf.Size = new System.Drawing.Size(66, 13);
            this.lbNomProf.TabIndex = 13;
            this.lbNomProf.Text = "Nom du Prof";
            // 
            // btnEnregistrer
            // 
            this.btnEnregistrer.Location = new System.Drawing.Point(18, 485);
            this.btnEnregistrer.Name = "btnEnregistrer";
            this.btnEnregistrer.Size = new System.Drawing.Size(137, 56);
            this.btnEnregistrer.TabIndex = 14;
            this.btnEnregistrer.Text = "Enregistrer le cours du jour";
            this.btnEnregistrer.UseVisualStyleBackColor = true;
            this.btnEnregistrer.Click += new System.EventHandler(this.btnEnregistrer_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Modern No. 20", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(415, 130);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "heure";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Modern No. 20", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(417, 190);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 15);
            this.label2.TabIndex = 16;
            this.label2.Text = "heure";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 573);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnEnregistrer);
            this.Controls.Add(this.lbNomProf);
            this.Controls.Add(this.lbTitreNomProf);
            this.Controls.Add(this.cbHeureFin);
            this.Controls.Add(this.cbHeureDebut);
            this.Controls.Add(this.lbHeureFin);
            this.Controls.Add(this.lbHeureDebut);
            this.Controls.Add(this.lbDateHeure);
            this.Controls.Add(this.lbResume);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lbChoixMatiere);
            this.Controls.Add(this.cbChoixMatiere);
            this.Controls.Add(this.mcDate);
            this.Controls.Add(this.tbTextPdf);
            this.Controls.Add(this.btnGenerer);
            this.Name = "Form1";
            this.Text = "Livret d\'apprentissage BTS SIO 1 et 2 LEGT Jacques Brel";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerer;
        private System.Windows.Forms.RichTextBox tbTextPdf;
        private System.Windows.Forms.MonthCalendar mcDate;
        private System.Windows.Forms.ComboBox cbChoixMatiere;
        private System.Windows.Forms.Label lbChoixMatiere;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lbResume;
        private System.Windows.Forms.Label lbDateHeure;
        private System.Windows.Forms.Label lbHeureDebut;
        private System.Windows.Forms.Label lbHeureFin;
        private System.Windows.Forms.ComboBox cbHeureDebut;
        private System.Windows.Forms.ComboBox cbHeureFin;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label lbTitreNomProf;
        private System.Windows.Forms.Label lbNomProf;
        private System.Windows.Forms.Button btnEnregistrer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

