using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

//////20220109增加获取优化单号查询（未生产）
namespace SUTAIMES.GetData
{
    class ClsGetData
    {
        public class ClsErpLotNo  //20220109增加优化单号查询
        {
            public string YHNo { get; set; }//优化单号
            public string DJDate { get; set; } //时间
            public string OpName { get; set; }//制单人
            public string CPNum { get; set; }//成品数量
            public string CPMJ { get; set; }//成品面积
            public string YPNum { get; set; }//原片数量
            public string YPMJ { get; set; }//原片面积
            public string CCL { get; set; }//出材率
            public string SheetName { get; set; }//原片名称
        }

        public class ClsBasicData
        {
            public string Optimize_batch { get; set; }
            public string Customer_name { get; set; }//客户名称
            public string Order_id { get; set; }//客户名称 订单编号
            public string Order_singleid { get; set; }//*成品单品id
            public string Order_length { get; set; }
            public string Order_width { get; set; }
            public string Single_id { get; set; }//单品ID
            public string Single_tag { get; set; }
            public string Single_long { get; set; }
            public string Single_short { get; set; }
            public string Process_number { get; set; }
            public string Single_name { get; set; }//单品名称
            public string Single_Str { get; set; }//成品的组成 A+B+C /A+B

        }

        public class ClsErpData//MES使用
        {
            public string Optimize_batch { get; set; }
            public string Sheet_glass_id { get; set; }
            public string Sheet_glass_name { get; set; }
            public string Sheet_glass_ply { get; set; }
            public string Sheet_glass_length { get; set; }
            public string Sheet_glass_width { get; set; }
            public string Sheet_glass_cdname { get; set; }
            public string Sheet_glass_djname { get; set; }
            public string Sheet_glass_Color { get; set; }
            public string Sheet_glass_NeedType { get; set; }
            public string Layout_number { get; set; }
            public string LSerial_number { get; set; }
            public string L_x_degree { get; set; }
            public string L_y_degree { get; set; }
            public string L_x_pos { get; set; }
            public string L_y_pos { get; set; }
            public string Single_id { get; set; }
            public string Single_tag { get; set; }
            public string Single_long { get; set; }
            public string Single_short { get; set; }
            public string Single_name { get; set; }
            public string Process_number { get; set; }
            public string Sing_Type { get; set; }
            public string Single_Str { get; set; }
            public string Edging_type { get; set; }
            public string Order_id { get; set; }
            public string Order_number { get; set; }
            public string Order_singleid { get; set; }
            public string Order_singlename { get; set; }
            public string Order_name { get; set; }
            public string Customer_name { get; set; }
            public string Project_name { get; set; }
            public string Order_length { get; set; }
            public string Order_width { get; set; }
            public string Order_type { get; set; }
            public string Production_type { get; set; }
            public string Process_count { get; set; }
            public string Technological_process { get; set; }
            public string Scheduling { get; set; }
            public string Label_tag { get; set; }
            public string Label_pos { get; set; }
            public string Label_type { get; set; }
            public string Label_name { get; set; }
            public string Label_size { get; set; }
            public string Label_color { get; set; }
            public string Hollow_length { get; set; }
            public string Hollow_width { get; set; }
            public string Hollow_shelfno { get; set; }
            public string Hollow_shelfcount { get; set; }
            public string Hollow_qscode { get; set; }
            public string Hollow_wh { get; set; }
            public string Print_tag { get; set; }
            public string Print_pos { get; set; }
            public string Print_name { get; set; }
            public string spare1 { get; set; }
            public string spare2 { get; set; }
            public string spare3 { get; set; }
            public string spare4 { get; set; }
            public string spare5 { get; set; }


        }
        /// <summary>
        /// /
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        /// 
        public class ErpData
        {
            public string code { get; set; }
            public data data { get; set; }
        }
        public class data
        {
            public string djno { get; set; }
            public string sodjno { get; set; }
            public string itemsort { get; set; }
            public string itemid { get; set; }
            public string itemname { get; set; }
            public string wip { get; set; }//工序
            public string sclineid { get; set; }
            public string solineid { get; set; }
            public string pici { get; set; }
            public string num { get; set; }
            public string length { get; set; }
            public string width { get; set; }
            public string item { get; set; }
            public string itemtag { get; set; }
            public string citemname { get; set; }
            public string ctname { get; set; }
            public string str1 { get; set; }
            public string str2 { get; set; }
            public string str3 { get; set; }
            public string str4 { get; set; }
            public string lotno { get; set; }
            public string lotcount { get; set; }
            public string lotminino { get; set; }
            public string lotminicount { get; set; }
            public string sheetwidth { get; set; }
            public string sheetlength { get; set; }
            public string pno { get; set; }
            public string mbtype { get; set; }
            public string scnum { get; set; }
            public string tagname { get; set; }
            public string Struct { get; set; }

        }
        public class ClsDataNew//最终erp通讯使用
        {
            public string djno { get; set; }//流程卡号
            public string sodjno { get; set; }//订单号
            public string itemsort { get; set; }//成品切出序号
            public string itemid { get; set; }//产品编号
            public string itemname { get; set; }//产品名称
            public string wip { get; set; }//工艺路线
            public string sclineid { get; set; }//流程卡明显索引
            public string solineid { get; set; }//订序
            public string pici { get; set; }//任务批次
            public string num { get; set; }//数量
            public string length { get; set; }//长
            public string width { get; set; }//宽
            public string item { get; set; }//楼层编号
            public string itemtag { get; set; }//片标记
            public string citemname { get; set; }//产品名称
            public string ctname { get; set; }//客户名称
            public string str1 { get; set; }//单片内成品小计数量
            public string str2 { get; set; }//单片内成品编号
            public string str3 { get; set; }//成品实际切尺寸
            public string str4 { get; set; }//备用
            public string lotno { get; set; }//联线计划单号（优化单号）
            public string lotcount { get; set; }//计划单总切出数量
            public string lotminino { get; set; }//版图编号
            public string lotminicount { get; set; }//单片切出成品总数量
            public string sheetwidth { get; set; }//原片宽度
            public string sheetlength { get; set; }//原片长
            public string pno { get; set; }//成品切出序号
            public string mbtype { get; set; }//磨边类型
            public string scnum { get; set; }//计划单片总数量
            public string tagname { get; set; }//单片名称
            public string boxno { get; set; }//旗滨箱号 （20210425加）
            public string sheetname { get; set; }//切割选择原片名称
            public string ply { get; set; }//厚度
            public string color { get; set; }//颜色
            public string glasstag { get; set; }//玻璃面A1表示打印在第一面
            public string temptag { get; set; }//钢标字符
            public string tempposition { get; set; }//钢标位置 列:内宽右下（说明：内视图宽边右下，安装成品设置）
            public string tempxy { get; set; }//钢标距边距离（cm）
            public string tempicon { get; set; }//钢标图号
            public string tempword { get; set; }//钢标内容
            public string tempsize { get; set; }//钢标尺寸
            public string djname { get; set; }//原片等级名称
            public string cdname { get; set; }//原片产地名称
            public string Struct { get; set; }//产品结构标识组成（A+B）
            public string dwidth { get; set; }//成品
            public string dlength { get; set; }//宽边

