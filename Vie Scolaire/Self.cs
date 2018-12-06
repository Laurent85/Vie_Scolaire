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

        public string GetWeekDayName(DateTime d)
        {
            return System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.DayNames[(int)d.DayOfWeek];
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
            int NuméroSemaine = myCal.GetWeekOfYear(DateTime.Now, myCWR, myFirstDOW) % 2;
            if (NuméroSemaine == 0) { lblSemaine.Text = "Nous sommes " + GetWeekDayName(DateTime.Now) +" en semaine PAIRE"; rdBtnPaire.Checked = true; }
            else { lblSemaine.Text = "Nous sommes " + GetWeekDayName(DateTime.Now) +"  en semaine IMPAIRE"; rdBtnImpaire.Checked = true; }
            chbx_CheckedChanged(sender, e);
        }

        private void chbx_CheckedChanged(object sender, EventArgs e)
        {
            if (_connexionBdd.State == ConnectionState.Closed) { _connexionBdd.Open(); }

            foreach (CheckBox control in gbxClasses.Controls)
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

        private void rdBtnImpaire_CheckedChanged(object sender, EventArgs e)
        {
            if (rdBtnImpaire.Checked == true && GetWeekDayName(DateTime.Now) == "mercredi")
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
            if (rdBtnPaire.Checked == true && GetWeekDayName(DateTime.Now) == "mercredi")
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
    }
}