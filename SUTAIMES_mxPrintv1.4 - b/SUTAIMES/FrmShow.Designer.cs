namespace SUTAIMES
{
    partial class FrmShow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labShow = new System.Windows.Forms.Label();
            this.butPassGet1 = new System.Windows.Forms.Button();
            this.panPass = new System.Windows.Forms.Panel();
            this.textPassGet1 = new System.Windows.Forms.TextBox();
            this.label160 = new System.Windows.Forms.Label();
            this.textUserGet1 = new System.Windows.Forms.TextBox();
            this.label159 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panPass.SuspendLayout();
            this.SuspendLayout();
            // 
            // labShow
            // 
            this.labShow.AutoSize = true;
            this.labShow.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labShow.ForeColor = System.Drawing.Color.Red;
            this.labShow.Location = new System.Drawing.Point(12, 58);
            this.labShow.Name = "labShow";
            this.labShow.Size = new System.Drawing.Size(20, 20);
            this.labShow.TabIndex = 0;
            this.labShow.Text = "0";
            this.labShow.Click += new System.EventHandler(this.labShow_Click);
            // 
            // butPassGet1
            // 
            this.butPassGet1.Location = new System.Drawing.Point(364, 9);
            this.butPassGet1.Name = "butPassGet1";
            this.butPassGet1.Size = new System.Drawing.Size(111, 62);
            this.butPassGet1.TabIndex = 4;
            this.butPassGet1.Text = "确 定";
            this.butPassGet1.UseVisualStyleBackColor = true;
            this.butPassGet1.Click += new System.EventHandler(this.butPassGet1_Click);
            // 
            // panPass
            // 
            this.panPass.Controls.Add(this.butPassGet1);
            this.panPass.Controls.Add(this.textPassGet1);
            this.panPass.Controls.Add(this.label160);
            this.panPass.Controls.Add(this.textUserGet1);
            this.panPass.Controls.Add(this.label159);
            this.panPass.Location = new System.Drawing.Point(16, 143);
            this.panPass.Name = "panPass";
            this.panPass.Size = new System.Drawing.Size(485, 88);
            this.panPass.TabIndex = 3;
            this.panPass.Visible = false;
            // 
            // textPassGet1
            // 
            this.textPassGet1.Location = new System.Drawing.Point(86, 44);
            this.textPassGet1.Name = "textPassGet1";
            this.textPassGet1.Size = new System.Drawing.Size(272, 21);
            this.textPassGet1.TabIndex = 3;
            // 
            // label160
            // 
            this.label160.AutoSize = true;
            this.label160.Location = new System.Drawing.Point(15, 47);
            this.label160.Name = "label160";
            this.label160.Size = new System.Drawing.Size(41, 12);
            this.label160.TabIndex = 2;
            this.label160.Text = "密码：";
            // 
            // textUserGet1
            // 
            this.textUserGet1.Location = new System.Drawing.Point(86, 9);
            this.textUserGet1.Name = "textUserGet1";
            this.textUserGet1.Size = new System.Drawing.Size(272, 21);
            this.textUserGet1.TabIndex = 1;
            // 
            // label159
            // 
            this.label159.AutoSize = true;
            this.label159.Location = new System.Drawing.Point(15, 12);
            this.label159.Name = "label159";
            this.label159.Size = new System.Drawing.Size(41, 12);
            this.label159.TabIndex = 0;
            this.label159.Text = "用户：";
            // 
            // timer1
            // 
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FrmShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 261);
            this.Controls.Add(this.panPass);
            this.Controls.Add(this.labShow);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmShow";
            this.Text = "提醒";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmShow_FormClosing);
            this.Load += new System.EventHandler(this.FrmShow_Load);
            this.panPass.ResumeLayout(false);
            this.panPass.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labShow;
        private System.Windows.Forms.Button butPassGet1;
        private System.Windows.Forms.Panel panPass;
        private System.Windows.Forms.TextBox textPassGet1;
        private System.Windows.Forms.Label label160;
        private System.Windows.Forms.TextBox textUserGet1;
        private System.Windows.Forms.Label label159;
        private System.Windows.Forms.Timer timer1;
    }
}