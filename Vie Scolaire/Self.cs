using Microsoft.Office.Interop.Excel;
using System;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Windows.Forms;

namespace Vie_Scolaire
{
    public partial class Self : Form
    {
        public Self()
        {
            InitializeComponent();
        }

        private readonly OleDbConnection _connexionBdd = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = \\Serveur2008\Apps\Vie scolaire\Viescolaire.accdb");

        private void Self_Load(object sender, EventArgs e)
        {
            if (_connexionBdd.State == ConnectionState.Closed) { _connexionBdd.Open(); }

            // Gets the Calendar instance associated with a CultureInfo.
            CultureInfo myCI = new CultureInfo("en-US");
            Calendar myCal = myCI.Calendar;
            // Gets the DTFI properties required by GetWeekOfYear.
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
            int NumSemaine = myCal.GetWeekOfYear(DateTime.Now, myCWR, myFirstDOW);
            int NuméroSemaine = myCal.GetWeekOfYear(DateTime.Now, myCWR, myFirstDOW) % 2;
            if (NuméroSemaine == 0) { lblSemaine.Text = "Nous sommes le " + NomDuJour(DateTime.Now) + " " + DateTime.Now.ToString("d MMMM yyyy") + "  (Semaine " + NumSemaine + ")"; rdBtnPaire.Checked = true; rdBtnPaire.BackColor = System.Drawing.Color.Yellow; rdBtnImpaire.BackColor = System.Drawing.Color.Transparent; }
            else { lblSemaine.Text = "Nous sommes le " + NomDuJour(DateTime.Now) + " " + DateTime.Now.ToString("d MMMM yyyy") + "  (Semaine " + NumSemaine + ")"; rdBtnImpaire.Checked = true; rdBtnImpaire.BackColor = System.Drawing.Color.Yellow; rdBtnPaire.BackColor = System.Drawing.Color.Transparent; }
            chbxClasse(sender, e);
        }

        private void chbxClasse(object sender, EventArgs e)
        {
            if (_connexionBdd.State == ConnectionState.Closed) { _connexionBdd.Open(); }

            foreach (System.Windows.Forms.CheckBox control in gbxClasses.Controls)
            {
                if (control.Checked == true)
                {
                    OleDbCommand cmd1 = new OleDbCommand("SELECT DISTINCT Eleve FROM Eleves WHERE Classe = '" + control.Text + "' ORDER BY Eleve", _connexionBdd);
                    OleDbDataReader reader1 = cmd1.ExecuteReader();
                    while (reader1 != null && reader1.Read())
                    {
                        if ((LbxAbsents.Items.Contains(reader1["Eleve"].ToString()) == true))
                        {
                        }
                        else
                        {
                            LbxAbsents.Items.Add(reader1["Eleve"].ToString());
                            LbxPrésents.Items.Remove(reader1["Eleve"].ToString());
                        }
                    }
                }
                if (control.Checked == false)
                {
                    OleDbCommand cmd2 = new OleDbCommand("SELECT DISTINCT Eleve FROM Eleves WHERE Regime = 'DP5' AND Classe = '" + control.Text + "' ORDER BY Eleve", _connexionBdd);
                    OleDbDataReader reader2 = cmd2.ExecuteReader();
                    while (reader2 != null && reader2.Read())
                    {
                        if ((LbxPrésents.Items.Contains(reader2["Eleve"].ToString()) == true))
                        {
                        }
                        else
                        {
                            LbxPrésents.Items.Add(reader2["Eleve"].ToString());
                            LbxAbsents.Items.Remove(reader2["Eleve"].ToString());
                        }
                    }
                    OleDbCommand cmd3 = new OleDbCommand("SELECT DISTINCT Eleve FROM Eleves WHERE Regime = 'EXT' AND Classe = '" + control.Text + "' ORDER BY Eleve", _connexionBdd);
                    OleDbDataReader reader3 = cmd3.ExecuteReader();
                    while (reader3 != null && reader3.Read())
                    {
                        if ((LbxAbsents.Items.Contains(reader3["Eleve"].ToString()) == true))
                        {
                        }
                        else
                        {
                            LbxAbsents.Items.Add(reader3["Eleve"].ToString());
                            LbxPrésents.Items.Remove(reader3["Eleve"].ToString());
                        }
                    }
                }
            }
            lblAbsents.Text = LbxAbsents.Items.Count.ToString() + @" élèves absents au self";
            lblPrésents.Text = LbxPrésents.Items.Count.ToString() + @" élèves présents au self";
            _connexionBdd.Close();
        }

