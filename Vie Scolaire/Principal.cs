using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

//using GemBox.Document;
using Color = System.Drawing.Color;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace Vie_Scolaire
{
    public partial class Principal : Form
    {
        public Principal()
        {
            InitializeComponent();
        }

        private readonly OleDbConnection _connexionBdd = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = \\Serveur2008\Apps\Vie scolaire\Viescolaire.accdb");

        //private readonly OleDbConnection _connexionBdd = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = D:\Viescolaire.accdb");
        public int Rowcount;

        public string DateDuJour = DateTime.Now.ToString("dd/MM");

        private void OuvertureLogiciel(object sender, EventArgs e)
        {
            btnImportPhotos.Visible = false;
            //btnMaj.Visible = false;
            progressBar1.Visible = false;
            Compteur.Visible = false;
            CopieRessources();
            if (_connexionBdd.State == ConnectionState.Closed) { _connexionBdd.Open(); }
            string requete;
            if (chkCacherFiches.Checked) { requete = "SELECT DISTINCT Classe FROM Eleves WHERE Infos <> '' OR Classe = '_Toutes les classes_' ORDER BY Classe"; }
            else { requete = "SELECT Classe FROM Classes ORDER BY Classe"; }
            //requete = "SELECT Classe FROM Classes";
            OleDbDataAdapter adapter = new OleDbDataAdapter(requete, _connexionBdd);
            DataTable source = new DataTable();
            adapter.Fill(source);
            CbxClasses.DataSource = source;
            CbxClasses.DisplayMember = "Classe";
            lblAnniversaire.Text = "Anniversaires du jour : ";
            ChercherAnniversaire();
            //CbxClasses.SelectedText = "Toutes les classes";
        }

        private void CbxClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            string requete;

            if (CbxClasses.Text.Contains("Toutes"))
            {
                if (chkCacherFiches.Checked) { requete = "SELECT Eleve, Classe FROM Eleves WHERE Infos <> '' ORDER BY Eleve"; }
                else { requete = "SELECT Eleve, Classe FROM Eleves ORDER BY Eleve"; }
                OleDbDataAdapter adapter = new OleDbDataAdapter(requete, _connexionBdd);
                DataTable source = new DataTable();
                adapter.Fill(source);
                cbxEleves.DataSource = source;
                cbxEleves.DisplayMember = "Eleve";
                var count = cbxEleves.Items.Count;
                lblNombre.Text = count.ToString() + @" élèves";
            }
            else
            {
                if (chkCacherFiches.Checked) { requete = "SELECT Eleve, Classe FROM Eleves WHERE Infos <> '' AND Classe = '" + CbxClasses.Text + "' ORDER BY Eleve "; }
                else { requete = "SELECT Eleve, Classe FROM Eleves WHERE Classe = '" + CbxClasses.Text + "' ORDER BY Eleve "; }
                OleDbDataAdapter adapter = new OleDbDataAdapter(requete, _connexionBdd);
                DataTable source = new DataTable();
                adapter.Fill(source);
                cbxEleves.DataSource = source;
                cbxEleves.DisplayMember = "Eleve";
                var count = cbxEleves.Items.Count;
                lblNombre.Text = count.ToString() + @" élèves";
            }
        }

        private void cbxEleves_SelectedIndexChanged(object sender, EventArgs e)
        {
            NettoyageRenseignements();

            OleDbCommand cmd = new OleDbCommand("SELECT Photo, * FROM Eleves WHERE Eleve = '" + cbxEleves.Text + "'", _connexionBdd);

            try
            {
                var imageByte = (byte[])cmd.ExecuteScalar();
                if (imageByte != null)
                {
                    var memStream = new MemoryStream(imageByte);
                    PhotoEleve.Image = Image.FromStream(memStream);
                }
            }
            catch
            {
                // ignored
            }

            OleDbDataReader reader = cmd.ExecuteReader();
            while (reader != null && reader.Read())
            {
                txbInfosEleves.Text = (reader["Infos"].ToString());
                lblNomEleve.Text = (reader["Eleve"].ToString());
                lblClasseEleve.Text = (reader["Classe"].ToString());
                lblResponsable.Text = (reader["Responsable"].ToString());
                lblAdresse.Text = (reader["Adresse"].ToString());
                lblCpVille.Text = (reader["CP_ville"].ToString());
                lblTelDom.Text = (reader["Tel_domicile"].ToString());
                lblTelPortResp.Text = (reader["Tel_port_resp"].ToString());
                lblMailResp.Text = (reader["Mail_resp"].ToString());
                lblConjoint.Text = (reader["Conjoint"].ToString());
                lblTelPortConjoint.Text = (reader["Tel_port_conjoint"].ToString());
                lblMailConjoint.Text = (reader["Mail_conjoint"].ToString());
            }
        }

        private void BtnImportPhotosClick(object sender, EventArgs e)
        {
            DirectoryInfo d = new DirectoryInfo(@"P:\ALCUIN\Photos\Eleves\SAINT JACQUES\2018-2019");//Assuming Test is your Folder
            FileInfo[] files = d.GetFiles("*.jpg"); //Getting Text files
            foreach (FileInfo file in files)
            {
                string nomFichier = (Path.GetFileNameWithoutExtension(file.Name));
                var photo = File.ReadAllBytes(@"P:\ALCUIN\Photos\Eleves\SAINT JACQUES\2018-2019\" + file.Name);

                OleDbCommand cmd = new OleDbCommand("update Eleves set Photo = @p1 WHERE Eleve= '" + nomFichier + "'", _connexionBdd);
                cmd.Parameters.AddWithValue("@p1", photo);
                cmd.ExecuteNonQuery();
            }
        }

        private void btnImprimer_Click(object sender, EventArgs e)
        {
            var microsoftWord = new Word.Application();
            var chemin = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FicheEleve.docx";
            var fichierWord = microsoftWord.Documents.Add(chemin);
            microsoftWord.Visible = false;

            foreach (Word.Field champs in fichierWord.Fields)
            {
                if (champs.Code.Text.Contains("Nom"))
                {
                    champs.Select();
                    microsoftWord.Selection.TypeText(CultureInfo.InvariantCulture.TextInfo.ToTitleCase(lblNomEleve.Text));
                }
                else if (champs.Code.Text.Contains("Infos"))
                {
                    champs.Select();
                    microsoftWord.Selection.TypeText(txbInfosEleves.Text);
                }
                else if (champs.Code.Text.Contains("Classe"))
                {
                    champs.Select();
                    microsoftWord.Selection.TypeText(lblClasseEleve.Text);
                }
                else if (champs.Code.Text.Contains("Photo"))
                {
                    champs.Select();
                    using (var image = Image.FromFile(@"P:\ALCUIN\Photos\Eleves\SAINT JACQUES\2018-2019\" + lblNomEleve.Text + ".jpg"))
                    using (var newImage = RedimensionnerPhoto(image, 183, 245))
                    {
                        newImage.Save(@"c:\intel\" + lblNomEleve.Text + ".jpg", ImageFormat.Jpeg);
                    }
                    microsoftWord.Selection.InlineShapes.AddPicture(@"c:\intel\" + lblNomEleve.Text + ".jpg");
                }
            }

            fichierWord.SaveAs(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + CultureInfo.InvariantCulture.TextInfo.ToTitleCase(lblNomEleve.Text) + ".docx");
            fichierWord.ExportAsFixedFormat(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + CultureInfo.InvariantCulture.TextInfo.ToTitleCase(lblNomEleve.Text) + ".pdf",
                         Word.WdExportFormat.wdExportFormatPDF);

            fichierWord.Close();
            microsoftWord.Quit();
            GC.Collect();

            microsoftWord.Visible = false;
            ProcessStartInfo pi = new ProcessStartInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + CultureInfo.InvariantCulture.TextInfo.ToTitleCase(lblNomEleve.Text) + ".docx");
            pi.UseShellExecute = true;
            pi.Verb = "print";
            //Process p = Process.Start(pi);
            //if (p != null) p.WaitForExit();
            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" +
            CultureInfo.InvariantCulture.TextInfo.ToTitleCase(lblNomEleve.Text) + ".docx");
            File.Delete(@"c:\Intel\" + lblNomEleve.Text + ".jpg");
            MessageBox.Show(@"Le fichier PDF est enregistré sur ton bureau mon petit...", @"Salut les coyotes !");
            //File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + CultureInfo.InvariantCulture.TextInfo.ToTitleCase(lblNomEleve.Text) + ".pdf");
        }

        private void btnImprimerClasse_Click(object sender, EventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Eleves WHERE Classe = '" + CbxClasses.Text + "'", _connexionBdd);
            OleDbDataReader reader = cmd.ExecuteReader();

            while (reader != null && reader.Read())
            {
                if (reader["Infos"].ToString() != "")
                {
                    txbInfosEleves.Text = (reader["Infos"].ToString());
                    lblNomEleve.Text = (reader["Eleve"].ToString());
                    lblClasseEleve.Text = (reader["Classe"].ToString());
                    lblResponsable.Text = (reader["Responsable"].ToString());
                    lblAdresse.Text = (reader["Adresse"].ToString());
                    lblCpVille.Text = (reader["CP_ville"].ToString());
                    lblTelDom.Text = (reader["Tel_domicile"].ToString());
                    lblTelPortResp.Text = (reader["Tel_port_resp"].ToString());
                    lblMailResp.Text = (reader["Mail_resp"].ToString());
                    lblConjoint.Text = (reader["Conjoint"].ToString());
                    lblTelPortConjoint.Text = (reader["Tel_port_conjoint"].ToString());
                    lblMailConjoint.Text = (reader["Mail_conjoint"].ToString());

                    byte[] productImage = (byte[])reader["Photo"];
                    var memStream = new MemoryStream(productImage);
                    PhotoEleve.Image = Image.FromStream(memStream);

                    var microsoftWord = new Word.Application();
                    var chemin = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FicheEleve.docx";
                    var fichierWord = microsoftWord.Documents.Add(chemin);
                    microsoftWord.Visible = false;

                    foreach (Word.Field champs in fichierWord.Fields)
                    {
                        if (champs.Code.Text.Contains("Nom"))
                        {
                            champs.Select();
                            microsoftWord.Selection.TypeText(
                                CultureInfo.InvariantCulture.TextInfo.ToTitleCase(lblNomEleve.Text));
                        }
                        else if (champs.Code.Text.Contains("Infos"))
                        {
                            champs.Select();
                            microsoftWord.Selection.TypeText(txbInfosEleves.Text);
                        }
                        else if (champs.Code.Text.Contains("Classe"))
                        {
                            champs.Select();
                            microsoftWord.Selection.TypeText(lblClasseEleve.Text);
                        }
                        else if (champs.Code.Text.Contains("Photo"))
                        {
                            champs.Select();
                            using (var image = Image.FromFile(@"P:\ALCUIN\Photos\Eleves\SAINT JACQUES\2018-2019\" + lblNomEleve.Text + ".jpg"))
                            using (var newImage = RedimensionnerPhoto(image, 183, 245))
                            {
                                newImage.Save(@"c:\" + lblNomEleve.Text + ".jpg", ImageFormat.Jpeg);
                            }
                            microsoftWord.Selection.InlineShapes.AddPicture(@"c:\" + lblNomEleve.Text + ".jpg");
                        }
                    }

                    fichierWord.SaveAs(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" +
                                       CultureInfo.InvariantCulture.TextInfo.ToTitleCase(lblNomEleve.Text) + ".docx");
                    fichierWord.ExportAsFixedFormat(
                        Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" +
                        CultureInfo.InvariantCulture.TextInfo.ToTitleCase(lblNomEleve.Text) + ".pdf",
                        Word.WdExportFormat.wdExportFormatPDF);

                    fichierWord.Close();
                    microsoftWord.Quit();
                    GC.Collect();

                    microsoftWord.Visible = true;
                    ProcessStartInfo pi = new ProcessStartInfo(
                        Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" +
                        CultureInfo.InvariantCulture.TextInfo.ToTitleCase(lblNomEleve.Text) + ".docx");
                    pi.UseShellExecute = true;
                    //pi.Verb = "print";
                    //Process p = Process.Start(pi);
                    //if (p != null) p.WaitForExit();
                    //File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" +
                    //CultureInfo.InvariantCulture.TextInfo.ToTitleCase(lblNomEleve.Text) + ".docx");
                    File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" +
                                CultureInfo.InvariantCulture.TextInfo.ToTitleCase(lblNomEleve.Text) + ".pdf");
                    File.Delete(@"c:\" + lblNomEleve.Text + ".jpg");
                }
            }
        }

        private void btnDateDuJour_Click(object sender, EventArgs e)
        {
            if (txbInfosEleves.Text == "") { txbInfosEleves.Text = txbInfosEleves.Text + DateTime.Now.ToString("dddd dd MMMM yyyy") + @" : "; }
            else { txbInfosEleves.Text = txbInfosEleves.Text + Environment.NewLine + Environment.NewLine + DateTime.Now.ToString("dddd dd MMMM yyyy") + @" : "; }
            txbInfosEleves.Select();
            txbInfosEleves.SelectionStart = txbInfosEleves.Text.Length + 1;
        }

        private void btnSelf_Click(object sender, EventArgs e)
        {
            Self self = new Self();
            self.Show();
        }

        private void btnMaj_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            Compteur.Visible = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void Maj_Méthode(object sender, DoWorkEventArgs e)
        {
            //Create COM Objects. Create a COM object for everything that is referenced
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"\\serveur2008\apps\Vie scolaire\Viesco.xls");
            Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            Rowcount = xlRange.Rows.Count;

            for (int i = 5; i <= Rowcount; i++)
            {
                {
                    string cmdStr = "Select count(*) from Eleves where Eleve = '" + xlRange.Cells[i, 1].Value2 + "'"; //get the existence of the record as count

                    OleDbCommand cmd = new OleDbCommand(cmdStr, _connexionBdd);

                    int count = (int)cmd.ExecuteScalar();

                    if (count > 0)
                    {
                        // L'élève existe déjà, on le met à jour
                        OleDbCommand cmd2 = new OleDbCommand("update Eleves set Classe = @p2, Naissance = @p3, Responsable = @p4, Adresse = @p5, CP_Ville = @p6, Tel_Domicile = @p7, Tel_port_resp = @p8, Mail_Resp = @p9, Conjoint = @p10, Tel_port_Conjoint = @p11, Mail_Conjoint = @p12, Flag = @p13, Regime = @p14 WHERE Eleve= '" + xlRange.Cells[i, 1].Value2 + "'", _connexionBdd);
                        cmd2.Parameters.AddWithValue("@p2", xlRange.Cells[i, 2].Value2); //classe
                        cmd2.Parameters.AddWithValue("@p3", xlRange.Cells[i, 3].Value2); //naissance
                        cmd2.Parameters.AddWithValue("@p4", xlRange.Cells[i, 4].Value2); //responsable
                        cmd2.Parameters.AddWithValue("@p5", xlRange.Cells[i, 5].Value2); //adresse
                        cmd2.Parameters.AddWithValue("@p6", xlRange.Cells[i, 6].Value2); //CP
                        if (xlRange.Cells[i, 7].Value2 != null) cmd2.Parameters.AddWithValue("@p7", String.Format("{0:0# ## ## ## ##}", xlRange.Cells[i, 7].Value2)); else cmd2.Parameters.AddWithValue("@p7", "");
                        if (xlRange.Cells[i, 8].Value2 != null) cmd2.Parameters.AddWithValue("@p8", String.Format("{0:0# ## ## ## ##}", xlRange.Cells[i, 8].Value2)); else cmd2.Parameters.AddWithValue("@p8", "");
                        if (xlRange.Cells[i, 9].Value2 != null) cmd2.Parameters.AddWithValue("@p9", xlRange.Cells[i, 9].Value2); else cmd2.Parameters.AddWithValue("@p9", "");//Mail_resp
                        if (xlRange.Cells[i, 10].Value2 != null) cmd2.Parameters.AddWithValue("@p10", xlRange.Cells[i, 10].Value2); else cmd2.Parameters.AddWithValue("@p10", "");//Conjoint
                        if (xlRange.Cells[i, 11].Value2 != null) cmd2.Parameters.AddWithValue("@p11", String.Format("{0:0# ## ## ## ##}", xlRange.Cells[i, 11].Value2)); else cmd2.Parameters.AddWithValue("@p11", "");
                        if (xlRange.Cells[i, 12].Value2 != null) cmd2.Parameters.AddWithValue("@p12", xlRange.Cells[i, 12].Value2); else cmd2.Parameters.AddWithValue("@p12", "");//Mail_conjoint
                        cmd2.Parameters.AddWithValue("@p13", 1);
                        cmd2.Parameters.AddWithValue("@p14", xlRange.Cells[i, 13].Value2);
                        cmd2.ExecuteNonQuery();
                    }
                    else if (count == 0)
                    {
                        //L'élève n'existe pas, on le créé
                        OleDbCommand cmd2 = new OleDbCommand("insert into Eleves (Eleve, Classe, Naissance, Responsable, Adresse, CP_Ville, Tel_Domicile, Tel_port_resp, Mail_Resp, Conjoint, Tel_port_Conjoint, Mail_Conjoint, Flag, Regime) values (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14)", _connexionBdd);
                        cmd2.Parameters.AddWithValue("@p1", xlRange.Cells[i, 1].Value2); //Nom Prénom
                        cmd2.Parameters.AddWithValue("@p2", xlRange.Cells[i, 2].Value2); //classe
                        cmd2.Parameters.AddWithValue("@p3", xlRange.Cells[i, 3].Value2); //naissance
                        cmd2.Parameters.AddWithValue("@p4", xlRange.Cells[i, 4].Value2); //responsable
                        cmd2.Parameters.AddWithValue("@p5", xlRange.Cells[i, 5].Value2); //adresse
                        cmd2.Parameters.AddWithValue("@p6", xlRange.Cells[i, 6].Value2); //CP
                        if (xlRange.Cells[i, 7].Value2 != null) cmd2.Parameters.AddWithValue("@p7", String.Format("{0:0# ## ## ## ##}", xlRange.Cells[i, 7].Value2)); else cmd2.Parameters.AddWithValue("@p7", "");
                        if (xlRange.Cells[i, 8].Value2 != null) cmd2.Parameters.AddWithValue("@p8", String.Format("{0:0# ## ## ## ##}", xlRange.Cells[i, 8].Value2)); else cmd2.Parameters.AddWithValue("@p8", "");
                        if (xlRange.Cells[i, 9].Value2 != null) cmd2.Parameters.AddWithValue("@p9", xlRange.Cells[i, 9].Value2); else cmd2.Parameters.AddWithValue("@p9", "");//Mail_resp
                        if (xlRange.Cells[i, 10].Value2 != null) cmd2.Parameters.AddWithValue("@p10", xlRange.Cells[i, 10].Value2); else cmd2.Parameters.AddWithValue("@p10", "");//Conjoint
                        if (xlRange.Cells[i, 11].Value2 != null) cmd2.Parameters.AddWithValue("@p11", String.Format("{0:0# ## ## ## ##}", xlRange.Cells[i, 11].Value2)); else cmd2.Parameters.AddWithValue("@p11", "");
                        if (xlRange.Cells[i, 12].Value2 != null) cmd2.Parameters.AddWithValue("@p12", xlRange.Cells[i, 12].Value2); else cmd2.Parameters.AddWithValue("@p12", "");//Mail_conjoint
                        cmd2.Parameters.AddWithValue("@p13", 1);
                        cmd2.Parameters.AddWithValue("@p14", xlRange.Cells[i, 13].Value2);
                        cmd2.ExecuteNonQuery();
                    }
                    // Wait 100 milliseconds.
                    //Thread.Sleep(100);
                    // Report progress.
                    backgroundWorker1.ReportProgress(i);
                }
            }
            //L'élève n'est plus au collège, on le supprime
            OleDbCommand cmd3 = new OleDbCommand("DELETE * FROM Eleves WHERE Flag Is Null", _connexionBdd);
            cmd3.ExecuteNonQuery();

            //Réinitialisation du flag
            OleDbCommand cmd4 = new OleDbCommand("update Eleves set Flag = Null", _connexionBdd);
            cmd4.ExecuteNonQuery();

            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);

            //close and release
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);

            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
        }

        private void Maj_Progression(object sender,
            ProgressChangedEventArgs e)
        {
            progressBar1.Maximum = Rowcount;
            int rowCount = Rowcount - 4;
            // Change the value of the ProgressBar to the BackgroundWorker progress.
            progressBar1.Value = e.ProgressPercentage;
            // Set the text.
            Compteur.Text = (e.ProgressPercentage - 4) + @" / " + rowCount;
        }

        private void Maj_Terminé(object sender, RunWorkerCompletedEventArgs e)
        {
            int rowCount = Rowcount - 4;
            progressBar1.Visible = false;
            Compteur.Visible = false;
            MessageBox.Show(@"Opération terminée avec succes !" + Environment.NewLine + rowCount + @" élèves mis à jour");
        }

        private void TxbInfosElevesTextChanged(object sender, EventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand("update Eleves set Infos = @p1 WHERE Eleve= '" + cbxEleves.Text + "'", _connexionBdd);
            cmd.Parameters.AddWithValue("@p1", txbInfosEleves.Text);
            cmd.ExecuteNonQuery();
        }

        private void chkCacherFiches_CheckedChanged(object sender, EventArgs e)
        {
            cbxEleves.Text = "";
            OuvertureLogiciel(sender, e);
            CbxClasses_SelectedIndexChanged(sender, e);
        }

        private void Calendrier_ValueChanged(object sender, EventArgs e)
        {
            Calendrier.Format = DateTimePickerFormat.Custom;
            Calendrier.CustomFormat = @"dddd dd MMMM yyyy";
            if (txbInfosEleves.Text == "") { txbInfosEleves.Text = txbInfosEleves.Text + Calendrier.Text + @" : "; }
            else { txbInfosEleves.Text = txbInfosEleves.Text + Environment.NewLine + Environment.NewLine + Calendrier.Text + @" : "; }
            txbInfosEleves.Select();
            txbInfosEleves.SelectionStart = txbInfosEleves.Text.Length + 1;
        }

        private void CopieRessources()
        {
            var chemin = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FicheEleve.docx";
            if (File.Exists(chemin)) File.Delete(chemin);
            var assembly = Assembly.GetExecutingAssembly();
            var source = assembly.GetManifestResourceStream("Vie_Scolaire.Resources.FicheEleve.docx");
            var destination = File.Open(chemin, FileMode.CreateNew);
            CopieFichiersTypeWord(source, destination);
            source?.Dispose();
            destination.Dispose();
        }

        private void CopieFichiersTypeWord(Stream input, Stream output)
        {
            var buffer = new byte[32768];
            while (true)
            {
                var read = input.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                    return;
                output.Write(buffer, 0, read);
            }
        }

        private static Image RedimensionnerPhoto(Image imgPhoto, int width, int height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent;
            float nPercentW;
            float nPercentH;

            nPercentW = (float)width / sourceWidth;
            nPercentH = height / (float)sourceHeight;
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = Convert.ToInt16((width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = Convert.ToInt16((height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(width, height,
                              PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Red);
            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        private void NettoyageRenseignements()
        {
            foreach (Control label in grpBoxResponsables.Controls)
            {
                label.Text = "";
            }

            PhotoEleve.Image = null;
            lblNomEleve.Text = "";
            lblClasseEleve.Text = "";
        }

        private void ChercherAnniversaire()
        {
            OleDbCommand cmd = new OleDbCommand("SELECT Eleve, Naissance FROM Eleves", _connexionBdd);
            OleDbDataReader reader = cmd.ExecuteReader();
            while (reader != null && reader.Read())
            {
                if (reader["Naissance"].ToString().Contains(DateDuJour))
                {
                    int annéeNaissance = int.Parse(reader["Naissance"].ToString().Substring(6, 2));
                    int annéeEnCours = int.Parse(DateTime.Now.ToString("yy"));
                    int age = annéeEnCours - annéeNaissance;

                    lblAnniversaire.Text = lblAnniversaire.Text + reader["Eleve"] + @" (" +
                                           age + @" ans) - ";
                }
            }
        }
    }
}