            public string mx1 { get; set; }//length对应mx1 mx2  
            public string mx2 { get; set; }//length对应mx1 mx2  
            public string my1 { get; set; }//width 对应my1 my2
            public string my2 { get; set; }//width 对应my1 my2

            public string img { get; set; }//图片信息20220109增加
        }

        public class ClsErpLayOut
        {
            public string layoutno { get; set; }
            public string lserialno { get; set; }
            public string lxdegree { get; set; }
            public string lxdegree1 { get; set; }
            public string lxpos { get; set; }
            public string lypos { get; set; }
        }
        public class ClsMesLayOut
        {
            public string Optimize_batch { get; set; }//优化单号
            public string Layout_number { get; set; }//版图
            public string LSerial_number { get; set; }//磨边上片序号
            public string L_x_degree { get; set; }//X轴宽宽
            public string L_y_degree { get; set; }//X轴宽宽
            public string L_x_pos { get; set; }//X轴宽宽
            public string L_y_pos { get; set; }//X轴宽宽

        }
        public DataTable GetLINSHIERPNew(string _InputStr,string _TagStr)
        {

            string[] tArrName = { };
            string[] tArrValue = { };
            List<ErpData> ListDataErp = new List<ErpData>();
            List<ClsDataNew> tListData = new List<ClsDataNew>();
            if (clsMyPublic.IsJArray(_InputStr))
            {

            }
            else if (clsMyPublic.IsJson(_InputStr))
            {
                JObject tJobect = JObject.Parse(_InputStr.ToString());
                tArrName = tJobect.Properties().Select(item => item.Name.ToString()).ToArray();//Value
                tArrValue = tJobect.Properties().Select(item => item.Value.ToString()).ToArray();//Name
                if (tArrValue[0].ToUpper() == "OK")
                {
                    if (clsMyPublic.IsJArray(tArrValue[1]))
                    {
                        var tJAr1 = JArray.Parse(tArrValue[1]);
                        foreach (var tItem in tJAr1)
                        {
                            JObject tJobectitem = JObject.Parse(tItem.ToString());

                            JsonSerializer serializer = new JsonSerializer();
                            StringReader sr = new StringReader(tJobectitem.ToString());
                            ClsDataNew _Data = (ClsDataNew)serializer.Deserialize(new JsonTextReader(sr), typeof(ClsDataNew));
                            tListData.Add(_Data);
                        }
                    }
                }
            }
            if (tListData.Count > 0)
            {
                DataTable _tDt = new DataTable();
                _tDt = ToDataTable<ClsDataNew>(tListData);
                DataTable trDt = new DataTable(); DataTable tNewDt = new DataTable();


                trDt = SortDesc(_tDt);
                tNewDt = DisposeData(trDt, _TagStr);
                return tNewDt;
            }
            return null;

        }


