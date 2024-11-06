using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace SUTAIMES
{
    public partial class FrmMain : Form
    {
        Program tSystem;
        GetData.ClsGetPlcAR  tClsGetPlcA;
        GetData.ClsGetPlcBR tClsGetPlcB;
        GetData.ClsGetPlcBYR tClsGetPlcBYR;

        GetData.ClsRetErpData tClsRetErpData;

        GetData.ClsShowFrm tClsShowFrm;
        private delegate void dedataTable(int _Index, DataTable _DataT, string _Text, string _ShowTex);//代理
        private delegate void deShowTexMes(int _Index, string _dShowStr);//代理显示文本

        private delegate void dedataGridView(int _Index, DataGridView _dataGridView, string _Text);//代理不适应

        private delegate void deShowlist(int tIndex, string tpShowStr);//代理

        private Button[] tZKButtonIn=new Button[19];
        private Button[] tMBButIn = new Button[6];
        FrmShow frmShow;
        public FrmMain(Program tSys)
        {
            tSystem = tSys;
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1920, 1030);
        }
        private void AddMBINButton()
        {
            tMBButIn[0] = ButMBINFLOW1; tMBButIn[1] = ButMBINFLOW2; tMBButIn[2] = ButMBINFLOW3; tMBButIn[3] = ButMBINFLOW4;
            tMBButIn[4] = ButMBINFLOW5; tMBButIn[5] = ButMBINFLOW6;
        }
        private void AddINButton()
        {
            tZKButtonIn[0] = this.ButZkINFLOW1; tZKButtonIn[1] = new Button(); tZKButtonIn[2] = this.ButZkINFLOW3; tZKButtonIn[3] = this.ButZkINFLOW4;
            tZKButtonIn[4] = this.ButZkINFLOW5; tZKButtonIn[5] = this.ButZkINFLOW6; tZKButtonIn[6] = this.ButZkINFLOW7; tZKButtonIn[7] = this.ButZkINFLOW8;
            tZKButtonIn[8] = this.ButZkINFLOW9; tZKButtonIn[9] = new Button(); tZKButtonIn[10] = this.ButZkINFLOW11; tZKButtonIn[11] = new Button();
            tZKButtonIn[12] = this.ButZkINFLOW13; tZKButtonIn[13] = this.ButZkINFLOW14; tZKButtonIn[14] = this.ButZkINFLOW15; tZKButtonIn[15] = this.ButZkINFLOW16;
            tZKButtonIn[16] = this.ButZkINFLOW17; tZKButtonIn[17] = this.ButZkINFLOW18; tZKButtonIn[18] = this.butZKFlow;
            for (int i = 0; i < 18; i++)
            {
                tZKButtonIn[i].Click += new EventHandler(ButZKINFlow_Click);
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            this.Text = "速泰MES20220730.0(202220315)Vp1.0" + clsMyPublic.mSYSNAME;
            AddMBINButton(); AddINButton();
            tClsShowFrm = new GetData.ClsShowFrm(tSystem);
            frmShow = new FrmShow(tSystem);
            //this.WindowState = FormWindowState.Maximized;    //最大化窗体 

            //this.tabPageC2.Parent = null;//中空库查询

           

            this.tabPageJ6.Parent = null;

            //this.tabPageJ7.Parent = null;
            //this.tabPageJ8.Parent = null;

            if (clsMyPublic.mYuanAuto == "1")
            {


            }
            else
            {
                this.tabPageJ1.Parent = null;//原片入口
                this.tabPageJ2.Parent = null;
            }

            if (clsMyPublic.mMonitor == "3")
            {
                butZKIFPrint.Visible = true;
                labZKIFPrint.Visible = true;
                butZKProPrint.Visible = true;
                butZKSinglePrint.Visible = true;
            }
            else
            {
                butZKIFPrint.Visible = false ;
                labZKIFPrint.Visible = false ;
                butZKProPrint.Visible = false;
                butZKSinglePrint.Visible = false;
            }
            if (clsMyPublic.mMBauto == "1")
            {
                this.checkMB.Visible = true;
                this.checkMB.Checked = true;
            }
            else if(clsMyPublic.mMBauto == "2")
            {
                this.checkMB.Visible = true;
                this.checkMB.Checked = false ;
            }
            else
            {
                this.checkMB.Visible = false ;

            }
            if (clsMyPublic.mZKauto == "1")
            {
                this.checkZK.Visible = true;
                this.checkZK.Checked = true;
            }
            else if (clsMyPublic.mZKauto == "2")
            {
                this.checkZK.Visible = true;
                this.checkZK.Checked = false;
            }
            else
            {
                this.checkZK.Visible = false;
            }

            this.comLine.Text = clsMyPublic.mLine;
            this.comMBline.Text = clsMyPublic.mLine;

            this.comMBoutExit.Text = clsMyPublic.mMBExit;
            this.texCha4.Text = clsMyPublic.mMBExit;
            this.texOUT6.Text = clsMyPublic.mMBExit;

            this.texZKEnport.Text = clsMyPublic.mZKEnport;
            this.texZKEnportSet.Text = clsMyPublic.mZKEnport;
            if (IfNum(clsMyPublic.mLine))
            {
                tClsShowFrm.mShowMBline = int.Parse(clsMyPublic.mLine);
            }
            

            this.StartPosition = FormStartPosition.CenterScreen;
            this.tabControlMain.SizeMode = TabSizeMode.Fixed;
            int tabWidth = 0,tabPagecount=0;
            tabWidth = this.tabControlMain.Width;
            tabPagecount = this.tabControlMain.TabPages.Count;
            this.tabControlMain.ItemSize = new System.Drawing.Size((tabWidth-5)/ tabPagecount, 25);

            this.tabControlJK.SizeMode = TabSizeMode.Fixed;
            tabWidth = this.tabControlJK.Width;
            tabPagecount = this.tabControlJK.TabPages.Count;
            this.tabControlJK.ItemSize = new System.Drawing.Size((tabWidth - 5) / tabPagecount, 20);

            this.tabControl1.SizeMode = TabSizeMode.Fixed;
            tabWidth = this.tabControl1.Width;
            tabPagecount = this.tabControl1.TabPages.Count;
            this.tabControl1.ItemSize = new System.Drawing.Size((tabWidth - 5) / tabPagecount, 20);

            this.tabControl2.SizeMode = TabSizeMode.Fixed;
            tabWidth = this.tabControl2.Width;
            tabPagecount = this.tabControl2.TabPages.Count;
            this.tabControl2.ItemSize = new System.Drawing.Size((tabWidth - 5) / tabPagecount, 20);


            this.dataGMB1.Tag = this.skinEngine1.DisableTag; labMBShow.Tag = 9999; dataGMB2.Tag = 9999; DataG.Tag = 9999;

            this.skinEngine1 = new Sunisoft.IrisSkin.SkinEngine(((System.ComponentModel.Component)(this)));
            this.skinEngine1.SkinFile = Application.StartupPath + "//MP10.ssk";
            Sunisoft.IrisSkin.SkinEngine se = null;
            se = new Sunisoft.IrisSkin.SkinEngine();
            se.SkinAllForm = true;


            AddPenLab(1,1,this.panelLab1, 29);
            AddPenLab(2, 1, this.panelZKLab1, 29);
          
            AddPanlMBWHBut(1,panMBwh1, 29);
            AddPanlMBWHBut(2, panelZKLab2, 29);

            this.textBox1.Text = this.Size.ToString();

            toolStripStatusLabel2.Text = "";
            toolStripStatusLabel3.Text = "";

            string tStrAA = "";
            tClsGetPlcA = new GetData.ClsGetPlcAR(tSystem);
            if (clsMyPublic.mPLCIFA == "1")
            {
                tStrAA = tClsGetPlcA.OpenThread(9);
                if (tStrAA.Contains("1") == false )
                {
                    toolStripStatusLabel2.Text = "理片仓储PLC连接";
                }
                else
                {
                    toolStripStatusLabel2.Text = "理片仓储PLC断开";
                }
            }
            tClsGetPlcB = new GetData.ClsGetPlcBR(tSystem);
            if (clsMyPublic.mPLCIFB == "1")
            {
                tStrAA = tClsGetPlcB.OpenThread(9);
                if (tStrAA.Contains("1") == false)
                {
                    toolStripStatusLabel3.Text = "中空仓储PLC连接";
                }
                else
                {
                    toolStripStatusLabel3.Text = "中空仓储PLC断开";
                }
            }

            tClsGetPlcBYR = new GetData.ClsGetPlcBYR(tSystem);
            if (clsMyPublic.mPLCIFBY == "1")
            {
                tStrAA = tClsGetPlcBYR.OpenThread(6);
                if (tStrAA.Contains("1") == false)
                {
                    toolStripStatusLabel4.Text = "原片仓储PLC连接";
                }
                else
                {
                    toolStripStatusLabel4.Text = "原片仓储PLC断开";
                }

            }
            if(clsMyPublic.mErpRetIF =="1")
            {
                toolStripStatusLabel5.Text = "ERP上报启动";
            }

            if (tSystem.mClsDBac.mOpenIs == 1)
            {
                toolStripStatusLabel1.Text = "系统连接";
                tClsShowFrm.mPlcStartGet = true;
                
            }
            else
            {
                toolStripStatusLabel1.Text = "系统断开";
            }
            if (clsMyPublic.mErpRetIF == "1")
            {
                tClsRetErpData = new GetData.ClsRetErpData(tSystem);
                tClsRetErpData.OpenThread();
            }

            tabControlMain.SelectedIndex = 0;

            DataTable tDt;
            string tSqlStr = "";
            tSqlStr = string.Concat("SELECT TOP 1[Out_to],[IfPrint] FROM [dbo].[TabZK_OutCmd] where Out_to ='",clsMyPublic.mZKExit,"'");
            tDt = new DataTable();

            if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
            {
                if (tDt.Rows.Count > 0)
                {
                    if (tDt.Rows[0][1].ToString() == "0")
                    {
                        labZKIFPrint.Text = "中空" + clsMyPublic.mZKExit + "线自动打印关闭";
                        butZKIFPrint.Text = "打开自动打印";
                    }
                    else
                    {
                        labZKIFPrint.Text = "中空" + clsMyPublic.mZKExit + "线自动打印打开";
                        butZKIFPrint.Text = "关闭自动打印";
                    }
                }
            }

            if (this.tabPageJ2.Parent != null)//原片仓切割界面
            {

                AddYuPos(1);
                
            }
            combMBIN_Status.Items.Add("");
            tSqlStr = string.Concat("SELECT TOP 1000 [TabName] ,[Status] ,[AssistSatusS] ,[AssistSatusE] ,[StatusName],AreaName FROM [dbo].[StatusNameT] where TabName ='TabMB_Data' and AreaName in ('磨边','公共') order by Status ");
            tDt = new DataTable();
            if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
            {
                if (tDt.Rows.Count > 0)
                {
                    for (int i = 0; i < tDt.Rows.Count; i++)
                    {
                        combMBIN_Status.Items.Add(tDt.Rows[i][4].ToString());
                    }
                }
            }
        }
        private void AddYuPos(int _sysType)
        {
            DataTable tDt;
            string tSqlStr = "";
            comboPos1.Items.Clear(); comboPos2.Items.Clear();
            tSqlStr = string.Concat("SELECT[Pos_NO],PLC_Status,PosType FROM [dbo].[BYuan_WH] a where not exists (select 1 from BYuanTractorCmd b where b.Cmd_Status <14 and (a.Pos_NO=b.To_Num or a.Pos_NO =b.From_Num)) order by Pos_NO ");
            tDt = new DataTable();
            if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
            {
                if (tDt.Rows.Count > 0)
                {
                    for (int i = 0; i < tDt.Rows.Count; i++)
                    {
                        if ((tDt.Rows[i]["PLC_Status"].ToString() == "1" | tDt.Rows[i]["PLC_Status"].ToString() == "2")
                            & (tDt.Rows[i]["PosType"].ToString() != "10"))
                        {
                            comboPos1.Items.Add(tDt.Rows[i]["Pos_NO"].ToString());
                        }
                        else if (tDt.Rows[i]["PLC_Status"].ToString() == "9")
                        {
                            comboPos2.Items.Add(tDt.Rows[i]["Pos_NO"].ToString());
                        }
                    }
                    if (comboPos1.Items.Count > 0)
                    {
                        comboPos1.SelectedIndex = 0;
                    }
                    if (comboPos2.Items.Count > 0)
                    {
                        comboPos2.SelectedIndex = 0;
                    }
                   
                }
                tSqlStr = "SELECT TOP 1000 [sheetid] ,[sheetname] ,[ply] ,[color] ,[unit] ,[sheettype] ,[fmemo] ,[colorno],[Addtime]FROM [BYuan_ErpBasic]";
                tDt = new DataTable();
                if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
                {
                    if (tDt.Rows.Count > 0)
                    {
                        string tTempStr = "";
                        tDt.DefaultView.Sort = "sheettype";//
                        tDt = tDt.DefaultView.ToTable();
                        comYErp1.Items.Clear(); tTempStr = "";
                        for (int i = 0; i < tDt.Rows.Count; i++)
                        {
                            if (tTempStr != tDt.Rows[i]["sheettype"].ToString())
                            {
                                tTempStr = tDt.Rows[i]["sheettype"].ToString();
                                comYErp1.Items.Add(tTempStr);
                            }
                        }

                        tDt.DefaultView.Sort = "sheetname";//
                        tDt = tDt.DefaultView.ToTable();
                        comYErp2.Items.Clear(); tTempStr = "";
                        for (int i = 0; i < tDt.Rows.Count; i++)
                        {
                            if (tTempStr != tDt.Rows[i]["sheetname"].ToString())
                            {
                                tTempStr = tDt.Rows[i]["sheetname"].ToString();
                                comYErp2.Items.Add(tTempStr);
                            }
                        }
                        tDt.DefaultView.Sort = "ply";//
                        tDt = tDt.DefaultView.ToTable();
                        comYErp3.Items.Clear(); tTempStr = "";
                        for (int i = 0; i < tDt.Rows.Count; i++)
                        {
                            if (tTempStr != tDt.Rows[i]["ply"].ToString())
                            {
                                tTempStr = tDt.Rows[i]["ply"].ToString();
                                comYErp3.Items.Add(tTempStr);
                            }
                        }
                    }

                }
            }


            
        }
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("您确定关闭MES系统?", "警告", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                base.Dispose();
                Application.Exit();
                System.Environment.Exit(0);
            }
            else
            {
                return;
            }
        }
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("您确定关闭MES系统?", "警告", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                base.Dispose();
                Application.Exit();
                System.Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;
            }
        }
        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            string PageText = tabControlMain.SelectedTab.Text.Trim();
            switch (PageText)
            {
                case "监控":
                    tClsShowFrm.mPlcStartGet = true  ;//启动刷新监控界面
                    tClsShowFrm.mStart = 0;//启动刷新界面
                    this.tClsShowFrm.mShowIndex = 2;
                    tabControlJK.SelectedIndex = 0;
                    
                    break;
                case "查询":
                    tClsShowFrm.mPlcStartGet = true ;//启动刷新界面
                    tabControl1.SelectedIndex = 0;
                    comboBox1.SelectedIndex = 0;
                    this.tClsShowFrm.mShowIndex = 101;
                    AddPenLine(1,this.panMBwh1, 29);
                    break;
                case "设置":
                    tClsShowFrm.mPlcStartGet = false ;//启动刷新界面
                    comShun.SelectedIndex = 0;
                    tabControl2.SelectedIndex = 0;

                    break;
            }

        }
        private void AddMBcombo(DataTable _DataT)
        {
            comboMB.Items.Clear();
            for (int i = 0; i < _DataT.Rows.Count; i++)
            {
                comboMB.Items.Add(_DataT.Rows[i]["Layout_number"].ToString());
            }
        }

        int mColor = 0;
        DateTime tZKenportTime = DateTime.Now;int tZKenportTag = 0;
        public void ShowdataTable(int _Index, DataTable _DataT, string _Text, string _ShowTex)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    dedataTable sl = new dedataTable(ShowdataTable);
                    this.Invoke(sl, new object[] { _Index, _DataT, _Text,_ShowTex });
                }
                else
                {
                    switch (_Index)
                    {
                       
                        case 1://更新版图
                            AddMBcombo(_DataT);
                            break;
                        case 2://磨边上片当前
                            //if (tButMBstop == true)
                            {
                                dataGMB1.DataSource = _DataT;

                                labMBShow.Text = _Text;
                                texLay.Text = _ShowTex;
                                tClsShowFrm.SetBackColor(dataGMB1, 4,0);
                                if (_DataT.Rows.Count > 0)
                                {
                                    textOptimize.Text =_DataT.Rows[0][7].ToString();
                                }
                                else 
                                {
                                    textOptimize.Text ="";
                                }
                                for (int i = 0; i < this.dataGMB1.Columns.Count; i++)
                                {
                                    dataGMB1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                    this.dataGMB1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                                }
                                ShowQigeiWindow(_DataT);

                                if (checkMB.Checked == true)
                                {
                                    if (_DataT.Select("状态='数据写入' or 状态='初始' or 状态='补片初始'").Length == 0 & _DataT.Rows.Count > 0)
                                    {


                                        if (IfNum(_DataT.Rows[0]["切割版图编号"].ToString()))
                                        {

                                            comboMB.SelectedIndex= int.Parse(_DataT.Rows[0]["切割版图编号"].ToString());
                                        }
                                    }
                                    if (File.Exists(@"C:\SUTAI\MESERP.ini") == true &_DataT.Rows.Count >0)//回馈erp版图
                                    {
                                        clsMyPublic.SetWritePrivateProfileString("SYS", "Optimize_batch", _DataT.Rows[0]["优化单号"].ToString(), @"C:\SUTAI\MESERP.ini");
                                        clsMyPublic.SetWritePrivateProfileString("SYS", "Layout_number", _DataT.Rows[0]["切割版图编号"].ToString(), @"C:\SUTAI\MESERP.ini");
                                    }
                                }
                            }
                            break;
                        case 3://磨边上片上次
                            //if (tButMBstop == true)
                            {
                                dataGMB2.DataSource = _DataT;
                                tClsShowFrm.SetBackColor(dataGMB2, 4, 0);
                                for (int i = 0; i < this.dataGMB2.Columns.Count; i++)
                                {
                                    dataGMB2.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                    this.dataGMB2.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                                }
                            }
                            break;
                        case 4://仓储入库

                            //if (tButMBstop == true)
                            {
                                textMBOPt.Text = _Text;
                                lbTotal.Text = _ShowTex;
                                DataG.DataSource = _DataT;
                                tClsShowFrm.SetBackColor(DataG, 4, 0);
                                for (int i = 0; i < this.DataG.Columns.Count; i++)
                                {
                                    DataG.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                    this.DataG.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                                }
                            }
                            break;
                        case 11:
                            ShowMBINFlow(_DataT);
                            break;
                        case 5://仓储出库

                            //if (tButMBstop == true)
                            {
                                textOptimizeMBOUT.Text = _Text;
                                if (_DataT.Rows.Count > 0) { texProcess.Text = _DataT.Rows[0]["流程卡号"].ToString(); } else { texProcess.Text = ""; }
                                labMBOUT.Text = _ShowTex;
                                dOutGridView.DataSource = _DataT;
                                tClsShowFrm.SetBackColor(dOutGridView, 4, 0);
                                for (int i = 0; i < this.dOutGridView.Columns.Count; i++)
                                {
                                    dOutGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                    this.dOutGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                                }
                            }
                            break;
                        case 30://dataZKIN
                            //if (tButMBstop == true)
                            {
                                if (checkZK.Checked == true)
                                {
                                    if (_DataT.Select(" 状态='中空库'or 状态='准备进中空' or 状态='离开中空' or 状态='入中空未完成' or 状态='出中空未完成' or 状态='准备出中空'or 状态='不符合尺寸'or 状态='破损'").Length == _DataT.Rows.Count & _DataT.Rows.Count > 0
                                            & DateTime.Now.Subtract(tZKenportTime).TotalSeconds > 1 & tZKenportTag == 0)
                                    {
                                        tZKenportTag = 1;
                                        tZKenportTime = DateTime.Now;
                                        string tRtrStr = "", tRetData = "";
                                        tSystem.mClsDBUPdateZK.ExecuteTvpOptimize_batch("Pro_EnPortZKauto", "", "", ref tRtrStr, ref tRetData);
                                        if (tRtrStr.ToUpper() != "Y")
                                        {
                                            tClsShowFrm.mStart = 0;//启动刷新界面
                                        }
                                        tSystem.mClsDBUPdateZK.DBClose();
                                        tZKenportTag = 0;

                                    }
                                }
                                texZKOptimize.Text = _Text;
                                if (_DataT.Rows.Count > 0) { texZKIprocess.Text = _DataT.Rows[0]["流程卡号"].ToString(); }
                                lbTotalZK.Text = _ShowTex;
                                dataZKIN.DataSource = _DataT;
                                tClsShowFrm.SetBackColor(dataZKIN, 4, 0);
                                showZKWindow(_DataT);
                                for (int i = 0; i < this.dataZKIN.Columns.Count; i++)
                                {
                                    dataZKIN.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                    this.dataZKIN.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                                }

                                
                            }
                            break;
                        case 31:
                            ShowZKINFlow(_DataT);
                            break;
                        case 40:
                            //if (tButMBstop == true)
                            {
                                texZKProcess.Text = _Text;
                                if (_DataT.Rows.Count > 0) { texProcess.Text = _DataT.Rows[0]["流程卡号"].ToString(); } else { texProcess.Text = ""; }
                                labZKOUT.Text = _ShowTex;
                                dOutGridViewZK.DataSource = _DataT;
                                tClsShowFrm.SetBackColor(dOutGridViewZK, 4, 0);
                                for (int i = 0; i < this.dOutGridViewZK.Columns.Count; i++)
                                {
                                    dOutGridViewZK.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                    this.dOutGridViewZK.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                                }
                            }
                            break;
                        case 101://仓储出库
                            ShowPanlMBWH(1,panMBwh1, _DataT);
                            break;
                        case 102://仓储出库
                            ShowPanlMBWH(2,panelZKLab2, _DataT);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message + "" + ex.StackTrace);
            }
        }
        private void ShowQigeiWindow(DataTable _DataT)//int _Index ,int _Length ,int _Width,string _ShowStr  成品长度
        {
            DataRow[] tDr1 = _DataT.Select("状态='初始' "); int MaxLong = groupm2.Width-45; int MaxShort = groupM3.Height-45;
            int tSize = MaxShort;
            if (tDr1.Length == 0 | _DataT.Rows.Count ==0) { this.pictureMB1.Visible = false; }
            else if(this.pictureMB1.AccessibleName==textOptimize.Text + tDr1[0]["切割版图编号"].ToString() + "_" + tDr1[0]["上片序号"].ToString())
            {
            }
            else
            {
                this.pictureMB1.Visible = true ;
                this.pictureMB1.Controls.Clear();
                System.Windows.Forms.Button tBtA = new System.Windows.Forms.Button(); int tLong = int.Parse(tDr1[0]["长边"].ToString());
                
                if (MaxShort > tLong) { tSize = tLong; };
                if (IfNum(tDr1[0]["成品长度"].ToString())==true  & int.Parse(tDr1[0]["成品长度"].ToString()) > int.Parse(tDr1[0]["成品宽度"].ToString()))
                {
                    //this.pictureMB1.Width = tSize; int XSizeA = int.Parse(tDr1[0]["成品长度"].ToString());
                    //decimal t1 = decimal.Parse(((float)XSizeA / (float)tSize).ToString("0.000")); //保留3位小数
                    //this.pictureMB1.Height = (int)(int.Parse(tDr1[0]["成品宽度"].ToString()) / t1);
                }
                else if (IfNum(tDr1[0]["成品长度"].ToString()) == true)
                {
                    //this.pictureMB1.Height = tSize; int YSizeA = int.Parse(tDr1[0]["成品宽度"].ToString());
                    //decimal t1 = decimal.Parse(((float)YSizeA / (float)tSize).ToString("0.000")); //保留3位小数
                    //this.pictureMB1.Width = (int)(int.Parse(tDr1[0]["成品长度"].ToString()) / t1);
                }
                else { return; }

                tBtA.Name =  textOptimize.Text + tDr1[0]["切割版图编号"].ToString() + "_" + tDr1[0]["上片序号"].ToString();//(i + 1).ToString() + "@" + tIndex + "@" + (i + 1).ToString() + "@" + (Y + 1 - k).ToString();

                this.pictureMB1.AccessibleName=textOptimize.Text + tDr1[0]["切割版图编号"].ToString() + "_" + tDr1[0]["上片序号"].ToString();
                tBtA.Text = tDr1[0]["切割版图编号"].ToString() + "_" + tDr1[0]["上片序号"].ToString() + ":" + "\r\n" + tDr1[0]["成品长度"].ToString() + "X" + tDr1[0]["成品宽度"].ToString();
                tBtA.Width = this.pictureMB1.Width;
                tBtA.Height = this.pictureMB1.Height;
                tBtA.BackColor = Color.Yellow;
                tBtA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                tBtA.Font = new Font("宋体", 35, label1.Font.Style);
                tBtA.Location = new System.Drawing.Point(0, 0);

                tBtA.Tag = 9999;
                this.pictureMB1.Controls.Add(tBtA);
            }

            DataRow[] tDr2 = _DataT.Select("状态='数据写入' or 状态='磨边已核对'  or 状态='进磨边'  "); 
            if (tDr2.Length == 0) { this.pictureMB2.Visible = false; }
            else if (this.pictureMB2.AccessibleName == textOptimize.Text + tDr2[0]["切割版图编号"].ToString() + "_" + tDr2[0]["上片序号"].ToString())
            {

            }
            else
            {
                this.pictureMB2.Visible = true;
                this.pictureMB2.Controls.Clear();
                System.Windows.Forms.Button tBtB = new System.Windows.Forms.Button(); int tLongB = int.Parse(tDr2[0]["长边"].ToString());
                if (MaxShort > tLongB) { tSize = tLongB; }

                if (IfNum(tDr2[0]["成品长度"].ToString())==true  &  int.Parse(tDr2[0]["成品长度"].ToString()) > int.Parse(tDr2[0]["成品宽度"].ToString()))
                {
                    //this.pictureMB2.Width = tSize; int XSizeB = int.Parse(tDr2[0]["成品长度"].ToString());
                    //decimal t2 = decimal.Parse(((float)XSizeB / (float)tSize).ToString("0.000")); //保留3位小数
                    //this.pictureMB2.Height = (int)(int.Parse(tDr2[0]["成品宽度"].ToString()) / t2);
                }
                else if (IfNum(tDr2[0]["成品长度"].ToString()) == true)
                {
                    //this.pictureMB2.Height = tSize; int YSizeB = int.Parse(tDr2[0]["成品宽度"].ToString());
                    //decimal t2 = decimal.Parse(((float)YSizeB / (float)tSize).ToString("0.000")); //保留3位小数
                    //this.pictureMB2.Width = (int)(int.Parse(tDr2[0]["成品长度"].ToString()) / t2);
                }
                else
                {
                    return;
                }
                tBtB.Name = textOptimize.Text + tDr2[0]["切割版图编号"].ToString() + "_" + tDr2[0]["上片序号"].ToString();//(i + 1).ToString() + "@" + tIndex + "@" + (i + 1).ToString() + "@" + (Y + 1 - k).ToString();
                this.pictureMB2.AccessibleName = textOptimize.Text + tDr2[0]["切割版图编号"].ToString() + "_" + tDr2[0]["上片序号"].ToString();
                tBtB.Text = tDr2[0]["切割版图编号"].ToString() + "_" + tDr2[0]["上片序号"].ToString() + ":" + "\r\n" + tDr2[0]["成品长度"].ToString() + "X" + tDr2[0]["成品宽度"].ToString();
                tBtB.Width = this.pictureMB1.Width;
                tBtB.Height = this.pictureMB1.Height;
                tBtB.BackColor = Color.Aqua;
                tBtB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                tBtB.Font = new Font("宋体", 35, label1.Font.Style);
                tBtB.Location = new System.Drawing.Point(0, 0);

                tBtB.Tag = 9999;
                this.pictureMB2.Controls.Add(tBtB);
            }
            
            
        }
        public void ShowTexMes(int _Index, string _dShowStr)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    deShowTexMes sl = new deShowTexMes(ShowTexMes);
                    this.Invoke(sl, new object[] { _Index, _dShowStr });
                }
                else
                {
                    switch (_Index)
                    {
                        case 1:
                            labMBmod.Text = _dShowStr;
                            break;
                        case 201://切割
                            if (_dShowStr.Length > 0)
                            {
                                if (mColor == 0)
                                {
                                    labMBPoShow.BackColor = Color.Yellow;
                                    labMBPoShow.Text = _dShowStr;
                                    mColor = 1;
                                }
                                else
                                {
                                    labMBPoShow.BackColor = Color.Red;
                                    labMBPoShow.Text = _dShowStr;
                                    mColor = 0;
                                }
                            }
                            else
                            {
                                labMBPoShow.BackColor = Color.Transparent;
                                labMBPoShow.Text = _dShowStr;
                            }
                            break;
                        case 202://磨边入库
                            if (_dShowStr.Length > 0)
                            {
                                if (mColor == 0)
                                {
                                    labMBWHIPoShow.BackColor = Color.Yellow;
                                    labMBWHIPoShow.Text = _dShowStr;
                                    mColor = 1;
                                }
                                else
                                {
                                    labMBWHIPoShow.BackColor = Color.Red;
                                    labMBWHIPoShow.Text = _dShowStr;
                                    mColor = 0;
                                }
                            }
                            else
                            {
                                labMBWHIPoShow.BackColor = Color.Transparent;
                                labMBWHIPoShow.Text = _dShowStr;
                            }
                            break;
                        case 203://磨边出库
                            if (_dShowStr.Length > 0)
                            {
                                if (mColor == 0)
                                {
                                    labMBWHOPoShow.BackColor = Color.Yellow;
                                    labMBWHOPoShow.Text = _dShowStr;
                                    mColor = 1;
                                }
                                else
                                {
                                    labMBWHOPoShow.BackColor = Color.Red;
                                    labMBWHOPoShow.Text = _dShowStr;
                                    mColor = 0;
                                }
                            }
                            else
                            {
                                labMBWHOPoShow.BackColor = Color.Transparent;
                                labMBWHOPoShow.Text = _dShowStr;
                            }
                            break;
                        case 204://中空入库
                            if (_dShowStr.Length > 0)
                            {
                                if (mColor == 0)
                                {
                                    labZKIPoShow.BackColor = Color.Yellow;
                                    labZKIPoShow.Text = _dShowStr;
                                    mColor = 1;
                                }
                                else
                                {
                                    labZKIPoShow.BackColor = Color.Red;
                                    labZKIPoShow.Text = _dShowStr;
                                    mColor = 0;
                                }
                            }
                            else
                            {
                                labZKIPoShow.BackColor = Color.Transparent;
                                labZKIPoShow.Text = _dShowStr;
                            }
                            break;
                        case 205://中空出库
                            if (_dShowStr.Length > 0)
                            {
                                if (mColor == 0)
                                {
                                    labZKOPoShow.BackColor = Color.Yellow;
                                    labZKOPoShow.Text = _dShowStr;
                                    mColor = 1;
                                }
                                else
                                {
                                    labZKOPoShow.BackColor = Color.Red;
                                    labZKOPoShow.Text = _dShowStr;
                                    mColor = 0;
                                }
                            }
                            else
                            {
                                labZKOPoShow.BackColor = Color.Transparent;
                                labZKOPoShow.Text = _dShowStr;
                            }
                            break;
                        case 9999://显示
                            if (frmShow != null)
                            {
                                frmShow.ShowStr = _dShowStr;
                                frmShow.TopMost = true;
                                frmShow.Show();
                            }
                            else
                            {
                                frmShow = new FrmShow(tSystem);
                                frmShow.WindowState = FormWindowState.Normal;
                                frmShow.StartPosition = FormStartPosition.CenterScreen;
                                frmShow.TopMost = true;
                                frmShow.Show();
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "" + ex.StackTrace);
            }
        }

        string tZKWcsid = "";
        private void showZKWindow(DataTable _DataT)
        {
            if (_DataT.Rows.Count > 0)
            {
                
                    tZKWcsid = _DataT.Rows[0]["序号"].ToString();
                    DataRow[] tDr1 = _DataT.Select("状态='离库' "); int MaxLong = groupm2.Width - 45; int MaxShort = pZK1.Height - 45;
                    int tSize = MaxShort;
                    if (tDr1.Length > 0)
                    {
                        System.Windows.Forms.Button tBtA = new System.Windows.Forms.Button(); int tLongB = int.Parse(tDr1[0]["长边"].ToString());

                        tBtA.Name = texZKOptimize.Text + tZKWcsid;//(i + 1).ToString() + "@" + tIndex + "@" + (i + 1).ToString() + "@" + (Y + 1 - k).ToString();

                        tBtA.Text = tDr1[0]["流程卡号"].ToString() + "_" + tDr1[0]["标记"].ToString() + ":" + "\r\n" + tDr1[0]["长边"].ToString() + "X" + tDr1[0]["短边"].ToString() + "\r\n" + tDr1[0]["单片名称"].ToString();
                        tBtA.Width = this.pZK1.Width;
                        tBtA.Height = this.pZK1.Height;
                        tBtA.BackColor = Color.Yellow;
                        tBtA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                        tBtA.Font = new Font("宋体", 35, label1.Font.Style);
                        tBtA.Location = new System.Drawing.Point(0, 0);
                        tBtA.Tag = 9999;
                        this.pZK1.Controls.Clear(); 
                        this.pZK1.Controls.Add(tBtA);
                    }
                    else
                    {
                        this.pZK1.Controls.Clear();
                    }
                
                    DataRow[] tDr2 = _DataT.Select("状态='准备进中空' ","中空入库时间 desc");
                    if (tDr2.Length > 0)
                    {
                        System.Windows.Forms.Button tBtB = new System.Windows.Forms.Button();

                        tBtB.Name = texZKOptimize.Text + tZKWcsid;//(i + 1).ToString() + "@" + tIndex + "@" + (i + 1).ToString() + "@" + (Y + 1 - k).ToString();

                        tBtB.Text = tDr1[0]["流程卡号"].ToString() + "_" + tDr2[0]["标记"].ToString() + ":" + "\r\n" + tDr2[0]["长边"].ToString() + "X" + tDr2[0]["短边"].ToString() + "\r\n" + tDr2[0]["单片名称"].ToString();
                    tBtB.Width = this.pZK1.Width;
                        tBtB.Height = this.pZK1.Height;
                        tBtB.BackColor = Color.Yellow;
                        tBtB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                        tBtB.Font = new Font("宋体", 35, label1.Font.Style);
                        tBtB.Location = new System.Drawing.Point(0, 0);
                        tBtB.Tag = 9999;
                        this.pZK2.Controls.Clear();
                        this.pZK2.Controls.Add(tBtB);
                    }
                    else
                    {
                        this.pZK2.Controls.Clear();
                    }
                
            }
            else
            {
                pZK1.Controls.Clear();
                pZK2.Controls.Clear();
            }
        }

        public void ShowdataGridView(int _Index, DataGridView _dataGridView,string _Text)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    dedataGridView sl = new dedataGridView(ShowdataGridView);
                    this.Invoke(sl, new object[] { _Index, _dataGridView, _Text });
                }
                else
                {
                    switch (_Index)
                    {
                        case 1:

                            break;
                        case 2://磨边上片
                            
                            dataGMB1 = _dataGridView;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "" + ex.StackTrace);
            }
        }

        bool tButMBstop = true;
        private void butMBstoop_Click(object sender, EventArgs e)
        {
            if (this.tButMBstop == true)
            {
                this.butMBstoop.Text = "启动刷新";
                
                tButMBstop = false;
                
            }
            else
            {
                tButMBstop = true; ;
                this.butMBstoop.Text = "停止刷新";
                
                
            }
        }

        private void tabControlJK_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            string tStrAa = tabControlJK.SelectedTab.Text.Trim();
            switch (tStrAa)
            {
                case "磨边上片":
                    tClsShowFrm.mStart = 0;//启动刷新界面
                    this.tClsShowFrm.mShowIndex = 2;
                    break;
                case "磨边仓储入库":
                    tClsShowFrm.mStart = 0;//启动刷新界面
                    this.tClsShowFrm.mShowIndex = 3;
                    break;
                case "磨边仓储出库":
                    tClsShowFrm.mStart = 0;//启动刷新界面
                    this.tClsShowFrm.mShowIndex = 4;
                    break;
                case "中空入口":
                    tClsShowFrm.mStart = 0;//启动刷新界面
                    this.tClsShowFrm.mShowIndex = 30;
                    break;
                case "中空出库":
                    tClsShowFrm.mStart = 0;//启动刷新界面
                    comboBox5.Text = clsMyPublic.mZKExit;
                    this.tClsShowFrm.mShowIndex = 40;
                    break;
            }
        }

        private void dataGMB1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = e.RowIndex;
          
            if (this.dataGMB1.Rows.Count > 1 && rowindex >= 0)
            {
                string value1 = dataGMB1.Rows[rowindex].Cells[0].Value.ToString();
                if (this.dataGMB1.CurrentRow.Index == 0 || this.dataGMB1.CurrentRow.Index == 0)
                {
                
                    texMsIn1.Text = dataGMB1.Rows[0].Cells[8].Value.ToString();
                    texMsIn2.Text = dataGMB1.Rows[0].Cells[0].Value.ToString();
                    texMsIn3.Text = dataGMB1.Rows[0].Cells[5].Value.ToString();
                    texMsIn4.Text = dataGMB1.Rows[0].Cells[3].Value.ToString();
                    texMsIn5.Text = dataGMB1.Rows[0].Cells[2].Value.ToString();
                    texMsIn6.Text = dataGMB1.Rows[0].Cells[6].Value.ToString();
                    texMsIn7.Text = dataGMB1.Rows[0].Cells[7].Value.ToString();
                    texMsIn8.Text = dataGMB1.Rows[0].Cells[4].Value.ToString();

                }
                else
                {

                    texMsIn1.Text = dataGMB1.Rows[rowindex].Cells[8].Value.ToString();
                    texMsIn2.Text = dataGMB1.Rows[rowindex].Cells[0].Value.ToString();
                    texMsIn3.Text = dataGMB1.Rows[rowindex].Cells[5].Value.ToString();
                    texMsIn4.Text = dataGMB1.Rows[rowindex].Cells[3].Value.ToString();
                    texMsIn5.Text = dataGMB1.Rows[rowindex].Cells[2].Value.ToString();
                    texMsIn6.Text = dataGMB1.Rows[rowindex].Cells[6].Value.ToString();
                    texMsIn7.Text = dataGMB1.Rows[rowindex].Cells[7].Value.ToString();
                    texMsIn8.Text = dataGMB1.Rows[rowindex].Cells[4].Value.ToString();
              

                }
            }
        }

        private void dataGMB2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = e.RowIndex;
       
            if (this.dataGMB2.Rows.Count > 1 && rowindex >= 0)
            {
                string value1 = dataGMB2.Rows[rowindex].Cells[0].Value.ToString();
                if (this.dataGMB2.CurrentRow.Index == 0 || this.dataGMB2.CurrentRow.Index == 0)
                {

                    texMsIn1.Text = dataGMB2.Rows[0].Cells[8].Value.ToString();
                    texMsIn2.Text = dataGMB2.Rows[0].Cells[0].Value.ToString();
                    texMsIn3.Text = dataGMB2.Rows[0].Cells[5].Value.ToString();
                    texMsIn4.Text = dataGMB2.Rows[0].Cells[3].Value.ToString();
                    texMsIn5.Text = dataGMB2.Rows[0].Cells[2].Value.ToString();
                    texMsIn6.Text = dataGMB2.Rows[0].Cells[6].Value.ToString();
                    texMsIn7.Text = dataGMB2.Rows[0].Cells[7].Value.ToString();
                    texMsIn8.Text = dataGMB2.Rows[0].Cells[4].Value.ToString();

                }
                else
                {

                    texMsIn1.Text = dataGMB2.Rows[rowindex].Cells[8].Value.ToString();
                    texMsIn2.Text = dataGMB2.Rows[rowindex].Cells[0].Value.ToString();
                    texMsIn3.Text = dataGMB2.Rows[rowindex].Cells[5].Value.ToString();
                    texMsIn4.Text = dataGMB2.Rows[rowindex].Cells[3].Value.ToString();
                    texMsIn5.Text = dataGMB2.Rows[rowindex].Cells[2].Value.ToString();
                    texMsIn6.Text = dataGMB2.Rows[rowindex].Cells[6].Value.ToString();
                    texMsIn7.Text = dataGMB2.Rows[rowindex].Cells[7].Value.ToString();
                    texMsIn8.Text = dataGMB2.Rows[rowindex].Cells[4].Value.ToString();


                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            OpenFileDialog frm = new OpenFileDialog();
            frm.Filter = "Excel文件(*.xls,xlsx)|*.xls;*.xlsx";
            string tFilename = "";
            if (frm.ShowDialog() == DialogResult.OK)
            {
                tFilename = frm.FileName;
              
                DataSet tDS = new DataSet();

                string tRetStr = "";
            
                DataTable trDt = new DataTable();
                trDt = Common.ClsNpoi.ExcelToDataTable(tFilename, true);

                if (trDt.Rows.Count > 0)
                {
                    string _tRetStr = "";
                    List<GetData.ClsGetData.ClsBasicData> tListObj = new List<GetData.ClsGetData.ClsBasicData>();
                    int SingId = 1; string tNowString = DateTime.Now.ToString("yyMMddHHmmss");
                    for (int i = 0; i < trDt.Rows.Count; i++)
                    {
                        for (int k = 0; k < int.Parse(trDt.Rows[i]["数量"].ToString()); k++)
                        {
                            GetData.ClsGetData.ClsBasicData tClsBasic = new GetData.ClsGetData.ClsBasicData();
                            tClsBasic.Optimize_batch = tNowString;
                            tClsBasic.Customer_name = trDt.Rows[i]["客户名称"].ToString();
                            tClsBasic.Order_id =trDt.Rows[i]["流程卡号"].ToString().Substring(0,trDt.Rows[i]["流程卡号"].ToString().IndexOf('.'));
                            tClsBasic.Order_singleid = trDt.Rows[i]["流程卡号"].ToString() + trDt.Rows[i]["订序"].ToString().PadLeft(5, '0') + (k + 1).ToString().PadLeft(3, '0');
                            tClsBasic.Order_length = trDt.Rows[i]["宽"].ToString();
                            tClsBasic.Order_width = trDt.Rows[i]["高"].ToString();

                            tClsBasic.Single_id = SingId.ToString(); SingId = SingId + 1;
                            tClsBasic.Single_tag = trDt.Rows[i]["片标记"].ToString();
                            if (int.Parse(trDt.Rows[i]["宽"].ToString()) > int.Parse(trDt.Rows[i]["高"].ToString()))
                            {
                                tClsBasic.Single_long = trDt.Rows[i]["宽"].ToString();
                                tClsBasic.Single_short = trDt.Rows[i]["高"].ToString();
                            }
                            else
                            {
                                tClsBasic.Single_long = trDt.Rows[i]["高"].ToString();
                                tClsBasic.Single_short = trDt.Rows[i]["宽"].ToString();
                            }
                            tClsBasic.Process_number = trDt.Rows[i]["流程卡号"].ToString();
                            tClsBasic.Single_name = trDt.Rows[i]["单片名"].ToString();
                            DataRow[] tDataRow = trDt.Select(string.Concat("流程卡号='", trDt.Rows[i]["流程卡号"].ToString(), "' and 订序='", trDt.Rows[i]["订序"].ToString(), "'"), "片标记");
                            if (tDataRow.Length > 0)
                            {
                                string tStrAA = "";
                                for (int ii = 0; ii < tDataRow.Length; ii++)
                                {
                                    if (tStrAA.Length == 0)
                                    {
                                        tStrAA =  tDataRow[ii]["片标记"].ToString();
                                    }
                                    else
                                    {
                                        tStrAA = tStrAA +"+"+ tDataRow[ii]["片标记"].ToString();
                                    }
                                }
                                tClsBasic.Single_Str = tStrAA;
                            }
                            

                            tListObj.Add(tClsBasic);

                        }
                    }

                    DataTable tnewDt = GetData.ClsGetData.ToDataTable<GetData.ClsGetData.ClsBasicData>(tListObj);

                    tSystem.mClsDBUPdate.ExecuteTvp("Pro_ToExcel", tnewDt, ref tRetStr);
                    tSystem.mClsDBUPdate.DBClose();
                    labShow.Text = tRetStr;
                 
                }
                else
                {
                    labShow.Text = "导入失败";
                }
            }
        }


        
        private void butQuery1_Click(object sender, EventArgs e)
        {
                 DataTable tDt;
                string tSqlStr = "";
                tSqlStr = string.Concat("SELECT TOP 10000 [Optimize_batch] ,[Sheet_glass_id],[Sheet_glass_name] ,[Sheet_glass_ply]  ,[Sheet_glass_length],[Sheet_glass_width] ,[Sheet_glass_cdname] ,[Sheet_glass_djname] ,[Sheet_glass_Color]  ,[Sheet_glass_NeedType]   ,[Layout_number] ,[LSerial_number]  ,[Single_id],[Single_tag] ,[Single_long] ,[Single_short] ,[Single_name] ,[Single_Str] ,[Process_number],[Edging_type] ,[Order_id],[Order_number] ,[Order_singleid] ,[Order_singlename]   ,[Order_name] ,[Customer_name] ,[Project_name],[Order_length]  ,[Order_width]  ,[Order_type]  ,[Production_type]  ,[Process_count] ,[Technological_process]  ,[Scheduling] FROM [dbo].[TabErp_Data] where Optimize_batch like '%"+this.textCha1.Text +"'  order by addtime desc");
                tDt = new DataTable();

                if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
                {
                    dataGrid1.DataSource = tDt;
                    labShow.Text = tDt.Rows.Count.ToString();
                    for (int i = 0; i < this.dataGrid1.Columns.Count; i++)
                    {
                        dataGrid1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        this.dataGrid1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                    tSystem.mClsDBUPdate.DBClose();
                }
        }
        private void butSetCha_Click(object sender, EventArgs e)
        {

        }
        private void dataGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = e.RowIndex;

            if (this.dataGrid1.Rows.Count > 1 && rowindex >= 0)
            {
                string value1 = dataGrid1.Rows[rowindex].Cells[0].Value.ToString();
                if (this.dataGrid1.CurrentRow.Index == 0 || this.dataGrid1.CurrentRow.Index == 0)
                {

                    textOptimize_batch.Text = dataGrid1.Rows[0].Cells[0].Value.ToString();
              

                }
                else
                {

                    textOptimize_batch.Text = dataGrid1.Rows[rowindex].Cells[0].Value.ToString();
                }
            }
        }

        private void butQuery2_Click(object sender, EventArgs e)
        {
            DataTable tDt;
            string tSqlStr = "";
            tSqlStr = string.Concat("SELECT [Optimize_batch] ,[Layout_number] ,[LSerial_number] ,[L_x_degree],[L_y_degree] ,[Single_id] ,[Single_tag],[Single_long] ,[Single_short] ,[Single_name] ,[Single_Str] ,[Single_Total] ,[Single_Num],[Process_number],[Edging_type],[Order_id] ,[Order_number]  ,[Order_singleid] ,[Order_singlename] ,[Order_name] ,[Customer_name] ,[Project_name],[Order_length]  ,[Order_width],[Order_type]  ,[Production_type] ,[Process_count],[Technological_process] ,[Scheduling] FROM [dbo].[TabMB_Data] where Optimize_batch like '%" + this.textCha1.Text + "'");
            tDt = new DataTable();

            if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
            {
                dataGrid1.DataSource = tDt;

                labShow.Text = tDt.Rows.Count.ToString();
                for (int i = 0; i < this.dataGrid1.Columns.Count; i++)
                {
                    dataGrid1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataGrid1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string tRetStr = "", tRetData = "", tSqlStr = ""; textUserGet1.Text = ""; textPassGet1.Text = ""; texGetShow.Text ="";
            DataTable tdt = new DataTable();
            tSqlStr = "select *,a.Count-DATEDIFF(day,a.Addtime,GETDATE ())'D' from  tab_SYS a where a.Sys_NO =0 and ((DATEDIFF(day,Addtime,GETDATE ())<= a.Count and a.Count!=0)  or a.Count=0)";
            if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlStr, ref tdt) == "")
            {
                if (tdt.Rows.Count > 0)
                {
                    tSystem.mClsDBUPdate.ExecuteTvpOptimize_batch("Pro_Optimize_batch", this.textOptimize_batch.Text.Trim(), comLine.Text.Trim() + "+" + comShun.Text.Trim(), ref tRetStr, ref tRetData);
                    tSystem.mClsDBUPdate.DBClose();
                    textCha1.Text = this.textOptimize_batch.Text.Trim();
                    butQuery2_Click(sender, e);
                    if (tdt.Rows[0]["Count"].ToString() != "0" & int.Parse(tdt.Rows[0]["D"].ToString()) <= 14)
                    {
                        tRetStr = "Y1"; tRetData = tdt.Rows[0]["D"].ToString();
                    }
                }
                else
                {
                    tRetStr = "N";
                }
            }
            if (tRetStr == "N")
            {
                texGetShow.Text = "使用到期，请联系供应商";
                groupPassGet1.Visible = true;
            }
            else if (tRetStr == "Y1")
            {
                texGetShow.Text = "使用期限快到期，请联系供应商! 还剩" + tRetData + " 天，暂时不影响使用";
                groupPassGet1.Visible = true;
            }
 
        }

        public DataTable SortDesc(DataTable dt)
        {
            dt.DefaultView.Sort = "流程卡号 ,订序 ASC";
            dt = dt.DefaultView.ToTable();
            return dt;
        }
        private void butExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog frm = new OpenFileDialog();
            frm.Filter = "Excel文件(*.xls,xlsx)|*.xls;*.xlsx";
            string tFilename = "";
            if (frm.ShowDialog() == DialogResult.OK)
            {
                tFilename = frm.FileName;

                DataSet tDS = new DataSet();

                string tRetStr = "";

                DataTable trDt = new DataTable(); DataTable tNewDt = new DataTable();
                tNewDt = Common.ClsNpoi.ExcelToDataTable(tFilename, true);



                trDt = SortDesc(tNewDt);
                labShow.Text = trDt.Rows.Count.ToString();
                string tStrAA = "";
                if (trDt.Rows.Count > 0)
                {
                    string _tRetStr = "";
                    List<GetData.ClsGetData.ClsErpData> tListObj = new List<GetData.ClsGetData.ClsErpData>();
                    int SingId = 1; string tNowString = DateTime.Now.ToString("yyMMddHHmmss");

                    

                    for (int i = 0; i < trDt.Rows.Count; i++)
                    {
                        
                            GetData.ClsGetData.ClsErpData tClsBasic = new GetData.ClsGetData.ClsErpData();
                            tClsBasic.Optimize_batch = tNowString;
                            tClsBasic.Sheet_glass_id = trDt.Rows[i]["原片序号"].ToString();//可能不是的
                            tClsBasic.Sheet_glass_name = trDt.Rows[i]["单片名称"].ToString();
                            //tClsBasic.Sheet_glass_ply = trDt.Rows[i]["原片长度"].ToString();
                            tClsBasic.Sheet_glass_length = trDt.Rows[i]["原片长度"].ToString();
                            tClsBasic.Sheet_glass_width = trDt.Rows[i]["原片宽度"].ToString();

                            //tClsBasic.Sheet_glass_cdname = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.Sheet_glass_djname = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.Sheet_glass_Color = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.Sheet_glass_NeedType = trDt.Rows[i]["客户名称"].ToString();

                            tClsBasic.Layout_number = trDt.Rows[i]["样图编号"].ToString();
                            tClsBasic.LSerial_number = trDt.Rows[i]["上片序号"].ToString();
                            //tClsBasic.L_x_degree = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.L_y_degree = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.L_x_pos = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.L_y_pos = trDt.Rows[i]["客户名称"].ToString();

                            tClsBasic.Single_id = trDt.Rows[i]["单片成品的全局编号"].ToString();
                            tClsBasic.Single_tag = trDt.Rows[i]["片标记"].ToString();
                            if (int.Parse(trDt.Rows[i]["成品实切长度"].ToString()) > int.Parse(trDt.Rows[i]["成品实切宽度"].ToString()))
                            {
                                tClsBasic.Single_long = trDt.Rows[i]["成品实切长度"].ToString();
                                tClsBasic.Single_short = trDt.Rows[i]["成品实切宽度"].ToString();
                            }
                            else
                            {
                                tClsBasic.Single_long = trDt.Rows[i]["成品实切宽度"].ToString();
                                tClsBasic.Single_short = trDt.Rows[i]["成品实切长度"].ToString();
                            }
                            tClsBasic.Single_name = trDt.Rows[i]["单片名称"].ToString();
                            tClsBasic.Process_number = trDt.Rows[i]["流程卡号"].ToString();
                            if (trDt.Rows[i]["单片名称"].ToString().Contains("白") == true){ tClsBasic.Sing_Type = "1";  } else { tClsBasic.Sing_Type = "2";  }
                            
                            string[] tArr=trDt.Rows[i]["产品名称"].ToString().Split('+');
                            string tStrBB = "";int tgCount=0;
                            for (int k1 = 0;k1< tArr.Length; k1++)
                            {
                                if (tgCount == 0 & tArr[k1].Contains("密") == false & tArr[k1].Contains("结") == false & Convert.ToBoolean(k1 % 2)==false )
                                {
                                    tgCount = 1;
                                    tStrBB = "A";
                                }
                                else if (tgCount == 1 & tArr[k1].Contains("密") == false & tArr[k1].Contains("结") == false & Convert.ToBoolean(k1 % 2) == false)
                                {
                                    tgCount = 2;
                                    tStrBB =tStrBB+ "+B";
                                }
                                else if (tgCount == 2 & tArr[k1].Contains("密") == false & tArr[k1].Contains("结") == false & Convert.ToBoolean(k1 % 2) == false)
                                {
                                    tgCount = 3;
                                    tStrBB = tStrBB + "+C";
                                }
                                else if (tgCount == 3 & tArr[k1].Contains("密") == false & tArr[k1].Contains("结") == false & Convert.ToBoolean(k1 % 2) == false)
                                {
                                    tgCount = 4;
                                    tStrBB = tStrBB + "+D";
                                }
                                else if (tgCount == 4 & tArr[k1].Contains("密") == false & tArr[k1].Contains("结") == false & Convert.ToBoolean(k1 % 2) == false)
                                {
                                    tgCount = 5;
                                    tStrBB = tStrBB + "+E";
                                }
                            }
                            tClsBasic.Single_Str = tStrBB;///////////////////////需要解析数据

                            if (trDt.Rows[i]["磨边类型"].ToString() == "M") { tClsBasic.Edging_type = "2"; } else { tClsBasic.Edging_type = "1"; }
                            tClsBasic.Order_id = trDt.Rows[i]["订单编号"].ToString();
                            tClsBasic.Order_number = trDt.Rows[i]["成品切出序号"].ToString();

                            if (tStrAA != trDt.Rows[i]["流程卡号"].ToString().Trim() + trDt.Rows[i]["片标记"].ToString().Trim() + trDt.Rows[i]["订序"].ToString().Trim())
                            {
                                SingId = 1;
                                tStrAA = trDt.Rows[i]["流程卡号"].ToString().Trim() + trDt.Rows[i]["片标记"].ToString().Trim() + trDt.Rows[i]["订序"].ToString().Trim();
                                tClsBasic.Order_singleid = trDt.Rows[i]["流程卡号"].ToString() + trDt.Rows[i]["订序"].ToString().PadLeft(5, '0') + SingId.ToString().PadLeft(3, '0');//////注意
                            }
                            else
                            {
                                SingId = 1+1;
                            
                                tClsBasic.Order_singleid = trDt.Rows[i]["流程卡号"].ToString() + trDt.Rows[i]["订序"].ToString().PadLeft(5, '0') + SingId.ToString().PadLeft(3, '0');//////注意
                            }
                            //tClsBasic.Order_singleid = trDt.Rows[i]["客户名称"].ToString();

                            tClsBasic.Order_singlename = trDt.Rows[i]["成品名称"].ToString();
                            tClsBasic.Order_name = trDt.Rows[i]["客户名称"].ToString();
                            tClsBasic.Customer_name = trDt.Rows[i]["客户名称"].ToString();
                            tClsBasic.Project_name = trDt.Rows[i]["客户名称"].ToString();
                            tClsBasic.Order_length = trDt.Rows[i]["成品长度"].ToString();
                            tClsBasic.Order_width = trDt.Rows[i]["成品宽度"].ToString();
                            //tClsBasic.Order_type = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.Production_type = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.Process_count = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.Technological_process = trDt.Rows[i]["成品名称"].ToString();
                            //tClsBasic.Scheduling = trDt.Rows[i]["成品名称"].ToString();
                            //tClsBasic.Label_tag = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.Label_pos = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.Label_type = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.Label_name = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.Label_size = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.Label_color = trDt.Rows[i]["客户名称"].ToString();

                            //tClsBasic.Hollow_length = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.Hollow_width = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.Hollow_shelfno = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.Hollow_shelfcount = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.Hollow_qscode = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.Hollow_wh = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.Print_tag = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.Print_pos = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.Print_name = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.spare1 = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.spare2 = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.spare3 = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.spare4 = trDt.Rows[i]["客户名称"].ToString();
                            //tClsBasic.spare5 = trDt.Rows[i]["客户名称"].ToString();

                            tListObj.Add(tClsBasic);
                    }

                    DataTable tnewToDt = GetData.ClsGetData.ToDataTable<GetData.ClsGetData.ClsErpData>(tListObj);

                    tSystem.mClsDBUPdate.ExecuteTvp("Pro_ToExcelB", tnewToDt, ref tRetStr);
                    tSystem.mClsDBUPdate.DBClose();
                    labShow.Text = tRetStr;

                }
                else
                {
                    labShow.Text = "导入失败";
                }
            }

        }

        

        private void DataG_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = e.RowIndex;

            if (this.DataG.Rows.Count > 1 && rowindex >= 0)
            {
                
                if (this.DataG.CurrentRow.Index == 0 )
                {

                    texMs1.Text = DataG.Rows[0].Cells[8].Value.ToString();
                    texMs2.Text = DataG.Rows[0].Cells[0].Value.ToString();
                    texMs12.Text = DataG.Rows[0].Cells[5].Value.ToString();
                    texMs4.Text = DataG.Rows[0].Cells[3].Value.ToString();
                    texMs5.Text = DataG.Rows[0].Cells[2].Value.ToString();
                    texMs6.Text = DataG.Rows[0].Cells[6].Value.ToString();
                    texMs7.Text = DataG.Rows[0].Cells[7].Value.ToString();
                    texMs8.Text = DataG.Rows[0].Cells[4].Value.ToString();

                }
                else
                {

                    texMs1.Text = DataG.Rows[rowindex].Cells[8].Value.ToString();
                    texMs2.Text = DataG.Rows[rowindex].Cells[0].Value.ToString();
                    texMs12.Text = DataG.Rows[rowindex].Cells[5].Value.ToString();
                    texMs4.Text = DataG.Rows[rowindex].Cells[3].Value.ToString();
                    texMs5.Text = DataG.Rows[rowindex].Cells[2].Value.ToString();
                    texMs6.Text = DataG.Rows[rowindex].Cells[6].Value.ToString();
                    texMs7.Text = DataG.Rows[rowindex].Cells[7].Value.ToString();
                    texMs8.Text = DataG.Rows[rowindex].Cells[4].Value.ToString();


                }
            }
        }
        private void dOutGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = e.RowIndex;

            if (this.dOutGridView.Rows.Count > 1 && rowindex >= 0)
            {

                if (this.dOutGridView.CurrentRow.Index == 0)
                {

                    texOMs1.Text = dOutGridView.Rows[0].Cells[8].Value.ToString();
                    texOMs2.Text = dOutGridView.Rows[0].Cells[0].Value.ToString();
                    texOMs3.Text = dOutGridView.Rows[0].Cells[5].Value.ToString();
                    texOMs4.Text = dOutGridView.Rows[0].Cells[3].Value.ToString();
                    texOMs5.Text = dOutGridView.Rows[0].Cells[2].Value.ToString();
                    texOMs6.Text = dOutGridView.Rows[0].Cells[6].Value.ToString();
                    texOMs7.Text = dOutGridView.Rows[0].Cells[7].Value.ToString();
                    texOMs8.Text = dOutGridView.Rows[0].Cells[4].Value.ToString();

                }
                else
                {

                    texOMs1.Text = dOutGridView.Rows[rowindex].Cells[8].Value.ToString();
                    texOMs2.Text = dOutGridView.Rows[rowindex].Cells[0].Value.ToString();
                    texOMs3.Text = dOutGridView.Rows[rowindex].Cells[5].Value.ToString();
                    texOMs4.Text = dOutGridView.Rows[rowindex].Cells[3].Value.ToString();
                    texOMs5.Text = dOutGridView.Rows[rowindex].Cells[2].Value.ToString();
                    texOMs6.Text = dOutGridView.Rows[rowindex].Cells[6].Value.ToString();
                    texOMs7.Text = dOutGridView.Rows[rowindex].Cells[7].Value.ToString();
                    texOMs8.Text = dOutGridView.Rows[rowindex].Cells[4].Value.ToString();


                }
            }
        }
        private bool IfNum(string _Data)
        {
            try
            {
                int aa = int.Parse(_Data);
                return true;
            }
            catch
            {
                return false;
            }
        }
        int mMBMBSelectIndex = 0;
        private void butProMBOUB_Click(object sender, EventArgs e)
        {
            mMBMBSelectIndex = 0;
            string tSqlStr = "";
            DataTable tDt = new DataTable();
            Common.ClsDbAcc tDbAccSet = new Common.ClsDbAcc(tSystem);
            tSqlStr = string.Concat("  SELECT  TOP 2000  a.Optimize_batch  '优化单号',a.Process_number  '架号',a.[Out_to] '出口',a.ProcessNO_Count 流程卡数量,case when status=1 then '在出库'when status=2 then '出库完成' when a.In_Count=0 then '未入库' when a.ProcessNO_Count=In_Count+Err_Size_Count or a.ProcessNO_Count*2=In_Count+Err_Size_Count then '入库完成'when status=3 then '强制出库'  else '未入库完成' end '当前状态',a.Order_id '订单号',Single_name '单片玻璃名称',case when Priority='1' then '优先出'else CONVERT(varchar(10), Priority) end  优先级,In_Count 已入数量,a.Addtime '开始时间',Scheduling '工艺路线'  FROM tabMB_Process a"
                      + " where a.Optimize_batch like '%", texOUT1.Text.Trim(), "' and a.Process_number like '%", texOUT2.Text.Trim(), "' and a.Order_id like '%", texOUT3.Text.Trim(), "' order by  case when DATEDIFF( DAY, a.Addtime ,getdate())<3 then 0 else 1 end, a.Optimize_batch ,a.Process_number  ");
            if (tDbAccSet.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
            {
                dataProOut.DataSource = tDt;

                tClsShowFrm.SetBackColor(dataProOut, 4, 0);
                for (int i = 0; i < this.dataProOut.ColumnCount; i++)
                {
                    dataProOut.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataProOut.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                tSystem.mClsDBUPdate.DBClose();
            }
        }
        private void butWHProMBOUB_Click(object sender, EventArgs e)//库内有玻璃流程卡
        {
            mMBMBSelectIndex = 0;
            string tSqlStr = "";
            DataTable tDt = new DataTable();
            Common.ClsDbAcc tDbAccSet = new Common.ClsDbAcc(tSystem);
            tSqlStr = string.Concat("  SELECT  TOP 2000  a.Optimize_batch  '优化单号',a.Process_number  '架号',a.[Out_to] '出口',a.ProcessNO_Count 流程卡数量,case when status=1 then '在出库'when status=2 then '出库完成' when a.In_Count=0 then '未入库' when a.ProcessNO_Count=In_Count+Err_Size_Count or a.ProcessNO_Count*2=In_Count+Err_Size_Count then '入库完成'when status=3 then '强制出库'  else '未入库完成' end '当前状态',a.Order_id '订单号',Single_name '单片玻璃名称',case when Priority='1' then '优先出'else CONVERT(varchar(10), Priority) end  优先级,In_Count 已入数量,a.Addtime '开始时间',Scheduling '工艺路线'  FROM tabMB_Process a"
                      + " where a.Optimize_batch like '%", texOUT1.Text.Trim(), "' and a.Process_number like '%", texOUT2.Text.Trim(), "' and a.Order_id like '%", texOUT3.Text.Trim(), "' "
                      + " and exists (select 1 from TabMB_Data b where a.Process_number =b.Process_number and b.Optimize_batch=a.Optimize_batch and b.Status =14) "
                      +" order by  case when DATEDIFF( DAY, a.Addtime ,getdate())<3 then 0 else 1 end, a.Optimize_batch ,a.Process_number  "
                      );
            if (tDbAccSet.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
            {
                dataProOut.DataSource = tDt;

                tClsShowFrm.SetBackColor(dataProOut, 4, 0);
                for (int i = 0; i < this.dataProOut.ColumnCount; i++)
                {
                    dataProOut.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataProOut.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                tSystem.mClsDBUPdate.DBClose();
            }
        }


        private void button27_Click(object sender, EventArgs e)//订单查询
        {
            mMBMBSelectIndex = 1;
            string tSqlStr = "";
            DataTable tDt = new DataTable();
            Common.ClsDbAcc tDbAccSet = new Common.ClsDbAcc(tSystem);
            tSqlStr = string.Concat("SELECT TOP 2000 [Optimize_batch]  '优化单号' ,[Order_id] '订单号名称' ,[Single_name] '单片玻璃名称'"
               + ",case when status=1 then '在出库'when status=2 then '出库完成' when a.Order_id_ActCount=0 then '未入库' when a.Order_id_ActCount=Order_id_Count+Err_Size_Count then '入库完成'when status=3 then '强制出库'  else '未入库完成' end '当前状态',[Order_id_Count] '订单数量' ,[Order_id_ActCount]'已入数量'  ,[Status]  ,[Addtime],Scheduling '工艺路线' FROM [dbo].[tabMB_Order] a"
               + " where a.Optimize_batch like '%", texOUT1.Text.Trim(), "' and a.Order_id like '%", texOUT3.Text.Trim(), "' order by a.Optimize_batch ,a.Order_id");
            if (tDbAccSet.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
            {
                dataProOut.DataSource = tDt;

                tClsShowFrm.SetBackColor(dataProOut, 3, 0);
                for (int i = 0; i < this.dataProOut.ColumnCount; i++)
                {
                    dataProOut.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataProOut.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                tSystem.mClsDBUPdate.DBClose();
            }
        }
        private void dataProOut_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = e.RowIndex;

            if (this.dataProOut.Rows.Count > 1 && rowindex >= 0)
            {
                string value1 = dataProOut.Rows[rowindex].Cells[0].Value.ToString();
                if (this.dataProOut.CurrentRow.Index == 0 )
                {
                    if (mMBMBSelectIndex == 0)
                    {

                        texOUT1.Text = dataProOut.Rows[0].Cells[0].Value.ToString();
                        texOUT2.Text = dataProOut.Rows[0].Cells[1].Value.ToString();
                        texOUT3.Text = dataProOut.Rows[0].Cells[5].Value.ToString();
                        texOUT4.Text = dataProOut.Rows[0].Cells[6].Value.ToString();
                        texOUT5.Text = dataProOut.Rows[0].Cells[3].Value.ToString();
                        texOUT6.Text = clsMyPublic.mMBExit ;
                    }
                    else
                    {
                        texOUT1.Text = dataProOut.Rows[0].Cells[0].Value.ToString();
                        texOUT2.Text = "";
                        texOUT3.Text = dataProOut.Rows[0].Cells[1].Value.ToString();
                        texOUT4.Text = dataProOut.Rows[0].Cells[2].Value.ToString();
                        texOUT5.Text = dataProOut.Rows[0].Cells[4].Value.ToString();
                        texOUT6.Text = clsMyPublic.mMBExit;
                    }
                 

                }
                else
                {
                    if (mMBMBSelectIndex == 0)
                    {

                        texOUT1.Text = dataProOut.Rows[rowindex].Cells[0].Value.ToString();
                        texOUT2.Text = dataProOut.Rows[rowindex].Cells[1].Value.ToString();
                        texOUT3.Text = dataProOut.Rows[rowindex].Cells[5].Value.ToString();
                        texOUT4.Text = dataProOut.Rows[rowindex].Cells[6].Value.ToString();
                        texOUT5.Text = dataProOut.Rows[rowindex].Cells[3].Value.ToString();
                        texOUT6.Text = clsMyPublic.mMBExit;
                    }
                    else
                    {
                        texOUT1.Text = dataProOut.Rows[rowindex].Cells[0].Value.ToString();
                        texOUT2.Text = "";
                        texOUT3.Text = dataProOut.Rows[rowindex].Cells[1].Value.ToString();
                        texOUT4.Text = dataProOut.Rows[rowindex].Cells[2].Value.ToString();
                        texOUT5.Text = dataProOut.Rows[rowindex].Cells[4].Value.ToString();
                        texOUT6.Text = clsMyPublic.mMBExit;
                    }
                }
            }
        }

        private void button54_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("    您确定对磨边仓储 优化单号： " + texOUT1.Text + "  流程卡号 " + texOUT2.Text + "进行按流程卡号出库?   \r\n   请看清流程卡号 ", "警告", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tSqlStr = "";
                DataTable tDt = new DataTable();
                Common.ClsDbAcc tDbAccSet = new Common.ClsDbAcc(tSystem);
                //tSqlStr = string.Concat("delete tabMBWHOut where Optimize_batch='", texOUT1.Text.Trim(), "' and  Process_number='", texOUT2.Text.Trim(), "'  "
                //       + " insert into tabMBWHOut(Optimize_batch ,Process_number,status,Out_Type,Order_id ,Single_name,ProcessNO_Count) values('", texOUT1.Text.Trim(), "','", texOUT2.Text.Trim(), "','0','0','", texOUT3.Text.Trim(), "','", texOUT4.Text.Trim(), "','", texOUT5.Text.Trim(), "')"
                //       + " update tabMB_Process set status=1 where Optimize_batch='", texOUT1.Text.Trim(), "' and Process_number='", texOUT2.Text.Trim(), "'"
                //       + " update [tabMB_Order] set status=1 where Optimize_batch='", texOUT1.Text.Trim(), "' and Order_id='", texOUT3.Text.Trim(), "'");
                //tDbAccSet.Execute_Command(tSqlStr);
                string tData = ""; string RetData = "", RetStr = "";
                tData = texOUT1.Text + "+" + texOUT2.Text.Trim() + "+" + texOUT3.Text.Trim() + "+" + texOUT6.Text.Trim();
                DataSet dataSet = new DataSet();
                dataSet = tDbAccSet.ExecuteDisposeStatus("Pro_OUTSET", "1", tData, ref RetData, ref RetStr);
                if (dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[dataSet.Tables.Count - 1].Rows.Count >0 && dataSet.Tables[dataSet.Tables.Count - 1].Columns.Count >4)
                    {

                    }
                }
                tDbAccSet.DBClose();
            }
        }
        private void butMBfanout_Click(object sender, EventArgs e)//反序出库
        {
            DialogResult dr = MessageBox.Show("    您确定对磨边仓储 优化单号： " + texOUT1.Text + "  流程卡号 " + texOUT2.Text + "进行按流程卡号出库?   \r\n   请看清流程卡号 ", "警告", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tSqlStr = "";
                DataTable tDt = new DataTable();
                Common.ClsDbAcc tDbAccSet = new Common.ClsDbAcc(tSystem);
                //tSqlStr = string.Concat("delete tabMBWHOut where Optimize_batch='", texOUT1.Text.Trim(), "' and  Process_number='", texOUT2.Text.Trim(), "'  "
                //       + " insert into tabMBWHOut(Optimize_batch ,Process_number,status,Out_Type,Order_id ,Single_name,ProcessNO_Count) values('", texOUT1.Text.Trim(), "','", texOUT2.Text.Trim(), "','0','0','", texOUT3.Text.Trim(), "','", texOUT4.Text.Trim(), "','", texOUT5.Text.Trim(), "')"
                //       + " update tabMB_Process set status=1 where Optimize_batch='", texOUT1.Text.Trim(), "' and Process_number='", texOUT2.Text.Trim(), "'"
                //       + " update [tabMB_Order] set status=1 where Optimize_batch='", texOUT1.Text.Trim(), "' and Order_id='", texOUT3.Text.Trim(), "'");
                //tDbAccSet.Execute_Command(tSqlStr);
                string tData = ""; string RetData = "", RetStr = "";
                tData = texOUT1.Text + "+" + texOUT2.Text.Trim() + "+" + texOUT3.Text.Trim() + "+" + texOUT6.Text.Trim();
                DataSet dataSet = new DataSet();
                dataSet = tDbAccSet.ExecuteDisposeStatus("Pro_OUTSET", "2", tData, ref RetData, ref RetStr);
                if (dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[dataSet.Tables.Count - 1].Rows.Count > 0 && dataSet.Tables[dataSet.Tables.Count - 1].Columns.Count > 4)
                    {

                    }
                }
                tDbAccSet.DBClose();
            }
        }
        private void butMBOUTCMD_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  不进中空库  您确定对磨边仓储 优化单号： " + texOUT1.Text + "  流程卡号 " + texOUT2.Text + "进行按流程卡号出库?   \r\n   请看清流程卡号 ", "警告", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tSqlStr = "";
                DataTable tDt = new DataTable();
                Common.ClsDbAcc tDbAccSet = new Common.ClsDbAcc(tSystem);
                //tSqlStr = string.Concat("delete tabMBWHOut where Optimize_batch='", texOUT1.Text.Trim(), "' and  Process_number='", texOUT2.Text.Trim(), "'  "
                //       + " insert into tabMBWHOut(Optimize_batch ,Process_number,status,Out_Type,Order_id ,Single_name,ProcessNO_Count) values('", texOUT1.Text.Trim(), "','", texOUT2.Text.Trim(), "','0','0','", texOUT3.Text.Trim(), "','", texOUT4.Text.Trim(), "','", texOUT5.Text.Trim(), "')"
                //       + " update tabMB_Process set status=1 where Optimize_batch='", texOUT1.Text.Trim(), "' and Process_number='", texOUT2.Text.Trim(), "'"
                //       + " update [tabMB_Order] set status=1 where Optimize_batch='", texOUT1.Text.Trim(), "' and Order_id='", texOUT3.Text.Trim(), "'");
                //tDbAccSet.Execute_Command(tSqlStr);
                string tData = ""; string RetData = "", RetStr = "";
                tData = texOUT1.Text + "+" + texOUT2.Text.Trim() + "+" + texOUT3.Text.Trim() + "+" + texOUT6.Text.Trim();
                DataSet dataSet = new DataSet();
                dataSet = tDbAccSet.ExecuteDisposeStatus("Pro_OUTSET", "1", tData, ref RetData, ref RetStr);
                if (dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[dataSet.Tables.Count - 1].Rows.Count > 0 && dataSet.Tables[dataSet.Tables.Count - 1].Columns.Count > 4)
                    {

                    }
                }
                tDbAccSet.DBClose();
            }
        }
        private void butMBordout_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("    您确定对磨边仓储 优化单号： " + texOUT1.Text + "  流程卡号 " + texOUT3.Text + "进行订单号出库?   \r\n   请看清订单号 ", "警告", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tSqlStr = "";
                DataTable tDt = new DataTable();
                Common.ClsDbAcc tDbAccSet = new Common.ClsDbAcc(tSystem);
                //tSqlStr = string.Concat(" delete tabMBWHOut  where  Optimize_batch='", texOUT1.Text.Trim(), "' and  Order_id='", texOUT3.Text.Trim(), "' and Out_Type='10' "
                //       + "  insert into tabMBWHOut(Optimize_batch ,Order_id,status,Out_Type,Process_number,Single_name,ProcessNO_Count ) values('", texOUT1.Text.Trim(), "','", texOUT3.Text.Trim(), "','0','10','','", texOUT4.Text.Trim(), "','", texOUT5.Text.Trim(), "')"
                //       + " update [tabMB_Order] set status=1 where Optimize_batch='", texOUT1.Text.Trim(), "' and Order_id='", texOUT3.Text.Trim(), "'");

                string tData = ""; string RetData = "", RetStr = "";
                tData = texOUT1.Text + "+" + texOUT2.Text.Trim() + "+" + texOUT3.Text.Trim();
                DataSet dataSet = new DataSet();
                dataSet = tDbAccSet.ExecuteDisposeStatus("Pro_OUTSET", "10", tData, ref RetData, ref RetStr);
                if (dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[dataSet.Tables.Count - 1].Rows.Count > 0 && dataSet.Tables[dataSet.Tables.Count - 1].Columns.Count > 4)
                    {

                    }
                }
                tDbAccSet.Execute_Command(tSqlStr);
            }
        }
        private void butMBOPTout_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("    您确定对磨边仓储 优化单号： " + texOUT1.Text + "  流程卡号 " + texOUT3.Text + "进行订单号出库?   \r\n   请看清订单号 ", "警告", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tSqlStr = "";
                DataTable tDt = new DataTable();
                Common.ClsDbAcc tDbAccSet = new Common.ClsDbAcc(tSystem);
                //tSqlStr = string.Concat(" delete tabMBWHOut  where  Optimize_batch='", texOUT1.Text.Trim(), "' and  Order_id='", texOUT3.Text.Trim(), "' and Out_Type='10' "
                //       + "  insert into tabMBWHOut(Optimize_batch ,Order_id,status,Out_Type,Process_number,Single_name,ProcessNO_Count ) values('", texOUT1.Text.Trim(), "','", texOUT3.Text.Trim(), "','0','10','','", texOUT4.Text.Trim(), "','", texOUT5.Text.Trim(), "')"
                //       + " update [tabMB_Order] set status=1 where Optimize_batch='", texOUT1.Text.Trim(), "' and Order_id='", texOUT3.Text.Trim(), "'");

                string tData = ""; string RetData = "", RetStr = "";
                tData = texOUT1.Text + "+" + texOUT2.Text.Trim() + "+" + texOUT3.Text.Trim();
                DataSet dataSet = new DataSet();
                dataSet = tDbAccSet.ExecuteDisposeStatus("Pro_OUTSET", "20", tData, ref RetData, ref RetStr);
                if (dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[dataSet.Tables.Count - 1].Rows.Count > 0 && dataSet.Tables[dataSet.Tables.Count - 1].Columns.Count > 4)
                    {

                    }
                }
                tDbAccSet.Execute_Command(tSqlStr);
            }
        }

        private void butMBinWH_Click(object sender, EventArgs e)///强制进库
        {
            Common.ClsDbAcc tDbAccSet = new Common.ClsDbAcc(tSystem);
            //string tRetData = "", tRetStr = ""; int tRows = 0;
            string tSqlStr = "";
            if (IfNum(this.texMs5.Text.Trim()) == true & IfNum(this.texMs4.Text.Trim()) == true)
            {
                tSqlStr = string.Concat("Update tab_Sys set force_tag='1',force_long='", int.Parse(this.texMs5.Text.Trim()) * 1000, "' ,force_short='", int.Parse(this.texMs4.Text.Trim()) * 1000, "'  where sys_no ='1'");
                tDbAccSet.Execute_Command(tSqlStr);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Common.ClsDbAcc tDbAccSet = new Common.ClsDbAcc(tSystem);
            string tRetData = "", tRetStr = ""; int tRows = 0;
            //tDbAccSet.ExecutePro("Pro_11FinishCmdIn", 100, texMs12.Text.Trim().ToString(), 0, 0, ref tRetData, ref tRetStr, ref tRows);
            if (IfNum(texMs12.Text.Trim()))
            {
                tDbAccSet.ExecuteProFinishOut("Pro_21FinishCmdOut", 100, int.Parse(texMs1.Text.Trim()), int.Parse(texMs12.Text.Trim()), 0, 0, 0,0, ref tRetData, ref tRetStr, ref tRows);
            }
            
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tClsShowFrm.mStart = 0;//启动刷新界面
            string tStrAa = tabControl1.SelectedTab.Text.Trim();
            switch (tStrAa)
            {
                case "磨边理片仓储":
                    this.tClsShowFrm.mShowIndex = 101;
                    comboBox1.SelectedIndex = 0;
                    AddPenLine(1,this.panMBwh1, 29);
                    break;
                case "中空理片库":
                   this.tClsShowFrm.mShowIndex = 102;
                    comboBox4.SelectedIndex = 0;
                    AddPenLine(2,this.panelZKLab2, 29);
                    break;
           
            }
        }
        public void AddPenLine(int _index, Panel _Panel,int _Count)
        {
            int tmHeight = _Panel.Height-2;
            int tHeight = tmHeight / _Count;
            //创建Graphics对象
            Graphics GPS = _Panel.CreateGraphics(); /// this.CreateGraphics();
                                                    
            Pen MyPen ;
            if (_index == 1)
            {
                MyPen = new Pen(Color.Gray, 2f);
            }
            else
            {
                MyPen = new Pen(Color.SeaGreen, 2f);
            }
            for(int i=0 ;i<(_Count+1) ;i++)
            {
                GPS.DrawLine(MyPen, 0, tHeight * (i )+1, _Panel.Width, tHeight * (i )+1);
            }
        }
        Label[] mLabel = new Label[29];
        Label[] mZKLabel = new Label[29];
        public void AddPenLab(int _Index,int _Start,Panel _Panel, int _Count)
        {
            int tmHeight = _Panel.Height - 2;
            int tHeight = tmHeight / _Count;
            for (int i=0; i < 29; i++)
            {
                Label tLab = new Label();
                if (_Index == 2)
                {
                    mZKLabel[i] = new Label();
                    tLab = mZKLabel[i];
                }
                else
                {
                    mLabel[i] = new Label();
                    tLab = mLabel[i];

                }
                tLab.Width = 25;
                tLab.Height = tHeight;
                tLab.Font = new Font("宋体", 12, Font.Style);
                tLab.Text = (_Start + i).ToString();
                tLab.Name = (_Index * 100 + _Start + i).ToString();
                tLab.Location = new Point(2 + 0, 17 + (tHeight) * i - 10);///3,17
                _Panel.Controls.Add(tLab);
            }
        }
        Button[,] mBut = new Button[29, 4]; Button[,] mZKBut = new Button[29, 4];
        public void AddPanlMBWHBut(int _index, Panel _Panel, int _Count)
        {
            int tmHeight = _Panel.Height - 2;
            int tHeight = tmHeight / _Count;
            for (int i = 0; i < 29; i++)
            {
                int tWidth = 0;
                for (int j = 0; j < 4; j++)
                {
                    Button tBut = new Button();
                    if (_index == 1)
                    {
                        mBut[i, j] = new Button();
                        tBut = mBut[i, j];
                        tBut.Click += new EventHandler(But_Click);
                        tBut.BackColor = Color.SkyBlue;
                        
                    }
                    else
                    {
                        mZKBut[i, j] = new Button();
                        tBut = mZKBut[i, j];
                        tBut.Click += new EventHandler(ButZK_Click);
                        tBut.BackColor = Color.YellowGreen;
                    }

                    tBut.Width = 100;
                    tBut.Height = tHeight-5;
                    tBut.Font = new Font("宋体", 12, Font.Style);
                    tBut.Text = (i*100+j).ToString();
                    tBut.Name = (i * 100 + j).ToString();
                    
                    
                    tBut.Location = new Point(10 + tWidth, 17 + (tHeight) * i - 14);///3,17
                    tWidth = tWidth + tBut.Width + 5;
                    tBut.Visible = false;
                                                               
                    _Panel.Controls.Add(tBut);
                }
            }
        }
        private void But_Click(object sender, EventArgs e)
        {
            string tPLCflow="";
            Button tBut = (Button)sender; tPLCflow = tBut.Tag.ToString();
            DataTable tDt;
            string tSqlStr = "";
            tSqlStr = string.Concat(" select top 1 [WcsID],[Process_number],MBPlcFlow ,[Single_short],[Single_long] ,[WH_NO] ,[WH_Num] "
                  + ",case [Status] when 0 then '初始' when 1 then '数据写入' when 2 then '磨边已核对' when 3 then '进磨边'when 10 then '准备进库' when 14 then '库内'when 3 then '出库' when 20 then '准备出库' when 24 then '离库'  when 188 then '入库未完成'  when 299 then '出库未完成' when 99 then '不符合尺寸'when 96 then '破损'when 97 then '设备异常' when 120 then '空取'  else '异常' end 状态"
                                 + " from [TabMB_Data] a where MBPlcFlow='", tPLCflow, "' ");
            tDt = new DataTable();
            if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
            {
                if (tDt.Rows.Count > 0)
                {
                    texMBWhC1.Text = tDt.Rows[0][0].ToString(); texMBWhC2.Text = tDt.Rows[0][1].ToString(); texMBWhC3.Text = tDt.Rows[0][2].ToString(); texMBWhC4.Text = tDt.Rows[0][3].ToString();
                    texMBWhC5.Text = tDt.Rows[0][4].ToString(); texMBWhC6.Text = tDt.Rows[0][5].ToString(); texMBWhC7.Text = tDt.Rows[0][6].ToString(); texMBWhC8.Text = tDt.Rows[0][7].ToString();
                  
                }
                GetdataWH(" and a.WH_NO='" + texMBWhC6.Text.Trim() + "' and a.WH_Num='" + texMBWhC7.Text.Trim() + "' ");
                tSystem.mClsDBUPdate.DBClose();
            }

        }
        private void ButZK_Click(object sender, EventArgs e)
        {
            string tPLCflow = "";
            Button tBut = (Button)sender; tPLCflow = tBut.Tag.ToString();
            DataTable tDt;
            string tSqlStr = "";
            tSqlStr = string.Concat(" select top 1 [WcsID],[Process_number],ZKPlcFlow ,[Single_short],[Single_long] ,[ZKWH_NO] ,[ZKWH_Num] "
                  + ",case [Status] when 0 then '初始' when 1 then '数据写入' when 2 then '磨边已核对' when 3 then '进磨边'when 10 then '准备进库' when 14 then '库内'when 3 then '出库' when 20 then '准备出库' when 24 then '离库'  when 188 then '入库未完成'  when 299 then '出库未完成' when 99 then '不符合尺寸'when 96 then '破损'when 97 then '设备异常' when 120 then '空取' when 30 then '准备进中空' when 34 then '中空库' when 40 then '准备出中空' when 44 then '离开中空'   else '异常' end 状态"
                                 + " from [TabMB_Data] a where ZKPlcFlow='", tPLCflow, "' ");
            tDt = new DataTable();
            if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
            {
                if (tDt.Rows.Count > 0)
                {
                    texZKWhC1.Text = tDt.Rows[0][0].ToString(); texZKWhC2.Text = tDt.Rows[0][1].ToString(); texZKWhC3.Text = tDt.Rows[0][2].ToString(); texZKWhC4.Text = tDt.Rows[0][3].ToString();
                    texZKWhC5.Text = tDt.Rows[0][4].ToString(); texZKWhC6.Text = tDt.Rows[0][5].ToString(); texZKWhC7.Text = tDt.Rows[0][6].ToString(); texZKWhC8.Text = tDt.Rows[0][7].ToString();

                }
                GetdataZKWH(" and a.WH_NO='" + texZKWhC6.Text.Trim() + "' and a.WH_Num='" + texZKWhC7.Text.Trim() + "' ");
                tSystem.mClsDBUPdate.DBClose();
            }

        }

        public void ShowPanlMBWH(int _index ,Panel _Panel, DataTable _Dt)
        {
            int tBei = 3000 / 600;Button[,] tBut ;
            if (_index == 1)
            {
                tBut = mBut;
            }
            else
            {
                tBut = mZKBut ;
            }
            for (int i = 0; i < 29; i++)
            {
                string tStrData = _Dt.Rows[i]["InputFlow"].ToString().Trim();
                string[] tData1 = tStrData.Split(',');
                int tWidth = 0;
                for (int j = 0; j < 4; j++)
                {
                    if (tData1.Length >j)
                    {
                         
                        string[] tData2 = tData1[tData1.Length - j - 1].Trim().Split('@');
                        if (tData2.Length > 1)
                        {
                            string[] tData3 = tData2[1].Trim().Split('*');

                            tBut[i, j].Width = int.Parse(tData3[0]) / tBei;
                            tBut[i, j].Text = tData2[1];
                            tBut[i, j].Tag = tData2[0];
                            tBut[i, j].Location = new Point(10 + tWidth, tBut[i, j].Location.Y);
                            tBut[i, j].Visible = true;

                        }
                        else
                        {
                            tBut[i, j].Visible = false;   
                        }

                    }
                    else
                    {
                        tBut[i, j].Visible = false;   
                    }
                    tWidth = tWidth + tBut[i, j].Width + 15;
                }
            }
      
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox tCB=(ComboBox)sender;
            textBox19.Text = tCB.Text;
            switch (tCB.SelectedIndex)
            {
                case 0:
                    labMBCha.Text = "一";
                    tClsShowFrm.mShowMBNO  = 1;tClsShowFrm.mShowMBNum1 =1;tClsShowFrm.mShowMBNum2 =30;
                    for (int i = 0; i < 29; i++)
                    {
                        mLabel[i].Text = (i + 1).ToString();
                    }
                    break;
                case 1:
                    labMBCha.Text = "一";
                    tClsShowFrm.mShowMBNO = 1; tClsShowFrm.mShowMBNum1 = 30; tClsShowFrm.mShowMBNum2 = 59;
                    for (int i = 0; i < 29; i++)
                    {
                        mLabel[i].Text = (i + 30).ToString();
                    }
                    break;
                case 2:
                    labMBCha.Text = "二";
                    tClsShowFrm.mShowMBNO = 2; tClsShowFrm.mShowMBNum1 = 1; tClsShowFrm.mShowMBNum2 = 30;
                    for (int i = 0; i < 29; i++)
                    {
                        mLabel[i].Text = (i + 1).ToString();
                    }
                    break;
                case 3:
                    labMBCha.Text = "二";
                    tClsShowFrm.mShowMBNO = 2; tClsShowFrm.mShowMBNum1 = 30; tClsShowFrm.mShowMBNum2 = 59;
                    for (int i = 0; i < 29; i++)
                    {
                        mLabel[i].Text = (i + 30).ToString();
                    }
                    break;
                case 4:
                    labMBCha.Text = "三";
                    tClsShowFrm.mShowMBNO = 3; tClsShowFrm.mShowMBNum1 = 1; tClsShowFrm.mShowMBNum2 = 30;
                    for (int i = 0; i < 29; i++)
                    {
                        mLabel[i].Text = (i + 1).ToString();
                    }
                    break;
                case 5:
                    labMBCha.Text = "三";
                    tClsShowFrm.mShowMBNO = 3; tClsShowFrm.mShowMBNum1 = 30; tClsShowFrm.mShowMBNum2 = 59;
                    for (int i = 0; i < 29; i++)
                    {
                        mLabel[i].Text = (i + 30).ToString();
                    }
                    break;
                case 6:
                    labMBCha.Text = "四";
                    tClsShowFrm.mShowMBNO = 4; tClsShowFrm.mShowMBNum1 = 1; tClsShowFrm.mShowMBNum2 = 30;
                    for (int i = 0; i < 29; i++)
                    {
                        mLabel[i].Text = (i + 1).ToString();
                    }
                    break;
                case 7:
                    labMBCha.Text = "四";
                    tClsShowFrm.mShowMBNO = 4; tClsShowFrm.mShowMBNum1 = 30; tClsShowFrm.mShowMBNum2 = 59;
                    for (int i = 0; i < 29; i++)
                    {
                        mLabel[i].Text = (i + 30).ToString();
                    }
                    break;
                case 8:
                    labMBCha.Text = "五";
                    tClsShowFrm.mShowMBNO = 5; tClsShowFrm.mShowMBNum1 = 1; tClsShowFrm.mShowMBNum2 = 30;
                    for (int i = 0; i < 29; i++)
                    {
                        mLabel[i].Text = (i + 1).ToString();
                    }
                    break;
                case 9:
                    labMBCha.Text = "五";
                    tClsShowFrm.mShowMBNO = 5; tClsShowFrm.mShowMBNum1 = 30; tClsShowFrm.mShowMBNum2 = 59;
                    for (int i = 0; i < 29; i++)
                    {
                        mLabel[i].Text = (i + 30).ToString();
                    }
                    break;
                case 10:
                    labMBCha.Text = "六";
                    tClsShowFrm.mShowMBNO = 6; tClsShowFrm.mShowMBNum1 = 1; tClsShowFrm.mShowMBNum2 = 30;
                    for (int i = 0; i < 29; i++)
                    {
                        mLabel[i].Text = (i + 1).ToString();
                    }
                    break;
                case 11:
                    labMBCha.Text = "六";
                    tClsShowFrm.mShowMBNO = 6; tClsShowFrm.mShowMBNum1 = 30; tClsShowFrm.mShowMBNum2 = 59;
                    for (int i = 0; i < 29; i++)
                    {
                        mLabel[i].Text = (i + 30).ToString();
                    }
                    break;
                case 12:
                    labMBCha.Text = "七";
                    tClsShowFrm.mShowMBNO = 7; tClsShowFrm.mShowMBNum1 = 1; tClsShowFrm.mShowMBNum2 = 30;
                    for (int i = 0; i < 29; i++)
                    {
                        mLabel[i].Text = (i + 1).ToString();
                    }
                    break;
                case 13:
                    labMBCha.Text = "七";
                    tClsShowFrm.mShowMBNO = 7; tClsShowFrm.mShowMBNum1 = 30; tClsShowFrm.mShowMBNum2 = 59;
                    for (int i = 0; i < 29; i++)
                    {
                        mLabel[i].Text = (i + 30).ToString();
                    }
                    break;
                case 14:
                    labMBCha.Text = "八";
                    tClsShowFrm.mShowMBNO = 8; tClsShowFrm.mShowMBNum1 = 1; tClsShowFrm.mShowMBNum2 = 30;
                    for (int i = 0; i < 29; i++)
                    {
                        mLabel[i].Text = (i + 1).ToString();
                    }
                    break;
                case 15:
                    labMBCha.Text = "八";
                    tClsShowFrm.mShowMBNO = 8; tClsShowFrm.mShowMBNum1 = 30; tClsShowFrm.mShowMBNum2 = 59;
                    for (int i = 0; i < 29; i++)
                    {
                        mLabel[i].Text = (i + 30).ToString();
                    }
                    break;

            }
            texMBWhC1.Text = ""; texMBWhC2.Text = ""; texMBWhC3.Text = ""; texMBWhC4.Text = ""; texMBWhC5.Text = ""; texMBWhC6.Text = ""; texMBWhC7.Text = ""; texMBWhC8.Text = "";
           
        }

       

        private void butMBSetStart_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("    您确定对磨边仓储进行初始化系统?   \r\n   后果数据全部清空 ", "警告", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tSqlStr = "";
                tSqlStr = "update tabMB_WH set WH_UseWidth=0,Optimize_batch=0,Process_number =0,InputFlow='',InPutFlow1=0,OutCmd=0,OutType=0,OutWidth =0,WH_Status =0,InSize=0,InArea=0,InLong=0,InAtime =null";
                tSystem.mClsDBUPdate.Execute_Command(tSqlStr);
                tSystem.mClsDBUPdate.DBClose();
            }
           
            
        }
        private void butZKSetStart_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("    您确定对中空仓储进行初始化系统?   \r\n   后果数据全部清空 ", "警告", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tSqlStr = "";
                tSqlStr = "update tabZK_WH set WH_UseWidth=0,Optimize_batch=0,Process_number =0,InputFlow='',InPutFlow1=0,OutCmd=0,OutType=0,OutWidth =0,WH_Status =0,InSize=0,InArea=0,InLong=0,InAtime =null";
                tSystem.mClsDBUPdate.Execute_Command(tSqlStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void butMBWHSet3_Click(object sender, EventArgs e)
        {
            if (MBcbSet1.Text == "0")
            {
                GetdataWH("");
            }
            else if (IfNum(MBcbSet1.Text.Trim()) & IfNum(MBcbSet2.Text.Trim()))
            {
                if (MBcbSet2.Text.Trim() == "0")
                {
                    GetdataWH(" and a.WH_NO='" + MBcbSet1.Text.Trim() + "'  ");
                }
                else
                {
                    GetdataWH(" and a.WH_NO='" + MBcbSet1.Text.Trim() + "' and a.WH_Num='" + MBcbSet2.Text.Trim() + "' ");
                }
            }
            else if (IfNum(MBcbSet1.Text.Trim()) )
            {
                GetdataWH(" and a.WH_NO='" + MBcbSet1.Text.Trim() + "'  ");
            }

        }

        private void butMBWHSet6_Click(object sender, EventArgs e)
        {
            try
            {
                string tSqlstr = "";
                DataTable tDT = new DataTable();
                tSqlstr = "SELECT rank() over(order by a.whid,b.InAtime ) 'ID号', a.[WH_NO] as '仓库号',a.[WH_Num]as '库位号',a.[WH_Status]'存储数量',case a.[WH_If] when 0 then '正常' else '禁用'end 库位状态,b.Optimize_batch '优化单号' ,b.Process_number '流程卡号',b.Single_short '短边',b.Single_long  '长边' ,b.InAtime  '入库时间' "
                        //+",case when a.[WH_If]='1' then '禁用' when b.status=10 then '准备进库' when b.status=14 then '库内' when b.status=20 then '准备出库' when b.status=24 then '离库'  when b.status=199 then '入库未完成'  when b.status=299 then '出库未完成' when b.status=99 then '不符合尺寸'when b.status= 96 then '破损'when b.status=97 then '设备异常' when b.status=120 then '空取'  else '异常'end   '状态'"
                        + ",(select top 1 isnull(c.statusName,b.Status ) from StatusNameT c where b.Status =c.Status order by case when b.Err_Tag>0 and c.AssistSatusS =1 then 0 else 1 end,c.AssistSatusS  asc ) '状态'"
                        +",b.Single_tag ,b.Single_name '单片玻璃名称',b.Single_Str '玻璃组成',b.Order_length '成品长',b.Order_width '成品宽' ,b.WcsID '序号',b.MBPlcFlow 'PLC流水号',b.Order_singleid '成品单片编号'"
                        + "FROM tabMB_WH a  join TabMB_Data b on a.WH_NO =b.WH_NO and a.WH_Num =b.WH_Num  and (b.Status =14  or b.Status =10) and a.Process_number =b.Process_number where a.whid>0   order by a.whid,b.InAtime  ";
                if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlstr, ref tDT) == "")
                {
                    if (tDT.Rows.Count > 0)
                    {
                        this.dataMBWH.DataSource = tDT;
                        labMbshu.Text = tDT.Rows.Count.ToString();
                        tClsShowFrm.SetBackColor(dataMBWH, 10, 4);
                        for (int i = 0; i < this.dataMBWH.Columns.Count; i++)
                        {
                            dataMBWH.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            this.dataMBWH.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                    }
                    tSystem.mClsDBUPdate.DBClose();
                }
            }
            catch
            {
            }
        }
        private void butMBWHSet5_Click(object sender, EventArgs e)
        {
            GetdataWH(" and a.[WH_Status]='0'");
        }
        private void butMBWHSet4_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("   磨边库 手动进行出库操作 库号： " + MBcbSet1.Text + "  格号 " + MBcbSet2.Text  + " 设备异常?   \r\n   请看清库位编号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                Common.ClsDbAcc tDbAccSet = new Common.ClsDbAcc(tSystem);
              
                string tSqlStr = "";
                if (IfNum(this.MBcbSet1.Text.Trim()) == true & IfNum(this.MBcbSet2.Text.Trim()) == true)
                {
                    tSqlStr = string.Concat("Update tab_Sys set oForce_Tag='1',oForce_WH_NO='", this.MBcbSet1.Text.Trim() , "' ,oForce_WH_Num='", this.MBcbSet2.Text.Trim(), "'  where sys_no ='1'");
                    tDbAccSet.Execute_Command(tSqlStr);
                    tDbAccSet.DBClose();
                }
            }
        }
        private void GetdataWH(string _WhereStr)
        {
            try
            {
                string tSqlstr = "";
                DataTable tDT = new DataTable();
                tSqlstr = "SELECT rank() over(order by a.whid,b.InAtime ) 'ID号', a.[WH_NO] as '仓库号',a.[WH_Num]as '库位号',a.[WH_Status]'存储数量',case a.[WH_If] when 0 then '正常' else '禁用'end 库位状态,b.Optimize_batch '优化单号' ,b.Process_number '流程卡号',b.Single_short '短边',b.Single_long  '长边' ,b.InAtime  '入库时间'"
                 //+ " ,case when a.[WH_If]='1' then '禁用' when b.status=10 then '准备进库' when b.status=14 then '库内' when b.status=20 then '准备出库' when b.status=24 then '离库'  when b.status=199 then '入库未完成'  when b.status=299 then '出库未完成' when b.status=99 then '不符合尺寸'when b.status= 96 then '破损'when b.status=97 then '设备异常' when b.status=120 then '空取' when b.Status  is null then '' else '异常'end   '状态'"
                 + ",(select top 1 isnull(c.statusName,b.Status ) from StatusNameT c where b.Status =c.Status order by case when b.Err_Tag>0 and c.AssistSatusS =1 then 0 else 1 end ,c.AssistSatusS  asc) '状态'"
                 +",b.Single_tag ,b.Single_name '单片玻璃名称',b.Single_Str '玻璃组成',b.Order_length '成品长',b.Order_width '成品宽' "
                 + ",b.WcsID '序号',b.MBPlcFlow 'PLC流水号',b.Order_singleid '成品单片编号',a.InputFlow "
                 + " FROM tabMB_WH a left join TabMB_Data b on CHARINDEX(convert(varchar,b.MBPlcFlow),a.InputFlow )>0 and  a.WH_NO =b.WH_NO and a.WH_Num =b.WH_Num  and ((b.status>=10 and b.Status <24 ) or b.Status >90 ) and a.Process_number =b.Process_number"
                            + " where a.whid>0  " + _WhereStr + " order by a.whid,b.InAtime ";
                if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlstr, ref tDT) == "")
                {
                    if (tDT.Rows.Count > 0)
                    {
                        this.dataMBWH.DataSource = tDT;
                        labMbshu.Text = tDT.Rows.Count.ToString();
                        tClsShowFrm.SetBackColor(dataMBWH, 10, 4);
                        for (int i = 0; i < this.dataMBWH.Columns.Count; i++)
                        {
                            dataMBWH.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            this.dataMBWH.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                    }
                    tSystem.mClsDBUPdate.DBClose();
                }
            }
            catch
            {
            }

        }

        private void dataMBWH_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = e.RowIndex;

            if (this.dataMBWH.Rows.Count > 1 && rowindex >= 0)
            {

                if (this.dataMBWH.CurrentRow.Index == 0)
                {

                    texMBWhC1.Text = dataMBWH.Rows[0].Cells[16].Value.ToString();
                    texMBWhC2.Text = dataMBWH.Rows[0].Cells[6].Value.ToString();
                    texMBWhC3.Text = dataMBWH.Rows[0].Cells[17].Value.ToString();
                    texMBWhC4.Text = dataMBWH.Rows[0].Cells[8].Value.ToString();
                    texMBWhC5.Text = dataMBWH.Rows[0].Cells[7].Value.ToString();
                    texMBWhC6.Text = dataMBWH.Rows[0].Cells[1].Value.ToString();
                    texMBWhC7.Text = dataMBWH.Rows[0].Cells[2].Value.ToString();
                    texMBWhC8.Text = dataMBWH.Rows[0].Cells[10].Value.ToString();

                }
                else
                {

                    texMBWhC1.Text = dataMBWH.Rows[rowindex].Cells[16].Value.ToString();
                    texMBWhC2.Text = dataMBWH.Rows[rowindex].Cells[6].Value.ToString();
                    texMBWhC3.Text = dataMBWH.Rows[rowindex].Cells[17].Value.ToString();
                    texMBWhC4.Text = dataMBWH.Rows[rowindex].Cells[8].Value.ToString();
                    texMBWhC5.Text = dataMBWH.Rows[rowindex].Cells[7].Value.ToString();
                    texMBWhC6.Text = dataMBWH.Rows[rowindex].Cells[1].Value.ToString();
                    texMBWhC7.Text = dataMBWH.Rows[rowindex].Cells[2].Value.ToString();
                    texMBWhC8.Text = dataMBWH.Rows[rowindex].Cells[10].Value.ToString();
                }
            }
        }
        private void butMBQing_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("    您确定对磨边仓储进行清库系统?   \r\n   后果数据全部清空 ", "警告", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tSqlStr = "";
                tSqlStr = "  update tab_SYS set SysOut=999 where Sys_NO='1' ";
                tSystem.mClsDBUPdate.Execute_Command(tSqlStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }
        private void butZKQing_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("    您确定对中空仓储进行清库系统?   \r\n   后果数据全部清空 ", "警告", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tSqlStr = "";
                tSqlStr = "  update tab_SYS set SysOut=999 where Sys_NO='2' ";
                tSystem.mClsDBUPdate.Execute_Command(tSqlStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }
        private void butMBWHSet1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("    您确定对磨边仓储  " + MBcbSet1.Text + "  格号 " + MBcbSet2.Text + " 进行禁用?   \r\n   后果不能进库使用   ", "警告", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tSqlStr = "";
                if (MBcbSet2.Text == "0")
                {
                    tSqlStr = string.Concat("  update tabMB_WH set WH_If =1 where WH_NO ='", MBcbSet1.Text, "'");
                }
                else
                {
                    tSqlStr = string.Concat("  update tabMB_WH set WH_If =1 where WH_NO ='", MBcbSet1.Text, "' and WH_Num='", MBcbSet2.Text, "' ");
                }
                tSystem.mClsDBUPdate.Execute_Command(tSqlStr);
                tSystem.mClsDBUPdate.DBClose();
            }

        }

        private void butMBWHSet2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("    您确定对磨边仓储 库号 " + MBcbSet1.Text + "  格号 " + MBcbSet2.Text  + "进行使用?   \r\n   后果该库位可以进库   ", "警告", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tSqlStr = "";
                if (MBcbSet2.Text == "0")
                {
                    tSqlStr = string.Concat("  update tabMB_WH set WH_If =0 where WH_NO ='", MBcbSet1.Text, "'");
                }
                else
                {
                    tSqlStr = string.Concat("  update tabMB_WH set WH_If =0 where WH_NO ='", MBcbSet1.Text, "' and WH_Num='", MBcbSet2.Text, "'");
                }
                tSystem.mClsDBUPdate.Execute_Command(tSqlStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox tCB = (ComboBox)sender;
            textBox15.Text = tCB.Text;
            switch (tCB.SelectedIndex)
            {
                case 0:
                    labZKCha.Text = "一";
                    tClsShowFrm.mShowMBNO = 1; tClsShowFrm.mShowMBNum1 = 1; tClsShowFrm.mShowMBNum2 = 30;
                    for (int i = 0; i < 29; i++)
                    {
                        mZKLabel[i].Text = (i + 1).ToString();
                    }
                    break;
                case 1:
                    labZKCha.Text = "一";
                    tClsShowFrm.mShowMBNO = 1; tClsShowFrm.mShowMBNum1 = 30; tClsShowFrm.mShowMBNum2 = 59;
                    for (int i = 0; i < 29; i++)
                    {
                        mZKLabel[i].Text = (i + 30).ToString();
                    }
                    break;
                case 2:
                    labZKCha.Text = "二";
                    tClsShowFrm.mShowMBNO = 2; tClsShowFrm.mShowMBNum1 = 1; tClsShowFrm.mShowMBNum2 = 30;
                    for (int i = 0; i < 29; i++)
                    {
                        mZKLabel[i].Text = (i + 1).ToString();
                    }
                    break;
                case 3:
                    labZKCha.Text = "二";
                    tClsShowFrm.mShowMBNO = 2; tClsShowFrm.mShowMBNum1 = 30; tClsShowFrm.mShowMBNum2 = 59;
                    for (int i = 0; i < 29; i++)
                    {
                        mZKLabel[i].Text = (i + 30).ToString();
                    }
                    break;
                case 4:
                    labZKCha.Text = "三";
                    tClsShowFrm.mShowMBNO = 3; tClsShowFrm.mShowMBNum1 = 1; tClsShowFrm.mShowMBNum2 = 30;
                    for (int i = 0; i < 29; i++)
                    {
                        mZKLabel[i].Text = (i + 1).ToString();
                    }
                    break;
                case 5:
                    labZKCha.Text = "三";
                    tClsShowFrm.mShowMBNO = 3; tClsShowFrm.mShowMBNum1 = 30; tClsShowFrm.mShowMBNum2 = 59;
                    for (int i = 0; i < 29; i++)
                    {
                        mZKLabel[i].Text = (i + 30).ToString();
                    }
                    break;
                case 6:
                    labZKCha.Text = "四";
                    tClsShowFrm.mShowMBNO = 4; tClsShowFrm.mShowMBNum1 = 1; tClsShowFrm.mShowMBNum2 = 30;
                    for (int i = 0; i < 29; i++)
                    {
                        mZKLabel[i].Text = (i + 1).ToString();
                    }
                    break;
                case 7:
                    labZKCha.Text = "四";
                    tClsShowFrm.mShowMBNO = 4; tClsShowFrm.mShowMBNum1 = 30; tClsShowFrm.mShowMBNum2 = 59;
                    for (int i = 0; i < 29; i++)
                    {
                        mZKLabel[i].Text = (i + 30).ToString();
                    }
                    break;
                case 8:
                    labZKCha.Text = "五";
                    tClsShowFrm.mShowMBNO = 5; tClsShowFrm.mShowMBNum1 = 1; tClsShowFrm.mShowMBNum2 = 30;
                    for (int i = 0; i < 29; i++)
                    {
                        mZKLabel[i].Text = (i + 1).ToString();
                    }
                    break;
                case 9:
                    labZKCha.Text = "五";
                    tClsShowFrm.mShowMBNO = 5; tClsShowFrm.mShowMBNum1 = 30; tClsShowFrm.mShowMBNum2 = 59;
                    for (int i = 0; i < 29; i++)
                    {
                        mZKLabel[i].Text = (i + 30).ToString();
                    }
                    break;
                case 10:
                    labZKCha.Text = "六";
                    tClsShowFrm.mShowMBNO = 6; tClsShowFrm.mShowMBNum1 = 1; tClsShowFrm.mShowMBNum2 = 30;
                    for (int i = 0; i < 29; i++)
                    {
                        mZKLabel[i].Text = (i + 1).ToString();
                    }
                    break;
                case 11:
                    labZKCha.Text = "六";
                    tClsShowFrm.mShowMBNO = 6; tClsShowFrm.mShowMBNum1 = 30; tClsShowFrm.mShowMBNum2 = 59;
                    for (int i = 0; i < 29; i++)
                    {
                        mZKLabel[i].Text = (i + 30).ToString();
                    }
                    break;
                case 12:
                    labZKCha.Text = "七";
                    tClsShowFrm.mShowMBNO = 7; tClsShowFrm.mShowMBNum1 = 1; tClsShowFrm.mShowMBNum2 = 30;
                    for (int i = 0; i < 29; i++)
                    {
                        mZKLabel[i].Text = (i + 1).ToString();
                    }
                    break;
                case 13:
                    labZKCha.Text = "七";
                    tClsShowFrm.mShowMBNO = 7; tClsShowFrm.mShowMBNum1 = 30; tClsShowFrm.mShowMBNum2 = 59;
                    for (int i = 0; i < 29; i++)
                    {
                        mZKLabel[i].Text = (i + 30).ToString();
                    }
                    break;
                case 14:
                    labZKCha.Text = "八";
                    tClsShowFrm.mShowMBNO = 8; tClsShowFrm.mShowMBNum1 = 1; tClsShowFrm.mShowMBNum2 = 30;
                    for (int i = 0; i < 29; i++)
                    {
                        mZKLabel[i].Text = (i + 1).ToString();
                    }
                    break;
                case 15:
                    labZKCha.Text = "八";
                    tClsShowFrm.mShowMBNO = 8; tClsShowFrm.mShowMBNum1 = 30; tClsShowFrm.mShowMBNum2 = 59;
                    for (int i = 0; i < 29; i++)
                    {
                        mZKLabel[i].Text = (i + 30).ToString();
                    }
                    break;

            }
            texZKWhC1.Text = ""; texZKWhC2.Text = ""; texZKWhC3.Text = ""; texZKWhC4.Text = ""; texZKWhC5.Text = ""; texZKWhC6.Text = ""; texZKWhC7.Text = ""; texZKWhC8.Text = "";
           
        }

        private void butProZKOUT_Click(object sender, EventArgs e)
        {
            mMBMBSelectIndex = 0;
            string tSqlStr = "";
            DataTable tDt = new DataTable();
            Common.ClsDbAcc tDbAccSet = new Common.ClsDbAcc(tSystem);
            tSqlStr = string.Concat(" SELECT TOP 1000 a.Order_id '订单号',a.Process_number  '架号',a.Exit_To '出口',a.Process_Count 流程卡数量,case when status=1 then '在出库'when status=2 then '出库完成' when a.Lot_ActualNum=0 then '未入库' when a.Lot_EstimateNum=Lot_ActualNum then '入库完成'when status=3 then '强制出库'  else '未入库完成' end '当前状态',Process_Technology '单片玻璃名',Lot_ActualNum 已入数量,a.Lot_EstimateNum '单片总算',case when a.Out_Type =99 then '强制出中空' when a.Out_Type='0'then '出中空' when a.Out_Type  is null then '未出库'end '出库方式' "
                 + " ,a.Single_Str '组成',a.Customer_name '客户名称' "
                 + "FROM tabZKProcess a"
                 + "  where a.Process_number like '%", texZKOut2.Text.Trim(), "'  and a.order_id like '%", texZKOut3.Text.Trim(), "'  order by a.Process_number ");
            if (tDbAccSet.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
            {
                dataZKProOut.DataSource = tDt;

                tClsShowFrm.SetBackColor(dataZKProOut, 4, 0);
                for (int i = 0; i < this.dataZKProOut.ColumnCount; i++)
                {
                    dataZKProOut.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataZKProOut.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                tSystem.mClsDBUPdate.DBClose();
            }
        }
        private void butKUProZKOUT_Click(object sender, EventArgs e)//库内查询
        {
            mMBMBSelectIndex = 0;
            string tSqlStr = "";
            DataTable tDt = new DataTable();
            Common.ClsDbAcc tDbAccSet = new Common.ClsDbAcc(tSystem);
            tSqlStr = string.Concat(" SELECT TOP 1000 a.Order_id '订单号',a.Process_number  '架号',a.Exit_To '出口',a.Process_Count 流程卡数量,case when status=1 then '在出库'when status=2 then '出库完成' when a.Lot_ActualNum=0 then '未入库' when a.Lot_EstimateNum=Lot_ActualNum then '入库完成'when status=3 then '强制出库'  else '未入库完成' end '当前状态',Process_Technology '单片玻璃名',Lot_ActualNum 已入数量,a.Lot_EstimateNum '单片总算',case when a.Out_Type =99 then '强制出中空' when a.Out_Type='0'then '出中空' when a.Out_Type  is null then '未出库'end '出库方式' "
                 + " ,a.Single_Str '组成',a.Customer_name '客户名称' "
                 +" FROM tabZKProcess a"
                 + "  where a.Process_number like '%", texZKOut2.Text.Trim(), "'  and a.order_id like '%", texZKOut3.Text.Trim(), "'  "
                 + " and exists (select 1 from TabMB_Data b where a.Process_number =b.Process_number  and b.Status =34) "
                 //+ " and not exists(select 1 from TabMB_Data b where a.Process_number =b.Process_number  and b.Status in(34,40,44)  )"// and 3=", clsMyPublic.mZKExit, " and b.ZKWH_NO =1) "//限制3号口
                 +"order by a.Process_number ");
            if (tDbAccSet.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
            {
                dataZKProOut.DataSource = tDt;

                tClsShowFrm.SetBackColor(dataZKProOut, 4, 0);
                for (int i = 0; i < this.dataZKProOut.ColumnCount; i++)
                {
                    dataZKProOut.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataZKProOut.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                tSystem.mClsDBUPdate.DBClose();
            }
        }
        private void butORDMBOUT_Click(object sender, EventArgs e)
        {
            mMBMBSelectIndex = 2;
            string tSqlStr = "";
            DataTable tDt = new DataTable();
            //Common.ClsDbAcc tDbAccSet = new Common.ClsDbAcc(tSystem);
            tSqlStr = string.Concat(" select * from [tabZKOrder] a where a.order_id like '%", texZKOut3.Text.Trim(), "'   order by order_singleid");
            if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
            {
                dataZKProOut.DataSource = tDt;

                tClsShowFrm.SetBackColor(dataZKProOut, 4, 0);
                for (int i = 0; i < this.dataZKProOut.ColumnCount; i++)
                {
                    dataZKProOut.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataZKProOut.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void butZKProOut_Click(object sender, EventArgs e)///中空流程卡号出
        {
            DialogResult dr = MessageBox.Show("    您确定对中空仓储 订单号： " + texZKOut3.Text + "  流程卡号 " + texZKOut2.Text + "进行按流程卡号出库?   \r\n   请看清流程卡号 ", "警告", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tExit_to = "1";
                if (texZKOut4.Text == "2")
                {
                    tExit_to = "2";
                }
                string tSqlStr = "";
                DataTable tDt = new DataTable();
                Common.ClsDbAcc tDbAccSet = new Common.ClsDbAcc(tSystem);
                //tSqlStr = string.Concat("update tabZKProcess set  status='1',Exit_To ='",tExit_to ,"' where Process_number='", texZKOut2.Text , "' "
                //     );
                //tDbAccSet.Execute_Command(tSqlStr);

                string tData = ""; string RetData = "", RetStr = "";
                tData = texZKOut2.Text + "+" + texZKOut3.Text.Trim() + "+" + tExit_to.Trim();
                DataSet dataSet = new DataSet();
                dataSet = tDbAccSet.ExecuteDisposeStatus("Pro_OUTSET", "101", tData, ref RetData, ref RetStr);
                if (dataSet.Tables.Count > 0)
                {
                  
                }
                tDbAccSet.DBClose();
            }

        }

      
        private void butZKProQiangOut_Click(object sender, EventArgs e)///
        {
            DialogResult dr = MessageBox.Show("    您确定对中空仓储 订单号： " + texZKOut3.Text + "  流程卡号 " + texZKOut2.Text + "进行按流程卡号强制出库?   \r\n   请看清流程卡号，强制出库未满配道都会出库 ", "警告", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tExit_to = "1";
                if (texZKOut4.Text == "2")
                {
                    tExit_to = "2";
                    comboBox5.SelectedIndex = 1;
                }
                else if (texZKOut4.Text == "3")
                {
                    tExit_to = "3";
                    comboBox5.SelectedIndex = 2;
                }
                else
                {
                    comboBox5.SelectedIndex = 0;
                }
                string tSqlStr = "";
                DataTable tDt = new DataTable();
                Common.ClsDbAcc tDbAccSet = new Common.ClsDbAcc(tSystem);
                //tSqlStr = string.Concat("update tabZKProcess set  status='1',Exit_To ='",tExit_to ,"' where Process_number='", texZKOut2.Text , "' "
                //     );
                //tDbAccSet.Execute_Command(tSqlStr);

                string tData = ""; string RetData = "", RetStr = "";
                tData = texZKOut2.Text + "+" + texZKOut3.Text.Trim() + "+" + tExit_to.Trim();
                DataSet dataSet = new DataSet();
                dataSet = tDbAccSet.ExecuteDisposeStatus("Pro_OUTSET", "102", tData, ref RetData, ref RetStr);
                if (dataSet.Tables.Count > 0)
                {

                }
               

                GetData.ClsGetData tClsGetData = new GetData.ClsGetData(); DataTable tDtatTable = new DataTable();
                //tData = tClsGetData.GetLINSHIERP(this.textBox6.Text); string tRetStr = "";
                //tSystem.mClsDBUPdate.ExecuteTvp("Pro_ToExcelB", tData, ref tRetStr);
                //tSystem.mClsDBUPdate.DBClose();
                string tSqlstr = " select a.WcsID, a.Order_id,a.Order_singleid, a.Order_singlename ,a.Process_number,a.Customer_name,a.Order_width,a.Order_length,a.ply  from [TabMB_Data]a where a.Process_number ='" + texZKOut2.Text + "' order by a.Order_singleid ";
                tSqlstr = 
                         //"select top 1000 a.WcsID, a.Order_id,a.Order_singleid, a.Order_singlename ,a.Process_number,a.Customer_name,a.Order_width,a.Order_length,a.ply "//李赛克
                         "select top 1000 a.Order_singlename,Single_tag,a.Order_singleid,single_long,single_short,ply,60 as sealing_depth "//格拉斯
                          + ",a.Process_number , row_number() over (order by b.status   desc "
                                              + "  , convert(  int,a.Single_long)*convert(int,a.Single_short) desc "
                                              + "  ,   a.Order_singleid "
                                              + "  ,a.Single_tag desc "
                             + "   ) as rowid  "
                  + "  from TabMB_Data  a join tabZKOrder b on a.Order_singleid =b.Order_singleid  where  "
                  + "  a.Order_id =b.Order_id  and Process_number='" + texZKOut2.Text + "'"
                  + (checkZKout.Checked == false ? "and   b.Status =2 and   a.Status =34 " : "")
                  + " order by rowid  ";
                tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlstr, ref tDtatTable);

                DataTable tDataCode = new DataTable();
                tSqlstr = " SELECT TOP 1000 [玻璃数据] ,[编号A],[玻璃产品名称] ,[框数据] ,[编号B] ,[框架产品名称] ,[备注] ,[Addtime] FROM [dbo].[TablisecCode]";
                tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlstr, ref tDataCode);
                if (tDtatTable.Rows.Count > 0)
                {
                    //string tStr = SUTAIMES.Common.LisecGPSOutput.SetTrfData(tDtatTable, tDataCode);//李赛克
                    string tStr = SUTAIMES.Common.glastonOutput.SetLenData(tDtatTable);//格拉斯通
                    string name = System.Text.RegularExpressions.Regex.Replace(tDtatTable.Rows[0]["Process_number"].ToString(), @"[^0-9]+", "");

                    //FileDialog.SaveFile("", "" + "\\LISEC" + name + ".trf");
                     
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    //saveFileDialog.Filter = "txt files(*.trf)|*.trf";//李赛克
                    saveFileDialog.Filter = "txt files(*.len)|*.len";//格拉斯通
                    try
                    {
                        //str = SetLenData(tDT);
                        //SaveFile(str, saveAddress + processNumber.Replace('.', '_') + "#" + DateTime.Now.ToString("ddhhmmss") + ".len");
                        File.WriteAllText("C:\\SUTAI\\glaston" + name + "#" + DateTime.Now.ToString("ddhhmmss") + ".len", tStr);//格拉斯通
                        //File.WriteAllText("C:\\SUTAI\\LISEC" + name + ".trf", tStr);//李赛克
                    }
                    catch
                    {
                        saveFileDialog.ShowDialog();
                        File.WriteAllText(saveFileDialog.FileName, tStr);
                    }
                    //MessageBox.Show("文件已保存");
                }
                tDbAccSet.DBClose();
            }
        }
        private void dataZKProOut_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = e.RowIndex;

            if (this.dataZKProOut.Rows.Count > 1 && rowindex >= 0)
            {
              
                if (this.dataZKProOut.CurrentRow.Index == 0 )
                {
                    if (mMBMBSelectIndex == 0)
                    {

                        texZKOut2.Text = dataZKProOut.Rows[0].Cells[1].Value.ToString();
                        texZKOut3.Text = dataZKProOut.Rows[0].Cells[0].Value.ToString();
                        texZKOut4.Text = clsMyPublic.mZKExit;
                    }
                }
                else
                {
                    if (mMBMBSelectIndex == 0)
                    {

                        texZKOut2.Text = dataZKProOut.Rows[rowindex].Cells[1].Value.ToString();
                        texZKOut3.Text = dataZKProOut.Rows[rowindex].Cells[0].Value.ToString();
                        texZKOut4.Text =  clsMyPublic.mZKExit;
                    }
                }
            }
        }
        private void butZKcha_Click(object sender, EventArgs e)
        {
            DataTable tDt;
            string tSqlStr = "";
            tSqlStr = string.Concat("SELECT TOP 1000 [Optimize_batch] '优化单号' ,[Order_id]'订单号' ,[Process_number] ' 流程卡号',[ProcessNO_Count] '数量',Single_name '玻璃名称', case [Out_Type] when '10' then '按订单出库' when '0' then '按流程卡出库'  end '磨边仓储出库方式',  case [Status] when 1 then '出库结束'else  '出库中'end '状态'  ,[Run_Status] ,[Updatetime] "
                  + "FROM [tabMBWHOut] a where Optimize_batch like '%", texZKOptiSet.Text.Trim(), "' and Process_number like '%", texZKProSet.Text.Trim(), "' order by a.Addtime  desc  , a.Optimize_batch ");
            tDt = new DataTable();

            if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
            {
                dataGZKSet.DataSource = tDt;

                for (int i = 0; i < this.dataGZKSet.Columns.Count; i++)
                {
                    dataGZKSet.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataGZKSet.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void dataGZKSet_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = e.RowIndex;

            if (this.dataGZKSet.Rows.Count > 1 && rowindex >= 0)
            {
              
                if (this.dataGZKSet.CurrentRow.Index == 0 || this.dataGZKSet.CurrentRow.Index == 0)
                {

                    texZKOptiSet.Text = dataGZKSet.Rows[0].Cells[0].Value.ToString();
                    texZKProSet.Text = dataGZKSet.Rows[0].Cells[2].Value.ToString();
                    texZKORDSet.Text = dataGZKSet.Rows[0].Cells[1].Value.ToString();


                }
                else
                {

                    texZKOptiSet.Text = dataGZKSet.Rows[rowindex].Cells[0].Value.ToString();
                    texZKProSet.Text = dataGZKSet.Rows[rowindex].Cells[2].Value.ToString();
                    texZKORDSet.Text = dataGZKSet.Rows[rowindex].Cells[1].Value.ToString();
                }
            }
        }

        private void butZKProset_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("    您确定对中空库入库 优化单号： " + texZKOptiSet.Text + "  流程卡号 " + texZKProSet.Text + "进行按流程卡号入库库?   \r\n      请看清流程卡号     ", "警告", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tInType = "10";
                if (comZKSet.Text == "2")
                {
                    tInType = "11";

                }

                //string tSqlStr = "";
                //DataTable tDt = new DataTable();
                //Common.ClsDbAcc tDbAccSet = new Common.ClsDbAcc(tSystem);
                //tSqlStr = string.Concat("update TabMB_Optimize set ZKInType='" + tInType + "' ,ZKProcess_number ='", texZKProSet.Text.Trim(), "' ,MBWHOut=2   where Optimize_batch='", texZKOptiSet.Text.Trim(), "'"
                //                 + " update TabMB_Optimize set MBWHOut=3   where Optimize_batch !='", texZKOptiSet.Text.Trim(), "' and MBWHOut=2  "
                //                 + " update tabZKOP set status=1 where Optimize_batch ='", texZKOptiSet.Text.Trim(), "' and Process_number ='", texZKOptiSet.Text.Trim(), "'  ");
                //tDbAccSet.Execute_Command(tSqlStr);
                //tDbAccSet.DBClose();


                string tSqlStr = "";
                DataTable tDt = new DataTable();
                Common.ClsDbAcc tDbAccSet = new Common.ClsDbAcc(tSystem);

                string tData = ""; string RetData = "", RetStr = "";
                tData = texZKOptiSet.Text + "+" + texZKProSet.Text.Trim() + "+" + texZKORDSet.Text.Trim() + "+" + tInType.Trim() + "+" + texZKEnportSet.Text.Trim();
                DataSet dataSet = new DataSet();
                dataSet = tDbAccSet.ExecuteDisposeStatus("Pro_OUTSET", "110", tData, ref RetData, ref RetStr);
                if (dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[dataSet.Tables.Count - 1].Rows.Count > 0 && dataSet.Tables[dataSet.Tables.Count - 1].Columns.Count > 4)
                    {

                    }
                }
                tDbAccSet.DBClose();

                //////////旗滨使用
                ////////string tSqlStr = "";
                ////////DataTable tDt = new DataTable();
                ////////Common.ClsDbAcc tDbAccSet = new Common.ClsDbAcc(tSystem);
              
                ////////string tData = ""; string RetData = "", RetStr = "";
                ////////tData = texZKOptiSet.Text + "+" + texZKProSet.Text.Trim() + "+" + texZKORDSet.Text.Trim() + "+" + tInType.Trim();
                ////////DataSet dataSet = new DataSet();
                ////////dataSet = tDbAccSet.ExecuteDisposeStatus("Pro_OUTSET", "201", tData, ref RetData, ref RetStr);
                ////////if (dataSet.Tables.Count > 0)
                ////////{
                ////////    if (dataSet.Tables[dataSet.Tables.Count - 1].Rows.Count > 0 && dataSet.Tables[dataSet.Tables.Count - 1].Columns.Count > 4)
                ////////    {

                ////////    }
                ////////}
                ////tDbAccSet.DBClose();
            }
        }
        private void butZKORDset_Click(object sender, EventArgs e)//订单模式
        {
            DialogResult dr = MessageBox.Show("    您确定对中空库入库 优化单号： " + texZKOptiSet.Text + " 订单号 " + texZKORDSet.Text.Trim() + "  流程卡号 " + texZKProSet.Text + "进行按流程卡号入库库?   \r\n      请看清流程卡号     ", "警告", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tInType = "20";
                if (comZKSet.Text == "2")
                {
                    tInType = "21";

                }

                string tSqlStr = "";
                DataTable tDt = new DataTable();
                Common.ClsDbAcc tDbAccSet = new Common.ClsDbAcc(tSystem);

                string tData = ""; string RetData = "", RetStr = "";
                tData = texZKOptiSet.Text + "+" + texZKProSet.Text.Trim() + "+" + texZKORDSet.Text.Trim() + "+" + tInType.Trim()+"+" + texZKEnportSet.Text.Trim();
                DataSet dataSet = new DataSet();
                dataSet = tDbAccSet.ExecuteDisposeStatus("Pro_OUTSET", "120", tData, ref RetData, ref RetStr);
                if (dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[dataSet.Tables.Count - 1].Rows.Count > 0 && dataSet.Tables[dataSet.Tables.Count - 1].Columns.Count > 4)
                    {

                    }
                }
                tDbAccSet.DBClose();
            }
        }
        private void button4_Click(object sender, EventArgs e)///优化单
        {
            DialogResult dr = MessageBox.Show("    您确定对中空库入库 优化单号： " + texZKOptiSet.Text +" 订单号 "+ texZKORDSet.Text.Trim() + "  流程卡号 " + texZKProSet.Text + "进行按流程卡号入库库?   \r\n      请看清流程卡号     ", "警告", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tInType = "20";
                if (comZKSet.Text == "2")
                {
                    tInType = "21";

                }

                string tSqlStr = "";
                DataTable tDt = new DataTable();
                Common.ClsDbAcc tDbAccSet = new Common.ClsDbAcc(tSystem);

                string tData = ""; string RetData = "", RetStr = "";
                tData = texZKOptiSet.Text + "+" + texZKProSet.Text.Trim() + "+" + texZKORDSet.Text.Trim() + "+" + tInType.Trim()+"+" + texZKEnportSet.Text.Trim();
                DataSet dataSet = new DataSet();
                dataSet = tDbAccSet.ExecuteDisposeStatus("Pro_OUTSET", "130", tData, ref RetData, ref RetStr);
                if (dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[dataSet.Tables.Count - 1].Rows.Count > 0 && dataSet.Tables[dataSet.Tables.Count - 1].Columns.Count > 4)
                    {

                    }
                }
                tDbAccSet.DBClose();
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.tClsShowFrm.mShowZKExitTo = int.Parse(comboBox5.Text.Trim());
        }

        private void dataZKIN_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = e.RowIndex;

            if (this.dataZKIN.Rows.Count > 1 && rowindex >= 0)
            {

                if (this.dataZKIN.CurrentRow.Index == 0)
                {

                    texZKIMs1.Text = dataZKIN.Rows[0].Cells[8].Value.ToString();
                    texZKIMs2.Text = dataZKIN.Rows[0].Cells[0].Value.ToString();
                    texZKIMs3.Text = dataZKIN.Rows[0].Cells[10].Value.ToString();
                    texZKIMs4.Text = dataZKIN.Rows[0].Cells[5].Value.ToString();
                    texZKIMs5.Text = dataZKIN.Rows[0].Cells[3].Value.ToString();
                    texZKIMs6.Text = dataZKIN.Rows[0].Cells[2].Value.ToString();
                    texZKIMs7.Text = dataZKIN.Rows[0].Cells[6].Value.ToString();
                    texZKIMs8.Text = dataZKIN.Rows[0].Cells[7].Value.ToString();
                    texZKIMs9.Text = dataZKIN.Rows[0].Cells[4].Value.ToString();

                }
                else
                {

                    texZKIMs1.Text = dataZKIN.Rows[rowindex].Cells[8].Value.ToString();
                    texZKIMs2.Text = dataZKIN.Rows[rowindex].Cells[0].Value.ToString();
                    texZKIMs3.Text = dataZKIN.Rows[rowindex].Cells[10].Value.ToString();
                    texZKIMs4.Text = dataZKIN.Rows[rowindex].Cells[5].Value.ToString();
                    texZKIMs5.Text = dataZKIN.Rows[rowindex].Cells[3].Value.ToString();
                    texZKIMs6.Text = dataZKIN.Rows[rowindex].Cells[2].Value.ToString();
                    texZKIMs7.Text = dataZKIN.Rows[rowindex].Cells[6].Value.ToString();
                    texZKIMs8.Text = dataZKIN.Rows[rowindex].Cells[7].Value.ToString();
                    texZKIMs9.Text = dataZKIN.Rows[rowindex].Cells[4].Value.ToString();


                }
            }
        }

        private void dOutGridViewZK_CellClick(object sender, DataGridViewCellEventArgs e)//texZKOMs
        {
            int rowindex = e.RowIndex;

            if (this.dOutGridViewZK.Rows.Count > 1 && rowindex >= 0)
            {

                if (this.dOutGridViewZK.CurrentRow.Index == 0)
                {

                    texZKOMs1.Text = dOutGridViewZK.Rows[0].Cells[8].Value.ToString();
                    texZKOMs2.Text = dOutGridViewZK.Rows[0].Cells[0].Value.ToString();
                    texZKOMs3.Text = dOutGridViewZK.Rows[0].Cells[10].Value.ToString();
                    texZKOMs4.Text = dOutGridViewZK.Rows[0].Cells[5].Value.ToString();
                    texZKOMs5.Text = dOutGridViewZK.Rows[0].Cells[3].Value.ToString();
                    texZKOMs6.Text = dOutGridViewZK.Rows[0].Cells[2].Value.ToString();
                    texZKOMs7.Text = dOutGridViewZK.Rows[0].Cells[6].Value.ToString();
                    texZKOMs8.Text = dOutGridViewZK.Rows[0].Cells[7].Value.ToString();
                    texZKOMs9.Text = dOutGridViewZK.Rows[0].Cells[4].Value.ToString();

                }
                else
                {

                    texZKOMs1.Text = dOutGridViewZK.Rows[rowindex].Cells[8].Value.ToString();
                    texZKOMs2.Text = dOutGridViewZK.Rows[rowindex].Cells[0].Value.ToString();
                    texZKOMs3.Text = dOutGridViewZK.Rows[rowindex].Cells[10].Value.ToString();
                    texZKOMs4.Text = dOutGridViewZK.Rows[rowindex].Cells[5].Value.ToString();
                    texZKOMs5.Text = dOutGridViewZK.Rows[rowindex].Cells[3].Value.ToString();
                    texZKOMs6.Text = dOutGridViewZK.Rows[rowindex].Cells[2].Value.ToString();
                    texZKOMs7.Text = dOutGridViewZK.Rows[rowindex].Cells[6].Value.ToString();
                    texZKOMs8.Text = dOutGridViewZK.Rows[rowindex].Cells[7].Value.ToString();
                    texZKOMs9.Text = dOutGridViewZK.Rows[rowindex].Cells[4].Value.ToString();


                }
            }
        }
        /// <summary>
        /// 破损
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void butMBPo_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(" 磨边   您确定该片破损 序号： " + texMsIn1.Text + "  流程卡号 " + texMsIn2.Text + " 尺寸 " + texMsIn5.Text + "*" + texMsIn4.Text + " 破损?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texMsIn1.Text, "", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }

        }
        private void butMBINPo_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  磨边仓储入库  您确定该片破损 序号： " + texMs1.Text + "  流程卡号 " + texMs2.Text + " 尺寸 " + texMs5.Text + "*" + texMs4.Text + " 破损?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texMs1.Text, "", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }
        private void butMBOUTPo_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  磨边仓储出库  您确定该片破损 序号： " + texOMs1.Text + "  流程卡号 " + texOMs2.Text + " 尺寸 " + texOMs5.Text + "*" + texOMs4.Text + " 破损?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texOMs1.Text, "", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }
        private void butZKINPo_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  中空入库  您确定该片破损 序号： " + texZKIMs1.Text + "  流程卡号 " + texZKIMs2.Text + " 尺寸 " + texZKIMs6.Text + "*" + texZKIMs5.Text + " 破损?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texZKIMs1.Text, "", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }
        private void buZKBPo_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  中空出库  您确定该片破损 序号： " + texZKOMs1.Text + "  流程卡号 " + texZKOMs2.Text + " 尺寸 " + texZKOMs6.Text + "*" + texZKOMs5.Text + " 破损?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texZKOMs1.Text, "", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void butMBChaPo_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  磨边查询  您确定该片破损 序号： " + texMBWhC1.Text + "  流程卡号 " + texMBWhC2.Text + " 尺寸 " + texMBWhC5.Text + "*" + texMBWhC4.Text + " 破损?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texMBWhC1.Text, "", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void butZKChaPo_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("   中空查询 您确定该片破损 序号： " + texZKWhC1.Text + "  流程卡号 " + texZKWhC2.Text + " 尺寸 " + texZKWhC5.Text + "*" + texZKWhC4.Text + " 破损?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texZKWhC1.Text, "", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }
        private void butXChaPo_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  详细  您确定该片破损 序号： " + texXCha1.Text + "  流程卡号 " + texXCha2.Text + " 尺寸 " + texXCha5.Text + "*" + texXCha4.Text + " 破损?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texXCha1.Text, "", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }

        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butMBStart_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("   磨边入口 您确定该片初始化 序号： " + texMsIn1.Text + "  流程卡号 " + texMsIn2.Text + " 尺寸 " + texMsIn5.Text + "*" + texMsIn4.Text + " 初始化?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texMsIn1.Text, "1", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void butMBINStart_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(" 磨边仓储入库   您确定该片初始化 序号： " + texMs1.Text + "  流程卡号 " + texMs2.Text + " 尺寸 " + texMs5.Text + "*" + texMs4.Text + " 初始化?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texMs1.Text, "1", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void butZKINStart_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  中空入库  您确定该片中空入口初始化 序号： " + texZKIMs1.Text + "  流程卡号 " + texZKIMs2.Text + " 尺寸 " + texZKIMs6.Text + "*" + texZKIMs5.Text + " 中空初始化?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texZKIMs1.Text, "30", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }
        private void butMBChaStart_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  磨边仓储查询  您确定该片初始化 序号： " + texMBWhC1.Text + "  流程卡号 " + texMBWhC2.Text + " 尺寸 " + texMBWhC5.Text + "*" + texMBWhC4.Text + " 初始化?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texMBWhC1.Text, "1", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }
        private void butZKChaStart_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  中空仓储查询  您确定该片初始化 序号： " + texZKWhC1.Text + "  流程卡号 " + texZKWhC2.Text + " 尺寸 " + texZKWhC5.Text + "*" + texZKWhC4.Text + " 初始化?   \r\n     请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texZKWhC1.Text, "1", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  中空仓储查询  您确定该片中空入口初始化 序号： " + texZKWhC1.Text + "  流程卡号 " + texZKWhC2.Text + " 尺寸 " + texZKWhC5.Text + "*" + texZKWhC4.Text + " 中空初始化?   \r\n     请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texZKWhC1.Text, "30", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }
        private void butXChaZKIn_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  详细  您确定该片中空入口初始化 序号： " + texXCha1.Text + "  流程卡号 " + texXCha2.Text + " 尺寸 " + texXCha5.Text + "*" + texXCha4.Text + " 中空初始化?   \r\n     请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texXCha1.Text, "30", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }
        private void butXChaStart_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  详细  您确定该片初始化 序号： " + texXCha1.Text + "  流程卡号 " + texXCha2.Text + " 尺寸 " + texXCha5.Text + "*" + texXCha4.Text + " 初始化?   \r\n     请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texXCha1.Text, "1", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }
        /// <summary>
        /// 确认磨边
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butMBMB_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("   磨边入口 您确定该片已经进磨边 序号： " + texMsIn1.Text + "  流程卡号 " + texMsIn2.Text + " 尺寸 " + texMsIn5.Text + "*" + texMsIn4.Text + " 已经进磨边?   \r\n     请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texMsIn1.Text, "3", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void butMBINMB_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(" 磨边仓储入库   您确定该片已经进磨边 序号： " + texMs1.Text + "  流程卡号 " + texMs2.Text + " 尺寸 " + texMs5.Text + "*" + texMs4.Text + " 已经进磨边?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texMs1.Text, "3", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void butMBChaMB_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  磨边仓储查询  您确定该片已经进磨边 序号： " + texMBWhC1.Text + "  流程卡号 " + texMBWhC2.Text + " 尺寸 " + texMBWhC5.Text + "*" + texMBWhC4.Text + " 已经进磨边?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texMBWhC1.Text, "3", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }
        private void butXChaMB_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  详细查询  您确定该片已经进磨边 序号： " + texXCha1.Text + "  流程卡号 " + texXCha1.Text + " 尺寸 " + texXCha1.Text + "*" + texXCha1.Text + " 已经进磨边?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texXCha1.Text, "3", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }
        /// <summary>
        /// 确认入库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void butINFinish_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(" 磨边仓储入库   您确定该片已经进入库 序号： " + texMs1.Text + "  流程卡号 " + texMs2.Text + " 尺寸 " + texMs5.Text + "*" + texMs4.Text + " 已经进入库?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                
                string tRetData = "", tRetStr = ""; int tRows = 0;
                tSystem.mClsDBUPdate.ExecutePro("Pro_11FinishCmdIn", 100, texMs12.Text.Trim().ToString(), 0, int.Parse(texMs1.Text.Trim()), 0,ref tRetData, ref tRetStr, ref tRows);
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void butMBOUTFinish_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("     磨边仓储出库   您确定该片已经进入库 序号： " + texOMs1.Text + "  流程卡号 " + texOMs2.Text + " 尺寸 " + texOMs5.Text + "*" + texOMs4.Text + " 已经进入库?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = ""; int tRows = 0;
                tSystem.mClsDBUPdate.ExecutePro("Pro_11FinishCmdIn", 100, texOMs3.Text.Trim().ToString(), 0, int.Parse(texOMs1.Text.Trim()), 0, ref tRetData, ref tRetStr, ref tRows);
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void butZKINFinish_Click(object sender, EventArgs e)
        {
            int tTroNo = 0;
            if(IfNum(texZKIMs7.Text))
            { if (int.Parse(texZKIMs7.Text) <= 3 & int.Parse(texZKIMs7.Text) >= 1) { tTroNo = 1; } else if (int.Parse(texZKIMs7.Text) <= 8 & int.Parse(texZKIMs7.Text) >= 4) { tTroNo = 1; } else { return; } }
            DialogResult dr = MessageBox.Show("     中空入库   您确定该片已经进中空入库 序号： " + texZKIMs1.Text + "  流程卡号 " + texZKIMs2.Text + " 尺寸 " + texZKIMs5.Text + "*" + texZKIMs4.Text + " 已经进中空入库?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = ""; int tRows = 0;
                tSystem.mClsDBUPdate.ExecutePro("Pro_31FinishCmdIn", 100, texZKIMs4.Text.Trim().ToString(), tTroNo, int.Parse(texZKIMs1.Text.Trim()), 0, ref tRetData, ref tRetStr, ref tRows);
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void buZKBFinish_Click(object sender, EventArgs e)
        {
            int tTroNo = 0;
            if (IfNum(texZKOMs7.Text))
            { if (int.Parse(texZKOMs7.Text) <= 3 & int.Parse(texZKOMs7.Text) >= 1) { tTroNo = 1; } else if (int.Parse(texZKOMs7.Text) <= 6 & int.Parse(texZKOMs7.Text) >= 4) { tTroNo = 1; } else { return; } }
            DialogResult dr = MessageBox.Show("     中空出库   您确定该片已经进中空入库 序号： " + texZKOMs1.Text + "  流程卡号 " + texZKOMs2.Text + " 尺寸 " + texZKOMs5.Text + "*" + texZKOMs4.Text + " 已经进中空入库?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = ""; int tRows = 0;
                tSystem.mClsDBUPdate.ExecutePro("Pro_31FinishCmdIn", 100, texZKOMs4.Text.Trim().ToString(), tTroNo, int.Parse(texZKOMs1.Text.Trim()), 0, ref tRetData, ref tRetStr, ref tRows);
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void butMBChaFinish_Click(object sender, EventArgs e)//texMBWhC
        {
            DialogResult dr = MessageBox.Show("     磨边仓储查询   您确定该片已经进入库 序号： " + texMBWhC1.Text + "  流程卡号 " + texMBWhC2.Text + " 尺寸 " + texOMs5.Text + "*" + texMBWhC4.Text + " 已经进入库?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = ""; int tRows = 0;
                tSystem.mClsDBUPdate.ExecutePro("Pro_11FinishCmdIn", 100, texMBWhC3.Text.Trim().ToString(), 0, int.Parse(texMBWhC1.Text.Trim()), 0, ref tRetData, ref tRetStr, ref tRows);
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void butMBXChaFinish_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("     详细   您确定该片已经进入库 序号： " + texXCha1.Text + "  流程卡号 " + texXCha2.Text + " 尺寸 " + texOMs5.Text + "*" + texXCha4.Text + " 已经进入库?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = ""; int tRows = 0;
                tSystem.mClsDBUPdate.ExecutePro("Pro_11FinishCmdIn", 100, texXCha3.Text.Trim().ToString(), 0, int.Parse(texXCha1.Text.Trim()), 0, ref tRetData, ref tRetStr, ref tRows);
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void butZKChaFinish_Click(object sender, EventArgs e)
        {
            int tTroNo = 0;
            if (IfNum(texZKWhC7.Text))
            { if (int.Parse(texZKWhC6.Text) <= 3 & int.Parse(texZKWhC6.Text) >= 1) { tTroNo = 1; } else if (int.Parse(texZKWhC6.Text) <= 8 & int.Parse(texZKWhC6.Text) >= 4) { tTroNo = 1; } else { return; } }
            DialogResult dr = MessageBox.Show("     中空仓储查询   您确定该片已经进中空入库 序号： " + texZKWhC1.Text + "  流程卡号 " + texZKWhC2.Text + " 尺寸 " + texZKWhC5.Text + "*" + texZKWhC4.Text + " 已经进中空入库?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = ""; int tRows = 0;
                tSystem.mClsDBUPdate.ExecutePro("Pro_31FinishCmdIn", 100, texZKWhC3.Text.Trim().ToString(), tTroNo, int.Parse(texZKWhC1.Text.Trim()), 0, ref tRetData, ref tRetStr, ref tRows);
                tSystem.mClsDBUPdate.DBClose();
            }
        }
        private void butZKXChaFinish_Click(object sender, EventArgs e)
        {
            int tTroNo = 0;
            if (IfNum(texXCha7.Text))
            { if (int.Parse(texXCha6.Text) <= 3 & int.Parse(texXCha6.Text) >= 1) { tTroNo = 1; } else if (int.Parse(texXCha6.Text) <= 8 & int.Parse(texXCha6.Text) >= 4) { tTroNo = 1; } else { return; } }
            DialogResult dr = MessageBox.Show("     中空仓储查询   您确定该片已经进中空入库 序号： " + texXCha1.Text + "  流程卡号 " + texXCha2.Text + " 尺寸 " + texXCha5.Text + "*" + texXCha4.Text + " 已经进中空入库?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = ""; int tRows = 0;
                tSystem.mClsDBUPdate.ExecutePro("Pro_31FinishCmdIn", 100, texXCha3.Text.Trim().ToString(), tTroNo, int.Parse(texXCha1.Text.Trim()), 0, ref tRetData, ref tRetStr, ref tRows);
                tSystem.mClsDBUPdate.DBClose();
            }
        }


      
        private void butMBOUTExitFinish_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("     磨边仓储出库   您确定该片已经出库 序号： " + texOMs1.Text + "  流程卡号 " + texOMs2.Text + " 尺寸 " + texOMs5.Text + "*" + texOMs4.Text + " 已经进出库?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = ""; int tRows = 0;
                tSystem.mClsDBUPdate.ExecuteProFinishOut("Pro_21FinishCmdOut", 100, 0, int.Parse(texOMs3.Text.Trim()), 0, 0, int.Parse(texOMs1.Text.Trim()), 0,ref tRetData, ref tRetStr, ref tRows);
              
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void buZKBExitFinish_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("     中空出库   您确定该片已经出库 序号： " + texZKOMs1.Text + "  流程卡号 " + texZKOMs2.Text + " 尺寸 " + texZKOMs5.Text + "*" + texZKOMs4.Text + " 已经进出库?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = ""; int tRows = 0;
                tSystem.mClsDBUPdate.ExecuteProFinishOut("Pro_41FinishCmdOut", 100, 0, int.Parse(texZKOMs4.Text.Trim()), 0, 0, int.Parse(texZKOMs1.Text.Trim()), 0,ref tRetData, ref tRetStr, ref tRows);

                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void butMBChaExitFinish_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("     磨边仓储查询   您确定该片已经出库 序号： " + texMBWhC1.Text + "  流程卡号 " + texMBWhC2.Text + " 尺寸 " + texMBWhC5.Text + "*" + texMBWhC4.Text + " 已经进出库?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = ""; int tRows = 0;
                tSystem.mClsDBUPdate.ExecuteProFinishOut("Pro_21FinishCmdOut", 100, 0, int.Parse(texMBWhC3.Text.Trim()), 0, 0, int.Parse(texMBWhC1.Text.Trim()),0, ref tRetData, ref tRetStr, ref tRows);

                tSystem.mClsDBUPdate.DBClose();
            }
        }
        private void butXChaExitFinish_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("     详细   您确定该片已经出库 序号： " + texXCha1.Text + "  流程卡号 " + texXCha2.Text + " 尺寸 " + texXCha5.Text + "*" + texXCha4.Text + " 已经进出库?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = ""; int tRows = 0;
                tSystem.mClsDBUPdate.ExecuteProFinishOut("Pro_21FinishCmdOut", 100, 0, int.Parse(texXCha3.Text.Trim()), 0, 0, int.Parse(texXCha1.Text.Trim()),0, ref tRetData, ref tRetStr, ref tRows);

                tSystem.mClsDBUPdate.DBClose();
            }
        }
        private void butZKChaExitFinish_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("     中空查询   您确定该片已经出库 序号： " + texZKWhC1.Text + "  流程卡号 " + texZKWhC2.Text + " 尺寸 " + texZKWhC5.Text + "*" + texZKWhC4.Text + " 已经进出库?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = ""; int tRows = 0;
                tSystem.mClsDBUPdate.ExecuteProFinishOut("Pro_41FinishCmdOut", 100, 0, int.Parse(texZKWhC3.Text.Trim()), 0, 0, int.Parse(texZKWhC1.Text.Trim()),0, ref tRetData, ref tRetStr, ref tRows);

                tSystem.mClsDBUPdate.DBClose();
            }
        }
        private void butZKXChaExitFinish_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("     详细   您确定该片已经出库 序号： " + texXCha1.Text + "  流程卡号 " + texXCha2.Text + " 尺寸 " + texXCha5.Text + "*" + texXCha4.Text + " 已经进出库?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = ""; int tRows = 0;
                tSystem.mClsDBUPdate.ExecuteProFinishOut("Pro_41FinishCmdOut", 100, 0, int.Parse(texXCha3.Text.Trim()), 0, 0, int.Parse(texXCha1.Text.Trim()),0, ref tRetData, ref tRetStr, ref tRows);

                tSystem.mClsDBUPdate.DBClose();
            }
        }

        Common.ClsPrintDocument tClsPrintDocument = new Common.ClsPrintDocument(clsMyPublic.mPrintName);
        private void butZKXChaSinglePrint_Click(object sender, EventArgs e)//单片
        {
            if (dataGridViewCha.CurrentRow != null)
            {
                if (dataGridViewCha.Rows.Count > 0)
                {
                    //string rowFirstCell;
                    //for (int i = 0; i < dataGridViewCha.SelectedRows.Count; i++)
                    //{
                    //    rowFirstCell = dataGridViewCha.SelectedRows[i].Cells[0].Value.ToString();
                    //    //在这里把rowFirstCell做为参数调用你的其他方法.
                    //}
               
                    int tSelIndex = dataGridViewCha.CurrentRow.Index;//默认是0
                    string[] tPrintData = new string[6];
                    if (dataGridViewCha.Columns.Count > 0)
                    {
                        for (int i = 0; i < dataGridViewCha.Columns.Count; i++)
                        {
                            switch (dataGridViewCha.Columns[i].HeaderText.ToString())
                            {
                                case "项目名称":
                                    tPrintData[0] = dataGridViewCha.Rows[tSelIndex].Cells[i].Value.ToString();
                                    break;
                                case "订单号":
                                    tPrintData[1] = dataGridViewCha.Rows[tSelIndex].Cells[i].Value.ToString();
                                    break;
                                case "规格":
                                    tPrintData[2] = dataGridViewCha.Rows[tSelIndex].Cells[i].Value.ToString();
                                    break;
                                case "产品配置":
                                    tPrintData[3] = dataGridViewCha.Rows[tSelIndex].Cells[i].Value.ToString();
                                    break;
                            }
                        }
                        tPrintData[4] = clsMyPublic.mPrintLine;
                        tPrintData[5] = clsMyPublic.mPrintQC;
                        tClsPrintDocument.MyManlprinter(tPrintData);
                    }
                }
               
            }
        }
        private void butZKXChaMulSelectPrint_Click(object sender, EventArgs e)//选择多片打印
        {
            if (dataGridViewCha.CurrentRow != null)
            {
                if (dataGridViewCha.SelectedRows.Count > 0)
                {
                    //for (int jj = 0; jj < 5; jj++)
                    {
                        int tSelIndex = 0;//默认是0
                        string[] tPrintData = new string[6];
                        string tOrder_singleid = "", tOrder_singleidlog = "";
                        int tPCount = 1;
                        for (int kk = 0; kk < dataGridViewCha.SelectedRows.Count; kk++)
                        {
                            tSelIndex = kk;
                            if (dataGridViewCha.Columns.Count > 0)
                            {
                                for (int i = 0; i < dataGridViewCha.Columns.Count; i++)
                                {
                                    switch (dataGridViewCha.Columns[i].HeaderText.ToString())
                                    {

                                        case "项目名称":
                                            tPrintData[0] = dataGridViewCha.SelectedRows[tSelIndex].Cells[i].Value.ToString();
                                            break;
                                        case "订单号":
                                            tPrintData[1] = dataGridViewCha.SelectedRows[tSelIndex].Cells[i].Value.ToString();
                                            break;
                                        case "规格":
                                            tPrintData[2] = dataGridViewCha.SelectedRows[tSelIndex].Cells[i].Value.ToString();
                                            break;
                                        case "产品配置":
                                            tPrintData[3] = dataGridViewCha.SelectedRows[tSelIndex].Cells[i].Value.ToString();
                                            break;
                                        case "成品单片编号":
                                            tOrder_singleid = dataGridViewCha.SelectedRows[tSelIndex].Cells[i].Value.ToString();
                                            break;
                                    }
                                }
                                tPrintData[4] = clsMyPublic.mPrintLine;
                                tPrintData[5] = clsMyPublic.mPrintQC;// +"         " + tPCount.ToString();
                                if (tOrder_singleid != tOrder_singleidlog)
                                {
                                    tPCount = tPCount + 1;
                                    tClsPrintDocument.MyManlprinter(tPrintData);
                                    label27.Text = "0";
                                }
                                else
                                {
                                }
                                tOrder_singleidlog = tOrder_singleid;
                            }
                        }
                    }
                }

            }
        }
        private void butZKXChaMulPrint_Click(object sender, EventArgs e)//整个查询打印
        {
            if (dataGridViewCha.CurrentRow != null)
            {
                if (dataGridViewCha.Rows.Count > 0)
                {
                    //for (int jj = 0; jj < 5; jj++)
                    {
                        int tSelIndex = 0;//默认是0
                        string[] tPrintData = new string[6];
                        string tOrder_singleid = "", tOrder_singleidlog = "";
                        int tPCount = 1;
                        for (int kk = 0; kk < dataGridViewCha.Rows.Count - 1; kk++)
                        {
                            tSelIndex = kk;
                            if (dataGridViewCha.Columns.Count > 0)
                            {
                                for (int i = 0; i < dataGridViewCha.Columns.Count; i++)
                                {
                                    switch (dataGridViewCha.Columns[i].HeaderText.ToString())
                                    {

                                        case "项目名称":
                                            tPrintData[0] = dataGridViewCha.Rows[tSelIndex].Cells[i].Value.ToString();
                                            break;
                                        case "订单号":
                                            tPrintData[1] = dataGridViewCha.Rows[tSelIndex].Cells[i].Value.ToString();
                                            break;
                                        case "规格":
                                            tPrintData[2] = dataGridViewCha.Rows[tSelIndex].Cells[i].Value.ToString();
                                            break;
                                        case "产品配置":
                                            tPrintData[3] = dataGridViewCha.Rows[tSelIndex].Cells[i].Value.ToString();
                                            break;
                                        case "成品单片编号":
                                            tOrder_singleid = dataGridViewCha.Rows[tSelIndex].Cells[i].Value.ToString();
                                            break;
                                    }
                                }
                                tPrintData[4] = clsMyPublic.mPrintLine;
                                tPrintData[5] = clsMyPublic.mPrintQC;// +"         " + tPCount.ToString();
                                if (tOrder_singleid != tOrder_singleidlog)
                                {
                                    tPCount = tPCount + 1;
                                    tClsPrintDocument.MyManlprinter(tPrintData);
                                }
                                else
                                {
                                }
                                tOrder_singleidlog = tOrder_singleid;
                            }
                        }
                    }
                }

            }
        }
        private void butZKSinglePrint_Click(object sender, EventArgs e)//选择片打印流程卡 
        {
            if (dOutGridViewZK.CurrentRow != null)
            {
                if (dOutGridViewZK.SelectedRows.Count > 0)
                {
                    //for (int jj = 0; jj < 5; jj++)
                    {
                        int tSelIndex = 0;//默认是0
                        string[] tPrintData = new string[6];
                        string tOrder_singleid = "", tOrder_singleidlog = "";
                        int tPCount = 1;
                        for (int kk = 0; kk < dOutGridViewZK.SelectedRows.Count; kk++)
                        {
                            tSelIndex = kk;
                            if (dOutGridViewZK.Columns.Count > 0)
                            {
                                for (int i = 0; i < dOutGridViewZK.Columns.Count; i++)
                                {
                                    switch (dOutGridViewZK.Columns[i].HeaderText.ToString())
                                    {

                                        case "项目名称":
                                            tPrintData[0] = dOutGridViewZK.SelectedRows[tSelIndex].Cells[i].Value.ToString();
                                            break;
                                        case "订单号":
                                            tPrintData[1] = dOutGridViewZK.SelectedRows[tSelIndex].Cells[i].Value.ToString();
                                            break;
                                        case "规格":
                                            tPrintData[2] = dOutGridViewZK.SelectedRows[tSelIndex].Cells[i].Value.ToString();
                                            break;
                                        case "产品配置":
                                            tPrintData[3] = dOutGridViewZK.SelectedRows[tSelIndex].Cells[i].Value.ToString();
                                            break;
                                        case "成品单片编号":
                                            tOrder_singleid = dOutGridViewZK.SelectedRows[tSelIndex].Cells[i].Value.ToString();
                                            break;
                                    }
                                }
                                tPrintData[4] = clsMyPublic.mPrintLine;
                                tPrintData[5] = clsMyPublic.mPrintQC;// +"         " + tPCount.ToString();
                                if (tOrder_singleid != tOrder_singleidlog)
                                {
                                    tPCount = tPCount + 1;
                                    tClsPrintDocument.MyManlprinter(tPrintData);
                                    labZKOUT001.Text = "0";
                                }
                                else
                                {
                                }
                                tOrder_singleidlog = tOrder_singleid;
                            }
                        }
                    }
                }

            }
        }

        
        private void butZKProPrint_Click(object sender, EventArgs e)//中空正流程卡打印
        {
            if (dOutGridViewZK.CurrentRow != null)
            {
                if (dOutGridViewZK.Rows.Count > 0)
                {
                    //for (int jj = 0; jj < 5; jj++)
                    {
                        int tSelIndex = 0;//默认是0
                        string[] tPrintData = new string[6];
                        string tOrder_singleid = "", tOrder_singleidlog = "";
                        int tPCount = 1;
                        for (int kk = 0; kk < dOutGridViewZK.Rows.Count - 1; kk++)
                        {
                            tSelIndex = kk;
                            if (dOutGridViewZK.Columns.Count > 0)
                            {
                                for (int i = 0; i < dOutGridViewZK.Columns.Count; i++)
                                {
                                    switch (dOutGridViewZK.Columns[i].HeaderText.ToString())
                                    {

                                        case "项目名称":
                                            tPrintData[0] = dOutGridViewZK.Rows[tSelIndex].Cells[i].Value.ToString();
                                            break;
                                        case "订单号":
                                            tPrintData[1] = dOutGridViewZK.Rows[tSelIndex].Cells[i].Value.ToString();
                                            break;
                                        case "规格":
                                            tPrintData[2] = dOutGridViewZK.Rows[tSelIndex].Cells[i].Value.ToString();
                                            break;
                                        case "产品配置":
                                            tPrintData[3] = dOutGridViewZK.Rows[tSelIndex].Cells[i].Value.ToString();
                                            break;
                                        case "成品单片编号":
                                            tOrder_singleid = dOutGridViewZK.Rows[tSelIndex].Cells[i].Value.ToString();
                                            break;
                                    }
                                }
                                tPrintData[4] = clsMyPublic.mPrintLine;
                                tPrintData[5] = clsMyPublic.mPrintQC;// +"         " + tPCount.ToString();
                                if (tOrder_singleid != tOrder_singleidlog)
                                {
                                    tPCount = tPCount + 1;
                                    tClsPrintDocument.MyManlprinter(tPrintData);
                                }
                                else
                                {
                                }
                                tOrder_singleidlog = tOrder_singleid;
                            }
                        }
                    }
                }

            }
        }

        private void butZKIFPrint_Click(object sender, EventArgs e)
        {
            string tSqlStr = "";
            if (butZKIFPrint.Text == "打开自动打印")
            {
                tSqlStr = "update TabZK_OutCmd set IfPrint ='1' where Out_to ='"+clsMyPublic.mZKExit+"'";
                labZKIFPrint.Text = "中空" + clsMyPublic.mZKExit + "线自动打印打开";
                butZKIFPrint.Text = "关闭自动打印";
            }
            else
            {
                tSqlStr = "update TabZK_OutCmd set IfPrint ='0' where Out_to ='" + clsMyPublic.mZKExit + "'";
                labZKIFPrint.Text = "中空" + clsMyPublic.mZKExit + "线自动打印关闭";
                butZKIFPrint.Text = "打开自动打印";
            }
            tSystem.mClsDBUPdate.Execute_Command(tSqlStr);
            tSystem.mClsDBUPdate.DBClose();
        }
        private void buzZKWHSet1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("    您确定对中空仓储  " + ZKcbSet1.Text + "  格号 " + ZKcbSet2.Text + " 进行禁用?   \r\n   后果不能中空进库   ", "警告", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tSqlStr = "";
                if (ZKcbSet2.Text == "0")
                {
                    tSqlStr = string.Concat("  update tabZK_WH set WH_If =1 where WH_NO ='", ZKcbSet1.Text, "'  ");
                }
                else
                {
                    tSqlStr = string.Concat("  update tabZK_WH set WH_If =1 where WH_NO ='", ZKcbSet1.Text, "' and WH_Num='", ZKcbSet2.Text, "' ");
                }
                tSystem.mClsDBUPdate.Execute_Command(tSqlStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void buzZKWHSet2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("    您确定对中空仓储 库号 " + ZKcbSet1.Text + "  格号 " + ZKcbSet2.Text + "进行使用?   \r\n   后果该库位可以进库   ", "警告", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tSqlStr = "";
                if (ZKcbSet2.Text == "0")
                {
                    tSqlStr = string.Concat("  update tabZK_WH set WH_If =0 where WH_NO ='", ZKcbSet1.Text, "'  ");
                }
                else
                {
                    tSqlStr = string.Concat("  update tabZK_WH set WH_If =0 where WH_NO ='", ZKcbSet1.Text, "' and WH_Num='", ZKcbSet2.Text, "'");
                }
                tSystem.mClsDBUPdate.Execute_Command(tSqlStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void buzZKWHSet3_Click(object sender, EventArgs e)
        {
            if (ZKcbSet1.Text == "0")
            {
                GetdataZKWH("");
            }
            else if (IfNum(ZKcbSet1.Text.Trim()) & IfNum(ZKcbSet2.Text.Trim()))
            {
                if (ZKcbSet2.Text.Trim() == "0")
                {
                    GetdataZKWH(" and a.WH_NO='" + ZKcbSet1.Text.Trim() + "'  ");
                }
                else
                {
                    GetdataZKWH(" and a.WH_NO='" + ZKcbSet1.Text.Trim() + "' and a.WH_Num='" + ZKcbSet2.Text.Trim() + "' ");
                }
            }
            else if (IfNum(ZKcbSet1.Text.Trim()) )
            {
                GetdataZKWH(" and a.WH_NO='" + ZKcbSet1.Text.Trim() + "'  ");
            }
        }
        private void GetdataZKWH(string _WhereStr)
        {
            try
            {
                string tSqlstr = "";
                DataTable tDT = new DataTable();
                tSqlstr = "SELECT rank() over(order by a.whid,b.InAtime ) 'ID号', a.[WH_NO] as '仓库号',a.[WH_Num]as '库位号',a.[WH_Status]'存储数量',case a.[WH_If] when 0 then '正常' else '禁用'end 库位状态,b.Optimize_batch '优化单号' ,b.Process_number '流程卡号',b.Single_short '短边',b.Single_long  '长边' ,b.ZKInAtime  '入库时间'"
                 //+ " ,case when a.[WH_If]='1' then '禁用' when b.status=10 then '准备进库' when b.status=14 then '库内' when b.status=20 then '准备出库' when b.status=24 then '离库'  when b.status=199 then '入库未完成'  when b.status=299 then '出库未完成' when b.status=99 then '不符合尺寸'when b.status= 96 then '破损'when b.status=97 then '设备异常' when b.status=120 then '空取'"
                 //+ " when b.status=140 then '中空空取' when b.status=399 then '入中空未完成'  when b.status=499 then '出中空未完成'  when  b.status='30' then '准备进中空' when  b.status='34' then '中空库' when  b.status='40' then '准备出中空' when  b.status='44' then '离开中空'when b.Status  is null then '' else '异常'end '状态'"
                 + ",(select top 1 isnull(c.statusName,b.Status ) from StatusNameT c where b.Status =c.Status order by case when b.Err_Tag>0 and c.AssistSatusS =1 then 0 else 1 end ,c.AssistSatusS  asc) '状态'"
                 +",b.Single_tag ,b.Single_name '单片玻璃名称',b.Single_Str '玻璃组成',b.Order_length '成品长',b.Order_width '成品宽' "
                 + ",b.WcsID '序号',b.zkPlcFlow 'PLC流水号',b.Order_singleid '成品单片编号',a.InAtime '磨边入库时间',a.InputFlow "
                 + " FROM tabZK_WH a left join TabMB_Data b on  CHARINDEX(convert(varchar,b.zkPlcFlow),a.InputFlow )>0 and  a.WH_NO =b.zkWH_NO and a.WH_Num =b.ZKWH_Num  and ((b.status>=30 and b.Status <44 ) or (b.Status >90 ))  "
                            + " where a.whid>0  " + _WhereStr + " order by a.whid,b.InAtime ";
                if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlstr, ref tDT) == "")
                {
                    if (tDT.Rows.Count > 0)
                    {
                        this.dataZKWH.DataSource = tDT;
                        labZKshu.Text = tDT.Rows.Count.ToString();
                        tClsShowFrm.SetBackColor(dataZKWH, 10, 4);
                        for (int i = 0; i < this.dataZKWH.Columns.Count; i++)
                        {
                            dataZKWH.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            this.dataZKWH.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                    }
                    tSystem.mClsDBUPdate.DBClose();
                }
            }
            catch
            {
            }

        }

        private void buzZKWHSet6_Click(object sender, EventArgs e)
        {
            try
            {
                string tSqlstr = "";
                DataTable tDT = new DataTable();
                tSqlstr = "SELECT rank() over(order by a.whid,b.InAtime ) 'ID号', a.[WH_NO] as '仓库号',a.[WH_Num]as '库位号',a.[WH_Status]'存储数量',case a.[WH_If] when 0 then '正常' else '禁用'end 库位状态,b.Optimize_batch '优化单号' ,b.Process_number '流程卡号',b.Single_short '短边',b.Single_long  '长边' ,b.InAtime  '入库时间'"
                 //+ " ,case when a.[WH_If]='1' then '禁用' when b.status=10 then '准备进库' when b.status=14 then '库内' when b.status=20 then '准备出库' when b.status=24 then '离库'  when b.status=199 then '入库未完成'  when b.status=299 then '出库未完成' when b.status=99 then '不符合尺寸'when b.status= 96 then '破损'when b.status=97 then '设备异常' when b.status=120 then '空取'"
                 //+ "  when  b.status='30' then '准备进中空' when  b.status='34' then '中空库' when  b.status='40' then '准备出中空' when  b.status='44' then '离开中空' when b.Status='140' then '中空空取' when b.status='399' then '入中空未完成' when b.status='499' then '出中空未完成' when b.Status  is null then '' else '异常'end '状态'"
                 + ",(select top 1 isnull(c.statusName,b.Status ) from StatusNameT c where b.Status =c.Status order by case when b.Err_Tag>0 and c.AssistSatusS =1 then 0 else 1 end,c.AssistSatusS  asc ) '状态'"
                 +",b.Single_tag ,b.Single_name '单片玻璃名称',b.Single_Str '玻璃组成',b.Order_length '成品长',b.Order_width '成品宽' "
                 + ",b.WcsID '序号',b.MBPlcFlow 'PLC流水号',b.Order_singleid '成品单片编号'"
                 + " FROM tabZK_WH a  join TabMB_Data b on a.WH_NO =b.zkWH_NO and a.WH_Num =b.ZKWH_Num  and (b.status=34 or b.status=30)    "
                            + "  order by a.whid,b.InAtime ";
                if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlstr, ref tDT) == "")
                {
                    if (tDT.Rows.Count > 0)
                    {
                        this.dataZKWH.DataSource = tDT;
                        labZKshu.Text = tDT.Rows.Count.ToString();
                        tClsShowFrm.SetBackColor(dataZKWH, 10, 4);
                        for (int i = 0; i < this.dataZKWH.Columns.Count; i++)
                        {
                            dataZKWH.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            this.dataZKWH.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                    }
                    tSystem.mClsDBUPdate.DBClose();
                }
            }
            catch
            {
            }
        }
        private void buzZKWHSet5_Click(object sender, EventArgs e)
        {
            GetdataZKWH(" and a.[WH_Status]='0'");
        }

        private void buzZKWHSet4_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("   中空库 手动进行出库操作 库号： " + ZKcbSet1.Text + "  格号 " + ZKcbSet2.Text + " 设备异常?   \r\n   请看清库位编号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                Common.ClsDbAcc tDbAccSet = new Common.ClsDbAcc(tSystem);

                string tSqlStr = "";
                if (IfNum(this.ZKcbSet1.Text.Trim()) == true & IfNum(this.ZKcbSet2.Text.Trim()) == true)
                {
                    tSqlStr = string.Concat("Update tab_Sys set oForce_Tag='1',oForce_WH_NO='", this.ZKcbSet1.Text.Trim(), "' ,oForce_WH_Num='", this.ZKcbSet2.Text.Trim(), "'  where sys_no ='2'");
                    tDbAccSet.Execute_Command(tSqlStr);
                    tDbAccSet.DBClose();
                }
            }
        }

        private void dataZKWH_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = e.RowIndex;

            if (this.dataZKWH.Rows.Count > 1 && rowindex >= 0)
            {

                if (this.dataZKWH.CurrentRow.Index == 0)
                {

                    texZKWhC1.Text = dataZKWH.Rows[0].Cells[16].Value.ToString();
                    texZKWhC2.Text = dataZKWH.Rows[0].Cells[6].Value.ToString();
                    texZKWhC3.Text = dataZKWH.Rows[0].Cells[17].Value.ToString();
                    texZKWhC4.Text = dataZKWH.Rows[0].Cells[8].Value.ToString();
                    texZKWhC5.Text = dataZKWH.Rows[0].Cells[7].Value.ToString();
                    texZKWhC6.Text = dataZKWH.Rows[0].Cells[1].Value.ToString();
                    texZKWhC7.Text = dataZKWH.Rows[0].Cells[2].Value.ToString();
                    texZKWhC8.Text = dataZKWH.Rows[0].Cells[10].Value.ToString();

                }
                else
                {

                    texZKWhC1.Text = dataZKWH.Rows[rowindex].Cells[16].Value.ToString();
                    texZKWhC2.Text = dataZKWH.Rows[rowindex].Cells[6].Value.ToString();
                    texZKWhC3.Text = dataZKWH.Rows[rowindex].Cells[17].Value.ToString();
                    texZKWhC4.Text = dataZKWH.Rows[rowindex].Cells[8].Value.ToString();
                    texZKWhC5.Text = dataZKWH.Rows[rowindex].Cells[7].Value.ToString();
                    texZKWhC6.Text = dataZKWH.Rows[rowindex].Cells[1].Value.ToString();
                    texZKWhC7.Text = dataZKWH.Rows[rowindex].Cells[2].Value.ToString();
                    texZKWhC8.Text = dataZKWH.Rows[rowindex].Cells[10].Value.ToString();


                }
            }
        }

        private void button16_Click(object sender, EventArgs e)//
        {
            DialogResult dr = MessageBox.Show("     中空入库   您确定该片已经出库 序号： " + texZKIMs1.Text + "  流程卡号 " + texZKIMs2.Text + " 尺寸 " + texZKIMs6.Text + "*" + texZKIMs5.Text + " 已经进出库?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {

                string tSqlStr = "";
                if (IfNum(this.texZKIMs6.Text.Trim()) == true & IfNum(this.texZKIMs5.Text.Trim()) == true)
                {
                    tSqlStr = string.Concat("Update tab_Sys set force_tag='1',force_long='", int.Parse(this.texZKIMs6.Text.Trim()) * 1000, "' ,force_short='", int.Parse(this.texZKIMs5.Text.Trim()) * 1000, "'  where sys_no ='2'");
                    tSystem.mClsDBUPdate.Execute_Command(tSqlStr);
                }
            }
        }

        private void buttest_Click(object sender, EventArgs e)
        {
            GetData.ClsGetData tClsGetData = new GetData.ClsGetData(); DataTable tData = new DataTable();
            //tData = tClsGetData.GetLINSHIERP(this.textBox6.Text); string tRetStr = "";
            //tSystem.mClsDBUPdate.ExecuteTvp("Pro_ToExcelB", tData, ref tRetStr);
            //tSystem.mClsDBUPdate.DBClose();
            string tSqlstr = " select a.WcsID, a.Order_id,a.Order_singleid, a.Order_singlename ,a.Process_number,a.Customer_name,a.Order_width,a.Order_length,a.ply  from [TabMB_Data]a where a.Process_number ='D22061410.002' order by a.Order_singleid,Single_tag desc ";
            tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlstr, ref tData);

            DataTable tDataCode = new DataTable();
            tSqlstr = " SELECT TOP 1000 [玻璃数据] ,[编号A],[玻璃产品名称] ,[框数据] ,[编号B] ,[框架产品名称] ,[备注] ,[Addtime]FROM [dbo].[TablisecCode]";
            tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlstr, ref tDataCode);
            if (tData.Rows.Count > 0)
            {
                string tStr = SUTAIMES.Common.LisecGPSOutput.SetTrfData(tData, tDataCode);

               string name = System.Text.RegularExpressions.Regex.Replace(tData.Rows[0]["Process_number"].ToString(), @"[^0-9]+", "");
               
               //FileDialog.SaveFile("", "" + "\\LISEC" + name + ".trf");


               SaveFileDialog saveFileDialog = new SaveFileDialog();
               saveFileDialog.Filter = "txt files(*.trf)|*.trf";
               try
               {
                   File.WriteAllText("C:\\LISEC\\LISEC" + name + ".trf", tStr);
               }
               catch
               {
                   saveFileDialog.ShowDialog();
                   File.WriteAllText(saveFileDialog.FileName, tStr);
               }
               MessageBox.Show("文件已保存");
            }

            //D22011704.001
        }

        private void buGetLot_Click(object sender, EventArgs e)
        {
            string tTagStr = "";
            if (this.GetLotCheck.Checked == true)
            {
                tTagStr = "X";
            }
            //DialogResult dr = MessageBox.Show(" 优化号"+tTagStr=="X"?" 当前选择是下片优化单号" :"当前选择是进钢化炉优化单号" +"请确认号是不是下架还是进钢化炉 ", "提示", MessageBoxButtons.YesNo);
            //if (dr == DialogResult.No )
            //{
            //    return;
            //}
            string tLogStr = "";

            string Url = "";
            string result2 = "";
            ///使用JsonWriter写字符串：
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("ucode");
            writer.WriteValue(clsMyPublic.mErp_ucode);// ("auto");
            writer.WritePropertyName("upwd");
            writer.WriteValue(clsMyPublic.mErp_upwd);// ("9A395478FE3EFEAE3323722C56B311F4");
            writer.WritePropertyName("passkey");
            writer.WriteValue(clsMyPublic.mErp_passkey);// ("5F229626553F69F01DAB380655702738918A2D2150AD44CF59910E0ADE552FBDF7A548489F8FC253");
            writer.WritePropertyName("djno");


            //writer.WritePropertyName("ucode");
            //writer.WriteValue("auto");
            //writer.WritePropertyName("upwd");
            //writer.WriteValue("lxauto");
            //writer.WritePropertyName("passkey");
            //writer.WriteValue("6D77B2206165CD196E28A68FD82F645654DF5FD6B58B377ACDFB7A82034D355145202D3F536296EC");
            //writer.WritePropertyName("djno");

            writer.WriteValue(textCha1.Text.Trim());
            writer.WriteEndObject();
            writer.Flush();
            string jsonText = "";
            jsonText = sw.GetStringBuilder().ToString();

            Url = clsMyPublic.mURl1;
            //if (Url.Trim().Length == 0)
            //{
            //    Url = "https://www.qdlkd.net/ypstock/lkd/lxauto/lxjhlinetest";//"https://www.qdlkd.net/ypstock/lkd/lxauto/lxjhline";
            //}
            Url = clsMyPublic.mErpUrl;  // "https://www.qdlkd.net/ypstock/lkd/lxauto/lxjhlinetemp";//20210446 带箱号
                //"https://www.qdlkd.net/ypstock/lkd/lxauto/lxjhlinetemp";
            DateTime tNow = DateTime.Now;
            result2 = clsMyPublic.PostUrl(Url, jsonText);
            double  tt = DateTime.Now.Subtract(tNow).TotalMilliseconds;
            texErpStr1.Text =tt.ToString() +" "+ DateTime.Now.ToString("yy-MM-dd HH:mm:ss ") + result2;
            tLogStr =" 基础数据 "+ result2;

            GetData.ClsGetData tClsGetData = new GetData.ClsGetData(); DataTable tDataTable = new DataTable();
            
            
            tDataTable = tClsGetData.GetLINSHIERPNew(result2, tTagStr);
            string tRetStr = "";
            tSystem.mClsDBUPdate.ExecuteTvp("Pro_ToExcelB", tDataTable, ref tRetStr);
            dataGrid1.DataSource = tDataTable;
            if (tDataTable != null)
            {
                labShow.Text = tDataTable.Rows.Count.ToString();
            }

            Url = "";
            Url = clsMyPublic.mErpUrlLay;
            if (Url.Trim().Length == 0)
            {
                Url = "https://www.qdlkd.net/ypstock/lkd/lxauto/lxjhposinfo";
            }
            result2 = clsMyPublic.PostUrl(Url, jsonText);
            tDataTable = tClsGetData.GetLinShiEepLay(result2, textCha1.Text.Trim());
            tRetStr = "";
            tSystem.mClsDBUPdate.ExecuteTvp("Pro_ToExcelLayB", tDataTable, ref tRetStr);
            texErpStr2.Text = DateTime.Now.ToString("yy-MM-dd HH:mm:ss ") + result2;

            tSystem.mClsDBUPdate.DBClose();
            tLogStr = tLogStr + "\r\n" + " 坐标数据 " + result2;
            tSystem.mFile.WriteLog("",tLogStr+ "N");
        }
        /// <summary>
        /// 设备异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        

        private void butMBmac_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  磨边  您确定该片设备异常 序号： " + texMsIn1.Text + "  流程卡号 " + texMsIn2.Text + " 尺寸 " + texMsIn5.Text + "*" + texMsIn4.Text + " 设备异常?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texMsIn1.Text, "97", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }

        }

        private void butMBINmac_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("   磨边仓储入库 您确定该片设备异常 序号： " + texMs1.Text + "  流程卡号 " + texMs2.Text + " 尺寸 " + texMs5.Text + "*" + texMs4.Text + " 设备异常?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texMs1.Text, "97", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void butMBOUTmac_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  磨边仓储出库  您确定该片设备异常 序号： " + texOMs1.Text + "  流程卡号 " + texOMs2.Text + " 尺寸 " + texOMs5.Text + "*" + texOMs4.Text + " 设备异常?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texOMs1.Text, "97", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void butZKINmac_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  中空库入库  您确定该片设备异常 序号： " + texZKIMs1.Text + "  流程卡号 " + texZKIMs2.Text + " 尺寸 " + texZKIMs6.Text + "*" + texZKIMs5.Text + " 设备异常?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texZKIMs1.Text, "97", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }
        private void buZKBmac_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("    中空 您确定该片设备异常 序号： " + texZKOMs1.Text + "  流程卡号 " + texZKOMs2.Text + " 尺寸 " + texZKOMs6.Text + "*" + texZKOMs5.Text + " 设备异常?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texZKOMs1.Text, "97", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void butMBChamac_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("   磨边库查询 您确定该片设备异常 序号： " + texMBWhC1.Text + "  流程卡号 " + texMBWhC2.Text + " 尺寸 " + texMBWhC5.Text + "*" + texMBWhC4.Text + " 设备异常?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texMBWhC1.Text, "97", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void butZKChamac_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  中空库  您确定该片设备异常 序号： " + texZKWhC1.Text + "  流程卡号 " + texZKWhC2.Text + " 尺寸 " + texZKWhC5.Text + "*" + texZKWhC4.Text + " 设备异常?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texZKWhC1.Text, "97", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }
        private void butXChamac_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  详细  您确定该片设备异常 序号： " + texXCha1.Text + "  流程卡号 " + texXCha2.Text + " 尺寸 " + texXCha5.Text + "*" + texXCha4.Text + " 设备异常?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", texXCha1.Text, "97", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }


        private void butZKSetClear_Click(object sender, EventArgs e)
        {
            texZKOptiSet.Text = ""; texZKProSet.Text = ""; texZKORDSet.Text = "";
        }

        private void butZKOUTclear_Click(object sender, EventArgs e)
        {
            texZKOut2.Text = ""; texZKOut3.Text = ""; texZKOut4.Text = "";
        }

        private void butMBOClear_Click(object sender, EventArgs e)
        {
            texOUT1.Text = ""; texOUT2.Text = ""; texOUT3.Text = ""; texOUT4.Text = ""; texOUT5.Text = ""; texOUT6.Text = "";
        }
        private void butMBOClear1_Click(object sender, EventArgs e)
        {
            texOUT2.Text = ""; texOUT3.Text = ""; texOUT4.Text = ""; texOUT5.Text = ""; texOUT6.Text = "";
        }
        private void butCha_Click(object sender, EventArgs e)//订单详细信息查询
        {
            try
            {
                string tT1 = "", tT2 = ""; string tStrAA = ""; string Wtime = "", WhereStr = "";
                tT1 = dp_S_pl_date1.Text + " " + dp_S_pl_time1.Text;
                tT2 = dp_End_date1.Text + " " + dp_End_time1.Text;
                if (this.checkCha1.Checked == true)
                {
                    Wtime = " and a.outAtime  between '" + tT1 + "' and '" + tT2 + "' ";
                }
                tT1 = dp_S_pl_date2.Text + " " + dp_S_pl_time2.Text;
                tT2 = dp_End_date2.Text + " " + dp_End_time2.Text;
                if (this.checkCha2.Checked == true)//切割
                {
                    Wtime = " and a.InputTime  between '" + tT1 + "' and '" + tT2 + "' ";
                }
                tT1 = dp_S_pl_date3.Text + " " + dp_S_pl_time3.Text;
                tT2 = dp_End_date3.Text + " " + dp_End_time3.Text;
                if (this.checkCha3.Checked == true)//磨边
                {
                    Wtime = " and a.InAtime  between '" + tT1 + "' and '" + tT2 + "' ";
                }


                if (this.checkCh1.Checked == true)
                {
                    if (texCha4.Text == "1")
                    {
                        WhereStr = " and MBExit ='1' ";
                    }
                    else
                    {
                        WhereStr = " and MBExit in ('2','3') ";
                    }
                }
                string tSqlstr = "";
                DataTable tDT = new DataTable();
                tSqlstr = string.Concat("  SELECT a.Optimize_batch '优化单号',[Process_number]流程卡号,[single_tag]'标记' ,[single_long] '长边' ,[single_short]  '短边',a.Single_Str '标记组合'  "
                    //+",case a.[Status] when 0 then '初始' when 1 then '数据写入' when 2 then '磨边已核对' when 3 then '进磨边'when 10 then '准备进库' when 14 then '库内'when 3 then '出库' when 20 then '准备出库' when 24 then '离库'  when 25 then '已进钢化炉'  when 199 then '入库未完成'  when 299 then '出库未完成' when 99 then '不符合尺寸'when 96 then '破损'when 97 then '设备异常' when 120 then '空取'"
                    //+ " when 26 then '钢化后复核成功' when 27 then '钢化下片完成'  when 28 then '中空上片' "            
                    //+" when 30 then '准备进中空' when 34 then '中空库' when 40 then '准备出中空' when 44 then '离开中空' when 140 then '中空空取' when 399 then '入中空未完成' when 499 then '出中空未完成'else '异常' end 状态,"
                    + ",(select top 1 isnull(c.statusName,a.Status ) from StatusNameT c where a.Status =c.Status order by case when a.Err_Tag>0 and c.AssistSatusS =1 then 0 else 1 end,c.AssistSatusS  asc ) '状态'"
                                   +",a.MBPlcFlow '磨边PLC流水号',a.ZKPlcFlow  '中空PLC流水号',a.WH_NO   '库号' ,a.WH_Num  '格号',a.ZKWH_NO   '中空库号' ,a.ZKWH_Num  '中空格号',WcsID 序号,a.Optimize_batch 优化单号,a.single_name '单片名称',convert(varchar(20),a.InAtime,120) '磨边库入库时间',convert(varchar(20),a.outAtime,120) '磨边库出库时间' , convert(varchar(20),a.ZKInAtime,120) '中空入库时间' ,convert(varchar(20),a.ZKoutAtime ,120) '中空出库时间' ,a.Order_id '订单号',a.Order_singleid '成品单片编号'"
                                   + ",a.boxno '箱号' ,a.[Label_tag]'钢标字符' ,a.[Label_pos]'钢标位置' ,a.[Label_type]'钢标边距' ,a.[Label_name]'钢标图号' ,a.[Label_size]'钢标尺寸' ,a.[Label_color]'钢标内容' "
                                   + " ,a.[MBEnport],a.[MBExit],a.MBCar,a.ZKExit,a.ZKCar"
                                   + ",a.Customer_name '项目名称',a.Order_singlename '产品配置',convert(varchar,a.Order_length)+'*'+convert(varchar,a.Order_width)'规格'"
                                   +" FROM [tabmb_data] a "
                                   + " where a.Optimize_batch like '%", texCha1.Text.Trim(), "' and a.Process_number like '%", texCha3.Text.Trim(), "' and a.Order_id like '%", texCha2.Text.Trim(), "'"
                                   ,Wtime,WhereStr,
                                   " order by a.Process_number ,  convert(float,a.Single_long)*convert(float,a.Single_short)  desc, a.Order_singleid ,a.Single_tag desc ,a.Optimize_batch  ");
                if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlstr, ref tDT) == "")
                {
                    this.dataGridViewCha.DataSource = null;
                    if (tDT.Rows.Count > 0)
                    {
                        this.dataGridViewCha.DataSource = tDT;
                        labZKChashu.Text = tDT.Rows.Count.ToString();
                        tClsShowFrm.SetBackColor(dataGridViewCha, 6, 0);
                        for (int i = 0; i < this.dataGridViewCha.Columns.Count; i++)
                        {
                            dataGridViewCha.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            //this.dataGridViewCha.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                    }
                    tSystem.mClsDBUPdate.DBClose();
                }
            }
            catch
            {
            }
        }

        private void butCha1_Click(object sender, EventArgs e)
        {
            try
            {
                string tT1 = "", tT2 = ""; string tStrAA = ""; string Wtime = "", WhereStr = "";
                tT1 = dp_S_pl_date1.Text + " " + dp_S_pl_time1.Text;
                tT2 = dp_End_date1.Text + " " + dp_End_time1.Text;
                if (this.checkCha1.Checked == true)
                {
                    Wtime = " and a.outAtime  between '" + tT1 + "' and '" + tT2 + "' ";
                }
                tT1 = dp_S_pl_date2.Text + " " + dp_S_pl_time2.Text;
                tT2 = dp_End_date2.Text + " " + dp_End_time2.Text;
                if (this.checkCha2.Checked == true)//切割
                {
                    Wtime = " and a.InputTime  between '" + tT1 + "' and '" + tT2 + "' ";
                }
                tT1 = dp_S_pl_date3.Text + " " + dp_S_pl_time3.Text;
                tT2 = dp_End_date3.Text + " " + dp_End_time3.Text;
                if (this.checkCha3.Checked == true)//磨边
                {
                    Wtime = " and a.InAtime  between '" + tT1 + "' and '" + tT2 + "' ";
                }


                if (this.checkCh1.Checked == true)
                {
                    if (texCha4.Text == "1")
                    {
                        WhereStr = " and MBExit ='1' ";
                    }
                    else
                    {
                        WhereStr = " and MBExit in ('2','3') ";
                    }
                }
                string tSqlstr = "";
                DataTable tDT = new DataTable();
                tSqlstr = string.Concat("  SELECT a.Optimize_batch '优化单号',[Process_number]流程卡号,[single_tag]'标记' ,[single_long] '长边' ,[single_short]  '短边',a.Single_Str '标记组合'  ,case a.[Status] when 0 then '初始' when 1 then '数据写入' when 2 then '磨边已核对' when 3 then '进磨边'when 10 then '准备进库' when 14 then '库内'when 3 then '出库' when 20 then '准备出库' when 24 then '离库'  when 25 then '已进钢化炉'  when 199 then '入库未完成'  when 299 then '出库未完成' when 99 then '不符合尺寸'when 96 then '破损'when 97 then '设备异常' when 120 then '空取'"
                    + " when 26 then '钢化后复核成功' when 27 then '钢化下片完成'  when 28 then '中空上片' "
                    + " when 30 then '准备进中空' when 34 then '中空库' when 40 then '准备出中空' when 44 then '离开中空' when 140 then '中空空取' when 399 then '入中空未完成' when 499 then '出中空未完成'else '异常' end 状态,"
                                   + "a.MBPlcFlow '磨边PLC流水号',a.ZKPlcFlow  '中空PLC流水号',a.WH_NO   '库号' ,a.WH_Num  '格号',a.ZKWH_NO   '中空库号' ,a.ZKWH_Num  '中空格号',WcsID 序号,a.Optimize_batch 优化单号,a.single_name '单片名称',convert(varchar(20),a.InAtime,120) '磨边库入库时间',convert(varchar(20),a.outAtime,120) '磨边库出库时间' , convert(varchar(20),a.ZKInAtime,120) '中空入库时间' ,convert(varchar(20),a.ZKoutAtime ,120) '中空出库时间' ,a.Order_id '订单号',a.Order_singleid '成品单片编号'"
                                   + ",a.boxno '箱号' ,a.[Label_tag]'钢标字符' ,a.[Label_pos]'钢标位置' ,a.[Label_type]'钢标边距' ,a.[Label_name]'钢标图号' ,a.[Label_size]'钢标尺寸' ,a.[Label_color]'钢标内容' "
                                   + " ,a.[MBEnport],a.[MBExit],a.MBCar,a.ZKExit,a.ZKCar"
                                   + " FROM [HisTabMB_Data] a "
                                   + " where a.Optimize_batch like '%", texCha1.Text.Trim(), "' and a.Process_number like '%", texCha3.Text.Trim(), "' and a.Order_id like '%", texCha2.Text.Trim(), "'"
                                   , Wtime, WhereStr,
                                   " order by a.Optimize_batch ,a.Process_number ,  convert(float,a.Single_long)*convert(float,a.Single_short)  desc  ,	 a.Order_singleid,	 a.Single_tag desc ");
                if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlstr, ref tDT) == "")
                {
                    this.dataGridViewCha.DataSource = null;
                    if (tDT.Rows.Count > 0)
                    {
                        this.dataGridViewCha.DataSource = tDT;
                        labZKChashu.Text = tDT.Rows.Count.ToString();
                        tClsShowFrm.SetBackColor(dataGridViewCha, 6, 0);
                        for (int i = 0; i < this.dataGridViewCha.Columns.Count; i++)
                        {
                            dataGridViewCha.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            this.dataGridViewCha.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                    }
                    tSystem.mClsDBUPdate.DBClose();
                }
            }
            catch
            {
            }
            //try
            //{
            //    string tT1 = "", tT2 = ""; string Wtime = "", WhereStr = "";
            //    tT1 = dp_S_pl_date1.Text + " " + dp_S_pl_time1.Text;
            //    tT2 = dp_End_date1.Text + " " + dp_End_time1.Text;
            //    if (this.checkCha1.Checked == true)
            //    {
            //        Wtime = " and a.Addtime  between '" + tT1 + "' and '" + tT2 + "' ";
            //    }
            //    string tSqlstr = "";
            //    DataTable tDT = new DataTable();
            //    tSqlstr = string.Concat("  SELECT a.Optimize_batch '优化单号',[Process_number]流程卡号,[single_tag]'标记' ,[single_long] '长边' ,[single_short]  '短边',a.Single_Str '标记组合'  ,case a.[Status] when 0 then '初始' when 1 then '数据写入' when 2 then '磨边已核对' when 3 then '进磨边'when 10 then '准备进库' when 14 then '库内'when 3 then '出库' when 20 then '准备出库' when 24 then '离库'  when 199 then '入库未完成'  when 299 then '出库未完成' when 99 then '不符合尺寸'when 96 then '破损'when 97 then '设备异常' when 120 then '空取' when 30 then '准备进中空' when 34 then '中空库' when 40 then '准备出中空' when 44 then '离开中空' when 140 then '中空空取' when 399 then '入中空未完成' when 499 then '出中空未完成'else '异常' end 状态,"
            //                       + "a.MBPlcFlow '磨边PLC流水号',a.ZKPlcFlow  '中空PLC流水号',a.WH_NO   '库号' ,a.WH_Num  '格号',a.ZKWH_NO   '中空库号' ,a.ZKWH_Num  '中空格号',WcsID 序号,a.Optimize_batch 优化单号,a.single_name '单片名称',convert(varchar(20),a.InAtime,120) '磨边库入库时间',convert(varchar(20),a.outAtime,120) '磨边库出库时间' , convert(varchar(20),a.ZKInAtime,120) '中空入库时间' ,convert(varchar(20),a.ZKoutAtime ,120) '中空出库时间' ,a.Order_id '订单号',a.Order_singleid '成品单片编号' FROM [tabmb_data] a "
            //                       + " where a.Optimize_batch like '%", texCha1.Text.Trim(), "' and a.Process_number like '%", texCha3.Text.Trim(), "' and a.Order_id like '%", texCha2.Text.Trim(), "'"
            //                       , Wtime,
            //                       " order by a.Optimize_batch ,a.Process_number ,  convert(float,a.Single_long)*convert(float,a.Single_short)  desc  ,	 a.Order_singleid,	 a.Single_tag desc ");
            //    if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlstr, ref tDT) == "")
            //    {
            //        this.dataGridViewCha.DataSource = null;
            //        if (tDT.Rows.Count > 0)
            //        {
            //            this.dataGridViewCha.DataSource = tDT;
            //            labZKChashu.Text = tDT.Rows.Count.ToString();
            //            tClsShowFrm.SetBackColor(dataGridViewCha, 6);
            //            for (int i = 0; i < this.dataGridViewCha.Columns.Count; i++)
            //            {
            //                dataGridViewCha.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            //                this.dataGridViewCha.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            //            }
            //        }
            //        tSystem.mClsDBUPdate.DBClose();
            //    }
            //}
            //catch
            //{
            //}
        }


        private void butPOcha_Click(object sender, EventArgs e)
        {
            try
            {
                //string tT1 = "", tT2 = ""; string tStrAA = ""; string Wtime = "", WhereStr = "";
                //tT1 = dp_S_pl_date.Text + " " + dp_S_pl_time.Text;
                //tT2 = dp_End_date.Text + " " + dp_End_time.Text;
                //if (this.checkCha.Checked == true)
                //{
                //    Wtime = " and a.Addtime  between '" + tT1 + "' and '" + tT2 + "' ";
                //}
                string tSqlstr = "", WhereStr = "";
                DataTable tDT = new DataTable();
                if (texPo3.Text.Length > 0)
                {
                   WhereStr="  and a.Lot_NO like '%"+ texPo3.Text.Trim()+"'";
                }
                
                tSqlstr = string.Concat(" SELECT a.[WcsID] '序号',a.[Optimize_batch]'优化单号' ,a.[Process_number]'流程卡号',a.[Single_name] '玻璃名称' ,a.[Single_long]'长边' ,a.[Single_short]'短边' ,a.[Single_tag] '标记' ,case  a.[Status] when 0 then '初始' when 1 then '已经补片' end '状态'  , case a.[RunStatus]  when 0 then '初始' when 1 then '数据写入' when 2 then '磨边已核对'  when 3 then '进磨边' when 10 then '准备进库'  when 14 then '库内'  when 20 then '准备出库'  when 24 then '离库'   when 199 then '入库未完成'   when 299 then '出库未完成'  when 99 then '不符合尺寸' when 96 then '破损' when 97 then '设备异常'when 120 then '空取'   when 30 then '准备进中空'  when 34 then '中空库'  when 40 then '准备出中空'  when 44 then '离开中空'  when 140 then '中空空取' when 399 then '入中空未完成'  when 499 then '出中空未完成'    else '异常' end  '破片时状态',case a.[Area_id] when 0 then '切割' when 2 then '磨边' when 4 then '磨边仓储' when 6 then '钢化' when 8 then '中空库' when 10 then '中空库后' else convert(varchar,a.Area_id) end '破片区域'  ,[WH_ID]'磨边库位编号' ,[ZHWH_ID]'中空库位编号' ,  convert(varchar(30),a.[Addtime],120) '确认破片时间' ,convert(varchar(30),[Updatetime],120)'触发补片时间' ,a.Lot_NO  '生成补片虚拟批次' FROM [dbo].[TabPoData] a "
                           + " where a.Process_number  like '%", texPo1.Text.Trim(), "' ", WhereStr, "  order by a.Process_number ,a.Addtime");
                if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlstr, ref tDT) == "")
                {
                    if (tDT.Rows.Count > 0)
                    {
                        this.dataGridViewPo.DataSource = tDT;
                        labZKChashu.Text = tDT.Rows.Count.ToString();
                      
                        for (int i = 0; i < this.dataGridViewPo.Columns.Count; i++)
                        {
                            dataGridViewPo.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            this.dataGridViewPo.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                    }
                    tSystem.mClsDBUPdate.DBClose();
                }
            }
            catch
            {
            }
        }
        private void butBuSet_Click(object sender, EventArgs e)
        {
            string tSqlstr ;
            tSqlstr = " update TabPoData set status ='1',Updatetime=getdate() ,lot_no ='" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "' where status ='0' ";
            tSystem.mClsDBUPdate.Execute_Command(tSqlstr);
            tSystem.mClsDBUPdate.DBClose();

        }
        private void butyiPO_Click(object sender, EventArgs e)
        {
            string tSqlstr;
            tSqlstr = " update TabPoData set status ='1' where WcsID='" + texPOS1.Text + "' ";
            tSystem.mClsDBUPdate.Execute_Command(tSqlstr);
            tSystem.mClsDBUPdate.DBClose();
        }
        private void butWeiPo_Click(object sender, EventArgs e)
        {
            string tSqlstr;
            tSqlstr = " update TabPoData set status ='0' where WcsID='" + texPOS1.Text + "' ";
            tSystem.mClsDBUPdate.Execute_Command(tSqlstr);
            tSystem.mClsDBUPdate.DBClose();
        }
        private void ShowMBINFlow(DataTable _datatT)
        {
            if (_datatT.Rows.Count > 0)
            {
                for (int i = 0; i < _datatT.Rows.Count; i++)
                {
                    string tIDStr = _datatT.Rows[i]["ID"].ToString();
                    if (IfNum(tIDStr))
                    {
                        int tId = int.Parse(tIDStr);

                        if (tId <= 6)
                        {
                            tId = tId - 1;
                            tMBButIn[tId].Text = _datatT.Rows[i]["Mac_NO"].ToString();
                        
                           tMBButIn[tId].Text = tMBButIn[tId].Text + "_" + _datatT.Rows[i]["Mac_id"].ToString();
                            
                            if (_datatT.Rows[i]["Plc_Flow"].ToString() != "0")
                            {
                                tMBButIn[tId].Text = tMBButIn[tId].Text + " \r\n" + _datatT.Rows[i]["Plc_Flow"].ToString()
                                   + " \r\n" + _datatT.Rows[i]["Single_long"].ToString() + "*" + _datatT.Rows[i]["Singel_Short"].ToString();
                                //tMBButIn[tId].Tag = _datatT.Rows[i]["Plc_Flow"].ToString();
                                tMBButIn[tId].BackColor = Color.YellowGreen;
                            }
                            else
                            {
                                tMBButIn[tId].BackColor = Color.WhiteSmoke;
                            }
                            if (_datatT.Rows[i]["Plc_Flow"].ToString() == "0")
                            {
                                tMBButIn[tId].Visible = false;
                            }
                            else { tMBButIn[tId].Visible = true; }
                        }
                     
                        else if (tId == 1001)
                        {
                            textMBINFLOW.Text = _datatT.Rows[i]["Mac_NO"].ToString();
                            if (_datatT.Rows[i]["Plc_Flow"].ToString() != "0" | _datatT.Rows[i]["log_Flow"].ToString() != "0") //log_Flow  状态  log_Str异常说明  Mac_id 长边  Mac_Num 短边
                            {
                                textMBINFLOW.Text = textMBINFLOW.Text + "  " + _datatT.Rows[i]["log_Flow"].ToString() + ":" + _datatT.Rows[i]["Plc_Flow"].ToString()
                                    + "  " + _datatT.Rows[i]["Mac_id"].ToString() + "*" + _datatT.Rows[i]["Mac_Num"].ToString() + " " + _datatT.Rows[i]["log_Str"].ToString();
                            }
                        }
                    }


                }
            }

        }
        private void ShowZKINFlow(DataTable _datatT)
        {
            if (_datatT.Rows.Count > 0)
            {
                for (int i = 0; i < _datatT.Rows.Count; i++)
                {
                    string tIDStr = _datatT.Rows[i]["ID"].ToString();
                    if (IfNum(tIDStr))
                    {
                        int tId = int.Parse(tIDStr);

                        if (tId <= 18)
                        {
                            tId = tId - 1;
                            tZKButtonIn[tId].Text = _datatT.Rows[i]["Mac_NO"].ToString();
                            if (_datatT.Rows[i]["Mac_NO"].ToString() != "3030" & _datatT.Rows[i]["Mac_NO"].ToString() != "3070" & _datatT.Rows[i]["Mac_NO"].ToString() != "3080")
                            {
                                tZKButtonIn[tId].Text = tZKButtonIn[tId].Text + "_" + _datatT.Rows[i]["Mac_id"].ToString();
                            }
                            if (_datatT.Rows[i]["Plc_Flow"].ToString() != "0")
                            {
                                tZKButtonIn[tId].Text = tZKButtonIn[tId].Text + " \r\n" + _datatT.Rows[i]["Plc_Flow"].ToString()
                                   + " \r\n" + _datatT.Rows[i]["Single_long"].ToString() + "*" + _datatT.Rows[i]["Singel_Short"].ToString();
                                //tZKButtonIn[tId].Tag = _datatT.Rows[i]["Plc_Flow"].ToString();
                                tZKButtonIn[tId].BackColor = Color.YellowGreen;
                            }
                            else
                            {
                                tZKButtonIn[tId].BackColor = Color.WhiteSmoke;
                            }
                            if (_datatT.Rows[i]["Plc_Flow"].ToString() == "0" && _datatT.Rows[i]["Mac_NO"].ToString() != "3030" & _datatT.Rows[i]["Mac_NO"].ToString() != "3070" & _datatT.Rows[i]["Mac_NO"].ToString() != "3080")
                            {
                                tZKButtonIn[tId].Visible = false;
                            }
                            else { tZKButtonIn[tId].Visible = true; }
                        }
                        else if (tId == 999)
                        {
                            tZKButtonIn[18].Text = _datatT.Rows[i]["Plc_Flow"].ToString();
                        }
                        else if (tId == 1001)
                        {
                            butZKIN.Text = _datatT.Rows[i]["Mac_NO"].ToString();
                            if (_datatT.Rows[i]["Plc_Flow"].ToString() != "0" | _datatT.Rows[i]["log_Flow"].ToString() != "0") //log_Flow  状态  log_Str异常说明  Mac_id 长边  Mac_Num 短边
                            {
                                butZKIN.Text = butZKIN.Text + " \r\n" + _datatT.Rows[i]["log_Flow"].ToString() + ":" + _datatT.Rows[i]["Plc_Flow"].ToString()
                                    + " \r\n" + _datatT.Rows[i]["Mac_id"].ToString() + "*" + _datatT.Rows[i]["Mac_Num"].ToString() + " " + _datatT.Rows[i]["log_Str"].ToString();
                            }
                        }
                    }
                    
                    
                }
            }
            
        }//
        private void ButZKINFlow_Click(object sender, EventArgs e)
        {
            Button tBut = (Button)sender;
            string[] tData = tBut.Text.Trim().Replace('\n', ' ').Split('\r');
            if (tData.Length > 1)
            {
                string tPlcFlow=tData[1].Trim();
                string tSqlStr = ""; DataTable tDt;
                tSqlStr = string.Concat(" select top 1 [WcsID],[Process_number],single_name,ZKPlcFlow ,[Single_short],[Single_long] ,[ZKWH_NO] ,[ZKWH_Num] "
                  + ",case [Status] when 0 then '初始' when 1 then '数据写入' when 2 then '磨边已核对' when 3 then '进磨边'when 10 then '准备进库' when 14 then '库内'when 3 then '出库' when 20 then '准备出库' when 24 then '离库'  when 188 then '入库未完成'  when 299 then '出库未完成' when 99 then '不符合尺寸'when 96 then '破损'when 97 then '设备异常' when 120 then '空取' when 30 then '准备进中空' when 34 then '中空库' when 40 then '准备出中空' when 44 then '离开中空'   else '异常' end 状态"
                                 + " from [TabMB_Data] a where ZKPlcFlow='", tPlcFlow, "' ");
                tDt = new DataTable();
                if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
                {
                    if (tDt.Rows.Count > 0)
                    {
                        texZKIMs1.Text = tDt.Rows[0][0].ToString(); texZKIMs2.Text = tDt.Rows[0][1].ToString(); texZKIMs3.Text = tDt.Rows[0][2].ToString(); texZKIMs4.Text = tDt.Rows[0][3].ToString();
                        texZKIMs5.Text = tDt.Rows[0][4].ToString(); texZKIMs6.Text = tDt.Rows[0][5].ToString(); texZKIMs7.Text = tDt.Rows[0][6].ToString(); texZKIMs8.Text = tDt.Rows[0][7].ToString();
                        texZKIMs9.Text = tDt.Rows[0][8].ToString();

                    }
                    tSystem.mClsDBUPdate.DBClose();
                }

            }

        }

        private void comboMB_SelectedIndexChanged(object sender, EventArgs e)
        {
            texLay.Text = comboMB.Text;
            //string tSqlStr = "";
            //tSqlStr = string.Concat("Update tabOp  set   Layout_number ='" + texLay.Text + "',lastLayout_number= case when Layout_number='" + texLay.Text + "' then 0 else Layout_number end   where NO='1'");
            //tSystem.mClsDBUPdate.Execute_Command(tSqlStr);
            //tSystem.mClsDBUPdate.DBClose();
            string tRetStr = "", tRetData = "";
            tSystem.mClsDBUPdate.ExecuteTvpOptimize_batch("Pro_LayOutUpdata", texLay.Text, comLine.Text.Trim(), ref tRetStr,ref tRetData);
            tSystem.mClsDBUPdate.DBClose();
        }

      

        private void texLay_KeyPress(object sender, KeyPressEventArgs e)
        {
             if(e.KeyChar=='\r')
             {
                 string tTex = this.texLay.Text.Trim();
                 foreach (var item in comboMB.Items)
                 {
                     if (tTex == item.ToString())
                     {
                         string tRetStr = "", tRetData = "";
                         tSystem.mClsDBUPdate.ExecuteTvpOptimize_batch("Pro_LayOutUpdata", tTex, comLine.Text.Trim(), ref tRetStr,ref tRetData);
                         tSystem.mClsDBUPdate.DBClose();
                         break;
                     }
                 }
                 this.texLay.Text = "";
             }
        }

        private void dataGridViewCha_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = e.RowIndex;

            if (this.dataGridViewCha.Rows.Count > 1 && rowindex >= 0)
            {

                if (this.dataGridViewCha.CurrentRow.Index == 0)
                {

                    texXCha1.Text = dataGridViewCha.Rows[0].Cells[13].Value.ToString();
                    texXCha2.Text = dataGridViewCha.Rows[0].Cells[1].Value.ToString();
                    texXCha3.Text = dataGridViewCha.Rows[0].Cells[7].Value.ToString();
                    texXCha4.Text = dataGridViewCha.Rows[0].Cells[3].Value.ToString();
                    texXCha5.Text = dataGridViewCha.Rows[0].Cells[4].Value.ToString();
                    texXCha6.Text = dataGridViewCha.Rows[0].Cells[2].Value.ToString();
                    texXCha7.Text = dataGridViewCha.Rows[0].Cells[5].Value.ToString();
                    texXCha8.Text = dataGridViewCha.Rows[0].Cells[6].Value.ToString();

                }
                else
                {

                    texXCha1.Text = dataGridViewCha.Rows[rowindex].Cells[13].Value.ToString();
                    texXCha2.Text = dataGridViewCha.Rows[rowindex].Cells[1].Value.ToString();
                    texXCha3.Text = dataGridViewCha.Rows[rowindex].Cells[7].Value.ToString();
                    texXCha4.Text = dataGridViewCha.Rows[rowindex].Cells[3].Value.ToString();
                    texXCha5.Text = dataGridViewCha.Rows[rowindex].Cells[4].Value.ToString();
                    texXCha6.Text = dataGridViewCha.Rows[rowindex].Cells[2].Value.ToString();
                    texXCha7.Text = dataGridViewCha.Rows[rowindex].Cells[5].Value.ToString();
                    texXCha8.Text = dataGridViewCha.Rows[rowindex].Cells[6].Value.ToString();


                }
            }
        }
        private void buGetLotNO_Click(object sender, EventArgs e)
        {
            string tLogStr = "";

            string Url = "";
            string result2 = "";
            ///使用JsonWriter写字符串：
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("ucode");
            writer.WriteValue(clsMyPublic.mErp_ucode);// ("auto");  ("ucode");// 
            writer.WritePropertyName("upwd");
            writer.WriteValue(clsMyPublic.mErp_upwd);// ("9A395478FE3EFEAE3323722C56B311F4");("upwd");// 
            writer.WritePropertyName("passkey");
            writer.WriteValue(clsMyPublic.mErp_passkey);// ("5F229626553F69F01DAB380655702738918A2D2150AD44CF59910E0ADE552FBDF7A548489F8FC253");  ("029E8");// 
            writer.WritePropertyName("djno");
            writer.WriteValue("");// (textCha1.Text.Trim());
            writer.WritePropertyName("start");
            writer.WriteValue(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));//("2022-01-01");// 
            writer.WritePropertyName("end");
            writer.WriteValue(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")); //("2022-01-10");// 
            writer.WriteEndObject();
            writer.Flush();
            string jsonText = "";
            jsonText = sw.GetStringBuilder().ToString();


            Url = "https://www.qdlkd.net/ypstock/lkd/lxauto/lxjhlist";// clsMyPublic.mErpUrlLot;  // "https://www.qdlkd.net/ypstock/lkd/lxauto/lxjhlinetemp";//20210446 带箱号
            //"https://www.qdlkd.net/ypstock/lkd/lxauto/lxjhlinetemp";
            DateTime tNow = DateTime.Now;
            result2 = clsMyPublic.PostUrl(Url, jsonText);
            double tt = DateTime.Now.Subtract(tNow).TotalMilliseconds;
            texErpStr1.Text = tt.ToString() + " " + DateTime.Now.ToString("yy-MM-dd HH:mm:ss ") + result2;
        }
        private void button2_Click(object sender, EventArgs e)
        {
          

            string Url = "";
            string result2 = "";
            ///使用JsonWriter写字符串：
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);

            writer.WriteStartObject();
            writer.WritePropertyName("ucode");
            writer.WriteValue(clsMyPublic.mErp_ucode);// ("auto");  ("ucode");// 
            writer.WritePropertyName("upwd");
            writer.WriteValue(clsMyPublic.mErp_upwd);// ("9A395478FE3EFEAE3323722C56B311F4");("upwd");// 
            writer.WritePropertyName("passkey");
            writer.WriteValue(clsMyPublic.mErp_passkey);
            writer.Flush();
            string jsonText = "";
            jsonText = sw.GetStringBuilder().ToString();

            Url = clsMyPublic.mURl1;
            //if (Url.Trim().Length == 0)
            {
                Url = "https://www.qdlkd.net/ypstock/lkd/ypstock/yplist";
            }


            result2 = clsMyPublic.PostUrl(Url, jsonText);


            sw = new StringWriter();
            writer = new JsonTextWriter(sw);

            writer.WriteStartObject();
            writer.WritePropertyName("ucode");
            writer.WriteValue("auto");
            writer.WritePropertyName("upwd");
            writer.WriteValue("auto139");
            writer.WritePropertyName("passkey");
            writer.WriteValue("8BD2AD871782BD660A75C5A6D2902851DF1D64AF829D65BB");
            writer.WritePropertyName("dicttype");
            writer.WriteValue("设备");//分类
            writer.WritePropertyName("wipno");
            writer.WriteValue("");
            writer.WritePropertyName("ckcode");
            writer.WriteValue("");
            writer.WriteEndObject();
            writer.Flush();

            jsonText = sw.GetStringBuilder().ToString();

            Url = clsMyPublic.mURl1;
            //if (Url.Trim().Length == 0)
            {
                Url = "https://www.qdlkd.net/ypstock/lkd/ypstock/datadict";
            }


            result2 = clsMyPublic.PostUrl(Url, jsonText);


            Url = "https://www.qdlkd.net/ypstock/lkd/ypstock/yplist";


            StringWriter swyu1 = new StringWriter();
            JsonWriter writeryu1 = new JsonTextWriter(swyu1);
            writeryu1.WriteStartObject();
            writeryu1.WritePropertyName("ucode");
            writeryu1.WriteValue("auto");
            writeryu1.WritePropertyName("upwd");
            writeryu1.WriteValue("auto139");
            writeryu1.WritePropertyName("passkey");
            writeryu1.WriteValue("8BD2AD871782BD660A75C5A6D2902851DF1D64AF829D65BB");
            writeryu1.WriteEndObject();
            writeryu1.Flush();
            string jsonTextyu1 = "";
            jsonTextyu1 = swyu1.GetStringBuilder().ToString();
            result2 = clsMyPublic.PostUrl(Url, jsonTextyu1);

            texErpStr1.Text = DateTime.Now.ToString("yy-MM-dd HH:mm:ss ") + result2;


            Url = "https://www.qdlkd.net/ypstock/lkd/ypstock/ypstock";//原片库存


            StringWriter swyu2 = new StringWriter();
            JsonWriter writeryu2 = new JsonTextWriter(swyu2);
            writeryu2.WriteStartObject();
            writeryu2.WritePropertyName("ucode");
            writeryu2.WriteValue("auto");
            writeryu2.WritePropertyName("upwd");
            writeryu2.WriteValue("auto139");
            writeryu2.WritePropertyName("passkey");
            writeryu2.WriteValue("8BD2AD871782BD660A75C5A6D2902851DF1D64AF829D65BB");

            writeryu2.WritePropertyName("sheetid");
            writeryu2.WriteValue("");
            writeryu2.WritePropertyName("sheetname");
            writeryu2.WriteValue("白");
            writeryu2.WritePropertyName("kwname");
            writeryu2.WriteValue("");
            writeryu2.WritePropertyName("ply");
            writeryu2.WriteValue("5");

            writeryu2.WritePropertyName("height");
            writeryu2.WriteValue("");
            writeryu2.WritePropertyName("width");
            writeryu2.WriteValue("");
            writeryu2.WritePropertyName("cdname");
            writeryu2.WriteValue("");
            writeryu2.WritePropertyName("djname");
            writeryu2.WriteValue("");

            writeryu2.WritePropertyName("isypcc");
            writeryu2.WriteValue("1");

            writeryu2.WriteEndObject();
            writeryu2.Flush();
            string jsonTextyu2 = "";
            jsonTextyu2 = swyu2.GetStringBuilder().ToString();
            result2 = clsMyPublic.PostUrl(Url, jsonTextyu2);

            texErpStr1.Text = DateTime.Now.ToString("yy-MM-dd HH:mm:ss ") + result2;

    //            "ucode": "user",
    //"upwd": "pwd",
    //"passkey": "111",
    //"dicttype": "工序",
    //"wipno": "",
    //"ckcode": ""
        }

        private void butQGCha_Click(object sender, EventArgs e)
        {
            labQGshow.Text = "";

            string tSqlStr = "";
            DataTable tDt = new DataTable();
            tSqlStr = string.Concat(" SELECT distinct [Optimize_batch]'优化单号' ,[Sheet_glass_length]'原片玻璃长度' ,[Sheet_glass_width]'原片玻璃宽度'  ,[Single_name]'玻璃名称' ,convert(int , a.[Layout_number])'切割版图' ,[Addtime]'时间' FROM [dbo].[TabMB_LayoutData]a order by convert(int , a.[Layout_number])"
                      + "");
            if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
            {
                dataQG.DataSource = tDt;
                labQGshow.Text = tDt.Rows.Count.ToString();
                for (int i = 0; i < this.dataQG.ColumnCount; i++)
                {
                    dataQG.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataQG.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void butYuanWH_Click(object sender, EventArgs e)
        {
            if (clsMyPublic.mYuanDB != "1")
            { return; }
            labYuShow.Text = "";

            SeclectCmd = 0;
            string tSqlStr = "";
            DataTable tDt = new DataTable();
            tSqlStr = string.Concat("SELECT TOP 1000 [ID] ,[Pos_NO],a.name '储位名称', case  [PosType]  when '1'then '储存库位' when '2' then '大梭车'when '4' then '切割机上片位'  when '5' then '原片仓入口' end '库位功能',a.Stutus '库位状态',a.cmdstatus,a.sheetid '原片ID',a.sheetCount '数量',b.sheetname '原片名称',b.ply '原片厚度',b.height '高度',b.width '宽度' ,b.unit '单位',b.sheettype '原片分类',b.cdname '生产地',b.djname '等级',b.fmemo '备注' FROM [dbo].[tabPosition]  a left join TabErpSheetIn b on a.sheetid=b.sheetsid      "
                      + "  where a.Pos_NO <60 order by a.Pos_NO ");
            if (tSystem.mClsDBYu.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
            {
                dataGridYuan.DataSource = tDt;
                labYuShow.Text = tDt.Rows.Count.ToString();
                for (int i = 0; i < this.dataGridYuan.ColumnCount; i++)
                {
                    dataGridYuan.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataGridYuan.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                tSystem.mClsDBYu.DBClose();
            }
        }
        int SeclectCmd = 0;
        private void butYuanCMD_Click(object sender, EventArgs e)
        {
            if (clsMyPublic.mYuanDB != "1")
            { return; }
            labYuShow.Text = "";

            SeclectCmd = 1;
            string tSqlStr = "";
            DataTable tDt = new DataTable();
            tSqlStr = string.Concat(" SELECT TOP 1000 [CmdID] '指令编号', [From_Num]'源位置' ,[To_Num]'目标位置' ,[sheetid]'原片ID' ,[Status] ,[CmdFlow] ,[CmdType] ,[Addtime]  ,[UpdateTime]  ,[Finishtime]  FROM [dbo].[tabCmd] a"
                      + "  ");
            if (tSystem.mClsDBYu.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
            {
                dataGridYuan.DataSource = tDt;
                labYuShow.Text = tDt.Rows.Count.ToString();
                for (int i = 0; i < this.dataGridYuan.ColumnCount; i++)
                {
                    dataGridYuan.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataGridYuan.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                tSystem.mClsDBYu.DBClose();
            }
        }

        private void dataGridViewPo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = e.RowIndex;

            if (this.dataGridViewPo.Rows.Count > 1 && rowindex >= 0)
            {

                if (this.dataGridViewPo.CurrentRow.Index == 0)
                {

                    texPOS1.Text = dataGridViewPo.Rows[0].Cells[0].Value.ToString();
                    texPOS2.Text = dataGridViewPo.Rows[0].Cells[2].Value.ToString();
                    texPOS3.Text = dataGridViewPo.Rows[0].Cells[3].Value.ToString();
                    texPOS4.Text = dataGridViewPo.Rows[0].Cells[5].Value.ToString();
                    texPOS5.Text = dataGridViewPo.Rows[0].Cells[4].Value.ToString();
                    texPOS6.Text = dataGridViewPo.Rows[0].Cells[6].Value.ToString();
                    texPOS7.Text = dataGridViewPo.Rows[0].Cells[1].Value.ToString();
                    texPOS8.Text = dataGridViewPo.Rows[0].Cells[7].Value.ToString();

                }
                else
                {

                    texPOS1.Text = dataGridViewPo.Rows[rowindex].Cells[0].Value.ToString();
                    texPOS2.Text = dataGridViewPo.Rows[rowindex].Cells[2].Value.ToString();
                    texPOS3.Text = dataGridViewPo.Rows[rowindex].Cells[3].Value.ToString();
                    texPOS4.Text = dataGridViewPo.Rows[rowindex].Cells[5].Value.ToString();
                    texPOS5.Text = dataGridViewPo.Rows[rowindex].Cells[4].Value.ToString();
                    texPOS6.Text = dataGridViewPo.Rows[rowindex].Cells[6].Value.ToString();
                    texPOS7.Text = dataGridViewPo.Rows[rowindex].Cells[1].Value.ToString();
                    texPOS8.Text = dataGridViewPo.Rows[rowindex].Cells[7].Value.ToString();


                }
            }
        }

        private void dataGridYuan_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataQG_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataGMB1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataGMB2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void DataG_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dOutGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataProOut_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataZKIN_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dOutGridViewZK_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataZKProOut_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataMBWH_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataZKWH_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataGridViewCha_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataGridViewPo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataGrid1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataGZKSet_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataGridYuan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = e.RowIndex;
            this.texCmdID.Text = "";
            if (this.dataGridYuan.Rows.Count > 1 && rowindex >= 0)
            {
                string tStr = "";
                if (this.dataGridYuan.CurrentRow.Index == 0)
                {
                     tStr = dataGridYuan.Rows[0].Cells[0].Value.ToString();
                     if (IfNum(tStr))
                     {
                         if (SeclectCmd == 1)
                         {
                             this.texCmdID.Text = tStr;
                             return;
                         }

                         int tIA = int.Parse(tStr);
                         if (tIA < 51)
                         {
                             this.comboPos2.Text  = tStr;
                         }
                         else
                         {
                             this.comboPos1.Text = tStr;
                         }
                     }
                }
                else
                {
                    tStr = dataGridYuan.Rows[rowindex].Cells[0].Value.ToString();
                    if (IfNum(tStr))
                    {
                        int tIA = int.Parse(tStr);
                        if (SeclectCmd == 1)
                        {
                            this.texCmdID.Text = tStr;
                            return;
                        }
                        if (tIA < 51)
                        {
                            this.comboPos2.Text = tStr;
                        }
                        else
                        {
                            this.comboPos1.Text = tStr;
                        }
                    }
                }
            }
        }


        public void ShowMessageList(int tIndex, string tpShowStr)
        {
            ListBox tpList;
            if (tIndex == 1)
            {
                tpList = this.listYuanIN;
            }
            else
            {
                tpList = this.listYuan;
            }

            if (this.InvokeRequired)
            {
                deShowlist sl = new deShowlist(ShowMessageList);
                this.Invoke(sl, new object[] { tIndex, tpShowStr });
            }
            else
            {
                if (tpList.Items.Count > 1000)
                {
                    tpList.SelectedIndex = 0;

                    tpList.Items.Remove(tpList.SelectedItem);//删除一条记录
                    //tpList.Items.Clear();//删除多条记录
                }

                tSystem.mFile.WriteLog("", tpShowStr + " ");
                tpShowStr = DateTime.Now.ToString("H:m:s") + " " + tpShowStr;
                tpList.Items.Add(tpShowStr);//添加记录
                
              
                {
                    tpList.SelectedIndex = tpList.Items.Count - 1;//保持最新记录保持在最前面
                }
            }
        }
        private void butYuCmd1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  原片仓储 进行回库操作  上片工位：" + comboPos1.Text + "  储存工位 " + comboPos2.Text + "  ?   \r\n   请看库位号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBYu.ExecuteDisposeStatus("Pro_SetCmd", "1", comboPos1.Text + "+" + comboPos2.Text, ref tRetData, ref tRetStr);
                tSystem.mClsDBYu.DBClose();
                ShowMessageList(0, "进库 "+tRetData);
            }
        }

        private void butYuCmd2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  原片仓储 出库到上片位操作  上片工位：" + comboPos1.Text + "  流程卡号 " + comboPos2.Text + " 尺寸 " + "*" + texXCha4.Text + " 设备异常?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBYu.ExecuteDisposeStatus("Pro_SetCmd", "2", comboPos1.Text + "+" + comboPos2.Text, ref tRetData, ref tRetStr);
                tSystem.mClsDBYu.DBClose();
                ShowMessageList(0, "出库 " + tRetData);
            }
        }

        private void butYuAuto_Click(object sender, EventArgs e)
        {
            string tSqlStr = "";

            if (clsMyPublic.mYuanAuto  == "1")
            {
                tSqlStr = " update [TabQG_OP] set OMode='99' where S_Line='3' ";
                butYuAuto.Text = "叫料模式（手动）"; clsMyPublic.SetPrivete("YuanAuto", "0");
                clsMyPublic.mYuanAuto = "0";
            }
            else
            {
                tSqlStr = " update [TabQG_OP] set OMode='1' where S_Line='3' ";
                butYuAuto.Text = "叫料模式（自动）";
                clsMyPublic.SetPrivete("YuanAuto", "1");
                clsMyPublic.mYuanAuto = "1";
            }
            


            tSystem.mClsDBYu.Execute_Command(tSqlStr);
            tSystem.mClsDBYu.DBClose();
        }

        private void butYuanKUcun_Click(object sender, EventArgs e)
        {
            string result2 = "", Url = "";
            Url = "https://www.qdlkd.net/ypstock/lkd/ypstock/ypstock";//原片库存


            StringWriter swyu2 = new StringWriter();
            JsonWriter writeryu2 = new JsonTextWriter(swyu2);
            writeryu2.WriteStartObject();

            //writeryu2.WritePropertyName("ucode");
            //writeryu2.WriteValue("auto");
            //writeryu2.WritePropertyName("upwd");
            //writeryu2.WriteValue("allauto");
            //writeryu2.WritePropertyName("passkey");
            //writeryu2.WriteValue("2736FA43824279C9CC81C2D46F4FACB59F442E30B93915586FA3E75D68459358");

            writeryu2.WritePropertyName("ucode");
            writeryu2.WriteValue(clsMyPublic.mErp_ucode);//("auto");
            writeryu2.WritePropertyName("upwd");
            writeryu2.WriteValue(clsMyPublic.mErp_upwd);// ("9A395478FE3EFEAE3323722C56B311F4");
            writeryu2.WritePropertyName("passkey");
            writeryu2.WriteValue(clsMyPublic.mErp_passkey);// ("5F229626553F69F01DAB380655702738918A2D2150AD44CF59910E0ADE552FBDF7A548489F8FC253");

            writeryu2.WritePropertyName("sheetid");
            writeryu2.WriteValue("");
            writeryu2.WritePropertyName("sheetname");
            writeryu2.WriteValue("");
            writeryu2.WritePropertyName("kwname");
            writeryu2.WriteValue("");
            writeryu2.WritePropertyName("ply");
            writeryu2.WriteValue("");

            writeryu2.WritePropertyName("height");
            writeryu2.WriteValue("");
            writeryu2.WritePropertyName("width");
            writeryu2.WriteValue("");
            writeryu2.WritePropertyName("cdname");
            writeryu2.WriteValue("");
            writeryu2.WritePropertyName("djname");
            writeryu2.WriteValue("");

            writeryu2.WritePropertyName("isypcc");
            writeryu2.WriteValue("1");

            writeryu2.WriteEndObject();
            writeryu2.Flush();
            string jsonTextyu2 = "";
            jsonTextyu2 = swyu2.GetStringBuilder().ToString();
            result2 = clsMyPublic.PostUrl(Url, jsonTextyu2);
            GetData.ClsGetYuanData tGetData = new GetData.ClsGetYuanData();
            DataTable tDt = new DataTable();
            tDt = tGetData.GetSheetIn(result2);

            if (tDt != null)
            {
                string tRetStr = "";
                tSystem.mClsDBYu.ExecuteTvpIndex("BYuan_InsertErpSheet", "1000", tDt, ref tRetStr);
                tSystem.mClsDBYu.DBClose();
            }

            

            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);

            writer.WriteStartObject();
            writer.WritePropertyName("ucode");
            writer.WriteValue(clsMyPublic.mErp_ucode);// ("auto");  ("ucode");// 
            writer.WritePropertyName("upwd");
            writer.WriteValue(clsMyPublic.mErp_upwd);// ("9A395478FE3EFEAE3323722C56B311F4");("upwd");// 
            writer.WritePropertyName("passkey");
            writer.WriteValue(clsMyPublic.mErp_passkey);
            writer.Flush();
            string jsonText = "";
            jsonText = sw.GetStringBuilder().ToString();


            {
                Url = "https://www.qdlkd.net/ypstock/lkd/ypstock/yplist";
            }

            result2 = clsMyPublic.PostUrl(Url, jsonText);
            tDt = new DataTable();
            tDt = tGetData.GetSheetBasic(result2);
            if (tDt != null)
            {
                string tRetStr = "";
                tSystem.mClsDBYu.ExecuteTvpIndex("BYuan_InsertErpBasic", "1000", tDt, ref tRetStr);
                tSystem.mClsDBYu.DBClose();
            }

            if (clsMyPublic.mYuanDB != "1")
            { return; }
            labYuShow.Text = "";

            string tSqlStr = "";
            tDt = new DataTable();
            tSqlStr = string.Concat("select * from [TabErpSheetIn] "
                      + "  ");
            if (tSystem.mClsDBYu.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
            {
                dataGridYuan.DataSource = tDt;
                labYuShow.Text = tDt.Rows.Count.ToString();
                for (int i = 0; i < this.dataGridYuan.ColumnCount; i++)
                {
                    dataGridYuan.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataGridYuan.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                tSystem.mClsDBYu.DBClose();
            }

        }

        private void butCmdDel_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  原片仓储 删除指令信息  指令编号：" + texCmdID.Text + " 设备异常?   \r\n   请看清序号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBYu.ExecuteDisposeStatus("Pro_SetCmd", "99", texCmdID.Text.Trim() , ref tRetData, ref tRetStr);
                tSystem.mClsDBYu.DBClose();
                ShowMessageList(0, "出库 " + tRetData);
            }
        }

        private void butMBFinish_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("   磨边仓储入库 强制完成优化单号： " + textMBOPt.Text + "  异常?   \r\n   请看清优化号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", textMBOPt.Text, "999", ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void butMBFinishOut_Click(object sender, EventArgs e)
        {
            if (dOutGridView.RowCount > 1)
            {
                string tOptimize = "", Process_number = "",tOrder_id="";
                tOptimize = dOutGridView.Rows[1].Cells[9].Value.ToString();
                Process_number = dOutGridView.Rows[1].Cells[0].Value.ToString(); tOrder_id = dOutGridView.Rows[1].Cells[10].Value.ToString();
                DialogResult dr = MessageBox.Show("   磨边仓储出库 强制完成 优化单号： " + tOptimize + " 订单号：" + tOrder_id + " 流程卡号：" + Process_number + "  异常?   \r\n   请看清优化号 ", "提示", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    string tRetData = "", tRetStr = "";
                    tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", tOptimize, "1999+" + tOrder_id + "+" + Process_number, ref tRetData, ref tRetStr);
                    tSystem.mClsDBUPdate.DBClose();
                }
            }
        }

        private delegate void deShowFrame(int _ShowIndex);//代理
        private FrmZKShow tFrmZkShow;
        public void ShowShowFrame(int _ShowIndex)
        {

            if (this.InvokeRequired)
            {
                deShowFrame sl = new deShowFrame(ShowShowFrame);
                this.Invoke(sl, new object[] { _ShowIndex });
            }
            else
            {
                if (_ShowIndex == 2)
                {
                    Control.CheckForIllegalCrossThreadCalls = false;
                    if (tFrmZkShow == null)
                    {
                        tFrmZkShow = new FrmZKShow(tSystem);
                        tFrmZkShow.StartPosition = FormStartPosition.CenterScreen;

                        tFrmZkShow.Show();
                    }
                    else
                    {
                        if (tFrmZkShow.IsDisposed)
                        {
                            tFrmZkShow = new FrmZKShow(tSystem);
                            tFrmZkShow.StartPosition = FormStartPosition.CenterScreen;

                            tFrmZkShow.Show();
                        }
                        else
                        {

                            tFrmZkShow.WindowState = FormWindowState.Normal;

                            tFrmZkShow.Activate();
                        }

                    }
                }
                else
                {

                }
            }
        }

        private void butZKEixtShow_Click(object sender, EventArgs e)
        {
            ShowShowFrame(2);
        }

        private void butMBWHSet7_Click(object sender, EventArgs e)
        {
            if (IfNum(MBcbSet1.Text) == false | IfNum(MBcbSet2.Text) == false)
            {
                MessageBox.Show("库号或格号不是数字", "错误", MessageBoxButtons.OK);
            }
            else if (int.Parse(MBcbSet1.Text) < 1 | int.Parse(MBcbSet1.Text) > 8)
            {
                MessageBox.Show("库号没有", "错误", MessageBoxButtons.OK);
            }
            else if (int.Parse(MBcbSet2.Text) < 1 | int.Parse(MBcbSet2.Text) > 58)
            {
                MessageBox.Show("格号没有", "错误", MessageBoxButtons.OK);
            }
            else
            {
                texMBWHset1.Text = MBcbSet1.Text ;
                texMBWHset2.Text = MBcbSet2.Text;
                texMBWHshow.Text = "磨边仓储库号 "+texMBWHset1.Text + "格号 "+texMBWHset2.Text+"进行库位初始化！";
                groupBoxMBpass.Visible = true;
            }
           
        }

        private void butMBWHok_Click(object sender, EventArgs e)
        {

            if (clsMyPublic.mUser_name == texMBuserName.Text.Trim() & clsMyPublic.mPassword == texMBpas.Text.Trim())
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", "", "2001+" + texMBWHset1.Text + "+" + texMBWHset2.Text, ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
                GetdataWH(" and a.WH_NO='" + texMBWHset1.Text.Trim() + "' and a.WH_Num='" + texMBWHset2.Text.Trim() + "' ");

                groupBoxMBpass.Visible = false;
                texMBuserName.Text  = "";
                texMBpas.Text = "";
            }
            else if (clsMyPublic.mUser_name != texMBuserName.Text.Trim())
            {
                texMBuserName.Text = "";
            }
            else
            {
                texMBpas.Text  = "";
            }
        }

        private void butMBWHcannel_Click(object sender, EventArgs e)
        {
            groupBoxMBpass.Visible = false ;
        }

        private void buzZKWHSet7_Click(object sender, EventArgs e)
        {
            if (IfNum(ZKcbSet1.Text) == false | IfNum(ZKcbSet2.Text) == false)
            {
                MessageBox.Show("库号或格号不是数字", "错误", MessageBoxButtons.OK);
            }
            else if (int.Parse(ZKcbSet1.Text) < 1 | int.Parse(ZKcbSet1.Text) > 8)
            {
                MessageBox.Show("库号没有", "错误", MessageBoxButtons.OK);
            }
            else if (int.Parse(ZKcbSet2.Text) < 1 | int.Parse(ZKcbSet2.Text) > 58)
            {
                MessageBox.Show("格号没有", "错误", MessageBoxButtons.OK);
            }
            else
            {
                texZKWHset1.Text = ZKcbSet1.Text;
                texZKWHset2.Text = ZKcbSet2.Text;
                texZKWHshow.Text = "中空仓储库号 " + texZKWHset1.Text + "格号 " + texZKWHset2.Text + "进行库位初始化！";
                groupBoxZKpass.Visible = true;
            }

        }

        private void butZKWHok_Click(object sender, EventArgs e)
        {
            if (clsMyPublic.mUser_name == texZKuserName.Text.Trim() & clsMyPublic.mPassword == texZKpas.Text.Trim())
            {
                string tRetData = "", tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteDisposeStatus("Pro_DisposeStatus", "", "3001+" + texZKWHset1.Text + "+" + texZKWHset2.Text, ref tRetData, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
                GetdataZKWH(" and a.WH_NO='" + texZKWHset1.Text.Trim() + "' and a.WH_Num='" + texZKWHset2.Text.Trim() + "' ");
                texZKuserName.Text = ""; texZKpas.Text = "";
                groupBoxZKpass.Visible = false;
            }
            else if (clsMyPublic.mUser_name != texZKuserName.Text.Trim())
            {
                texZKuserName.Text = "";
            }
            else
            {
                texZKpas.Text  = "";
            }
        }

        private void comMBline_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(IfNum(comMBline.Text))
            {
                tClsShowFrm.mShowMBline = int.Parse(comMBline.Text);
            }
        }

        private void butStatSet1_Click(object sender, EventArgs e)
        {

            tSystem.mClsDBUPdate.Execute_Command("  update [tabOp] set InStatus ='3' where NO ='" + comMBline .Text.Trim()+ "'");
            tSystem.mClsDBUPdate.DBClose();
        }

        private void butStatSet2_Click(object sender, EventArgs e)
        {
            tSystem.mClsDBUPdate.Execute_Command("  update [tabOp] set InStatus ='2' where NO ='" + comMBline.Text.Trim() + "'");
            tSystem.mClsDBUPdate.DBClose();

        }

        private void butMBWHcount_Click(object sender, EventArgs e)
        {
            try
            {
                string tSqlstr = "";
                DataTable tDT = new DataTable();
                tSqlstr = "select WH_NO 库号 ,count(WH_NO) 数量  from TabMB_Data where Status=14 group by WH_NO  order by WH_NO ";
                if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlstr, ref tDT) == "")
                {
                    if (tDT.Rows.Count > 0)
                    {
                        this.dataMBWH.DataSource = tDT;
                        labMbshu.Text = tDT.Rows.Count.ToString();
                        tClsShowFrm.SetBackColor(dataMBWH, 10, 0);
                        for (int i = 0; i < this.dataMBWH.Columns.Count; i++)
                        {
                            dataMBWH.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                            this.dataMBWH.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                    }
                    tSystem.mClsDBUPdate.DBClose();
                }
            }
            catch
            {
            }
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comShun.SelectedIndex = 0;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                string tT1 = "", tT2 = ""; string Wtime = "", WhereStr = "";
               
                dp_S_pl_dateC.Format = DateTimePickerFormat.Custom;
                dp_S_pl_dateC.CustomFormat = "yyyy-MM-dd";// HH:mm:ss
                dp_End_dateC.Format = DateTimePickerFormat.Custom;
                dp_End_dateC.CustomFormat = "yyyy-MM-dd";// HH:mm:ss
                dp_S_pl_timeC.Format = DateTimePickerFormat.Custom;
                dp_S_pl_timeC.CustomFormat = "HH:mm:ss";// 
                dp_End_timeC.Format = DateTimePickerFormat.Custom;
                dp_End_timeC.CustomFormat = "HH:mm:ss";// 


                tT1 = dp_S_pl_dateC.Text + " " + dp_S_pl_timeC.Text.ToString();
                tT2 = dp_End_dateC.Text + " " + dp_End_timeC.Text;


                string tSqlStr = "";
                if (radioButtonC1.Checked == true)
                {
                    chart1.Series[0].YValueMembers = "切割片数";
                    chart1.Series[1].YValueMembers = "磨边片数";
                    chart1.Series[2].YValueMembers = "钢化片数";


                    chart1.Series[1].XValueMember = "时间";
                    chart1.Series[0].Color = Color.Yellow;
                    chart1.Series[1].Color = Color.Cyan;
                    chart1.Series[2].Color = Color.Lime;


                    //chart1.ChartAreas[0].Area3DStyle.LightStyle = LightStyle.Realistic;

                    this.chart1.BackSecondaryColor = Color.FromArgb(6, 64, 102);
                    string str = chart1.Series[0].YValueMembers.ToString();

                    chart1.DataSource = null;
                    chart1.DataBind();


                    tSqlStr = "select RIGHT(RTRIM( case when a.date  is not null then a.date when   b.date  is not null then b.date  when  c.date  is not null then c.date end),5)'时间',sum(isnull( a.切割片数,0)) '切割片数',sum(isnull(b.磨边片数,0))'磨边片数' ,sum(isnull(c.钢化片数 ,0))'钢化片数'"
                                + " from "
                                + "(select convert(varchar(13),a.InputTime ,120) as [date] ,count(convert(varchar(13),a.InputTime ,120))'切割片数'  from TabMB_Data a where a.InputTime  between '" + tT1 + "' and '" + tT2 + "'group by convert(varchar(13),a.InputTime ,120)  ) a "
                                + " full join "
                                + "(select convert(varchar(13),a.InAtime ,120) as [date] ,count(convert(varchar(13),a.InAtime ,120))'磨边片数'  from TabMB_Data a where a.InAtime  between '" + tT1 + "' and '" + tT2 + "'group by convert(varchar(13),a.InAtime ,120)) b "
                                + " on a.date =b.date "
                                + " full join "
                                + "(select convert(varchar(13),a.outAtime ,120) as [date] ,count(convert(varchar(13),a.outAtime ,120))'钢化片数'  from TabMB_Data a where a.outAtime  between '" + tT1 + "' and '" + tT2 + "'  and a.MBExit in (1,2,3)  group by convert(varchar(13),a.outAtime ,120)) c "
                                + " on  b.date=c.date and a.date =b.date  group by  case when a.date  is not null then a.date when   b.date  is not null then b.date  when  c.date  is not null then c.date end   order by '时间'";
                }
                else
                {

                    chart1.Series[0].YValueMembers = "切割面积";
                    chart1.Series[1].YValueMembers = "磨边面积";
                    chart1.Series[2].YValueMembers = "钢化面积";


                    chart1.Series[1].XValueMember = "时间";
                    chart1.Series[0].Color = Color.Yellow;
                    chart1.Series[1].Color = Color.Cyan;
                    chart1.Series[2].Color = Color.Lime;


                    //chart1.ChartAreas[0].Area3DStyle.LightStyle = LightStyle.Realistic;

                    this.chart1.BackSecondaryColor = Color.FromArgb(6, 64, 102);
                    string str = chart1.Series[0].YValueMembers.ToString();

                    chart1.DataSource = null;
                    chart1.DataBind();


                    tSqlStr = "select RIGHT(RTRIM( case when a.date  is not null then a.date when   b.date  is not null then b.date  when  c.date  is not null then c.date end),5) '时间',sum(isnull( a.切割面积,0)) '切割面积',sum(isnull(b.磨边面积,0))'磨边面积' ,sum(isnull(c.钢化面积 ,0))'钢化面积'"
                               + " from "
                               + "(select convert(varchar(13),a.InputTime ,120) as [date] ,sum(convert(float,a.Single_long)/1000 * convert(float,a.Single_short)/1000)'切割面积'   from TabMB_Data a where a.InputTime  between '" + tT1 + "' and '" + tT2 + "'group by convert(varchar(13),a.InputTime ,120)  ) a "
                               + " full join "
                               + " (select convert(varchar(13),a.InAtime ,120) as [date] ,sum(convert(float,a.Single_long)/1000 * convert(float,a.Single_short)/1000) '磨边面积'  from TabMB_Data a where a.InAtime  between '" + tT1 + "' and '" + tT2 + "'group by convert(varchar(13),a.InAtime ,120)) b"
                               + " on a.date =b.date "
                               + " full join "
                               + " (select convert(varchar(13),a.outAtime ,120) as [date] ,sum(convert(float,a.Single_long)/1000 * convert(float,a.Single_short)/1000) '钢化面积'  from TabMB_Data a where a.outAtime  between '" + tT1 + "' and '" + tT2 + "'  and a.MBExit in (2,3)  group by convert(varchar(13),a.outAtime ,120)) c"
                               + " on  b.date=c.date and a.date =b.date     group by  case when a.date  is not null then a.date when   b.date  is not null then b.date  when  c.date  is not null then c.date end   order by '时间'";

                }
                DataTable tDT = new DataTable();
          
                if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlStr, ref tDT) == "")
                {
                    chart1.DataSource = tDT;
                    chart1.DataBind();
                    dataGridViewChart.DataSource = tDT;
                    float T1 = 0, T2 = 0, T3 = 0;
                    if (tDT.Rows.Count > 0)
                    {
                        for (int i = 0; i < tDT.Rows.Count; i++)
                        {
                            T1 = T1 + float.Parse(tDT.Rows[i][1].ToString());
                            T2 = T2 + float.Parse(tDT.Rows[i][2].ToString());
                            T3 = T3 + float.Parse(tDT.Rows[i][3].ToString());
                        }
                        this.textTcha1.Text = T1.ToString(); this.textTcha2.Text = T2.ToString(); this.textTcha3.Text = T3.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

        private void butZKFlow_Click(object sender, EventArgs e)
        {
            string tRtrStr = "", tRetData = "";
            tSystem.mClsDBUPdateZK.ExecuteTvpOptimize_batch("Pro_EnPortZKauto", "", "", ref tRtrStr,ref tRetData);
        }

       

        private void dataGridViewCha_SelectionChanged(object sender, EventArgs e)
        {
            label27.Text = dataGridViewCha.SelectedRows.Count.ToString();
        }

        private void texZKEnport_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.tClsShowFrm.mShowZKEnport = int.Parse(texZKEnport.Text.Trim());
        }

        private void butNYuCmd1_Click(object sender, EventArgs e)//原片仓创建指令
        {
            DialogResult dr = MessageBox.Show("  原片仓储 进行指令创建  源工位：" + comboPos1.Text + "  目的工位 " + comboPos2.Text + "  ?   \r\n   请看库位号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                DataSet dataSet = new DataSet(); DataTable tDt = new DataTable(); tDt = null;
                string tRetData = "", tRetStr = ""; int tRows = 0;

                dataSet = tSystem.mClsDBUPdate.ExecuteBYuan("BYuan_00AddCmd", 1, 2, comboPos1.Text.Trim() + "_" + comboPos2.Text.Trim(), tDt, "1", ref tRetData, ref tRetStr, ref tRows);
                tSystem.mClsDBUPdate.DBClose();
                if (dataSet == null) { return ; }
                if (dataSet.Tables.Count > 0)
                {
                    ShowMessageList(0, "指令创建 结果：" + dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0].ToString() + " 指令" + dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][1]);
                    comboPos1.Text = "";
                    comboPos2.Text = "";
                }
            }
        }

        private void butNYuCmd3_Click(object sender, EventArgs e)
        {
            AddYuPos(1);
        }

        private void butYIN2_Click(object sender, EventArgs e)
        {
            string result2 = "", Url = "";
            Url = "https://www.qdlkd.net/ypstock/lkd/ypstock/ypstock";//原片库存


            StringWriter swyu2 = new StringWriter();
            JsonWriter writeryu2 = new JsonTextWriter(swyu2);
            writeryu2.WriteStartObject();

            //writeryu2.WritePropertyName("ucode");
            //writeryu2.WriteValue("auto");
            //writeryu2.WritePropertyName("upwd");
            //writeryu2.WriteValue("allauto");
            //writeryu2.WritePropertyName("passkey");
            //writeryu2.WriteValue("2736FA43824279C9CC81C2D46F4FACB59F442E30B93915586FA3E75D68459358");

            writeryu2.WritePropertyName("ucode");
            writeryu2.WriteValue(clsMyPublic.mErp_ucode);//("auto");
            writeryu2.WritePropertyName("upwd");
            writeryu2.WriteValue(clsMyPublic.mErp_upwd);// ("9A395478FE3EFEAE3323722C56B311F4");
            writeryu2.WritePropertyName("passkey");
            writeryu2.WriteValue(clsMyPublic.mErp_passkey);// ("5F229626553F69F01DAB380655702738918A2D2150AD44CF59910E0ADE552FBDF7A548489F8FC253");

            writeryu2.WritePropertyName("sheetid");
            writeryu2.WriteValue("");
            writeryu2.WritePropertyName("sheetname");
            writeryu2.WriteValue("");
            writeryu2.WritePropertyName("kwname");
            writeryu2.WriteValue("");
            writeryu2.WritePropertyName("ply");
            writeryu2.WriteValue("");

            writeryu2.WritePropertyName("height");
            writeryu2.WriteValue("");
            writeryu2.WritePropertyName("width");
            writeryu2.WriteValue("");
            writeryu2.WritePropertyName("cdname");
            writeryu2.WriteValue("");
            writeryu2.WritePropertyName("djname");
            writeryu2.WriteValue("");

            writeryu2.WritePropertyName("isypcc");
            writeryu2.WriteValue("1");

            writeryu2.WriteEndObject();
            writeryu2.Flush();
            string jsonTextyu2 = "";
            jsonTextyu2 = swyu2.GetStringBuilder().ToString();
            result2 = clsMyPublic.PostUrl(Url, jsonTextyu2);
            GetData.ClsGetYuanData tGetData = new GetData.ClsGetYuanData();
            DataTable tDt = new DataTable();
            tDt = tGetData.GetSheetIn(result2);

            if (tDt != null)
            {
                string tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteTvpIndex("BYuan_InsertErpSheet", "1000", tDt, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }



            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);

            writer.WriteStartObject();
            writer.WritePropertyName("ucode");
            writer.WriteValue(clsMyPublic.mErp_ucode);// ("auto");  ("ucode");// 
            writer.WritePropertyName("upwd");
            writer.WriteValue(clsMyPublic.mErp_upwd);// ("9A395478FE3EFEAE3323722C56B311F4");("upwd");// 
            writer.WritePropertyName("passkey");
            writer.WriteValue(clsMyPublic.mErp_passkey);
            writer.Flush();
            string jsonText = "";
            jsonText = sw.GetStringBuilder().ToString();


            {
                Url = "https://www.qdlkd.net/ypstock/lkd/ypstock/yplist";
            }

            result2 = clsMyPublic.PostUrl(Url, jsonText);
            tDt = new DataTable();
            tDt = tGetData.GetSheetBasic(result2);
            if (tDt != null)
            {
                string tRetStr = "";
                tSystem.mClsDBUPdate.ExecuteTvpIndex("BYuan_InsertErpBasic", "1000", tDt, ref tRetStr);
                tSystem.mClsDBUPdate.DBClose();
            }

            //if (clsMyPublic.mYuanDB != "1")
            //{ return; }

            string tWhereStr = "";
            if (comYErp1.Text.Trim().Length > 0)
            {
                tWhereStr = tWhereStr + " and   a.sheettype ='" + comYErp1.Text.Trim() + "' ";
            }
            if (comYErp2.Text.Trim().Length > 0)
            {
                tWhereStr = tWhereStr + " and   a.sheetname='" + comYErp2.Text.Trim() + "' ";
            }
            if (comYErp3.Text.Trim().Length > 0)
            {
                tWhereStr = tWhereStr + " and   a.ply='" + comYErp3.Text.Trim() + "' ";
            }
            labYINShow1.Text = "";

            string tSqlStr = "";
            tDt = new DataTable();
            tSqlStr = string.Concat("select * from BYuan_ErpSheetIn a where YId>0" + tWhereStr
                      + "  ");
            if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
            {
                dataGridYuanErp1.DataSource = tDt;
                labYINShow1.Text = tDt.Rows.Count.ToString();
                for (int i = 0; i < this.dataGridYuanErp1.ColumnCount; i++)
                {
                    dataGridYuanErp1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataGridYuanErp1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void dataGridYuanErp1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = e.RowIndex;

            if (this.dataGridYuanErp1.Rows.Count > 1 && rowindex >= 0)
            {
                string value1 = dataGridYuanErp1.Rows[rowindex].Cells[0].Value.ToString();
                if (this.dataGridYuanErp1.CurrentRow.Index == 0 || this.dataGridYuanErp1.CurrentRow.Index == 0)
                {

                    textYIN1.Text = dataGridYuanErp1.Rows[0].Cells[1].Value.ToString();
                    textYIN2.Text = dataGridYuanErp1.Rows[0].Cells[2].Value.ToString();
                    textYIN3.Text = dataGridYuanErp1.Rows[0].Cells[3].Value.ToString();
                    textYIN4.Text = dataGridYuanErp1.Rows[0].Cells[4].Value.ToString();
                    textYIN5.Text = dataGridYuanErp1.Rows[0].Cells[13].Value.ToString();
                    textYIN6.Text = dataGridYuanErp1.Rows[0].Cells[14].Value.ToString();
                    textYIN7.Text = dataGridYuanErp1.Rows[0].Cells[18].Value.ToString();
                    //textYIN8.Text = dataGridYuanErp1.Rows[0].Cells[4].Value.ToString();

                    comYErp1.Text =dataGridYuanErp1.Rows[0].Cells[13].Value.ToString();
                    comYErp2.Text = dataGridYuanErp1.Rows[0].Cells[14].Value.ToString();
                    comYErp3.Text = dataGridYuanErp1.Rows[0].Cells[15].Value.ToString();

                }
                else
                {

                    textYIN1.Text = dataGridYuanErp1.Rows[rowindex].Cells[1].Value.ToString();
                    textYIN2.Text = dataGridYuanErp1.Rows[rowindex].Cells[2].Value.ToString();
                    textYIN3.Text = dataGridYuanErp1.Rows[rowindex].Cells[3].Value.ToString();
                    textYIN4.Text = dataGridYuanErp1.Rows[rowindex].Cells[4].Value.ToString();
                    textYIN5.Text = dataGridYuanErp1.Rows[rowindex].Cells[13].Value.ToString();
                    textYIN6.Text = dataGridYuanErp1.Rows[rowindex].Cells[14].Value.ToString();
                    textYIN7.Text = dataGridYuanErp1.Rows[rowindex].Cells[18].Value.ToString();
                    //textYIN8.Text = dataGridYuanErp1.Rows[rowindex].Cells[4].Value.ToString();

                    comYErp1.Text = dataGridYuanErp1.Rows[rowindex].Cells[13].Value.ToString();
                    comYErp2.Text = dataGridYuanErp1.Rows[rowindex].Cells[14].Value.ToString();
                    comYErp3.Text = dataGridYuanErp1.Rows[rowindex].Cells[15].Value.ToString();


                }
            }
        }

        private void butYIN1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  原片仓储 进行入库资料创建  入口工位：" + comYuan.Text + "  原片编号 " + textYIN1.Text + "  数量 " + textYIN8.Text + "  ?   \r\n   请看库位号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                DataSet dataSet = new DataSet(); DataTable tDt = new DataTable(); tDt = null;
                string tRetData = "", tRetStr = ""; int tRows = 0;

                dataSet = tSystem.mClsDBUPdate.ExecuteBYuan("BYuan_00AddTask", 1, int.Parse(textYIN1.Text), textYIN8.Text.Trim(), tDt, comYuan.Text.Trim(), ref tRetData, ref tRetStr, ref tRows);
                tSystem.mClsDBUPdate.DBClose();
                if (dataSet == null) { return; }
                if (dataSet.Tables.Count > 0)
                {
                    ShowMessageList(1, "入口信息创建 结果：" + dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0].ToString() + " 指令" + dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][1]);
                    comboPos1.Text = "";
                    comboPos2.Text = "";
                    butYIN3_Click(sender, e);
                }
            }
        }

        private void butYIN3_Click(object sender, EventArgs e)
        {
            labYINShow2.Text = "";

            DataTable tDt;
            string tSqlStr = "";
            tDt = new DataTable();
            tSqlStr = string.Concat("select * from BYuanEnport_Data where  Enport ='" + comYuan.Text.Trim() + "'"
                      + "  ");
            if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
            {
                dataGridYuanIN1.DataSource = tDt;
                labYINShow2.Text = tDt.Rows.Count.ToString();
                for (int i = 0; i < this.dataGridYuanIN1.ColumnCount; i++)
                {
                    dataGridYuanIN1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataGridYuanIN1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void butYIN4_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  原片仓储 删除资料  入口工位：" + comYuan.Text + "  原片编号 " + textYXc1.Text + "  名称 " + textYXc2.Text + "  ?   \r\n   请看库位号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                string tSqlstr = "  delete BYuanEnport_Data where Mat_ID ='" + textYXc1.Text + " '";
                tSqlstr = "  delete BYuanEnport_Data where Mat_ID ='" + textYXc1.Text + " '";

                tSystem.mClsDBUPdate.Execute_Command(tSqlstr);
                tSystem.mClsDBUPdate.DBClose();

                ShowMessageList(1, "  原片仓储 删除资料  入口工位：" + comYuan.Text + "  原片编号 " + textYXc1.Text + "  名称 " + textYXc2.Text);
                   
                    butYIN3_Click(sender, e);
                
            }
        }

        private void dataGridYuanIN1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = e.RowIndex;

            if (this.dataGridYuanIN1.Rows.Count > 1 && rowindex >= 0)
            {
                string value1 = dataGridYuanIN1.Rows[rowindex].Cells[0].Value.ToString();
                if (this.dataGridYuanIN1.CurrentRow.Index == 0 || this.dataGridYuanIN1.CurrentRow.Index == 0)
                {

                    textYXc1.Text = dataGridYuanIN1.Rows[0].Cells[0].Value.ToString();
                    textYXc2.Text = dataGridYuanIN1.Rows[0].Cells[1].Value.ToString();
              

                }
                else
                {

                    textYXc1.Text = dataGridYuanIN1.Rows[rowindex].Cells[0].Value.ToString();
                    textYXc2.Text = dataGridYuanIN1.Rows[rowindex].Cells[1].Value.ToString();
                 


                }
            }
        }

        private void butYIN6_Click(object sender, EventArgs e)
        {
            labYINShow2.Text = "";

            DataTable tDt;
            string tSqlStr = "";
            tDt = new DataTable();
            tSqlStr = string.Concat("select a.Pos_NO ,b.Mat_Name ,b.Mat_Width,b.Mat_Length ,b.Mat_ply ,b.mat_count ,b.Mat_cdname ,b.Mat_djname ,b.sheetsid ,b.Enport ,b.Addtime ,a.PLC_Flow, case when a.Plc_Status =1 and b.Mat_Name  is not null  then '库内' when a.PLC_Status =2 then '空载架' when a.PLC_Status =9 then '空位'else '异常' end '库位状态',case when a.Pos_if=0 then '正常' else '禁用' end '使用状态' from BYuan_WH a left join BYuanMat_Data b on a.PLC_Flow =b.BY_Plcflow order by a.Pos_NO   "
                      + "  ");
            if (tSystem.mClsDBUPdate.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
            {
                dataGridYUWH1.DataSource = tDt;
                labYINShow2.Text = tDt.Rows.Count.ToString();
                tClsShowFrm.SetBackColor(dataGridYUWH1, 12, 13);
                for (int i = 0; i < this.dataGridYUWH1.ColumnCount; i++)
                {
                    dataGridYUWH1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    this.dataGridYUWH1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                tSystem.mClsDBUPdate.DBClose();
            }
        }

        private void dataGridYUWH1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowindex = e.RowIndex;

            if (this.dataGridYUWH1.Rows.Count > 1 && rowindex >= 0)
            {
                string value1 = dataGridYUWH1.Rows[rowindex].Cells[0].Value.ToString();
                if (this.dataGridYUWH1.CurrentRow.Index == 0 || this.dataGridYUWH1.CurrentRow.Index == 0)
                {

                    textYUWH1.Text = dataGridYUWH1.Rows[0].Cells[0].Value.ToString();
                    textYUWH2.Text = dataGridYUWH1.Rows[0].Cells[11].Value.ToString();
                }
                else
                {

                    textYUWH1.Text = dataGridYUWH1.Rows[rowindex].Cells[0].Value.ToString();
                    textYUWH2.Text = dataGridYUWH1.Rows[rowindex].Cells[11].Value.ToString();

                }
            }
        }

        private void butYIN5_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  原片仓储 库位信息修改  工位：" + textYUWH1.Text + "  原片编号 " + textYIN1.Text + "  数量 " + textYIN8.Text + "  ?   \r\n   请看库位号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                DataSet dataSet = new DataSet(); DataTable tDt = new DataTable(); tDt = null;
                string tRetData = "", tRetStr = ""; int tRows = 0;
                string tDataStr = "";
                tDataStr = textYIN8.Text + "_" + textYUWH1.Text + "_" + textYUWH2.Text;
                dataSet = tSystem.mClsDBUPdate.ExecuteBYuan("BYuan_00AddTask", 2, int.Parse(textYIN1.Text), tDataStr, tDt, comYuan.Text.Trim(), ref tRetData, ref tRetStr, ref tRows);
                tSystem.mClsDBUPdate.DBClose();
                if (dataSet == null) { return; }
                if (dataSet.Tables.Count > 0)
                {
                    ShowMessageList(1, "入口信息创建 结果：" + dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0].ToString() + " 指令" + dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][1]);
                    comboPos1.Text = "";
                    comboPos2.Text = "";
                    butYIN6_Click(sender, e);
                }
            }
        }

        private void butYIN7_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  原片仓储 库位状态修改  工位：" + textYUWH1.Text  +"  禁用?   \r\n   请看库位号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                DataSet dataSet = new DataSet(); DataTable tDt = new DataTable(); tDt = null;
                string tRetData = "", tRetStr = ""; int tRows = 0;
                string tDataStr = "";
                tDataStr = textYIN8.Text + "_" + textYUWH1.Text + "_" + textYUWH2.Text;
                dataSet = tSystem.mClsDBUPdate.ExecuteBYuan("BYuan_00AddTask", 3, int.Parse(textYIN1.Text), tDataStr, tDt, comYuan.Text.Trim(), ref tRetData, ref tRetStr, ref tRows);
                tSystem.mClsDBUPdate.DBClose();
                if (dataSet == null) { return; }
                if (dataSet.Tables.Count > 0)
                {
                    ShowMessageList(1, "库位状态修改 结果：" + dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0].ToString() + " 指令" + dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][1]);
                    comboPos1.Text = "";
                    comboPos2.Text = "";
                    butYIN6_Click(sender, e);
                }
            }
        }

        private void butYIN8_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("  原片仓储 库位状态修改  工位：" + textYUWH1.Text + "正常   ?   \r\n   请看库位号 ", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                DataSet dataSet = new DataSet(); DataTable tDt = new DataTable(); tDt = null;
                string tRetData = "", tRetStr = ""; int tRows = 0;
                string tDataStr = "";
                tDataStr = textYIN8.Text + "_" + textYUWH1.Text + "_" + textYUWH2.Text;
                dataSet = tSystem.mClsDBUPdate.ExecuteBYuan("BYuan_00AddTask", 4, int.Parse(textYIN1.Text), tDataStr, tDt, comYuan.Text.Trim(), ref tRetData, ref tRetStr, ref tRows);
                tSystem.mClsDBUPdate.DBClose();
                if (dataSet == null) { return; }
                if (dataSet.Tables.Count > 0)
                {
                    ShowMessageList(1, "库位状态修改 结果：" + dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0].ToString() + " 指令" + dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][1]);
                    comboPos1.Text = "";
                    comboPos2.Text = "";
                    butYIN6_Click(sender, e);
                }
            }

        }

        private void combMBIN_Status_SelectedIndexChanged(object sender, EventArgs e)
        {
            tClsShowFrm.mShowMBInStatus = combMBIN_Status.Text; 
        }

        private void butMBWHErr_Click(object sender, EventArgs e)
        {
            string tWhereSql = " and ((a.WH_Status >0 and b.Optimize_batch is null) or ( b.Status>40) )";
            GetdataWH(tWhereSql);
        }

        private void butZKWHErr_Click(object sender, EventArgs e)
        {
            string tWhereSql = " and ((a.WH_Status >0 and b.Optimize_batch is null) or ( b.Status>40) )";
            GetdataZKWH(tWhereSql);
        }

        private void butMBCannel_Click(object sender, EventArgs e)
        {
            

            DialogResult dr = MessageBox.Show("    您确定对磨边仓储 优化单号： " + texOUT1.Text + "  流程卡号 " + texOUT2.Text + "按流程卡号取消出库?   \r\n   请看清流程卡号 ", "警告", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
               
                DataTable tDt = new DataTable();
                Common.ClsDbAcc tDbAccSet = new Common.ClsDbAcc(tSystem);
          
                string tData = ""; string RetData = "", RetStr = "";
                tData = texOUT1.Text + "+" + texOUT2.Text.Trim() + "+" + texOUT3.Text.Trim() + "+" + texOUT6.Text.Trim();
                DataSet dataSet = new DataSet();
                dataSet = tDbAccSet.ExecuteDisposeStatus("Pro_OUTSET", "9001", tData, ref RetData, ref RetStr);
                if (dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[dataSet.Tables.Count - 1].Rows.Count > 0 && dataSet.Tables[dataSet.Tables.Count - 1].Columns.Count > 4)
                    {

                    }
                }
                tDbAccSet.DBClose();
            }

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
                            tSqlstr = "update tab_SYS set Addtime =getdate(),Count=0 ";
                        }
                        else
                        {
                            tSqlstr = "update tab_SYS set Addtime =getdate(),Count='" + tUserData + "' ";
                        }
                        Common.ClsDbAcc clsDbAcc = new Common.ClsDbAcc(tSystem);
                        clsDbAcc.Execute_Command(tSqlstr);
                    }
                }
            }
            groupPassGet1.Visible = false;
        }

       

       

       

       

      

       

        


       

       

        

        

        

        

        //
    }
}
