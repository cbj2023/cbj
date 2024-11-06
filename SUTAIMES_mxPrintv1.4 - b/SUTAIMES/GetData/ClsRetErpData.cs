using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using Newtonsoft.Json;
using System.IO;


using System.Xml;

using System.Collections;
using System.Reflection;//PropertyInfo引用

namespace SUTAIMES.GetData
{
    class ClsRetErpData
    {
        Thread mThreadGetPlcData;
        public bool mPlcStartGet = false;


        Thread mThreadGetQiGe;
        public bool mPlcStartGetQiGe = false;

        Program tSystem;
        public ClsRetErpData(Program tSys)
        {
            tSystem = tSys;
        }
        public void OpenThread()
        {
            mThreadGetPlcData = new Thread(GetPlcData);
            mThreadGetPlcData.Start();
            mPlcStartGet = true;

            mThreadGetQiGe = new Thread(GetQiGe);
            mThreadGetQiGe.Start();
            mPlcStartGetQiGe = true;
        }
        private void GetPlcData()//5
        {
            Common.ClsDbAcc tClsDB = new Common.ClsDbAcc(tSystem);
            DataTable tDt; string tWhereStr = "";
            string tSqlStr = "";
            do
            {
                if (mPlcStartGet == true)
                {
                    tDt = new DataTable();
                    tSqlStr = "SELECT TOP 400 [tID],Wcsid ,[Process_number] ,[Order_number] ,[Order_singleid],[Single_tag] ,[Status]  ,[wipname]  ,[devcno] ,[UpStatus] ,convert(varchar, [Addtime],120) 'Addtime'  FROM [dbo].[TabErpRetData] a where UpStatus='0' "
                        //+" and  Process_number='M21060400518.001'"
                                + " order by a.Addtime asc, a.wipname,a.devcno ";
                    tClsDB.RetrieveDataTable_from_DB(tSqlStr, ref tDt);
                    if (tDt.Rows.Count > 0)
                    {


                        for (int i = 0; i < 3; i++)
                        {
                            DataRow[] tDRs1 = tDt.Select("wipname='切割' and devcno='二厂切割自动" + (i + 1).ToString() + "线'", " Addtime asc ");//and devcno='二线（北）自动切'
                            if (tDRs1.Length > 0)
                            {
                                tWhereStr = "";
                                if (DisData(tDRs1[0][7].ToString(), tDRs1[0][8].ToString(), tDRs1, ref tWhereStr) == true)
                                {
                                    tSqlStr = string.Concat("update TabErpRetData set UpStatus='1',Updatetime=getdate() where tID in (", tWhereStr, ")");
                                    tClsDB.Execute_Command(tSqlStr);

                                }
                            }
                        }

                        for (int i = 0; i < 3; i++)
                        {
                            DataRow[] tDRs2 = tDt.Select("wipname='磨边' and devcno='二厂磨边自动" + (i + 1).ToString() + "线' ", " Addtime asc ");//and devcno='二线（北）磨边机'
                            if (tDRs2.Length > 0)
                            {
                                tWhereStr = "";
                                if (DisData(tDRs2[0][7].ToString(), tDRs2[0][8].ToString(), tDRs2, ref tWhereStr) == true)
                                {
                                    tSqlStr = string.Concat("update TabErpRetData set UpStatus='1',Updatetime=getdate() where tID in (", tWhereStr, ")");
                                    tClsDB.Execute_Command(tSqlStr);

                                }

                            }
                        }

                        for (int i = 0; i < 3; i++)
                        {
                            DataRow[] tDRs3 = tDt.Select("wipname='钢化' and devcno='二厂钢化自动" + (i + 1) + "线'", " Addtime asc ");
                            if (tDRs3.Length > 0)
                            {
                                tWhereStr = "";
                                if (DisData(tDRs3[0][7].ToString(), tDRs3[0][8].ToString(), tDRs3, ref tWhereStr) == true)
                                {
                                    tSqlStr = string.Concat("update TabErpRetData set UpStatus='1',Updatetime=getdate() where tID in (", tWhereStr, ")");
                                    tClsDB.Execute_Command(tSqlStr);

                                }

                            }
                        }
                        DataRow[] tDRszk1 = tDt.Select("wipname='中空' and devcno='中空1线'", " Addtime asc ");
                        if (tDRszk1.Length > 0)
                        {
                            tWhereStr = "";
                            if (DisData(tDRszk1[0][7].ToString(), tDRszk1[0][8].ToString(), tDRszk1, ref tWhereStr) == true)
                            {
                                tSqlStr = string.Concat("update TabErpRetData set UpStatus='1',Updatetime=getdate() where tID in (", tWhereStr, ")");
                                tClsDB.Execute_Command(tSqlStr);

                            }

                        }
                        DataRow[] tDRszk2 = tDt.Select("wipname='中空' and devcno='中空2线'", " Addtime asc ");
                        if (tDRszk2.Length > 0)
                        {
                            tWhereStr = "";
                            if (DisData(tDRszk2[0][7].ToString(), tDRszk2[0][8].ToString(), tDRszk2, ref tWhereStr) == true)
                            {
                                tSqlStr = string.Concat("update TabErpRetData set UpStatus='1',Updatetime=getdate() where tID in (", tWhereStr, ")");
                                tClsDB.Execute_Command(tSqlStr);

                            }

                        }

                        DataRow[] tDRszk3 = tDt.Select("wipname='中空' and devcno='中空3线'", " Addtime asc ");
                        if (tDRszk3.Length > 0)
                        {
                            tWhereStr = "";
                            if (DisData(tDRszk3[0][7].ToString(), tDRszk3[0][8].ToString(), tDRszk3, ref tWhereStr) == true)
                            {
                                tSqlStr = string.Concat("update TabErpRetData set UpStatus='1',Updatetime=getdate() where tID in (", tWhereStr, ")");
                                tClsDB.Execute_Command(tSqlStr);

                            }

                        }

                    }


                    //tDt = new DataTable();
                    //tSqlStr = //"SELECT TOP 200 tID,Wcsid ,[Process_number] ,[Order_number] ,[Order_singleid],[Single_tag] , '99' as 'Status'  ,  '破损' as 'wipname'  ,  '异常' as 'devcno' ,[UpStatus] ,convert(varchar, [Addtime],120) 'Addtime'  FROM [dbo].[TabPoData] a where UpStatus='0' order by a.Addtime asc, wipname,devcno";
                    //         "SELECT TOP 200 tID,Wcsid ,[Process_number] ,[Order_number] ,[Order_singleid],[Single_tag] , '50' as 'Status'  ,"
                    //         + "case when RunStatus =0 then '切割破损'  when (RunStatus>0 and RunStatus<24) or RunStatus=120 or RunStatus=199 or RunStatus=299  then '磨边破损' else '钢化破损' end 'wipname',"
                    //         +"'异常' as 'devcno' ,[UpStatus] ,convert(varchar, [Addtime],120) 'Addtime'  FROM [dbo].[TabPoData] a where UpStatus='0' order by a.Addtime asc, wipname,devcno";

                    //    tClsDB.RetrieveDataTable_from_DB(tSqlStr, ref tDt);
                    //if (tDt.Rows.Count > 0)
                    //{
                    //    DataRow[] tDRs6 = tDt.Select("wipname='切割破损' and devcno='异常'", " Addtime asc ");// tDt.Select("wipname='破损' and devcno='异常'", " Addtime asc ");
                    //    if (tDRs6.Length > 0)
                    //    {
                    //        tWhereStr = "";
                    //        if (DisData(tDRs6[0][7].ToString(), tDRs6[0][8].ToString(), tDRs6, ref tWhereStr) == true)
                    //        {
                    //            tSqlStr = string.Concat("update TabPoData set UpStatus='1',Updatetime=getdate() where tID in (", tWhereStr, ")");
                    //            tClsDB.Execute_Command(tSqlStr);

                    //        }
                    //    }

                    //    tDRs6 = tDt.Select("wipname='磨边破损' and devcno='异常'", " Addtime asc ");// tDt.Select("wipname='破损' and devcno='异常'", " Addtime asc ");
                    //    if (tDRs6.Length > 0)
                    //    {
                    //        tWhereStr = "";
                    //        if (DisData(tDRs6[0][7].ToString(), tDRs6[0][8].ToString(), tDRs6, ref tWhereStr) == true)
                    //        {
                    //            tSqlStr = string.Concat("update TabPoData set UpStatus='1',Updatetime=getdate() where tID in (", tWhereStr, ")");
                    //            tClsDB.Execute_Command(tSqlStr);

                    //        }
                    //    }

                    //    tDRs6 = tDt.Select("wipname='钢化破损' and devcno='异常'", " Addtime asc ");// tDt.Select("wipname='破损' and devcno='异常'", " Addtime asc ");
                    //    if (tDRs6.Length > 0)
                    //    {
                    //        tWhereStr = "";
                    //        if (DisData(tDRs6[0][7].ToString(), tDRs6[0][8].ToString(), tDRs6, ref tWhereStr) == true)
                    //        {
                    //            tSqlStr = string.Concat("update TabPoData set UpStatus='1',Updatetime=getdate() where tID in (", tWhereStr, ")");
                    //            tClsDB.Execute_Command(tSqlStr);

                    //        }
                    //    }

                    //}
                }


                //GetQieGe();///获取切割数据

                Thread.Sleep(300000);
                System.Windows.Forms.Application.DoEvents();
            } while (true);
        }