        public DataTable GetLINSHIERP(string _InputStr, string _TagStr)
        {

            string[] tArrName = { };
            string[] tArrValue = { };
            List<ErpData> ListDataErp = new List<ErpData>();
            List<data> tListData = new List<data>();
            if (clsMyPublic.IsJArray(_InputStr))
            {

            }
            else if (clsMyPublic.IsJson(_InputStr))
            {
                JObject tJobect = JObject.Parse(_InputStr.ToString());
                tArrName = tJobect.Properties().Select(item => item.Name.ToString()).ToArray();//Value
                tArrValue = tJobect.Properties().Select(item => item.Value.ToString()).ToArray();//Name
                if (tArrValue[0].ToUpper() == "OK")
                {
                    if (clsMyPublic.IsJArray(tArrValue[1]))
                    {
                        var tJAr1 = JArray.Parse(tArrValue[1]);
                        foreach (var tItem in tJAr1)
                        {
                            JObject tJobectitem = JObject.Parse(tItem.ToString());

                            JsonSerializer serializer = new JsonSerializer();
                            StringReader sr = new StringReader(tJobectitem.ToString());
                            data _Data = (data)serializer.Deserialize(new JsonTextReader(sr), typeof(data));
                            tListData.Add(_Data);
                        }
                    }
                }
            }
            if (tListData.Count > 0)
            {
                DataTable _tDt = new DataTable();
                _tDt = ToDataTable<data>(tListData);
                DataTable trDt = new DataTable(); DataTable tNewDt = new DataTable();


                trDt = SortDesc(_tDt);
                tNewDt = DisposeData(trDt,_TagStr);
                return tNewDt;
            }
            return null;

        }

