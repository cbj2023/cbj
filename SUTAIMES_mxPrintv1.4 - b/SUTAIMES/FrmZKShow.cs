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
    public partial class FrmZKShow : Form
    {
        public Program tSystem;
        public FrmZKShow(Program tSys)
        {
            InitializeComponent();
        }

        private void FrmZKShow_Load(object sender, EventArgs e)
        {
           
            if (System.Windows.Forms.Screen.AllScreens.Count() > 1)
            {
                System.Windows.Forms.Screen s2 = System.Windows.Forms.Screen.AllScreens[1];
                System.Drawing.Rectangle r2 = s2.WorkingArea;


                this.Top = r2.Top;
                this.Left = r2.Left;
                this.Width = r2.Width;
                this.Height = r2.Height;
                this.Show();
                this.WindowState = FormWindowState.Maximized;
            }
        }
    }
}