        private bool DisData(string _wipname, string _devcno, DataRow[] _DtRow, ref string _WhereStr)
        {
            try
            {
                bool tRetBool = false;
                string tSdata = "", tWhereStr = "";

                for (int i = 0; i < _DtRow.Length; i++)
                {
                    if (i == 0)
                    {
                        tWhereStr = "'" + _DtRow[i]["tID"].ToString() + "'";
                        tSdata = _DtRow[i]["Process_number"].ToString() + "+" + _DtRow[i]["Order_number"].ToString() + "+" + _DtRow[i]["Single_tag"].ToString() + "+" + _DtRow[i]["Wcsid"].ToString() + "+" + _DtRow[i]["Status"].ToString() + "+" + _DtRow[i]["Addtime"].ToString();
                    }
                    else
                    {
                        tWhereStr = tWhereStr + ",'" + _DtRow[i]["tID"].ToString() + "'";
                        tSdata = tSdata + "," + _DtRow[i]["Process_number"].ToString() + "+" + _DtRow[i]["Order_number"].ToString() + "+" + _DtRow[i]["Single_tag"].ToString() + "+" + _DtRow[i]["Wcsid"].ToString() + "+" + _DtRow[i]["Status"].ToString() + "+" + _DtRow[i]["Addtime"].ToString();
                    }
                }
                _WhereStr = tWhereStr;
                if (tSdata.Length > 0)
                {
                    tRetBool = SendUrl(_wipname, _devcno, tSdata);

                }
                return tRetBool;
            }
            catch
            {
                return false;
            }
        }
        private bool SendUrl(string _wipname, string _devcno, string _wgrecord)
        {
            try
            {
                string Url = "";
                string result2 = "";
                ///使用JsonWriter写字符串：
                StringWriter sw = new StringWriter();
                JsonWriter writer = new JsonTextWriter(sw);
                writer.WriteStartObject();
                writer.WritePropertyName("ucode");
                writer.WriteValue(clsMyPublic.mErp_ucode);//("auto");
                writer.WritePropertyName("upwd");
                writer.WriteValue(clsMyPublic.mErp_upwd);// ("9A395478FE3EFEAE3323722C56B311F4");
                writer.WritePropertyName("passkey");
                writer.WriteValue(clsMyPublic.mErp_passkey);// ("5F229626553F69F01DAB380655702738918A2D2150AD44CF59910E0ADE552FBDF7A548489F8FC253");
                writer.WritePropertyName("wipname");
                writer.WriteValue(_wipname);
                writer.WritePropertyName("devcno");
                writer.WriteValue(_devcno);
                writer.WritePropertyName("finstime");
                writer.WriteValue(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                writer.WritePropertyName("wgrecord");
                writer.WriteValue(_wgrecord);
                writer.WriteEndObject();
                writer.Flush();
                string jsonText = "";
                jsonText = sw.GetStringBuilder().ToString();

                Url = " https://www.qdlkd.net/ypstock/lkd/lxauto/lxjhautofinish";
                result2 = clsMyPublic.PostUrl(Url, jsonText);
                if (result2.Length < 250 && result2.ToUpper().Contains("OK"))
                {
                    tSystem.mFile.WriteLog("", "ERP数据" + jsonText + " " + result2 + "N");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }

        private void GetQiGe()
        {
            Common.ClsDbAcc tClsDB = new Common.ClsDbAcc(tSystem);
            DataTable tDt; string tWhereStr = "";
            string tSqlStr = "";
            do
            {
                if (mPlcStartGetQiGe == true)
                {
                    tDt = new DataTable();
                    tSqlStr = //"SELECT TOP 200 tID,Wcsid ,[Process_number] ,[Order_number] ,[Order_singleid],[Single_tag] , '99' as 'Status'  ,  '破损' as 'wipname'  ,  '异常' as 'devcno' ,[UpStatus] ,convert(varchar, [Addtime],120) 'Addtime'  FROM [dbo].[TabPoData] a where UpStatus='0' order by a.Addtime asc, wipname,devcno";
                             "SELECT TOP 200 tID,Wcsid ,[Process_number] ,[Order_number] ,[Order_singleid],[Single_tag] , '50' as 'Status'  ,"
                             + "case when RunStatus =0 then '切割'  when (RunStatus>0 and RunStatus<24) or RunStatus=120 or RunStatus=199 or RunStatus=299  then '磨边' else '钢化' end 'wipname',"
                             + "'异常' as 'devcno' ,[UpStatus] ,convert(varchar, [Addtime],120) 'Addtime'  FROM [dbo].[TabPoData] a where UpStatus='0' order by a.Addtime asc, wipname,devcno";

                    tClsDB.RetrieveDataTable_from_DB(tSqlStr, ref tDt);
                    if (tDt.Rows.Count > 0)
                    {
                        DataRow[] tDRs6 = tDt.Select("wipname='切割' and devcno='异常'", " Addtime asc ");// tDt.Select("wipname='破损' and devcno='异常'", " Addtime asc ");
                        if (tDRs6.Length > 0)
                        {
                            tWhereStr = "";
                            if (DisData(tDRs6[0][7].ToString(), tDRs6[0][8].ToString(), tDRs6, ref tWhereStr) == true)
                            {
                                tSqlStr = string.Concat("update TabPoData set UpStatus='1',Updatetime=getdate() where tID in (", tWhereStr, ")");
                                tClsDB.Execute_Command(tSqlStr);

                            }
                        }

                        tDRs6 = tDt.Select("wipname='磨边' and devcno='异常'", " Addtime asc ");// tDt.Select("wipname='破损' and devcno='异常'", " Addtime asc ");
                        if (tDRs6.Length > 0)
                        {
                            tWhereStr = "";
                            if (DisData(tDRs6[0][7].ToString(), tDRs6[0][8].ToString(), tDRs6, ref tWhereStr) == true)
                            {
                                tSqlStr = string.Concat("update TabPoData set UpStatus='1',Updatetime=getdate() where tID in (", tWhereStr, ")");
                                tClsDB.Execute_Command(tSqlStr);

                            }
                        }

                        tDRs6 = tDt.Select("wipname='钢化' and devcno='异常'", " Addtime asc ");// tDt.Select("wipname='破损' and devcno='异常'", " Addtime asc ");
                        if (tDRs6.Length > 0)
                        {
                            tWhereStr = "";
                            if (DisData(tDRs6[0][7].ToString(), tDRs6[0][8].ToString(), tDRs6, ref tWhereStr) == true)
                            {
                                tSqlStr = string.Concat("update TabPoData set UpStatus='1',Updatetime=getdate() where tID in (", tWhereStr, ")");
                                tClsDB.Execute_Command(tSqlStr);

                            }
                        }

                    }
                }
                GetQieGe();

                Thread.Sleep(10000);
                System.Windows.Forms.Application.DoEvents();
            } while (true);
        }

        public void GetQieGe()
        {
            try
            {
                XmlDocument doc1 = new XmlDocument(); string tRetStr = ""; string tFile = "";
                tFile = @"\\10.10.21.48\Scambio\MonolithicProgress.xml";
                doc1.Load(tFile);
                DisposXmlBo(doc1.InnerXml, tFile, 1, ref tRetStr);

                doc1 = new XmlDocument();
                tFile = @"\\10.10.21.49\Scambio\MonolithicProgress.xml";
                doc1.Load(tFile);
                DisposXmlBo(doc1.InnerXml, tFile, 2, ref tRetStr);

                doc1 = new XmlDocument();
                tFile = @"\\10.10.21.50\Scambio\MonolithicProgress.xml";
                doc1.Load(tFile);
                DisposXmlBo(doc1.InnerXml, tFile, 3, ref tRetStr);
            }
            catch
            {
            }
        }
        class clsQIEGEInfo
        {
            public string MachineName { get; set; }
            public string MachineType { get; set; }
            public string Type { get; set; }
            public string PlateId { get; set; }
            public string ProductionId { get; set; }
            public string GlassCode { get; set; }
            public string Description { get; set; }
            public string Length { get; set; }
            public string Width { get; set; }
            public string Thickness { get; set; }
            public string GlassType { get; set; }

            public string Name { get; set; }
            public string Block { get; set; }
            public string Content { get; set; }

            public string CreationTime { get; set; }
            public string LastWriteTime { get; set; }
            public string LastAccessTime { get; set; }

            public string Line { get; set; }

        }
        public string[] LastWriteTime = new string[] { "", "", "" };
        public void DisposXmlBo(string XmlStr, string _File, int _line, ref string _RetStr)
        {
            clsQIEGEInfo tclsQIEGEInfo = new clsQIEGEInfo();
            List<clsQIEGEInfo> tListclsQIEGEInfos = new List<clsQIEGEInfo>();
            FileInfo fi = new FileInfo(_File);// (@"\\10.10.21.48\Scambio\MonolithicProgress.xml");
            //string ske = "创建时间：" + fi.CreationTime.ToString() + "写入文件的时间" + fi.LastWriteTime + "访问的时间" + fi.LastAccessTime;
            tclsQIEGEInfo.Line = _line.ToString();
            if (LastWriteTime[_line - 1] == fi.LastWriteTime.ToString())
            {
                return;
            }
            LastWriteTime[_line - 1] = fi.LastWriteTime.ToString();

            tclsQIEGEInfo.CreationTime = fi.CreationTime.ToString(); tclsQIEGEInfo.LastWriteTime = fi.LastWriteTime.ToString(); tclsQIEGEInfo.LastAccessTime = fi.LastAccessTime.ToString();
            System.Xml.XmlDocument doc1 = new XmlDocument();
            doc1.LoadXml(XmlStr);
            List<string> tlist = new List<string>();
            XmlElement rootElem = doc1.DocumentElement;   //获取根节点  

            string tstr1 = rootElem.GetAttribute("MachineName");
            tclsQIEGEInfo.MachineName = tstr1;
            string tstr2 = rootElem.GetAttribute("MachineType");
            tclsQIEGEInfo.MachineType = tstr2;
            XmlNodeList personNodes = doc1.DocumentElement.ChildNodes;// rootElem.GetElementsByTagName("WorkProgress");//获取子节点集合 

            string aa = "";
            foreach (XmlNode node in personNodes)
            {
                string tstr3 = ((XmlElement)node).GetAttribute("Type");   //获取name属性值  
                if (tstr3 != "PlateProcessingEnd")
                {
                    return;
                }
                tclsQIEGEInfo.Type = tstr3;
                switch (node.Name.ToLower())
                {
                    case "monolithicprogress"://MonolithicProgress
                        XmlNodeList nodelist1 = node.ChildNodes;
                        if (nodelist1.Count > 0)
                        {
                            foreach (XmlNode Node1 in nodelist1)
                            {
                                switch (Node1.Name.ToLower())
                                {
                                    case "sheets"://Sheets
                                        XmlNodeList nodelist2 = Node1.ChildNodes;
                                        if (nodelist2.Count > 0)
                                        {
                                            foreach (XmlNode Node2 in nodelist2)
                                            {
                                                switch (Node2.Name.ToLower())
                                                {
                                                    case "sheet"://Sheet
                                                        string tstr4 = ((XmlElement)Node2).GetAttribute("PlateId");
                                                        tclsQIEGEInfo.PlateId = tstr4;
                                                        XmlNodeList nodelist3 = Node2.ChildNodes;
                                                        if (nodelist3.Count > 0)
                                                        {
                                                            foreach (XmlNode Node3 in nodelist3)
                                                            {
                                                                switch (Node3.Name.ToLower())
                                                                {
                                                                    case "productionid"://ProductionId
                                                                        aa = Node3.InnerXml.ToString().Trim();
                                                                        tclsQIEGEInfo.ProductionId = aa;
                                                                        break;
                                                                    case "glassCode"://GlassCode
                                                                        aa = Node3.InnerXml.ToString().Trim();
                                                                        tclsQIEGEInfo.GlassCode = aa;
                                                                        break;
                                                                    case "description "://Description 
                                                                        aa = Node3.InnerXml.ToString().Trim();
                                                                        tclsQIEGEInfo.Description = aa;
                                                                        break;
                                                                    case "length"://Length
                                                                        aa = Node3.InnerXml.ToString().Trim();
                                                                        tclsQIEGEInfo.Length = aa;
                                                                        break;
                                                                    case "width"://Width
                                                                        aa = Node3.InnerXml.ToString().Trim();
                                                                        tclsQIEGEInfo.Width = aa;
                                                                        break;
                                                                    case "thickness"://Thickness
                                                                        aa = Node3.InnerXml.ToString().Trim();
                                                                        tclsQIEGEInfo.Thickness = aa;
                                                                        break;
                                                                    case "glasstype"://GlassType
                                                                        aa = Node3.InnerXml.ToString().Trim();
                                                                        tclsQIEGEInfo.GlassType = aa;
                                                                        break;
                                                                }
                                                            }
                                                        }
                                                        break;

                                                }
                                            }
                                        }


                                        break;
                                    case "opt"://Opt
                                        string tstr5 = ((XmlElement)Node1).GetAttribute("Name");
                                        tclsQIEGEInfo.Name = tstr5;
                                        string tstr6 = ((XmlElement)Node1).GetAttribute("Block");
                                        tclsQIEGEInfo.Block = tstr6;
                                        XmlNodeList nodelist4 = Node1.ChildNodes;
                                        if (nodelist4.Count > 0)
                                        {
                                            foreach (XmlNode Node4 in nodelist4)
                                            {
                                                switch (Node4.Name.ToLower())
                                                {
                                                    case "content"://Content
                                                        aa = Node4.InnerXml.ToString().Trim();
                                                        tclsQIEGEInfo.Content = aa;
                                                        break;

                                                }
                                            }
                                        }
                                        break;
                                }

                            }
                        }

                        break;
                }
            }
            tListclsQIEGEInfos.Add(tclsQIEGEInfo);
            DataTable _tDt = new DataTable();
            _tDt = ToDataTable<clsQIEGEInfo>(tListclsQIEGEInfos);
            string tRetStr = "";
            tSystem.mClsDBUPdate.ExecuteTvp("Pro_InserQiGet", _tDt, ref tRetStr);


        }

        /// <summary>
        /// list<T>转成datatable格式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }


        ///
    }
}