        private DataTable DisposeData(DataTable trDt, string _TagStr)
        {
            string tStrAA = "";
            int SingId = 1; string tNowString = DateTime.Now.ToString("yyMMddHHmmss");


            List<GetData.ClsGetData.ClsErpData> tListObj = new List<GetData.ClsGetData.ClsErpData>();

            for (int i = 0; i < trDt.Rows.Count; i++)
            {

                GetData.ClsGetData.ClsErpData tClsBasic = new GetData.ClsGetData.ClsErpData();
                tClsBasic.Optimize_batch = trDt.Rows[i]["lotno"].ToString() + _TagStr;///优化单号
                tClsBasic.Sheet_glass_id = trDt.Rows[i]["boxno"].ToString();//产地ID  变成旗滨箱号20210426
                tClsBasic.Sheet_glass_name = trDt.Rows[i]["sheetname"].ToString();////原片名称
                tClsBasic.Sheet_glass_ply = trDt.Rows[i]["ply"].ToString();/////厚度
                tClsBasic.Sheet_glass_length = trDt.Rows[i]["sheetlength"].ToString();//原片长度
                tClsBasic.Sheet_glass_width = trDt.Rows[i]["sheetwidth"].ToString();//原片宽度

                tClsBasic.Sheet_glass_cdname = trDt.Rows[i]["cdname"].ToString();//产地名称
                tClsBasic.Sheet_glass_djname = trDt.Rows[i]["djname"].ToString();//等级名称

                tClsBasic.Sheet_glass_Color = trDt.Rows[i]["color"].ToString();//原片颜色
                tClsBasic.Sheet_glass_NeedType = trDt.Rows[i]["sclineid"].ToString();//流程卡明细索引

                tClsBasic.Layout_number = trDt.Rows[i]["lotminino"].ToString();//原片上片序号
                tClsBasic.LSerial_number = trDt.Rows[i]["PNO"].ToString();//上片

                //tClsBasic.L_x_pos = trDt.Rows[i]["str1"].ToString();//单片内成品小计数量
                //tClsBasic.L_y_pos = trDt.Rows[i]["str2"].ToString();//单片内成品编号
                tClsBasic.Sing_Type = "1";//
                tClsBasic.Single_id = (i + 1).ToString(); //产品的编号
                tClsBasic.Single_tag = trDt.Rows[i]["itemtag"].ToString();//产品标记名
                float tMBA = 0, tMBB = 0;
               
                if (float.Parse(trDt.Rows[i]["length"].ToString()) > float.Parse(trDt.Rows[i]["width"].ToString()))
                {
                    if (IfFloat(trDt.Rows[i]["mx1"].ToString()) == true)//length对应mx1 mx2  
                    {
                        tMBA = float.Parse(trDt.Rows[i]["mx1"].ToString());
                    }
                    if (IfFloat(trDt.Rows[i]["mx2"].ToString()) == true)
                    {
                        tMBA = tMBA + float.Parse(trDt.Rows[i]["mx2"].ToString());
                    }

                    if (IfFloat(trDt.Rows[i]["my1"].ToString()) == true)//width 对应my1 my2
                    {
                        tMBB = float.Parse(trDt.Rows[i]["my1"].ToString());
                    }
                    if (IfFloat(trDt.Rows[i]["my2"].ToString()) == true)
                    {
                        tMBB = tMBB + float.Parse(trDt.Rows[i]["my2"].ToString());
                    }
                    tClsBasic.L_x_pos = tMBA.ToString();//磨边量  长边
                    tClsBasic.L_y_pos = tMBB.ToString();//磨边量  短边

                    //tClsBasic.Single_long = ((int)(float.Parse(trDt.Rows[i]["length"].ToString()) + 0.5) - tMBA).ToString();//长边
                    //tClsBasic.Single_short = ((int)(float.Parse(trDt.Rows[i]["width"].ToString()) + 0.5) - tMBB).ToString();//短边
                    tClsBasic.Single_long = (float.Parse(trDt.Rows[i]["length"].ToString())  - tMBA).ToString();//长边
                    tClsBasic.Single_short = (float.Parse(trDt.Rows[i]["width"].ToString()) - tMBB).ToString();//短边

                    //tClsBasic.L_x_degree
                }
                else
                {

                    if (IfFloat(trDt.Rows[i]["mx1"].ToString()) == true)
                    {
                        tMBA = float.Parse(trDt.Rows[i]["mx1"].ToString());
                    }
                    if (IfFloat(trDt.Rows[i]["mx2"].ToString()) == true)
                    {
                        tMBA = tMBA + float.Parse(trDt.Rows[i]["mx2"].ToString());
                    }

                    if (IfFloat(trDt.Rows[i]["my1"].ToString()) == true)
                    {
                        tMBB = float.Parse(trDt.Rows[i]["my1"].ToString());
                    }
                    if (IfFloat(trDt.Rows[i]["my2"].ToString()) == true)
                    {
                        tMBB = tMBB + float.Parse(trDt.Rows[i]["my2"].ToString());
                    }
                    tClsBasic.L_y_pos = tMBA.ToString();//磨边量 短板
                    tClsBasic.L_x_pos = tMBB.ToString();//磨边量 长边

                    //tClsBasic.Single_long = ((int)(float.Parse(trDt.Rows[i]["width"].ToString()) + 0.5) - tMBB).ToString();//短边
                    //tClsBasic.Single_short = ((int)(float.Parse(trDt.Rows[i]["length"].ToString()) + 0.5) - tMBA).ToString();//长边

                    tClsBasic.Single_long = (float.Parse(trDt.Rows[i]["width"].ToString()) - tMBB).ToString();//短边
                    tClsBasic.Single_short = (float.Parse(trDt.Rows[i]["length"].ToString()) - tMBA).ToString();//长边


                }

                tClsBasic.Single_name = trDt.Rows[i]["tagname"].ToString();//单片名称
                tClsBasic.Process_number = trDt.Rows[i]["djno"].ToString();//流程卡号



                tClsBasic.Single_Str = trDt.Rows[i]["struct"].ToString();///////////////////////需要解析数据  成品玻璃的组成 A+B+C

                //if (trDt.Rows[i]["mbtype"].ToString() == "M") { tClsBasic.Edging_type = "2"; } else { tClsBasic.Edging_type = "2"; }//磨边方向

                tClsBasic.Edging_type = trDt.Rows[i]["ply"].ToString();//订单号
                tClsBasic.Order_id = trDt.Rows[i]["sodjno"].ToString();//订单号
                tClsBasic.Order_number = trDt.Rows[i]["solineid"].ToString();//订序

                if (tStrAA != trDt.Rows[i]["djno"].ToString().Trim() + trDt.Rows[i]["itemtag"].ToString().Trim() + trDt.Rows[i]["solineid"].ToString().Trim())
                {
                    SingId = 1;
                    tStrAA = trDt.Rows[i]["djno"].ToString().Trim() + trDt.Rows[i]["itemtag"].ToString().Trim() + trDt.Rows[i]["solineid"].ToString().Trim();
                    tClsBasic.Order_singleid = trDt.Rows[i]["djno"].ToString() + trDt.Rows[i]["solineid"].ToString().PadLeft(4, '0') + SingId.ToString().PadLeft(3, '0');//////注意 唯一成品编号
                }
                else
                {
                    SingId = SingId + 1;

                    tClsBasic.Order_singleid = trDt.Rows[i]["djno"].ToString() + trDt.Rows[i]["solineid"].ToString().PadLeft(4, '0') + SingId.ToString().PadLeft(3, '0');//////注意 唯一成品编号
                }
                tClsBasic.Scheduling = trDt.Rows[i]["wip"].ToString();//工序

                //CC14李赛克使用
                string tStrBB= trDt.Rows[i]["citemname"].ToString();
                string[] tStrC1 = tStrBB.Split('+'); tStrBB = "";
                for (int CC1 = 0; CC1 < tStrC1.Length; CC1++)
                {
                    if (tStrBB == "")
                    {
                        tStrBB =  tStrC1[tStrC1.Length - CC1-1];
                    }
                    else
                    {
                        tStrBB = tStrBB +"+"+ tStrC1[tStrC1.Length - CC1-1];
                    }
                }
                tClsBasic.Order_singlename = tStrBB;// trDt.Rows[i]["citemname"].ToString();//产品名称
                //tClsBasic.Order_name = trDt.Rows[i]["lotcount"].ToString();
                tClsBasic.Customer_name = trDt.Rows[i]["ctname"].ToString();
                //tClsBasic.Project_name = trDt.Rows[i]["lotminicount"].ToString();

                string[] tStrArr = trDt.Rows[i]["str3"].ToString().Split('×');
                //if (tStrArr.Length == 2)
                {
                    tClsBasic.L_x_degree = trDt.Rows[i]["width"].ToString();// tStrArr[0];//成本尺寸
                    tClsBasic.L_y_degree = trDt.Rows[i]["length"].ToString();// tStrArr[1];//


                }
                tClsBasic.Order_length = trDt.Rows[i]["dwidth"].ToString();
                tClsBasic.Order_width = trDt.Rows[i]["dlength"].ToString(); //宽边

                tClsBasic.Order_type = trDt.Rows[i]["num"].ToString();
                tClsBasic.Production_type = trDt.Rows[i]["item"].ToString();
                tClsBasic.Process_count = trDt.Rows[i]["scnum"].ToString();//流程卡数量
                //tClsBasic.Technological_process = trDt.Rows[i]["成品名称"].ToString();
                //钢标打印
                tClsBasic.Label_tag = trDt.Rows[i]["temptag"].ToString();//钢标字符
                tClsBasic.Label_pos = trDt.Rows[i]["tempposition"].ToString();//钢标位置 列:内宽右下（说明：内视图宽边右下，安装成品设置）
                tClsBasic.Label_type = trDt.Rows[i]["tempxy"].ToString();///钢标距边距离（cm）
                tClsBasic.Label_name = trDt.Rows[i]["tempicon"].ToString();//钢标图号
                tClsBasic.Label_size = trDt.Rows[i]["tempsize"].ToString();//钢标尺寸
                tClsBasic.Label_color = trDt.Rows[i]["tempword"].ToString();//钢标内容

                //tClsBasic.Hollow_length = trDt.Rows[i]["客户名称"].ToString();
                //tClsBasic.Hollow_width = trDt.Rows[i]["客户名称"].ToString();
                //tClsBasic.Hollow_shelfno = trDt.Rows[i]["客户名称"].ToString();
                //tClsBasic.Hollow_shelfcount = trDt.Rows[i]["客户名称"].ToString();
                //tClsBasic.Hollow_qscode = trDt.Rows[i]["客户名称"].ToString();
                //tClsBasic.Hollow_wh = trDt.Rows[i]["客户名称"].ToString();
                //tClsBasic.Print_tag = trDt.Rows[i]["客户名称"].ToString();
                //tClsBasic.Print_pos = trDt.Rows[i]["客户名称"].ToString();
                //tClsBasic.Print_name = trDt.Rows[i]["客户名称"].ToString();
                tClsBasic.spare1 = trDt.Rows[i]["lotcount"].ToString();//优化单数量
                tClsBasic.spare2 = trDt.Rows[i]["lotminicount"].ToString();//原片出片数量
                tClsBasic.spare3 = trDt.Rows[i]["itemid"].ToString(); //产品的编号
                tClsBasic.spare4 = trDt.Rows[i]["pici"].ToString();//批次
                tClsBasic.spare5 = trDt.Rows[i]["itemsort"].ToString();//成品切出序号

                tListObj.Add(tClsBasic);
            }

            DataTable tnewToDt = GetData.ClsGetData.ToDataTable<GetData.ClsGetData.ClsErpData>(tListObj);


            return tnewToDt;
            //tSystem.mClsDBUPdate.ExecuteTvp("Pro_ToExcelB", tnewToDt, ref tRetStr);

        }


