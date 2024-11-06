using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SUTAIMES
{
    public partial class FrmShow : Form
    {
        Program tSystem;
        public string ShowStr = "";
        public FrmShow(Program _Sys)
        {
            tSystem=_Sys ;
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.labShow.Text = ShowStr;
        }

        private void butPassGet1_Click(object sender, EventArgs e)
        {
            string KeyStr = clsMyPublic.GetMd5Str("st123");
            string tDate = DateTime.Now.ToString("MMdd");
            if (this.textUserGet1.Text.Length > 0)
            {
                string tUser = clsMyPublic.AESDecrypt(this.textUserGet1.Text, KeyStr).ToUpper();

                int tA1 = tUser.IndexOf(tDate), tA2 = tUser.IndexOf("E");
                string tUserData = "";
                if (tA1 >= 0 & tA2 > 0)
                {
                    tUserData = tUser.Substring(tA1 + tDate.Length, tA2 - tDate.Length - tA1).Trim();
                }
                if (this.textPassGet1.Text.Length > 0)
                {
                    string tPass = clsMyPublic.AESDecrypt(this.textPassGet1.Text, KeyStr).ToUpper();
                    int tB1 = tUser.IndexOf("ST"), tB2 = tUser.IndexOf("E");
                    string tPassData = "";
                    if (tB1 >= 0 & (tB2 > 0))
                    {
                        tPassData = tPass.Substring(tB1 + "ST".Length, tB2 - "ST".Length - tB1).Trim();
                    }
                    int tC1 = 0;
                    if (int.TryParse(tUserData, out tC1) == true & tPassData == tUserData)
                    {
                        string tSqlstr = "";
                        if (tC1 >= 999)
                        {
                            tSqlstr = "update tab_SYS set Addtime ='2099-12-30 23:59:59.453' ";
                        }
                        else
                        {
                            tSqlstr = "update tab_SYS set Addtime ==DATEADD(day,'" + tUserData + "',getdate()) ";
                        }
                        Common.ClsDbAcc clsDbAcc = new Common.ClsDbAcc(tSystem);
                        clsDbAcc.Execute_Command(tSqlstr);
                    }
                }
            }
            this.Close();
        }

        private void labShow_Click(object sender, EventArgs e)
        {
            if(panPass.Visible==false)

            panPass.Visible = true;
            else
                panPass.Visible = false;

        }

        private void FrmShow_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
            this.Hide();
        }

        private void FrmShow_Load(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
        }
    }
}