        private void rdBtnPaireImpaire(object sender, EventArgs e)
        {
            if (rdBtnImpaire.Checked == true && NomDuJour(DateTime.Now) == "mercredi")
            {
                chbx5A.Checked = true;
                chbx5B.Checked = true;
                chbx5C.Checked = true;
                chbx6D.Checked = true;
                chbx6E.Checked = true;
                chbx5D.Checked = false;
                chbx5E.Checked = false;
                chbx6A.Checked = false;
                chbx6B.Checked = false;
                chbx6C.Checked = false;
            }
            if (rdBtnPaire.Checked == true && NomDuJour(DateTime.Now) == "mercredi")
            {
                chbx5A.Checked = false;
                chbx5B.Checked = false;
                chbx5C.Checked = false;
                chbx6D.Checked = false;
                chbx6E.Checked = false;
                chbx5D.Checked = true;
                chbx5E.Checked = true;
                chbx6A.Checked = true;
                chbx6B.Checked = true;
                chbx6C.Checked = true;
            }
        }

        public string NomDuJour(DateTime d)
        {
            return System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.DayNames[(int)d.DayOfWeek];
        }

        private void TraitementFichierAbsents()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = @"\\serveur2008\apps\Vie scolaire\";
            openFileDialog1.Title = @"Fichier des absences";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.DefaultExt = "jpg";
            openFileDialog1.Filter = @"Text files (*.xls)|*.xls|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.ShowReadOnly = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)

            {
                lblFichiersAbsents.Text = openFileDialog1.FileName;
            }

            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            var chemin = lblFichiersAbsents.Text;
            excelApp.Visible = false;

            var workbook = excelApp.Workbooks.Add(chemin);
            var ws = excelApp.Worksheets[1] as Worksheet;

            ws.Columns[1].Delete();

            var usedRange = ws.UsedRange;
            var startRow = usedRange.Row;
            var endRow = startRow + usedRange.Rows.Count - 1;

            for (var row = 2; row <= endRow; row++)
            {
                if ((usedRange.Cells[row, 1].Value) == null)
                {
                    ((Range)ws.Rows[row, Type.Missing]).Delete(XlDeleteShiftDirection.xlShiftUp);
                }
            }

            for (var row = 2; row <= endRow; row++)
            {
                usedRange.Cells[row, 4].Value = usedRange.Cells[row, 1].Value + " " + usedRange.Cells[row, 2].Value;

                if (usedRange.Cells[row, 1].Value != null)
                {
                    if ((LbxAbsents.Items.Contains(usedRange.Cells[row, 4].Value) == true))
                    {
                    }
                    else
                    {
                        LbxAbsents.Items.Add(usedRange.Cells[row, 4].Value);
                        LbxPrésents.Items.Remove(usedRange.Cells[row, 4].Value);
                    }
                }
            }

            lblAbsents.Text = LbxAbsents.Items.Count.ToString() + @" élèves absents au self";
            lblPrésents.Text = LbxPrésents.Items.Count.ToString() + @" élèves présents au self";

            excelApp.DisplayAlerts = false;
            workbook.Close();
            excelApp.Quit();
            GC.Collect();
        }

        private void btnAbsentsDuJour(object sender, EventArgs e)
        {
            TraitementFichierAbsents();
        }
    }
}