        public DataTable GetLinShiEepLay(string _InputStr, string _Optiomize)
        {
            string[] tArrName = { };
            string[] tArrValue = { };
            List<ErpData> ListDataErp = new List<ErpData>();
            List<ClsMesLayOut> tListData = new List<ClsMesLayOut>();
            if (clsMyPublic.IsJArray(_InputStr))
            {

            }
            else if (clsMyPublic.IsJson(_InputStr))
            {
                JObject tJobect = JObject.Parse(_InputStr.ToString());
                tArrName = tJobect.Properties().Select(item => item.Name.ToString()).ToArray();//Value
                tArrValue = tJobect.Properties().Select(item => item.Value.ToString()).ToArray();//Name
                if (tArrValue[0].ToUpper() == "OK")
                {
                    if (clsMyPublic.IsJArray(tArrValue[1]))
                    {
                        var tJAr1 = JArray.Parse(tArrValue[1]);
                        foreach (var tItem in tJAr1)
                        {
                            JObject tJobectitem = JObject.Parse(tItem.ToString());

                            JsonSerializer serializer = new JsonSerializer();
                            StringReader sr = new StringReader(tJobectitem.ToString());
                            ClsErpLayOut _Data = (ClsErpLayOut)serializer.Deserialize(new JsonTextReader(sr), typeof(ClsErpLayOut));
                            ClsMesLayOut tMesData = new ClsMesLayOut();
                            tMesData.Optimize_batch = _Optiomize; tMesData.Layout_number = _Data.layoutno; tMesData.LSerial_number = _Data.lserialno;
                            tMesData.L_x_degree = _Data.lxdegree; tMesData.L_y_degree = _Data.lxdegree1; tMesData.L_x_pos = _Data.lxpos; tMesData.L_y_pos = _Data.lypos;
                            tListData.Add(tMesData);
                        }
                    }
                }
            }
            if (tListData.Count > 0)
            {
                DataTable _tDt = new DataTable();
                _tDt = ToDataTable<ClsMesLayOut>(tListData);

                return _tDt;
            }
            return null;

        }

        private DataTable SortDesc(DataTable dt)
        {
            dt.DefaultView.Sort = "djno ,solineid,itemtag asc";//"流程卡号 ,订序 ASC";
            dt = dt.DefaultView.ToTable();
            return dt;
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
        private bool IfFloat(string _Data)
        {
            try
            {
                float aa = float.Parse(_Data);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static DataTable ToDataTable<T>(IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    System.Collections.ArrayList tempList = new System.Collections.ArrayList();
                    foreach (System.Reflection.PropertyInfo pi in props)
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

    }
}
