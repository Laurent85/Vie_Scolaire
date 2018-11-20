namespace Vie_Scolaire
{
    partial class Principal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Principal));
            this.CbxClasses = new System.Windows.Forms.ComboBox();
            this.cbxEleves = new System.Windows.Forms.ComboBox();
            this.PhotoEleve = new System.Windows.Forms.PictureBox();
            this.btnImportPhotos = new System.Windows.Forms.Button();
            this.txbInfosEleves = new System.Windows.Forms.RichTextBox();
            this.lblNomEleve = new System.Windows.Forms.Label();
            this.lblClasseEleve = new System.Windows.Forms.Label();
            this.lblTitre = new System.Windows.Forms.Label();
            this.PhotoLogoCollege = new System.Windows.Forms.PictureBox();
            this.lblResponsable = new System.Windows.Forms.Label();
            this.lblAdresse = new System.Windows.Forms.Label();
            this.lblCpVille = new System.Windows.Forms.Label();
            this.lblTelDom = new System.Windows.Forms.Label();
            this.lblTelPortResp = new System.Windows.Forms.Label();
            this.lblMailResp = new System.Windows.Forms.Label();
            this.lblConjoint = new System.Windows.Forms.Label();
            this.lblTelPortConjoint = new System.Windows.Forms.Label();
            this.lblMailConjoint = new System.Windows.Forms.Label();
            this.grpBoxResponsables = new System.Windows.Forms.GroupBox();
            this.btnDateDuJour = new System.Windows.Forms.Button();
            this.btnImprimer = new System.Windows.Forms.Button();
            this.Calendrier = new System.Windows.Forms.DateTimePicker();
            this.btnImprimerClasse = new System.Windows.Forms.Button();
            this.chkCacherFiches = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.PhotoEleve)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhotoLogoCollege)).BeginInit();
            this.grpBoxResponsables.SuspendLayout();
            this.SuspendLayout();
            // 
            // CbxClasses
            // 
            this.CbxClasses.BackColor = System.Drawing.Color.Ivory;
            this.CbxClasses.FormattingEnabled = true;
            this.CbxClasses.Location = new System.Drawing.Point(285, 87);
            this.CbxClasses.Name = "CbxClasses";
            this.CbxClasses.Size = new System.Drawing.Size(147, 21);
            this.CbxClasses.TabIndex = 0;
            this.CbxClasses.SelectedIndexChanged += new System.EventHandler(this.CbxClasses_SelectedIndexChanged);
            // 
            // cbxEleves
            // 
            this.cbxEleves.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbxEleves.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbxEleves.BackColor = System.Drawing.Color.Ivory;
            this.cbxEleves.FormattingEnabled = true;
            this.cbxEleves.Location = new System.Drawing.Point(482, 87);
            this.cbxEleves.Name = "cbxEleves";
            this.cbxEleves.Size = new System.Drawing.Size(220, 21);
            this.cbxEleves.TabIndex = 1;
            this.cbxEleves.SelectedIndexChanged += new System.EventHandler(this.cbxEleves_SelectedIndexChanged);
            // 
            // PhotoEleve
            // 
            this.PhotoEleve.Location = new System.Drawing.Point(1032, 87);
            this.PhotoEleve.Name = "PhotoEleve";
            this.PhotoEleve.Size = new System.Drawing.Size(147, 196);
            this.PhotoEleve.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PhotoEleve.TabIndex = 2;
            this.PhotoEleve.TabStop = false;
            // 
            // btnImportPhotos
            // 
            this.btnImportPhotos.Location = new System.Drawing.Point(780, 85);
            this.btnImportPhotos.Name = "btnImportPhotos";
            this.btnImportPhotos.Size = new System.Drawing.Size(147, 23);
            this.btnImportPhotos.TabIndex = 3;
            this.btnImportPhotos.Text = "Importer les photos";
            this.btnImportPhotos.UseVisualStyleBackColor = true;
            this.btnImportPhotos.Click += new System.EventHandler(this.BtnImportPhotosClick);
            // 
            // txbInfosEleves
            // 
            this.txbInfosEleves.BackColor = System.Drawing.Color.Ivory;
            this.txbInfosEleves.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbInfosEleves.Location = new System.Drawing.Point(72, 190);
            this.txbInfosEleves.Name = "txbInfosEleves";
            this.txbInfosEleves.Size = new System.Drawing.Size(857, 391);
            this.txbInfosEleves.TabIndex = 4;
            this.txbInfosEleves.Text = "";
            this.txbInfosEleves.TextChanged += new System.EventHandler(this.TxbInfosElevesTextChanged);
            // 
            // lblNomEleve
            // 
            this.lblNomEleve.AutoSize = true;
            this.lblNomEleve.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNomEleve.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblNomEleve.Location = new System.Drawing.Point(1029, 36);
            this.lblNomEleve.Name = "lblNomEleve";
            this.lblNomEleve.Size = new System.Drawing.Size(145, 17);
            this.lblNomEleve.TabIndex = 5;
            this.lblNomEleve.Text = "Nom Prénom Eleve";
            // 
            // lblClasseEleve
            // 
            this.lblClasseEleve.AutoSize = true;
            this.lblClasseEleve.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClasseEleve.ForeColor = System.Drawing.Color.Brown;
            this.lblClasseEleve.Location = new System.Drawing.Point(1087, 62);
            this.lblClasseEleve.Name = "lblClasseEleve";
            this.lblClasseEleve.Size = new System.Drawing.Size(44, 13);
            this.lblClasseEleve.TabIndex = 6;
            this.lblClasseEleve.Text = "Classe";
            // 
            // lblTitre
            // 
            this.lblTitre.AutoSize = true;
            this.lblTitre.Font = new System.Drawing.Font("Lucida Calligraphy", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitre.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblTitre.Location = new System.Drawing.Point(362, 10);
            this.lblTitre.Name = "lblTitre";
            this.lblTitre.Size = new System.Drawing.Size(285, 52);
            this.lblTitre.TabIndex = 7;
            this.lblTitre.Text = "Vie scolaire";
            // 
            // PhotoLogoCollege
            // 
            this.PhotoLogoCollege.Image = global::Vie_Scolaire.Properties.Resources.LOGO_COLLEGE_SAINTJACQUES_MOYEN;
            this.PhotoLogoCollege.Location = new System.Drawing.Point(72, 13);
            this.PhotoLogoCollege.Name = "PhotoLogoCollege";
            this.PhotoLogoCollege.Size = new System.Drawing.Size(132, 71);
            this.PhotoLogoCollege.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PhotoLogoCollege.TabIndex = 8;
            this.PhotoLogoCollege.TabStop = false;
            // 
            // lblResponsable
            // 
            this.lblResponsable.AutoSize = true;
            this.lblResponsable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResponsable.Location = new System.Drawing.Point(15, 35);
            this.lblResponsable.Name = "lblResponsable";
            this.lblResponsable.Size = new System.Drawing.Size(80, 13);
            this.lblResponsable.TabIndex = 9;
            this.lblResponsable.Text = "Responsable";
            // 
            // lblAdresse
            // 
            this.lblAdresse.AutoSize = true;
            this.lblAdresse.Location = new System.Drawing.Point(15, 58);
            this.lblAdresse.Name = "lblAdresse";
            this.lblAdresse.Size = new System.Drawing.Size(45, 13);
            this.lblAdresse.TabIndex = 10;
            this.lblAdresse.Text = "Adresse";
            // 
            // lblCpVille
            // 
            this.lblCpVille.AutoSize = true;
            this.lblCpVille.Location = new System.Drawing.Point(15, 82);
            this.lblCpVille.Name = "lblCpVille";
            this.lblCpVille.Size = new System.Drawing.Size(42, 13);
            this.lblCpVille.TabIndex = 11;
            this.lblCpVille.Text = "Cp Ville";
            // 
            // lblTelDom
            // 
            this.lblTelDom.AutoSize = true;
            this.lblTelDom.Location = new System.Drawing.Point(15, 106);
            this.lblTelDom.Name = "lblTelDom";
            this.lblTelDom.Size = new System.Drawing.Size(65, 13);
            this.lblTelDom.TabIndex = 12;
            this.lblTelDom.Text = "Tel Domicile";
            // 
            // lblTelPortResp
            // 
            this.lblTelPortResp.AutoSize = true;
            this.lblTelPortResp.Location = new System.Drawing.Point(15, 131);
            this.lblTelPortResp.Name = "lblTelPortResp";
            this.lblTelPortResp.Size = new System.Drawing.Size(64, 13);
            this.lblTelPortResp.TabIndex = 13;
            this.lblTelPortResp.Text = "Tel Portable";
            // 
            // lblMailResp
            // 
            this.lblMailResp.AutoSize = true;
            this.lblMailResp.Location = new System.Drawing.Point(15, 156);
            this.lblMailResp.Name = "lblMailResp";
            this.lblMailResp.Size = new System.Drawing.Size(32, 13);
            this.lblMailResp.TabIndex = 14;
            this.lblMailResp.Text = "eMail";
            // 
            // lblConjoint
            // 
            this.lblConjoint.AutoSize = true;
            this.lblConjoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConjoint.Location = new System.Drawing.Point(15, 208);
            this.lblConjoint.Name = "lblConjoint";
            this.lblConjoint.Size = new System.Drawing.Size(53, 13);
            this.lblConjoint.TabIndex = 15;
            this.lblConjoint.Text = "Conjoint";
            // 
            // lblTelPortConjoint
            // 
            this.lblTelPortConjoint.AutoSize = true;
            this.lblTelPortConjoint.Location = new System.Drawing.Point(15, 230);
            this.lblTelPortConjoint.Name = "lblTelPortConjoint";
            this.lblTelPortConjoint.Size = new System.Drawing.Size(64, 13);
            this.lblTelPortConjoint.TabIndex = 16;
            this.lblTelPortConjoint.Text = "Tel Portable";
            // 
            // lblMailConjoint
            // 
            this.lblMailConjoint.AutoSize = true;
            this.lblMailConjoint.Location = new System.Drawing.Point(15, 252);
            this.lblMailConjoint.Name = "lblMailConjoint";
            this.lblMailConjoint.Size = new System.Drawing.Size(32, 13);
            this.lblMailConjoint.TabIndex = 17;
            this.lblMailConjoint.Text = "eMail";
            // 
            // grpBoxResponsables
            // 
            this.grpBoxResponsables.Controls.Add(this.lblCpVille);
            this.grpBoxResponsables.Controls.Add(this.lblMailConjoint);
            this.grpBoxResponsables.Controls.Add(this.lblResponsable);
            this.grpBoxResponsables.Controls.Add(this.lblTelPortConjoint);
            this.grpBoxResponsables.Controls.Add(this.lblAdresse);
            this.grpBoxResponsables.Controls.Add(this.lblConjoint);
            this.grpBoxResponsables.Controls.Add(this.lblTelDom);
            this.grpBoxResponsables.Controls.Add(this.lblMailResp);
            this.grpBoxResponsables.Controls.Add(this.lblTelPortResp);
            this.grpBoxResponsables.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxResponsables.Location = new System.Drawing.Point(979, 304);
            this.grpBoxResponsables.Name = "grpBoxResponsables";
            this.grpBoxResponsables.Size = new System.Drawing.Size(261, 277);
            this.grpBoxResponsables.TabIndex = 18;
            this.grpBoxResponsables.TabStop = false;
            this.grpBoxResponsables.Text = "Responsables";
            // 
            // btnDateDuJour
            // 
            this.btnDateDuJour.Location = new System.Drawing.Point(72, 157);
            this.btnDateDuJour.Name = "btnDateDuJour";
            this.btnDateDuJour.Size = new System.Drawing.Size(132, 23);
            this.btnDateDuJour.TabIndex = 19;
            this.btnDateDuJour.Text = "Insérer date du jour";
            this.btnDateDuJour.UseVisualStyleBackColor = true;
            this.btnDateDuJour.Click += new System.EventHandler(this.btnDateDuJour_Click);
            // 
            // btnImprimer
            // 
            this.btnImprimer.Location = new System.Drawing.Point(780, 157);
            this.btnImprimer.Name = "btnImprimer";
            this.btnImprimer.Size = new System.Drawing.Size(149, 23);
            this.btnImprimer.TabIndex = 20;
            this.btnImprimer.Text = "Imprimer la fiche en PDF";
            this.btnImprimer.UseVisualStyleBackColor = true;
            this.btnImprimer.Click += new System.EventHandler(this.btnImprimer_Click);
            // 
            // Calendrier
            // 
            this.Calendrier.Location = new System.Drawing.Point(232, 157);
            this.Calendrier.Name = "Calendrier";
            this.Calendrier.Size = new System.Drawing.Size(200, 20);
            this.Calendrier.TabIndex = 21;
            this.Calendrier.ValueChanged += new System.EventHandler(this.Calendrier_ValueChanged);
            // 
            // btnImprimerClasse
            // 
            this.btnImprimerClasse.Location = new System.Drawing.Point(780, 114);
            this.btnImprimerClasse.Name = "btnImprimerClasse";
            this.btnImprimerClasse.Size = new System.Drawing.Size(149, 23);
            this.btnImprimerClasse.TabIndex = 22;
            this.btnImprimerClasse.Text = "Imprimer la classe en PDF";
            this.btnImprimerClasse.UseVisualStyleBackColor = true;
            this.btnImprimerClasse.Click += new System.EventHandler(this.btnImprimerClasse_Click);
            // 
            // chkCacherFiches
            // 
            this.chkCacherFiches.AutoSize = true;
            this.chkCacherFiches.Location = new System.Drawing.Point(482, 157);
            this.chkCacherFiches.Name = "chkCacherFiches";
            this.chkCacherFiches.Size = new System.Drawing.Size(144, 17);
            this.chkCacherFiches.TabIndex = 23;
            this.chkCacherFiches.Text = "Cacher les fiches vierges";
            this.chkCacherFiches.UseVisualStyleBackColor = true;
            this.chkCacherFiches.CheckedChanged += new System.EventHandler(this.chkCacherFiches_CheckedChanged);
            // 
            // Principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PapayaWhip;
            this.ClientSize = new System.Drawing.Size(1252, 621);
            this.Controls.Add(this.chkCacherFiches);
            this.Controls.Add(this.btnImprimerClasse);
            this.Controls.Add(this.Calendrier);
            this.Controls.Add(this.btnImprimer);
            this.Controls.Add(this.btnDateDuJour);
            this.Controls.Add(this.grpBoxResponsables);
            this.Controls.Add(this.PhotoLogoCollege);
            this.Controls.Add(this.lblTitre);
            this.Controls.Add(this.lblNomEleve);
            this.Controls.Add(this.lblClasseEleve);
            this.Controls.Add(this.txbInfosEleves);
            this.Controls.Add(this.btnImportPhotos);
            this.Controls.Add(this.PhotoEleve);
            this.Controls.Add(this.cbxEleves);
            this.Controls.Add(this.CbxClasses);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Principal";
            this.Text = "Vie scolaire";
            this.Load += new System.EventHandler(this.OuvertureLogiciel);
            ((System.ComponentModel.ISupportInitialize)(this.PhotoEleve)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PhotoLogoCollege)).EndInit();
            this.grpBoxResponsables.ResumeLayout(false);
            this.grpBoxResponsables.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CbxClasses;
        private System.Windows.Forms.ComboBox cbxEleves;
        private System.Windows.Forms.PictureBox PhotoEleve;
        private System.Windows.Forms.Button btnImportPhotos;
        private System.Windows.Forms.RichTextBox txbInfosEleves;
        private System.Windows.Forms.Label lblNomEleve;
        private System.Windows.Forms.Label lblClasseEleve;
        private System.Windows.Forms.Label lblTitre;
        private System.Windows.Forms.PictureBox PhotoLogoCollege;
        private System.Windows.Forms.Label lblResponsable;
        private System.Windows.Forms.Label lblAdresse;
        private System.Windows.Forms.Label lblCpVille;
        private System.Windows.Forms.Label lblTelDom;
        private System.Windows.Forms.Label lblTelPortResp;
        private System.Windows.Forms.Label lblMailResp;
        private System.Windows.Forms.Label lblConjoint;
        private System.Windows.Forms.Label lblTelPortConjoint;
        private System.Windows.Forms.Label lblMailConjoint;
        private System.Windows.Forms.GroupBox grpBoxResponsables;
        private System.Windows.Forms.Button btnDateDuJour;
        private System.Windows.Forms.Button btnImprimer;
        private System.Windows.Forms.DateTimePicker Calendrier;
        private System.Windows.Forms.Button btnImprimerClasse;
        private System.Windows.Forms.CheckBox chkCacherFiches;
    }
}

