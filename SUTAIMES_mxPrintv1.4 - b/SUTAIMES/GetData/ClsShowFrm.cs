using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;

using System.Windows.Forms;
using System.Drawing;

using Newtonsoft.Json;

using System.IO;

namespace SUTAIMES.GetData
{
    class ClsShowFrm
    {
        Program tSystem;
        Thread mThreadShowFrm;
        public bool mPlcStartGet = false;
        public int mShowIndex = 3;
        public int mShowMBNO = 1;
        public int mShowMBNum1 = 1;
        public int mShowMBNum2 = 30;

        public int mShowMBline = 0;

        public int mShowZKExitTo = 1;
        public int mShowZKEnport = 1;

        public string mShowMBInStatus = "";
        public ClsShowFrm(Program tSys)
        {
            tSystem = tSys;

            mThreadShowFrm = new Thread(ShowFrmData);
            mThreadShowFrm.Start();
            //mPlcStartGet = true;
        }
        public void SetBackColor(DataGridView tDataView, int tIndex,int _IndexA)
        {
            if (tDataView.Rows.Count > 0)
            {
                for (int i = 0; i < tDataView.Rows.Count - 1; i++)
                {
                    string tStatus = tDataView.Rows[i].Cells[tIndex].Value.ToString().Trim();
                    string tStatusA = tDataView.Rows[i].Cells[_IndexA].Value.ToString().Trim();
                    if (tStatusA == "禁用")
                    {
                        tDataView.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                    }
                    switch (tStatus)
                    {
                        case "禁用":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.Red ;
                            break;
                        case "入库完成":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                            break;
                        case "初始":
                            break;
                        case "进磨边":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.Chocolate;
                            break;
                        case "数据写入":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.Aqua;
                            break;
                        case "进库":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                            //tDataView.Rows[i].DefaultCellStyle.Font = new Font("宋体", tDataView.Font.Size, FontStyle.Bold);
                            break;
                        case "库内":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                            break;
                        case "已经出库":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.GreenYellow;
                            break;
                        case "出库":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.GreenYellow;
                            break;
                        case "离库":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.Gray;
                            break;
                        case "入库未完成":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.CadetBlue;
                            break;
                        case "出库未完成":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.BlueViolet;
                            break;
                        case "异常":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                            break;
                        case "破损":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.DarkRed;
                            break;
                        case "不符合尺寸":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.PowderBlue;
                            break;
                        case "空取":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.BurlyWood;
                            break;
                        case "强制完成":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.Bisque;
                            break;
                        case "强制出库":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                            break;
                        case "准备进库":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                            break;
                        case "准备进中空":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.Bisque;
                            break;
                        case "入中空未完成":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.CadetBlue;
                            break;
                        case "中空库":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.Aqua;
                            break;
                        case "准备出中空":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.Beige;
                            break;
                        case "离开中空":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.BlanchedAlmond;
                            break;
                        case "出中空未完成":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.BlanchedAlmond;
                            break;
                        case "出库完成":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.Aqua;
                            break;
                        case "准备出库":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.Aquamarine;
                            break;
                        case "未入库完成":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.AntiqueWhite;
                            break;
                        case "已进磨边":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.RosyBrown;
                            break;
                        case "设备异常":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.Purple;;
                            break;
                        case "空载架":
                            tDataView.Rows[i].DefaultCellStyle.BackColor = Color.Bisque;
                            break;

                        default:
                            break;
                    }
                }
            }
        }
        private void ShowFrmData()
        {
            do
            {
                if (mPlcStartGet == true)
                {
                    switch (mShowIndex)
                    {
                        case 1:
                            
                            break;
                        case 2://磨边上片
                            ShowMB();
                            ShowPo(201, " and status ='0'");
                            break;
                        case 3://入库
                            ShowPo(202, " and status ='0' and Area_id ='4'");
                            ShowMBInWH();
                            break;
                        case 4://磨边出库
                            ShowPo(203, " and status ='0' and Area_id ='6'");
                            ShowMBOutWH();
                            break;
                        case 101://磨边库查询显示
                            ShowMBWHCha();
                            break;
                        case 102://中空库查询显示
                            ShowZKWHCha();
                            break;
                        case 30://中空库入库
                            ShowPo(204, " and status ='0' and Area_id ='8'");
                            ShowZKInWH();
                            break;
                        case 40://中空库出库库
                            ShowPo(205, " and status ='0' and Area_id ='10'");
                            ShowZKOutWH();
                            break;
                        
                     
                    }
                }
                Thread.Sleep(1777);                                                                                              
                System.Windows.Forms.Application.DoEvents();
            } while (true);
        }

