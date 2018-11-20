using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using GemBox.Document;
using Color = System.Drawing.Color;
using LoadOptions = System.Xml.Linq.LoadOptions;
using SaveOptions = System.Xml.Linq.SaveOptions;
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

        private void OuvertureLogiciel(object sender, EventArgs e)
        {
            //btnImportPhotos.Visible = false;
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
            //CbxClasses.SelectedText = "Toutes les classes";
        }

        private void CbxClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            string requete;

            if (CbxClasses.Text.Contains("Toutes"))
            {
                if (chkCacherFiches.Checked) { requete = "SELECT Eleve, Classe FROM Eleves WHERE Infos <> ''"; }
                else { requete = "SELECT Eleve, Classe FROM Eleves"; }
                OleDbDataAdapter adapter = new OleDbDataAdapter(requete, _connexionBdd);
                DataTable source = new DataTable();
                adapter.Fill(source);
                cbxEleves.DataSource = source;
                cbxEleves.DisplayMember = "Eleve";
            }
            else
            {
                if (chkCacherFiches.Checked) { requete = "SELECT Eleve, Classe FROM Eleves WHERE Infos <> '' AND Classe = '" + CbxClasses.Text + "'"; }
                else { requete = "SELECT Eleve, Classe FROM Eleves WHERE Classe = '" + CbxClasses.Text + "'"; }
                OleDbDataAdapter adapter = new OleDbDataAdapter(requete, _connexionBdd);
                DataTable source = new DataTable();
                adapter.Fill(source);
                cbxEleves.DataSource = source;
                cbxEleves.DisplayMember = "Eleve";
            }
        }

        private void cbxEleves_SelectedIndexChanged(object sender, EventArgs e)
        {
            NettoyageRenseignements();

            OleDbCommand cmd = new OleDbCommand("SELECT Photo, * FROM Eleves WHERE Eleve = '" + cbxEleves.Text + "'", _connexionBdd);

            var imageByte = (byte[])cmd.ExecuteScalar();

            if (imageByte != null)
            {
                var memStream = new MemoryStream(imageByte);
                PhotoEleve.Image = Image.FromStream(memStream);
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
    }
}