        DataTable mMBdt1 = new DataTable(); DataTable mMBdt2 = new DataTable();
        public int mStart = 0;
        public string mOptimize_batch = "";
        private void ShowMB()
        {
            try
            {
                DataTable tDt;
                string tSqlStr = ""; string tInStatus = "";
                tSqlStr = string.Concat(" SELECT [Process_number]'流程卡号',[single_tag]'标记' ,[single_long]  '长边' ,[single_short]  '短边' "
                                //+",case  when [Status]=0 and [Err_Tag]>0 then '补片初始' when [Status]=0  then '初始' when [Status]=1 then '数据写入' when [Status]=2 then '磨边已核对'"
                                //+ "when [Status]=3 then '进磨边'when [Status]=10 then '准备进库' when [Status]=14 then '库内'when [Status]=20 then '准备出库' when [Status]=24 then '离库'  when [Status]=25 then '已进钢化炉' when [Status]=199 then '入库未完成'  when [Status]=299 then '出库未完成' when [Status]=99 then '不符合尺寸'when [Status]=96 then '破损'when [Status]=97 then '设备异常' when [Status]=120 then '空取'  else '异常' end '状态'"
                                + ",(select top 1 isnull(c.statusName,a.Status ) from StatusNameT c where a.Status =c.Status order by case when a.Err_Tag>0 and c.AssistSatusS =1 then 0 else 1 end ,c.AssistSatusS  asc) '状态'"
                                + ",[Edge_long] '长磨边量' ,[Edge_short] '短磨边量',a.optimize_batch '优化单号',a.WcsId '序号',[Lserial_number] '上片序号', a.Layout_number '切割版图编号',a.Order_width'成品宽度', a.Order_length '成品长度',a.single_name '单片玻璃名称',a.[MBPlcFlow]  PLC流水号,b.QGstatus ,b.InStatus,b.lastLayout_number"
                              + " FROM [tabmb_data] a  join TabOp b on  b.NO='", mShowMBline, "' and "
                             + " a.optimize_batch =b.optimize_batch   and (( b.InStatus =2) or ( a.Layout_number =b.Layout_number or a.Layout_number =b.lastLayout_number )) where a.WcsId>0 order by case when b.InStatus =2 then a.Status else 0 end ,    convert(float,isnull(a.Layout_number,0)),convert(float,isnull(LSerial_number,0))");
                tDt = new DataTable();
                if (tSystem.mClsDBac.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
                {
                    int count = tDt.Rows.Count;
                    DataTable tDt2;
                    string tShowStr = ""; string tShowTex = "";
                    if (tDt.Rows.Count > 0)
                    {
                        tShowTex = tDt.Rows[0]["lastLayout_number"].ToString().Trim();//上次版图
                        tInStatus = tDt.Rows[0]["InStatus"].ToString().Trim();//入库方式  2模糊查询
                        if (mOptimize_batch != tDt.Rows[0]["优化单号"].ToString())
                        {
                            mOptimize_batch = tDt.Rows[0]["优化单号"].ToString();
                            tSqlStr = "SELECT distinct [Optimize_batch] ,convert(float,Layout_number) 'Layout_number' ,[Addtime]  FROM [dbo].[TabMB_Lay] where Optimize_batch='" + mOptimize_batch + "' order by  convert(float,Layout_number) ";
                            DataTable tDtLay = new DataTable();
                            if (tSystem.mClsDBac.RetrieveDataTable_from_DB(tSqlStr, ref tDtLay) == "")
                            {
                                tSystem.mFrmMain.ShowdataTable(1, tDtLay, "", "");
                            }

                        }

                        if (tDt.Rows[0]["QGstatus"].ToString() == "0"  & tDt.Select("状态='数据写入' or 状态='磨边已核对' or 状态='进磨边'").Length >0)
                        {
                            SendErpStatus(tDt.Rows[0]["优化单号"].ToString());
                        }

                    }
                    else { tShowTex = ""; }

                    
                    int tB = 1;
                    if (mMBdt1.Rows.Count == tDt.Rows.Count)
                    {
                        for (int i = 0; i < mMBdt1.Rows.Count; i++)
                        {
                            if (tDt.Rows[i]["状态"].ToString() != mMBdt1.Rows[i]["状态"].ToString() | tDt.Rows[i]["序号"].ToString() != mMBdt1.Rows[i]["序号"].ToString()
                                | tDt.Rows[i]["lastLayout_number"].ToString() != mMBdt1.Rows[i]["lastLayout_number"].ToString())
                            {
                                tB = 1;
                                break;
                            }
                            tB = 0;
                        }
                    }
                    else
                    {
                        tB = 1;
                    }



                    tDt2 = tDt.Copy();//全部数据

                    
                    DataTable tDt3 = new DataTable(); tDt3 = tDt.Copy();
                    int tRowCount = tDt.Rows.Count;
                    if (tDt.Rows.Count > 0)
                    {
                        for (int i = tDt.Rows.Count - 1; i >= 0; i--)//当前
                        {
                            if (tDt.Rows[i]["切割版图编号"].ToString() == tShowTex && tShowTex != "0" & tInStatus !="2")
                            {
                                tDt.Rows.RemoveAt(i);///删除上一个版图
                            }
                        }

                        for (int i = tDt2.Rows.Count - 1; i >= 0; i--)//上次
                        {
                            if (tDt2.Rows[i]["切割版图编号"].ToString() == tShowTex)
                            {

                            }
                            else { tDt2.Rows.RemoveAt(i); }
                        }
                    }
                    if (tDt.Rows.Count > 0) { tShowTex = tDt.Rows[0]["切割版图编号"].ToString(); } else { tShowTex = ""; }
                    tShowStr = string.Format("{0}/{1}", tRowCount, tDt.Select("状态='初始' or 状态= '补片初始'").Length);
                    if (tB == 1 | mStart == 0)
                    {
                        mMBdt1 = new DataTable();
                        mMBdt1 = tDt3;
                        tDt.Columns.RemoveAt(tDt.Columns.Count - 1);
                        tDt.Columns.RemoveAt(tDt.Columns.Count - 1);

                        tSystem.mFrmMain.ShowdataTable(2, tDt, tShowStr, tShowTex);
                        tSystem.mFrmMain.ShowdataTable(3, tDt2, "", "");
                        switch (tInStatus)
                        {
                            case "3":
                                tInStatus = "版面模糊查找模式";
                                break;
                            case "2":
                                tInStatus = "优化单模糊查找模式";
                                break;
                            case "1":
                                tInStatus = "顺序上片模式";
                                break;
                        }
                        tSystem.mFrmMain.ShowTexMes(1, tInStatus);
                    }
                    mStart = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SendErpStatus(string _Optimize_batch)
        {
            string tUrl = "";
            tUrl = "https://www.qdlkd.net/ypstock/lkd/lxauto/lxedstatus";

            string Url = "";
            string result2 = "";
            ///使用JsonWriter写字符串：
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("ucode");
            writer.WriteValue("auto");
            writer.WritePropertyName("upwd");
            writer.WriteValue("auto139");
            writer.WritePropertyName("passkey");
            writer.WriteValue("8BD2AD871782BD660A75C5A6D2902851DF1D64AF829D65BB");
            writer.WritePropertyName("djno");
            writer.WriteValue(_Optimize_batch );
            writer.WritePropertyName("lxstatus");
            writer.WriteValue("2");
            writer.WriteEndObject();
            writer.Flush();
            string jsonText = "";
            jsonText = sw.GetStringBuilder().ToString();

           
            DateTime tNow = DateTime.Now;
            result2 = clsMyPublic.PostUrl(tUrl, jsonText);


            tSystem.mClsDBac.Execute_Command("update TabOp set QGstatus='1',Updatetime=getdate() where NO='1'");
        }

        public void ShowPo(int _Index,string _WhereStr)
        {
            string tShowStr = "";
            DataTable tDt;
            string tSqlStr = "";
            tSqlStr = string.Concat(" SELECT top 1000 [WcsID] ,[Optimize_batch] ,[Process_number] ,[Single_name] ,[Single_long] ,[Single_short] ,[Single_tag]  ,[Status] ,[RunStatus] ,[Area_id]  ,[WH_ID] ,[ZHWH_ID]  ,[Addtime]  ,[Updatetime] ,[Lot_NO] FROM [dbo].[TabPoData] a"
                  + "     where wcsid>0  ", _WhereStr, " order by Addtime desc ");
            tDt = new DataTable();
            if (tSystem.mClsDBac.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
            {
                if (tDt.Rows.Count > 0)
                {
                    for (int i = 0; i < tDt.Rows.Count; i++)
                    {
                        tShowStr = tShowStr + "  流程卡号：" + tDt.Rows[i]["Process_number"].ToString() + " " + tDt.Rows[i]["Single_tag"].ToString() + " " + tDt.Rows[i]["Single_name"].ToString() + " " + tDt.Rows[i]["Single_long"].ToString() + "*" + tDt.Rows[i]["Single_short"].ToString() +"\r\n";
                    }
                }
                else
                {
                    tShowStr = "";
                }
                tSystem.mFrmMain.ShowTexMes(_Index, tShowStr );
            }
        }

        DataTable mMBINdt1= new DataTable();
        public void ShowMBInWH()
        {
            try
            {
                DataTable tDt;
                string tSqlStr = "";
                //tSqlStr = string.Concat("  SELECT [Process_number]流程卡号,[single_tag]标记 ,[single_long] 长边 ,[single_short]  短边  "
                //    //+",case a.[Status] when 0 then '初始' when 1 then '数据写入' when 2 then '磨边已核对' when 3 then '进磨边'when 10 then '准备进库' when 14 then '库内'when 3 then '出库' when 20 then '准备出库' when 24 then '离库'  when 25 then '已进钢化炉'  when 199 then '入库未完成'  when 299 then '出库未完成' when 99 then '不符合尺寸'when 96 then '破损'when 97 then '设备异常' when 120 then '空取' "
                //    //+ " when 26 then '钢化后复核成功' when 27 then '钢化下片完成'  when 28 then '中空上片' "
                //    //+ "when 30 then '准备进中空' when 34 then '中空库' when 40 then '准备出中空' when 44 then '离开中空' when 140 then '中空空取' when 399 then '入中空未完成' when 499 then '出中空未完成' "
                //    //+ "else '异常' end 状态"
                //    + ",(select top 1 isnull(c.statusName,a.Status ) from StatusNameT c where a.Status =c.Status order by case when a.Err_Tag>0 and c.AssistSatusS =1 then 0 else 1 end ,c.AssistSatusS  asc) '状态'"
                //    +",[MBPlcFlow]  PLC流水号,[WH_NO]库号 ,[WH_Num] 格号,WcsID 序号,a.Optimize_batch 优化单号,a.single_name '单片名称',convert(varchar(20),a.InAtime,120) '入库时间', a.Layout_number 切割版图编号 "
                //    + ",MBEnport '磨边线' "
                //    +" FROM [tabmb_data] a join [TabMB_Optimize] b on a.Optimize_batch =b.Optimize_batch and b.MBRunStatus >0 and  b.MBRunStatus <24 "
                //    + "  join TabOp c on  c.NO='", mShowMBline, "' and "
                //    + " (a.optimize_batch =c.optimize_batch    or a.optimize_batch =c.lastOptimize_batch)"
                //     + "   order by a.Addtime,case  when   a.Status =24 then 0 when a.Status in (30) then 1 when a.Status =34 then 2 else 3 end ,convert(float,a.Single_long)*convert(float,a.Single_short), a.Order_singleid, a.Single_tag ");

                tSqlStr = string.Concat("  SELECT distinct [Process_number]流程卡号,[single_tag]标记 ,[single_long] 长边 ,[single_short]  短边  "
                    //+",case a.[Status] when 0 then '初始' when 1 then '数据写入' when 2 then '磨边已核对' when 3 then '进磨边'when 10 then '准备进库' when 14 then '库内'when 3 then '出库' when 20 then '准备出库' when 24 then '离库'  when 25 then '已进钢化炉'  when 199 then '入库未完成'  when 299 then '出库未完成' when 99 then '不符合尺寸'when 96 then '破损'when 97 then '设备异常' when 120 then '空取' "
                    //+ " when 26 then '钢化后复核成功' when 27 then '钢化下片完成'  when 28 then '中空上片' "
                    //+ "when 30 then '准备进中空' when 34 then '中空库' when 40 then '准备出中空' when 44 then '离开中空' when 140 then '中空空取' when 399 then '入中空未完成' when 499 then '出中空未完成' "
                    //+ "else '异常' end 状态"
                    + ",(select top 1 isnull(c.statusName,a.Status ) from StatusNameT c where a.Status =c.Status order by case when a.Err_Tag>0 and c.AssistSatusS =1 then 0 else 1 end ,c.AssistSatusS  asc) '状态'"
                    + ",[MBPlcFlow]  PLC流水号,[WH_NO]库号 ,[WH_Num] 格号,WcsID 序号,a.Optimize_batch 优化单号,a.single_name '单片名称',convert(varchar(20),a.InAtime,120) '入库时间', a.Layout_number 切割版图编号 "
                    + ",MBEnport '磨边线' "
                    + ",a.Addtime,case  when   a.Status =24 then 0 when a.Status in (30) then 1 when a.Status =34 then 2 else 3 end ,convert(float,a.Single_long)*convert(float,a.Single_short), a.Order_singleid"
                    + " FROM [tabmb_data] a join [TabMB_Optimize] b on a.Optimize_batch =b.Optimize_batch and b.MBRunStatus >0 and  b.MBRunStatus <24   "
                    + "  join TabOp c on  (c.NO='", mShowMBline, "'   and "
                    + " (a.optimize_batch =c.optimize_batch    or a.optimize_batch =c.lastOptimize_batch)) or a.MBEnport='", mShowMBline, "'"
                    + " where  exists (select 1 from StatusNameT e where a.Status =e.Status and (e.StatusName ='", mShowMBInStatus, "' or '", mShowMBInStatus, "'='') )"
                     + "   order by a.Addtime,case  when   a.Status =24 then 0 when a.Status in (30) then 1 when a.Status =34 then 2 else 3 end ,convert(float,a.Single_long)*convert(float,a.Single_short), a.Order_singleid, a.Single_tag ");
                tDt = new DataTable();

                if (tSystem.mClsDBac.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
                {
                    int count = tDt.Rows.Count;
                    int tB = 1; string tShowStr = ""; string tShowTex = "";
                    if (mMBINdt1.Rows.Count == tDt.Rows.Count)
                    {
                        for (int i = 0; i < mMBINdt1.Rows.Count; i++)
                        {
                            if (tDt.Rows.Count > 0 & tDt.Rows[i]["状态"].ToString() != mMBINdt1.Rows[i]["状态"].ToString() | tDt.Rows[i]["序号"].ToString() != mMBINdt1.Rows[i]["序号"].ToString())
                            {
                                tB = 1;
                                break;
                            }
                            tB = 0;
                        }
                    }
                    else { tB = 1; }


                    if (tB == 1 | mStart == 0)
                    {
                        mMBINdt1 = new DataTable();
                        mMBINdt1 = tDt; if (tDt.Rows.Count > 0) { tShowStr = tDt.Rows[0]["优化单号"].ToString(); }
                        tShowTex = string.Format("{0}:{1}/{2}/{3}", tDt.Rows.Count, tDt.Select("状态='已进磨边'").Length, tDt.Select("状态='初始' or 状态= '补片初始'").Length, tDt.Select("状态='库内'").Length);
                        tSystem.mFrmMain.ShowdataTable(4, tDt, tShowStr, tShowTex);
                    }


                    tDt = new DataTable();
                    tSqlStr = "SELECT TOP 20 a.ID,a.Mac_NO,a.Mac_id ,a.Plc_Flow ,b.Single_long ,b.Singel_Short,a.log_Flow,a.log_Str,a.Mac_Num  FROM TabMBMachine a left join TabMBPlcflow b on a.Plc_Flow =b.PlcFlow   where Mac_Are ='2' order by ID ,a.Updatetime   ";
                    if (tSystem.mClsDBac.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
                    {
                        tSystem.mFrmMain.ShowdataTable(11, tDt, "", "");
                    }
                    mStart = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void ShowMBOutWH()
        {
            try
            {
                DataTable tDt;
                string tSqlStr = "";
                tSqlStr = string.Concat("  SELECT a.[Process_number]流程卡号,[single_tag]标记 ,[single_long] 长边 ,[single_short]  短边 "
                               //+",case a.[Status] when 0 then '初始' when 1 then '数据写入' when 2 then '磨边已核对' when 3 then '进磨边'when 10 then '准备进库' when 14 then '库内'when 3 then '出库' when 20 then '准备出库' when 24 then '离库'  when 25 then '已进钢化炉'  when 188 then '入库未完成'  when 299 then '出库未完成' when 99 then '不符合尺寸'when 96 then '破损'when 97 then '设备异常' when 120 then '空取'"
                               //+ " when 26 then '钢化后复核成功' when 27 then '钢化下片完成'  when 28 then '中空上片' "
                               //+ " when 30 then '准备进中空' when 34 then '中空库' when 40 then '准备出中空' when 44 then '离开中空' when 140 then '中空空取' when 399 then '入中空未完成' when 499 then '出中空未完成'  else '异常' end 状态"
                               + ",(select top 1 isnull(c.statusName,a.Status ) from StatusNameT c where a.Status =c.Status order by case when a.Err_Tag>0 and c.AssistSatusS =1 then 0 else 1 end ,c.AssistSatusS  asc) '状态'"
                               +",[MBPlcFlow]  PLC流水号,[WH_NO]库号 ,[WH_Num] 格号,WcsID 序号,a.Optimize_batch 优化单号,a.single_name '玻璃名称',a.order_id '订单号',  convert(varchar(20),a.outAtime,120) '出库时间' "
                               + ",a.Order_number'订序', a.MBExit '磨边出口',a.MBCar  '出库车' "
                               + " ,a.[Label_tag]'钢标字符' ,a.[Label_pos]'钢标位置' ,a.[Label_type]'钢标边距' ,a.[Label_name]'钢标图号' ,a.[Label_size]'钢标尺寸' ,a.[Label_color]'钢标内容' "
                                + " FROM [tabmb_data] a where exists(select 1 from tabMBWHOut b where a.Optimize_batch =b.Optimize_batch and b.Status=0  and b.Exit_To ="+clsMyPublic.mMBExit  + " and"
                                +"((a.Process_number =b.Process_number and b.Out_Type =0) or(a.Order_id =b.Order_id and b.Out_Type=10) ))"
                                + "order by case when a.Status=20 then 0 when a.outAtime is not null then 2 else 1 end asc"
                                + ",convert(int,a.Order_number)"
                                //+",convert(float,a.single_long)*convert(float,a.single_short) desc"
                                +",a.outAtime  ");
                tDt = new DataTable();

                if (tSystem.mClsDBac.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
                {
                    int count = tDt.Rows.Count;
                    int tB = 1; string tShowStr = ""; string tShowTex = "";
                    if (mMBINdt1.Rows.Count == tDt.Rows.Count)
                    {
                        for (int i = 0; i < mMBINdt1.Rows.Count; i++)
                        {
                            if (tDt.Rows.Count > 0)
                            {
                                if ((tDt.Rows[i]["状态"].ToString() != mMBINdt1.Rows[i]["状态"].ToString() | tDt.Rows[i]["序号"].ToString() != mMBINdt1.Rows[i]["序号"].ToString()))
                                {
                                    tB = 1;
                                    break;
                                }
                            }
                            tB = 0;
                        }
                    }
                    else { tB = 1; }

                    if (tB == 1 | mStart == 0)
                    {
                        mMBINdt1 = new DataTable();
                        mMBINdt1 = tDt; if (tDt.Rows.Count > 0) { tShowStr = tDt.Rows[0]["优化单号"].ToString(); }
                        tShowTex = string.Format("{0}:{1}/{2}", tDt.Rows.Count, tDt.Select("状态='离库'").Length, tDt.Select("状态='库内'").Length);
                        tSystem.mFrmMain.ShowdataTable(5, tDt, tShowStr, tShowTex);
                    }

                    mStart = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        DataTable mChaDt = new DataTable();
        public void ShowMBWHCha()
        {
            try
            {
                DataTable tDt;
                string tSqlStr = "";
                tSqlStr = string.Concat(" SELECT WH_NO ,WH_Num ,WH_Status, InputFlow  FROM [tabMB_WH] where WH_NO ='",mShowMBNO ,"' and WH_Num >='",mShowMBNum1 ,"' and WH_Num <'",mShowMBNum2 ,"' order by WH_NO,WH_Num  ");
                tDt = new DataTable();

                if (tSystem.mClsDBac.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
                {

                    tSystem.mFrmMain.ShowdataTable(101, tDt, "", "");

                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ShowZKWHCha()
        {
            try
            {
                DataTable tDt;
                string tSqlStr = "";
                tSqlStr = string.Concat(" SELECT WH_NO ,WH_Num ,WH_Status, InputFlow  FROM [tabZK_WH] where WH_NO ='", mShowMBNO, "' and WH_Num >='", mShowMBNum1, "' and WH_Num <'", mShowMBNum2, "' order by WH_NO,WH_Num  ");
                tDt = new DataTable();

                if (tSystem.mClsDBac.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
                {

                    tSystem.mFrmMain.ShowdataTable(102, tDt, "", "");
                    mStart = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        DataTable mZKINdt1 = new DataTable();
        public void ShowZKInWH()
        {
            try
            {
                DataTable tDt;
                string tSqlStr = "";
                tSqlStr = string.Concat("  SELECT [Process_number]流程卡号,[single_tag]标记 ,[single_long] 长边 ,[single_short]  短边  "
                    //+",case a.[Status] when 0 then '初始' when 1 then '数据写入' when 2 then '磨边已核对' when 3 then '进磨边'when 10 then '准备进库' when 14 then '库内'when 3 then '出库' when 20 then '准备出库' when 24 then '离库'  when 25 then '已进钢化炉'  when 199 then '入库未完成'  when 299 then '出库未完成' when 99 then '不符合尺寸'when 96 then '破损'when 97 then '设备异常' when 120 then '空取' "
                    //+ " when 26 then '钢化后复核成功' when 27 then '钢化下片完成'  when 28 then '中空上片' "
                    //+ "when 30 then '准备进中空' when 34 then '中空库' when 40 then '准备出中空' when 44 then '离开中空' when 140 then '中空空取' when 399 then '入中空未完成' when 499 then '出中空未完成'"
                    //+ "else '异常' end 状态"
                    + ",(select top 1 isnull(c.statusName,a.Status ) from StatusNameT c where a.Status =c.Status order by case when a.Err_Tag>0 and c.AssistSatusS =1 then 0 else 1 end,c.AssistSatusS  asc ) '状态'"
                    +",a.ZKPlcFlow  PLC流水号,a.ZKWH_NO   库号 ,a.ZKWH_Num  格号,WcsID 序号,a.Optimize_batch 优化单号,a.single_name '单片名称' , convert(varchar(20),a.ZKInAtime,120) '中空入库时间' ,a.Order_id '订单号',a.Order_singleid '成品单片编号' FROM [tabmb_data] a join [TabMB_Optimize] b on a.Optimize_batch =b.Optimize_batch and   b.MBWHOut=2 "
                    //+ " and ( b.ZKProcess_number is null or b.ZKProcess_number='' or b.ZKProcess_number =a.Process_number  ) "
                    + " and b.ZkEnport='"+mShowZKEnport.ToString()+"'"
                    + "  and ( (b.ZKProcess_number =a.Process_number and b.ZKInType in ('10','11')) or (b.ZKOrder_id =a.Order_id and b.ZKInType in('20','21')) or ( b.ZKInType in ('30','31')))"
                    + "   order by case  when   a.Status =30 then 0 when   a.Status in (24) then 1 when a.Status =34 then 2 else 3 end ,"
                    + " case when   b.ZKInType in('11','21','31') then  '5000002'-convert(float,a.Single_long)*convert(float,a.Single_short) else convert(float,a.Single_long)*convert(float,a.Single_short) end desc  ,"
					+"	 a.Order_singleid,"
					+"	 a.Single_tag desc ");
                tDt = new DataTable();

                if (tSystem.mClsDBac.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
                {
                    int count = tDt.Rows.Count;
                    int tB = 1; string tShowStr = ""; string tShowTex = "";
                    if (mZKINdt1.Rows.Count == tDt.Rows.Count)
                    {
                        for (int i = 0; i < mZKINdt1.Rows.Count ; i++)
                        {
                            if (tDt.Rows.Count > 0 & tDt.Rows[i]["状态"].ToString() != mZKINdt1.Rows[i]["状态"].ToString() | tDt.Rows[i]["序号"].ToString() != mZKINdt1.Rows[i]["序号"].ToString())
                            {
                                tB = 1;
                                break;
                            }
                            tB = 0;
                        }
                    }
                    else { tB = 1; }
                    if (tB == 1 | mStart == 0)
                    {
                        mZKINdt1 = new DataTable();
                        mZKINdt1 = tDt; if (tDt.Rows.Count > 0) { tShowStr = tDt.Rows[0]["优化单号"].ToString(); }
                        tShowTex = string.Format("{0}:{1}/{2}/{3}", tDt.Rows.Count, tDt.Select("状态='离库'").Length, tDt.Select("状态='库内'").Length, tDt.Select("状态='中空库'").Length);
                        tSystem.mFrmMain.ShowdataTable(30, tDt, tShowStr, tShowTex);
                    }

                    tDt = new DataTable();
                    tSqlStr = "SELECT TOP 30 a.ID,a.Mac_NO,a.Mac_id ,a.Plc_Flow ,b.Single_long ,b.Singel_Short,a.log_Flow,a.log_Str,a.Mac_Num  FROM [TabZKMachine] a left join TabZKPlcflow b on a.Plc_Flow =b.PlcFlow   where Mac_Are =10 order by ID ,a.Updatetime  ";
                    if (tSystem.mClsDBac.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
                    {
                        tSystem.mFrmMain.ShowdataTable(31, tDt, "", "");
                    }
                    mStart = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message +ex.StackTrace );
            }
        }

        DataTable  mZkOutDt = new DataTable();
        public void ShowZKOutWH()
        {
            try
            {
                DataTable tDt;
                string tSqlStr = "";
                tSqlStr = string.Concat("   SELECT a.[Process_number]流程卡号,[single_tag]标记 ,[single_long] 长边 ,[single_short]  短边 "
                                        //+",case a.[Status] when 0 then '初始' when 1 then '数据写入' when 2 then '磨边已核对' when 3 then '进磨边'when 10 then '准备进库' when 14 then '库内'when 3 then '出库' when 20 then '准备出库' when 24 then '离库'  when 25 then '已进钢化炉'  when 199 then '入库未完成'  when 299 then '出库未完成' when 99 then '不符合尺寸'when 96 then '破损'when 97 then '设备异常' when 120 then '空取' "
                                        // + " when 26 then '钢化后复核成功' when 27 then '钢化下片完成'  when 28 then '中空上片' "
                                        //+ " when 30 then '准备进中空' when 34 then '中空库' when 40 then '准备出中空' when 44 then '离开中空' when 140 then '中空空取'when 399 then '入中空未完成' when 499 then '出中空未完成'   else '异常' end 状态"
                                        + ",(select top 1 isnull(c.statusName,a.Status ) from StatusNameT c where a.Status =c.Status order by case when a.Err_Tag>0 and c.AssistSatusS =1 then 0 else 1 end,c.AssistSatusS  asc ) '状态'"
                                        +",A.ZKPlcFlow 'PLC流水号',a.ZKWH_NO '库号' ,a.ZKWH_Num '格号',WcsID 序号,a.Optimize_batch 优化单号,a.single_name '玻璃名称',  convert(varchar(20),a.ZKoutAtime,120) '出库时间', convert(varchar(20),a.ZKInAtime,120) '中空入库时间' ,a.Order_singleid '成品单片编号',a.single_Str '标记组成'"
                                        + ",a.Customer_name '项目名称',a.Order_id '订单号',a.Order_singlename '产品配置',convert(varchar,a.Order_length)+'*'+convert(varchar,a.Order_width)'规格'"
                                        + " FROM [tabmb_data] a where exists(select 1 from tabZKProcess b where a.Process_number =b.Process_number and b.Status=1 and b.Exit_To ='", mShowZKExitTo.ToString(), "'  )order by case when a.Status=40 then 0 else 1 end asc ,convert(float,a.single_long)*convert(float,a.single_short) desc,a.Order_singleid ,a.single_tag desc ,a.ZKoutAtime desc ");
                tDt = new DataTable();

                if (tSystem.mClsDBac.RetrieveDataTable_from_DB(tSqlStr, ref tDt) == "")
                {
                    int count = tDt.Rows.Count;
                    int tB = 1; string tShowStr = ""; string tShowTex = "";
                    if (mZkOutDt.Rows.Count == tDt.Rows.Count)
                    {
                        for (int i = 0; i < mZkOutDt.Rows.Count - 1; i++)
                        {
                            if (tDt.Rows.Count > 0)
                            {
                                if ((tDt.Rows[i]["状态"].ToString() != mZkOutDt.Rows[i]["状态"].ToString() | tDt.Rows[i]["序号"].ToString() != mZkOutDt.Rows[i]["序号"].ToString()))
                                {
                                    tB = 1;
                                    break;
                                }
                            }
                            tB = 0;
                        }
                    }
                    else { tB = 1; }
                    if (tB == 1 | mStart == 0)
                    {
                        mZkOutDt = new DataTable();
                        mZkOutDt = tDt; if (tDt.Rows.Count > 0) { tShowStr = tDt.Rows[0]["优化单号"].ToString(); }
                        tShowTex = string.Format("{0}:{1}/{2}", tDt.Rows.Count, tDt.Select("状态='中空库'").Length, tDt.Select("状态='离开中空'").Length);
                        tSystem.mFrmMain.ShowdataTable(40, tDt, tShowStr, tShowTex);
                    }

                    mStart = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //

    }
}
