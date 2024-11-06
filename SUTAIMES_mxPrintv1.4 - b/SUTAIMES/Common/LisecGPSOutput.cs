using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;
using System.IO;
//using lisec_layout_test.data;

using System.Text.RegularExpressions;

namespace SUTAIMES.Common
{
    class LisecGPSOutput
    {
        public static int indexGLX = 0; //项目数量索引 对方和说明书上说玻璃条目相同为0，不同则为实际数量，但是我没搞清楚，如果不同的至少是2，为什么给我的样例里会出现1
        public static int indexFRX = 0;

        private static DataTable mDataCode = new DataTable();

    //    ////输出文本output
        public static string SetTrfData(DataTable dt, DataTable _DtCode)
        {
            mDataCode = _DtCode;
            string[] singlename = dt.Rows[0]["Order_singleid"].ToString().Split('+');
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                string[] singlenamei = dt.Rows[i]["Order_singleid"].ToString().Split('+');
                if (singlenamei.Length == singlename.Length && singlenamei.Length >= 2)
                {
                    if (singlename[0] != singlename[0])
                    {
                        indexGLX = 1;
                    }
                    if (singlename[1] != singlename[1])
                    {
                        indexFRX = 1;
                    }
                }
            }

            string output = LayOutREL(dt) + LayOutBTH(dt) + LayOutORD(dt);
            IEnumerable<IGrouping<string, DataRow>> result = dt.Rows.Cast<DataRow>().GroupBy(dr => dr["Order_singleid"].ToString());
            if (result != null && result.Count() > 0)
            {
                foreach (IGrouping<string, DataRow> rows in result)
                {
                    DataTable subData = rows.ToArray().CopyToDataTable();
                    output += LayOutPOS(subData);
                    for (int i = 0; i < subData.Rows.Count; i++)
                    {
                        output += LayOutGLX(subData, i + 1);
                    }
                    for (int i = 0; i < subData.Rows.Count - 1; i++)
                    {
                        output += LayOutFRX(subData, i + 1);
                    }
                }
            }

            return output;
        }

        public static string[] GetCodePOS(string ordersinglename)   // Order_singlename   对应玻璃代码和框架代码，这里用来比对的是ordername
        {
            DataTable dt = mDataCode;// FileDialog.GetXlsData(@"..\Debug\玻璃代码.xls");
            string[] valuesCode = new string[10] { "", "", "", "", "", "", "", "", "", "" };
            string[] a = ordersinglename.Split('+');
            DataRow[] tDR = dt.Select("玻璃产品名称 like '%' and 框数据 like '%'");

            string tStrA1 = "";
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < dt.Rows.Count; j++)
                {

                    //if (a[i].Contains(dt.Rows[j][2].ToString()) && dt.Rows[j][2].ToString() != "" && dt.Rows[j][1].ToString() != "" )
                    
                    //if (a[i].Contains(AA1) && dt.Rows[j][2].ToString() != "" && AA2 != "" && i %2==0)
                    //{
                    //    valuesCode[i] = dt.Rows[j][1].ToString();
                    //}
                    if(i%2==0)
                    {
                        string AA1 = dt.Rows[j][2].ToString(), AA2 = dt.Rows[j][1].ToString();
                        if (a[i].Contains(AA1) && dt.Rows[j][2].ToString() != "" && AA2 != "" )
                        {
                            valuesCode[i] = dt.Rows[j][1].ToString();
                            //tStrA1 = dt.Rows[j][1].ToString();
                        }
                    }
                    else if(i%2==1)
                    {
                        string BB1 = dt.Rows[j][5].ToString(), BB2 = dt.Rows[j][4].ToString();
                        string BB3 = dt.Rows[j][1].ToString();
                        if (a[i].Contains(BB1) && dt.Rows[j][5].ToString() != "" && BB2 != ""
                           
                            )
                        {
                            valuesCode[i] = dt.Rows[j][4].ToString();
                        }
                    }
                    //if (a[i].Contains(dt.Rows[j][5].ToString()) && dt.Rows[j][5].ToString() != "" && dt.Rows[j][4].ToString() != "")
                    
                    //if (a[i].Contains(BB1) && dt.Rows[j][5].ToString() != "" && BB2 != "" && i % 2 == 0)
                    //{
                    //    valuesCode[i] = dt.Rows[j][4].ToString();
                    //}
                }
            }
            return valuesCode;
        }
        //材料代码在对方软件中 
        public static int[] GetCodeGLX(string name)   // Order_singlename    values[0]玻璃代码  values[1]框架代码  values[2]密封材料   values[3]框架材料   values[4]气体类型
        {
            int[] valuesCode = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            if (name.Contains("密"))
            {
                valuesCode[2] = 1;
            }
            if (name.Contains("结"))
            {
                valuesCode[2] = 2;
            }

            return valuesCode;
        }

        public static int[] GetCodeFRX(string name)   // Order_singlename    values[0]玻璃代码  values[1]框架代码  values[2]密封材料   values[3]框架材料   values[4]气体类型
        {
            int[] valuesCode = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            if (name.Contains("铝"))
            {
                valuesCode[3] = 0;
            }
            if (name.Contains("钢"))
            {
                valuesCode[3] = 1;
            }
            if (name.Contains("不锈钢"))
            {
                valuesCode[3] = 2;
            }
            if (name.Contains("塑"))
            {
                valuesCode[3] = 3;
            }
            if (name.Contains("间隔条"))
            {
                valuesCode[3] = 4;
            }
            if (name.Contains("TPS"))
            {
                valuesCode[3] = 5;
            }

            return valuesCode;
        }




        public static string LayOutREL(DataTable dt)
        {
            // 记录类型<REL>传输文件记录  |  传输文件编号：02.90  |  备注：空 
            string rel = "<REL>|02.90|" + ReLen("", 40) + "\n";
            return rel;
        }


        public static string LayOutBTH(DataTable dt)
        {
            string tProcess_number = System.Text.RegularExpressions.Regex.Replace(dt.Rows[0]["Process_number"].ToString(), @"[^0-9]+", "");//仅数字
            //  记录类型<BTH>  |  附加批次信息                                      |启动条码（仅适用于唯一的条码）: 空 |  实际批次号                              
            string bth = "<BTH>|" + ReLen(dt.Rows[0]["Order_id"].ToString(), 10) + "|" + ReLen("", 6, '0') + "|" + ReLen(tProcess_number, 8, '0') + "\n";   //注："Order_id"替换成了"Process_number"
            return bth;
        }



        public static string LayOutORD(DataTable dt)
        {
            string tProcess_number = System.Text.RegularExpressions.Regex.Replace(dt.Rows[0]["Process_number"].ToString(), @"[^0-9]+", "");//仅数字，最高9位
            //  记录类型<ORD>  | 订单号(最高9位)：                                  |  客户编号： 空   |  客户名                                                 |information 1               2                    3                       4                      5            |  生产日期                                 |  交货日期|Delivery area 
            string ord = "<ORD>|" + ReLen(tProcess_number, 9) + " |" + ReLen("1", 10) + "|" + ReLen(dt.Rows[0]["Customer_name"].ToString(), 40) + "|" + ReLen("", 40) + "|" + ReLen("", 40) + "|" + ReLen("", 40) + "|" + ReLen("", 40) + "|" + ReLen("", 40) + "|" + DateTime.Now.ToString("dd/MM/yyyy") + "|00/00/0000|" + ReLen("", 10, '0') + "\n";
            return ord;
        }


        //在POS中通过表格将玻璃和框架转换成对应代码 对方表示客户只是使用了一种密封材料  这里缺少一个参数密封深度，当前设为70
        public static string LayOutPOS(DataTable dt)
        {
            string[] a = dt.Rows[0]["Order_singleid"].ToString().Split('.');
            string[] value = GetCodePOS(dt.Rows[0]["Order_singlename"].ToString());

            // 记录类型<POS>物料项目记录| 项目号: （对每订单唯一） |  ID             |     条码                |       单位数量           |宽                                                         | 高                                                        |
            string pos = "<POS>|" + ReLen(a[1], 5, '0') + "|" + ReLen("1", 8, '0') + "|" + ReLen("1", 4, '0') + "|" + ReLen("1", 5, '0') + "|" + ReLen(dt.Rows[0]["Order_width"].ToString() + "0", 6, '0') + "|" + ReLen(dt.Rows[0]["Order_length"] + "0".ToString(), 6, '0') + "|"
                //  玻璃代码1/信息                 | 玻璃框架代码1/信息|                                        玻璃代码2/信息 | 玻璃框架代码2/信息|                                            玻璃代码3/信息 | 玻璃框架代码3/信息|      玻璃代码4/信息 | 玻璃框架代码4/信息 |     玻璃代码5/信息                                                |密封(1/10mm)                 | 框架文本号：00          | 气体类型01 00 00 00                                                                                    |    密封材料代码          |    窗口类型              | 窗框高度               |方向0（0无方向1水平2垂直               | 物品使用0
                         + ReLen(value[0].ToString(), 5) + "|" + ReLen(value[1].ToString(), 3) + "|" + ReLen(value[2].ToString(), 5) + "|" + ReLen(value[3].ToString(), 3) + "|" + ReLen(value[4].ToString(), 5) + "|" + ReLen(value[5].ToString(), 3) + "|" + ReLen(value[6].ToString(), 5) + "|" + ReLen(value[7].ToString(), 3) + "|" + ReLen("", 5) + "|" + ReLen("70", 4, '0') + "|" + ReLen("", 2, '0') + "|" + ReLen("1", 2, '0') + "|" + ReLen("", 2, '0') + "|" + ReLen("", 2, '0') + "|" + ReLen("", 2, '0') + "|" + ReLen("1", 1, '0') + "|" + ReLen("1", 1, '0') + "|" + ReLen("", 6, '0') + "|" + ReLen("", 1, '0') + "|" + ReLen("", 1, '0') + "\n";
            return pos;
        }

        //这里缺少一个参数玻璃厚度 当前设为50
        public static string LayOutGLX(DataTable dt, int i)
        {
            int[] value = GetCodeGLX(dt.Rows[0]["Order_singlename"].ToString());
            string[] a = dt.Rows[0]["Order_singlename"].ToString().Split('+');
            string describe = "";
            if (a.Length >= 2 * i - 2)
            {
                describe = a[2 * i - 2];
            }
            string strAfter = Regex.Replace(describe, @"[^a-zA-Z0-9\u4e00-\u9fa5\s]", "");
            //                                                                                         输入信息少玻璃厚度！
            //记录类型<GLX>第X片玻璃信息| 项目数量索引（全部相同为0）|  玻璃描述| 玻璃表面类型：0   | 玻璃厚度                 | DGU图案/图层：0（0无1正面2反面）| ID（推车/支架信息）：1   | 图案方向0（0无方向1水平2垂直） |  玻璃面板ID（条形码） | GPS.prod中的窗格号码  |  GPS.prod中的组件号：00  | 材料类别：00
            string glx = "<GL" + i + ">|" + ReLen(indexGLX.ToString(), 5, '0') + "|" + ReLen(strAfter, 40) + "|0|" + ReLen(dt.Rows[i-1]["ply"].ToString() + "0", 5, '0') + "|0|" + ReLen("", 10) + "|0|" + ReLen("", 10, '0') + "|" + ReLen(i.ToString(), 1, '0') + "|" + ReLen("", 2, '0') + "|" + ReLen("", 2, '0') + "\n";
            return glx;
        }


        //对方说目前客户使用的框架类型是5（TPA）框架高60 框架宽就是Order_singlename中间那部分
        public static string LayOutFRX(DataTable dt, int i)
        {
            int[] value = GetCodeFRX(dt.Rows[0]["Order_singlename"].ToString());
            string[] a = dt.Rows[0]["Order_singlename"].ToString().Split('+');
            string describe = "";
            if (a.Length >= 2 * i - 1)
            {
                describe = a[2 * i - 1];
            }
            string strAfter = Regex.Replace(describe, @"[^a-zA-Z0-9\u4e00-\u9fa5\s]", "");
            string tHeight = System.Text.RegularExpressions.Regex.Replace(describe.Substring(0, 2), @"[^0-9]+", "");
            //记录类型<FRX>第X个框架信息 |项目数量索引（全部相同为0) | 框架描述                      | 框架类型|框架颜色| 框架宽| 框架高|ID                             |条形码
            string frx = "<FR" + i + ">|" + ReLen(indexFRX.ToString(), 5, '0') + "|" + ReLen(strAfter, 40) + "|05|00|" + ReLen(tHeight + '0', 5, '0') + "|00060|" + ReLen(dt.Rows[0]["WcsID"].ToString(), 10) + "|" + ReLen("", 10, '0') + "\n";
            return frx;
        }



        //字符串固定长度
        public static string ReLen(String strX, int n, char a = ' ')
        {
            if (StrLength(strX) <= n)
            {
                return strX.PadLeft(n - (StrLength(strX) - strX.Length), a);
            }
            else
            {
                strX = strX.Substring(1);
                return ReLen(strX, n, a);
            }

        }

        //一个汉字占三个字符
        public static int StrLength(string s)
        {
            int length = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] > 127)
                    length += 3;
                else
                    length += 1;
            }
            return length;
        }

    }
    //class LisecGPSOutput
    //{
    //    public static int indexGLX = 0; //项目数量索引 对方和说明书上说玻璃条目相同为0，不同则为实际数量，但是我没搞清楚，如果不同的至少是2，为什么给我的样例里会出现1
    //    public static int indexFRX = 0;

    //    private static DataTable mDataCode = new DataTable();

    //    ////输出文本output
    //    public static string SetTrfData(DataTable dt, DataTable _DtCode)
    //    {
    //        mDataCode = _DtCode;
    //        string[] singlename = dt.Rows[0]["Order_singleid"].ToString().Split('+');
    //        for (int i = 0; i < dt.Rows.Count; i++)
    //        {

    //            string[] singlenamei = dt.Rows[i]["Order_singleid"].ToString().Split('+');
    //            if (singlenamei.Length == singlename.Length && singlenamei.Length >= 2)
    //            {
    //                if (singlename[0] != singlename[0])
    //                {
    //                    indexGLX = 1;
    //                }
    //                if (singlename[1] != singlename[1])
    //                {
    //                    indexFRX = 1;
    //                }
    //            }
    //        }

    //        string output = LayOutREL(dt) + LayOutBTH(dt) + LayOutORD(dt);
    //        IEnumerable<IGrouping<string, DataRow>> result = dt.Rows.Cast<DataRow>().GroupBy(dr => dr["Order_singleid"].ToString());
    //        if (result != null && result.Count() > 0)
    //        {
    //            foreach (IGrouping<string, DataRow> rows in result)
    //            {
    //                DataTable subData = rows.ToArray().CopyToDataTable();
    //                output += LayOutPOS(subData);
    //                for (int i = 0; i < subData.Rows.Count; i++)
    //                {
    //                    output += LayOutGLX(subData, i + 1);
    //                }
    //                for (int i = 0; i < subData.Rows.Count - 1; i++)
    //                {
    //                    output += LayOutFRX(subData, i + 1);
    //                }
    //            }
    //        }

    //        return output;
    //    }

    //    public static string[] GetCodePOS(string ordersinglename)   // Order_singlename   对应玻璃代码和框架代码，这里用来比对的是ordername
    //    {
    //        DataTable dt = mDataCode;// FileDialog.GetXlsData(@"..\Debug\玻璃代码.xls");
    //        string[] valuesCode = new string[10] { "", "", "", "", "", "", "", "", "", "" };
    //        string[] a = ordersinglename.Split('+');
    //        for (int i = 0; i < a.Length; i++)
    //        {


    //            for (int j = 0; j < dt.Rows.Count; j++)
    //            {


    //                //if (a[i].Contains(dt.Rows[j][2].ToString()) && dt.Rows[j][2].ToString() != "" && dt.Rows[j][1].ToString() != "")
    //                string AA1 = dt.Rows[j][2].ToString(), AA2 = dt.Rows[j][1].ToString();
    //                if (a[i].Contains(AA1) && dt.Rows[j][2].ToString() != "" && AA2 != "")
    //                {
    //                    valuesCode[i] = dt.Rows[j][1].ToString();
    //                }
    //                //if (a[i].Contains(dt.Rows[j][5].ToString()) && dt.Rows[j][5].ToString() != "" && dt.Rows[j][4].ToString() != "")
    //                string BB1 = dt.Rows[j][5].ToString(), BB2 = dt.Rows[j][4].ToString();
    //                if (a[i].Contains(BB1) && dt.Rows[j][5].ToString() != "" && BB2 != "")
    //                {
    //                    valuesCode[i] = dt.Rows[j][4].ToString();
    //                }


    //            }
    //        }
    //        return valuesCode;
    //    }


    //    //材料代码在对方软件中 
    //    public static int[] GetCodeGLX(string name)   // Order_singlename    values[0]玻璃代码  values[1]框架代码  values[2]密封材料   values[3]框架材料   values[4]气体类型
    //    {
    //        int[] valuesCode = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    //        if (name.Contains("密"))
    //        {
    //            valuesCode[2] = 1;
    //        }
    //        if (name.Contains("结"))
    //        {
    //            valuesCode[2] = 2;
    //        }

    //        return valuesCode;
    //    }

    //    public static int[] GetCodeFRX(string name)   // Order_singlename    values[0]玻璃代码  values[1]框架代码  values[2]密封材料   values[3]框架材料   values[4]气体类型
    //    {
    //        int[] valuesCode = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    //        if (name.Contains("铝"))
    //        {
    //            valuesCode[3] = 0;
    //        }
    //        if (name.Contains("钢"))
    //        {
    //            valuesCode[3] = 1;
    //        }
    //        if (name.Contains("不锈钢"))
    //        {
    //            valuesCode[3] = 2;
    //        }
    //        if (name.Contains("塑"))
    //        {
    //            valuesCode[3] = 3;
    //        }
    //        if (name.Contains("间隔条"))
    //        {
    //            valuesCode[3] = 4;
    //        }
    //        if (name.Contains("TPS"))
    //        {
    //            valuesCode[3] = 5;
    //        }

    //        return valuesCode;
    //    }




    //    public static string LayOutREL(DataTable dt)
    //    {
    //        // 记录类型<REL>传输文件记录  |  传输文件编号：02.90  |  备注：空 
    //        string rel = "<REL>|02.90|" + ReLen("", 40) + "\n";
    //        return rel;
    //    }


    //    public static string LayOutBTH(DataTable dt)
    //    {
    //        string tProcess_number = System.Text.RegularExpressions.Regex.Replace(dt.Rows[0]["Process_number"].ToString(), @"[^0-9]+", "");//仅数字
    //        //  记录类型<BTH>  |  附加批次信息                                      |启动条码（仅适用于唯一的条码）: 空 |  实际批次号                              
    //        string bth = "<BTH>|" + ReLen(dt.Rows[0]["Order_id"].ToString(), 10) + "|" + ReLen("", 6, '0') + "|" + ReLen(tProcess_number, 8, '0') + "\n";   //注："Order_id"替换成了"Process_number"
    //        return bth;
    //    }



    //    public static string LayOutORD(DataTable dt)
    //    {
    //        string tProcess_number = System.Text.RegularExpressions.Regex.Replace(dt.Rows[0]["Process_number"].ToString(), @"[^0-9]+", "");//仅数字，最高9位
    //        //  记录类型<ORD>  | 订单号(最高9位)：                                  |  客户编号： 空   |  客户名                                                 |information 1               2                    3                       4                      5            |  生产日期                                 |  交货日期|Delivery area 
    //        string ord = "<ORD>|" + ReLen(tProcess_number, 9) + " |" + ReLen("1", 10) + "|" + ReLen(dt.Rows[0]["Customer_name"].ToString(), 40) + "|" + ReLen("", 40) + "|" + ReLen("", 40) + "|" + ReLen("", 40) + "|" + ReLen("", 40) + "|" + ReLen("", 40) + "|" + DateTime.Now.ToString("dd/MM/yyyy") + "|00/00/0000|" + ReLen("", 10, '0') + "\n";
    //        return ord;
    //    }


    //    //在POS中通过表格将玻璃和框架转换成对应代码 对方表示客户只是使用了一种密封材料  这里缺少一个参数密封深度，当前设为70
    //    public static string LayOutPOS(DataTable dt)
    //    {
    //        string[] a = dt.Rows[0]["Order_singleid"].ToString().Split('.');
    //        string[] value = GetCodePOS(dt.Rows[0]["Order_singlename"].ToString());

    //        // 记录类型<POS>物料项目记录| 项目号: （对每订单唯一） |  ID             |     条码                |       单位数量           |宽                                                         | 高                                                        |
    //        string pos = "<POS>|" + ReLen(a[1], 5, '0') + "|" + ReLen("1", 8, '0') + "|" + ReLen("1", 4, '0') + "|" + ReLen("1", 5, '0') + "|" + ReLen(dt.Rows[0]["Order_width"].ToString() + "0", 6, '0') + "|" + ReLen(dt.Rows[0]["Order_length"] + "0".ToString(), 6, '0') + "|"
    //            //  玻璃代码1/信息                 | 玻璃框架代码1/信息|                                        玻璃代码2/信息 | 玻璃框架代码2/信息|                                            玻璃代码3/信息 | 玻璃框架代码3/信息|      玻璃代码4/信息 | 玻璃框架代码4/信息 |     玻璃代码5/信息                                                |密封(1/10mm)                 | 框架文本号：00          | 气体类型01 00 00 00                                                                                    |    密封材料代码          |    窗口类型              | 窗框高度               |方向0（0无方向1水平2垂直               | 物品使用0
    //                     + ReLen(value[0].ToString(), 5) + "|" + ReLen(value[1].ToString(), 3) + "|" + ReLen(value[2].ToString(), 5) + "|" + ReLen(value[3].ToString(), 3) + "|" + ReLen(value[4].ToString(), 5) + "|" + ReLen(value[5].ToString(), 3) + "|" + ReLen(value[6].ToString(), 5) + "|" + ReLen(value[7].ToString(), 3) + "|" + ReLen("", 5) + "|" + ReLen("70", 4, '0') + "|" + ReLen("", 2, '0') + "|" + ReLen("1", 2, '0') + "|" + ReLen("", 2, '0') + "|" + ReLen("", 2, '0') + "|" + ReLen("", 2, '0') + "|" + ReLen("1", 1, '0') + "|" + ReLen("1", 1, '0') + "|" + ReLen("", 6, '0') + "|" + ReLen("", 1, '0') + "|" + ReLen("", 1, '0') + "\n";
    //        return pos;
    //    }

    //    //这里缺少一个参数玻璃厚度 当前设为50
    //    public static string LayOutGLX(DataTable dt, int i)
    //    {
    //        int[] value = GetCodeGLX(dt.Rows[0]["Order_singlename"].ToString());
    //        string[] a = dt.Rows[0]["Order_singlename"].ToString().Split('+');
    //        string describe = "";
    //        if (a.Length >= 2 * i - 2)
    //        {
    //            describe = a[2 * i - 2];
    //        }

    //        //                                                                                         输入信息少玻璃厚度！
    //        //记录类型<GLX>第X片玻璃信息| 项目数量索引（全部相同为0）|  玻璃描述| 玻璃表面类型：0   | 玻璃厚度                 | DGU图案/图层：0（0无1正面2反面）| ID（推车/支架信息）：1   | 图案方向0（0无方向1水平2垂直） |  玻璃面板ID（条形码） | GPS.prod中的窗格号码  |  GPS.prod中的组件号：00  | 材料类别：00
    //        string glx = "<GL" + i + ">|" + ReLen(indexGLX.ToString(), 5, '0') + "|" + ReLen(describe, 40) + "|0|" + ReLen(dt.Rows[0]["ply"].ToString() + "0", 5, '0') + "|0|" + ReLen("", 10) + "|0|" + ReLen("", 10, '0') + "|" + ReLen(i.ToString(), 1, '0') + "|" + ReLen("", 2, '0') + "|" + ReLen("", 2, '0') + "\n";
    //        return glx;
    //    }


    //    //对方说目前客户使用的框架类型是5（TPA）框架高60 框架宽就是Order_singlename中间那部分
    //    public static string LayOutFRX(DataTable dt, int i)
    //    {
    //        int[] value = GetCodeFRX(dt.Rows[0]["Order_singlename"].ToString());
    //        string[] a = dt.Rows[0]["Order_singlename"].ToString().Split('+');
    //        string describe = "";
    //        if (a.Length >= 2 * i - 1)
    //        {
    //            describe = a[2 * i - 1];
    //        }
    //        string tHeight = System.Text.RegularExpressions.Regex.Replace(describe.Substring(0, 2), @"[^0-9]+", "");
    //        //记录类型<FRX>第X个框架信息 |项目数量索引（全部相同为0) | 框架描述                      | 框架类型|框架颜色| 框架宽| 框架高|ID                             |条形码
    //        string frx = "<FR" + i + ">|" + ReLen(indexFRX.ToString(), 5, '0') + "|" + ReLen(describe, 40) + "|05|00|" + ReLen(tHeight + '0', 5, '0') + "|00060|" + ReLen(dt.Rows[0]["WcsID"].ToString(), 10) + "|" + ReLen("", 10, '0') + "\n";
    //        return frx;
    //    }



    //    //字符串固定长度
    //    public static string ReLen(String strX, int n, char a = ' ')
    //    {
    //        if (StrLength(strX) <= n)
    //        {
    //            return strX.PadLeft(n - (StrLength(strX) - strX.Length), a);
    //        }
    //        else
    //        {
    //            strX = strX.Substring(1);
    //            return ReLen(strX, n, a);
    //        }

    //    }

    //    //一个汉字占三个字符
    //    public static int StrLength(string s)
    //    {
    //        int length = 0;
    //        for (int i = 0; i < s.Length; i++)
    //        {
    //            if (s[i] > 127)
    //                length += 3;
    //            else
    //                length += 1;
    //        }
    //        return length;
    //    }

    //}

    //class LisecGPSOutput
    //{
    //    public static int indexGLX = 0; //项目数量索引 对方和说明书上说玻璃条目相同为0，不同则为实际数量，但是我没搞清楚，如果不同的至少是2，为什么给我的样例里会出现1
    //    public static int indexFRX = 0;

    //    private static  DataTable mDataCode = new DataTable();

    //    //输出文本output
    //    public static string SetTrfData(DataTable dt,DataTable _DtCode)
    //    {
    //        mDataCode = _DtCode;
    //        string[] singlename = dt.Rows[0]["Order_singleid"].ToString().Split('+');
    //        for (int i = 0; i < dt.Rows.Count; i++)
    //        {

    //            string[] singlenamei = dt.Rows[i]["Order_singleid"].ToString().Split('+');
    //            if (singlenamei.Length == singlename.Length && singlenamei.Length >= 2)
    //            {
    //                if (singlename[0] != singlename[0])
    //                {
    //                    indexGLX = 1;
    //                }
    //                if (singlename[1] != singlename[1])
    //                {
    //                    indexFRX = 1;
    //                }
    //            }
    //        }

    //        string output = LayOutREL(dt) + LayOutBTH(dt) + LayOutORD(dt);
    //        IEnumerable<IGrouping<string, DataRow>> result = dt.Rows.Cast<DataRow>().GroupBy(dr => dr["Order_singleid"].ToString());
    //        if (result != null && result.Count() > 0)
    //        {
    //            foreach (IGrouping<string, DataRow> rows in result)
    //            {
    //                DataTable subData = rows.ToArray().CopyToDataTable();
    //                output += LayOutPOS(subData);
    //                for (int i = 0; i < subData.Rows.Count; i++)
    //                {
    //                    output += LayOutGLX(subData, i + 1);
    //                }
    //                for (int i = 0; i < subData.Rows.Count - 1; i++)
    //                {
    //                    output += LayOutFRX(subData, i + 1);
    //                }
    //            }
    //        }

    //        return output;
    //    }

    //    public static string[] GetCodePOS(string ordersinglename)   // Order_singlename   对应玻璃代码和框架代码，这里用来比对的是ordername
    //    {
    //        DataTable dt = new DataTable();// FileDialog.GetXlsData(@"..\Debug\玻璃代码.xls");
    //        dt = mDataCode;
    //        string[] valuesCode = new string[10] { "", "", "", "", "", "", "", "", "", "" };
    //        string[] a = ordersinglename.Split('+');
    //        for (int i = 0; i < a.Length; i++)
    //        {


    //            for (int j = 0; j < dt.Rows.Count; j++)
    //            {

    //                if (a[i].Contains(dt.Rows[j][2].ToString()) && dt.Rows[j][2].ToString() != "" && dt.Rows[j][1].ToString() != "")
    //                {
    //                    valuesCode[i] = dt.Rows[j][1].ToString();
    //                }
    //                if (a[i].Contains(dt.Rows[j][5].ToString()) && dt.Rows[j][5].ToString() != "" && dt.Rows[j][4].ToString() != "")
    //                {
    //                    valuesCode[i] = dt.Rows[j][4].ToString();
    //                }


    //            }
    //        }
    //        return valuesCode;
    //    }


    //    //材料代码在对方软件中 
    //    public static int[] GetCodeGLX(string name)   // Order_singlename    values[0]玻璃代码  values[1]框架代码  values[2]密封材料   values[3]框架材料   values[4]气体类型
    //    {
    //        int[] valuesCode = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    //        if (name.Contains("密"))
    //        {
    //            valuesCode[2] = 1;
    //        }
    //        if (name.Contains("结"))
    //        {
    //            valuesCode[2] = 2;
    //        }

    //        return valuesCode;
    //    }

    //    public static int[] GetCodeFRX(string name)   // Order_singlename    values[0]玻璃代码  values[1]框架代码  values[2]密封材料   values[3]框架材料   values[4]气体类型
    //    {
    //        int[] valuesCode = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    //        if (name.Contains("铝"))
    //        {
    //            valuesCode[3] = 0;
    //        }
    //        if (name.Contains("钢"))
    //        {
    //            valuesCode[3] = 1;
    //        }
    //        if (name.Contains("不锈钢"))
    //        {
    //            valuesCode[3] = 2;
    //        }
    //        if (name.Contains("塑"))
    //        {
    //            valuesCode[3] = 3;
    //        }
    //        if (name.Contains("间隔条"))
    //        {
    //            valuesCode[3] = 4;
    //        }
    //        if (name.Contains("TPS"))
    //        {
    //            valuesCode[3] = 5;
    //        }

    //        return valuesCode;
    //    }




    //    public static string LayOutREL(DataTable dt)
    //    {
    //        // 记录类型<REL>传输文件记录  |  传输文件编号：02.90  |  备注：空 
    //        string rel = "<REL>|02.90|" + ReLen("", 40) + "\n";
    //        return rel;
    //    }


    //    public static string LayOutBTH(DataTable dt)
    //    {
    //        string tProcess_number = System.Text.RegularExpressions.Regex.Replace(dt.Rows[0]["Process_number"].ToString(), @"[^0-9]+", "");//仅数字
    //        //  记录类型<BTH>  |  附加批次信息                                      |启动条码（仅适用于唯一的条码）: 空 |  实际批次号                              
    //        string bth = "<BTH>|" + ReLen(dt.Rows[0]["Order_id"].ToString(), 10) + "|" + ReLen("", 6, '0') + "|" + ReLen(tProcess_number, 8, '0') + "\n";   //注："Order_id"替换成了"Process_number"
    //        return bth;
    //    }



    //    public static string LayOutORD(DataTable dt)
    //    {
    //        string tProcess_number = System.Text.RegularExpressions.Regex.Replace(dt.Rows[0]["Process_number"].ToString(), @"[^0-9]+", "");//仅数字，最高9位
    //        //  记录类型<ORD>  | 订单号(最高9位)：                                  |  客户编号： 空   |  客户名                                                 |information 1               2                    3                       4                      5            |  生产日期                                 |  交货日期|Delivery area 
    //        string ord = "<ORD>|" + ReLen(tProcess_number, 9) + " |" + ReLen("1", 10) + "|" + ReLen(dt.Rows[0]["Customer_name"].ToString(), 40) + "|" + ReLen("", 40) + "|" + ReLen("", 40) + "|" + ReLen("", 40) + "|" + ReLen("", 40) + "|" + ReLen("", 40) + "|" + DateTime.Now.ToString("dd/MM/yyyy") + "|00/00/0000|" + ReLen("", 10, '0') + "\n";
    //        return ord;
    //    }


    //    //在POS中通过表格将玻璃和框架转换成对应代码 对方表示客户只是使用了一种密封材料  这里缺少一个参数密封深度，当前设为70
    //    public static string LayOutPOS(DataTable dt)
    //    {
    //        string[] a = dt.Rows[0]["Order_singleid"].ToString().Split('.');
    //        string[] value = GetCodePOS(dt.Rows[0]["Order_singlename"].ToString());

    //        // 记录类型<POS>物料项目记录| 项目号: （对每订单唯一） |  ID             |     条码                |       单位数量           |宽                                                         | 高                                                        |
    //        string pos = "<POS>|" + ReLen(a[1], 5, '0') + "|" + ReLen("1", 8, '0') + "|" + ReLen("1", 4, '0') + "|" + ReLen("1", 5, '0') + "|" + ReLen(dt.Rows[0]["Order_width"].ToString() + "0", 6, '0') + "|" + ReLen(dt.Rows[0]["Order_length"] + "0".ToString(), 6, '0') + "|"
    //            //  玻璃代码1/信息                 | 玻璃框架代码1/信息|                                        玻璃代码2/信息 | 玻璃框架代码2/信息|                                            玻璃代码3/信息 | 玻璃框架代码3/信息|      玻璃代码4/信息 | 玻璃框架代码4/信息 |     玻璃代码5/信息                                                |密封(1/10mm)                 | 框架文本号：00          | 气体类型01 00 00 00                                                                                    |    密封材料代码          |    窗口类型              | 窗框高度               |方向0（0无方向1水平2垂直               | 物品使用0
    //                     + ReLen(value[0].ToString(), 5) + "|" + ReLen(value[1].ToString(), 3) + "|" + ReLen(value[2].ToString(), 5) + "|" + ReLen(value[3].ToString(), 3) + "|" + ReLen(value[4].ToString(), 5) + "|" + ReLen(value[5].ToString(), 3) + "|" + ReLen(value[6].ToString(), 5) + "|" + ReLen(value[7].ToString(), 3) + "|" + ReLen("", 5) + "|" + ReLen("70", 4, '0') + "|" + ReLen("", 2, '0') + "|" + ReLen("1", 2, '0') + "|" + ReLen("", 2, '0') + "|" + ReLen("", 2, '0') + "|" + ReLen("", 2, '0') + "|" + ReLen(value[0].ToString(), 1, '0') + "|" + ReLen("1", 1, '0') + "|" + ReLen("", 6, '0') + "|" + ReLen("", 1, '0') + "|" + ReLen("", 1, '0') + "\n";
    //        return pos;
    //    }

    //    //这里缺少一个参数玻璃厚度 当前设为50
    //    public static string LayOutGLX(DataTable dt, int i)
    //    {
    //        int[] value = GetCodeGLX(dt.Rows[0]["Order_singlename"].ToString());
    //        string[] a = dt.Rows[0]["Order_singlename"].ToString().Split('+');
    //        string describe = "";
    //        if (a.Length >= 2 * i - 2)
    //        {
    //            describe = a[2 * i - 2];
    //        }

    //        //                                                                                         输入信息少玻璃厚度！
    //        //记录类型<GLX>第X片玻璃信息| 项目数量索引（全部相同为0）|  玻璃描述| 玻璃表面类型：0   | 玻璃厚度                 | DGU图案/图层：0（0无1正面2反面）| ID（推车/支架信息）：1   | 图案方向0（0无方向1水平2垂直） |  玻璃面板ID（条形码） | GPS.prod中的窗格号码  |  GPS.prod中的组件号：00  | 材料类别：00
    //        string glx = "<GL" + i + ">|" + ReLen(indexGLX.ToString(), 5, '0') + "|" + ReLen(describe, 40) + "|0|" + ReLen("50", 5, '0') + "|0|" + ReLen("", 10) + "|0|" + ReLen("", 10, '0') + "|" + ReLen(i.ToString(), 1, '0') + "|" + ReLen("", 2, '0') + "|" + ReLen("", 2, '0') + "\n";
    //        return glx;
    //    }


    //    //对方说目前客户使用的框架类型是5（TPA）框架高60 框架宽就是Order_singlename中间那部分
    //    public static string LayOutFRX(DataTable dt, int i)
    //    {
    //        int[] value = GetCodeFRX(dt.Rows[0]["Order_singlename"].ToString());
    //        string[] a = dt.Rows[0]["Order_singlename"].ToString().Split('+');
    //        string describe = "";
    //        if (a.Length >= 2 * i - 1)
    //        {
    //            describe = a[2 * i - 1];
    //        }
    //        string tHeight = System.Text.RegularExpressions.Regex.Replace(describe.Substring(0, 2), @"[^0-9]+", "");
    //        //记录类型<FRX>第X个框架信息 |项目数量索引（全部相同为0) | 框架描述                      | 框架类型|框架颜色| 框架宽| 框架高|ID                             |条形码
    //        string frx = "<FR" + i + ">|" + ReLen(indexFRX.ToString(), 5, '0') + "|" + ReLen(describe, 40) + "|05|00|" + ReLen(tHeight + '0', 5, '0') + "|00060|" + ReLen(dt.Rows[0]["WcsID"].ToString(), 10) + "|" + ReLen("", 10, '0') + "\n";
    //        return frx;
    //    }



    //    //字符串固定长度
    //    public static string ReLen(String strX, int n, char a = ' ')
    //    {
    //        if (StrLength(strX) <= n)
    //        {
    //            return strX.PadLeft(n - (StrLength(strX) - strX.Length), a);
    //        }
    //        else
    //        {
    //            strX = strX.Substring(1);
    //            return ReLen(strX, n, a);
    //        }

    //    }

    //    //一个汉字占三个字符
    //    public static int StrLength(string s)
    //    {
    //        int length = 0;
    //        for (int i = 0; i < s.Length; i++)
    //        {
    //            if (s[i] > 127)
    //                length += 3;
    //            else
    //                length += 1;
    //        }
    //        return length;
    //    }

    //}


    ////class LisecGPSOutput
    ////{
    ////    public static int indexGLX = 0; //项目数量索引 对方和说明书上说玻璃条目相同为0，不同则为实际数量，但是我没搞清楚，如果不同的至少是2，为什么给我的样例里会出现1
    ////    public static int indexFRX = 0;


    ////    //输出文本output
    ////    public static string SetTrfData(DataTable dt)
    ////    {
    ////        string[] singlename = dt.Rows[0]["Order_singleid"].ToString().Split('+');
    ////        for (int i = 0; i < dt.Rows.Count; i++)
    ////        {

    ////            string[] singlenamei = dt.Rows[i]["Order_singleid"].ToString().Split('+');
    ////            if (singlenamei.Length == singlename.Length && singlenamei.Length >= 2)
    ////            {
    ////                if (singlename[0] != singlename[0])
    ////                {
    ////                    indexGLX = 1;
    ////                }
    ////                if (singlename[1] != singlename[1])
    ////                {
    ////                    indexFRX = 1;
    ////                }
    ////            }
    ////        }

    ////        string output = LayOutREL(dt) + LayOutBTH(dt) + LayOutORD(dt);
    ////        IEnumerable<IGrouping<string, DataRow>> result = dt.Rows.Cast<DataRow>().GroupBy(dr => dr["Order_singleid"].ToString());
    ////        if (result != null && result.Count() > 0)
    ////        {
    ////            foreach (IGrouping<string, DataRow> rows in result)
    ////            {
    ////                DataTable subData = rows.ToArray().CopyToDataTable();
    ////                output += LayOutPOS(subData);
    ////                for (int i = 0; i < subData.Rows.Count; i++)
    ////                {
    ////                    output += LayOutGLX(subData, i + 1);
    ////                }
    ////                for (int i = 0; i < subData.Rows.Count - 1; i++)
    ////                {
    ////                    output += LayOutFRX(subData, i + 1);
    ////                }
    ////            }
    ////        }

    ////        return output;
    ////    }

    ////    public static string[] GetCodePOS(string ordersinglename)   // Order_singlename   对应玻璃代码和框架代码，这里用来比对的是ordername
    ////    {
    ////        DataTable dt = new DataTable();// FileDialog.GetXlsData(@"..\Debug\玻璃代码.xls");
    ////        string[] valuesCode = new string[10] { "", "", "", "", "", "", "", "", "", "" };
    ////        string[] a = ordersinglename.Split('+');
    ////        for (int i = 0; i < a.Length; i++)
    ////        {


    ////            for (int j = 0; j < dt.Rows.Count; j++)
    ////            {

    ////                if (a[i].Contains(dt.Rows[j][2].ToString()) && dt.Rows[j][2].ToString() != "" && dt.Rows[j][1].ToString() != "")
    ////                {
    ////                    valuesCode[i] = dt.Rows[j][1].ToString();
    ////                }
    ////                if (a[i].Contains(dt.Rows[j][5].ToString()) && dt.Rows[j][5].ToString() != "" && dt.Rows[j][4].ToString() != "")
    ////                {
    ////                    valuesCode[i] = dt.Rows[j][4].ToString();
    ////                }


    ////            }
    ////        }
    ////        return valuesCode;
    ////    }


    ////    //材料代码在对方软件中 
    ////    public static int[] GetCodeGLX(string name)   // Order_singlename    values[0]玻璃代码  values[1]框架代码  values[2]密封材料   values[3]框架材料   values[4]气体类型
    ////    {
    ////        int[] valuesCode = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    ////        if (name.Contains("密"))
    ////        {
    ////            valuesCode[2] = 1;
    ////        }
    ////        if (name.Contains("结"))
    ////        {
    ////            valuesCode[2] = 2;
    ////        }

    ////        return valuesCode;
    ////    }

    ////    public static int[] GetCodeFRX(string name)   // Order_singlename    values[0]玻璃代码  values[1]框架代码  values[2]密封材料   values[3]框架材料   values[4]气体类型
    ////    {
    ////        int[] valuesCode = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    ////        if (name.Contains("铝"))
    ////        {
    ////            valuesCode[3] = 0;
    ////        }
    ////        if (name.Contains("钢"))
    ////        {
    ////            valuesCode[3] = 1;
    ////        }
    ////        if (name.Contains("不锈钢"))
    ////        {
    ////            valuesCode[3] = 2;
    ////        }
    ////        if (name.Contains("塑"))
    ////        {
    ////            valuesCode[3] = 3;
    ////        }
    ////        if (name.Contains("间隔条"))
    ////        {
    ////            valuesCode[3] = 4;
    ////        }
    ////        if (name.Contains("TPS"))
    ////        {
    ////            valuesCode[3] = 5;
    ////        }

    ////        return valuesCode;
    ////    }




    ////    public static string LayOutREL(DataTable dt)
    ////    {
    ////        // 记录类型<REL>传输文件记录  |  传输文件编号：02.90  |  备注：空 
    ////        string rel = "<REL>|02.90|" + ReLen("", 40) + "\n";
    ////        return rel;
    ////    }


    ////    public static string LayOutBTH(DataTable dt)
    ////    {
    ////        string tProcess_number = System.Text.RegularExpressions.Regex.Replace(dt.Rows[0]["Process_number"].ToString(), @"[^0-9]+", "");//仅数字
    ////        //  记录类型<BTH>  |  附加批次信息                                      |启动条码（仅适用于唯一的条码）: 空 |  实际批次号                              
    ////        string bth = "<BTH>|" + ReLen(dt.Rows[0]["Order_id"].ToString(), 10) + "|" + ReLen("", 6, '0') + "|" + ReLen(tProcess_number, 8, '0') + "\n";   //注："Order_id"替换成了"Process_number"
    ////        return bth;
    ////    }



    ////    public static string LayOutORD(DataTable dt)
    ////    {
    ////        string tProcess_number = System.Text.RegularExpressions.Regex.Replace(dt.Rows[0]["Process_number"].ToString(), @"[^0-9]+", "");//仅数字，最高9位
    ////        //  记录类型<ORD>  | 订单号(最高9位)：                                  |  客户编号： 空   |  客户名                                                 |information 1               2                    3                       4                      5            |  生产日期                                 |  交货日期|Delivery area 
    ////        string ord = "<ORD>|" + ReLen(tProcess_number, 9) + " |" + ReLen("1", 10) + "|" + ReLen(dt.Rows[0]["Customer_name"].ToString(), 40) + "|" + ReLen("", 40) + "|" + ReLen("", 40) + "|" + ReLen("", 40) + "|" + ReLen("", 40) + "|" + ReLen("", 40) + "|" + DateTime.Now.ToString("dd/MM/yyyy") + "|00/00/0000|" + ReLen("", 10, '0') + "\n";
    ////        return ord;
    ////    }


    ////    //在POS中通过表格将玻璃和框架转换成对应代码 对方表示客户只是使用了一种密封材料  这里缺少一个参数密封深度，当前设为70
    ////    public static string LayOutPOS(DataTable dt)
    ////    {
    ////        string[] a = dt.Rows[0]["Order_singleid"].ToString().Split('.');
    ////        string[] value = GetCodePOS(dt.Rows[0]["Order_singlename"].ToString());

    ////        // 记录类型<POS>物料项目记录| 项目号: （对每订单唯一） |  ID             |     条码                |       单位数量           |宽                                                         | 高                                                        |
    ////        string pos = "<POS>|" + ReLen(a[1], 5, '0') + "|" + ReLen("1", 8, '0') + "|" + ReLen("1", 4, '0') + "|" + ReLen("1", 5, '0') + "|" + ReLen(dt.Rows[0]["Order_width"].ToString() + "0", 6, '0') + "|" + ReLen(dt.Rows[0]["Order_length"] + "0".ToString(), 6, '0') + "|"
    ////            //  玻璃代码1/信息                 | 玻璃框架代码1/信息|                                        玻璃代码2/信息 | 玻璃框架代码2/信息|                                            玻璃代码3/信息 | 玻璃框架代码3/信息|      玻璃代码4/信息 | 玻璃框架代码4/信息 |     玻璃代码5/信息                                                |密封(1/10mm)                 | 框架文本号：00          | 气体类型01 00 00 00                                                                                    |    密封材料代码          |    窗口类型              | 窗框高度               |方向0（0无方向1水平2垂直               | 物品使用0
    ////                     + ReLen(value[0].ToString(), 5) + "|" + ReLen(value[1].ToString(), 3) + "|" + ReLen(value[2].ToString(), 5) + "|" + ReLen(value[3].ToString(), 3) + "|" + ReLen(value[4].ToString(), 5) + "|" + ReLen(value[5].ToString(), 3) + "|" + ReLen(value[6].ToString(), 5) + "|" + ReLen(value[7].ToString(), 3) + "|" + ReLen("", 5) + "|" + ReLen("70", 4, '0') + "|" + ReLen("", 2, '0') + "|" + ReLen("1", 2, '0') + "|" + ReLen("", 2, '0') + "|" + ReLen("", 2, '0') + "|" + ReLen("", 2, '0') + "|" + ReLen(value[0].ToString(), 1, '0') + "|" + ReLen("1", 1, '0') + "|" + ReLen("", 6, '0') + "|" + ReLen("", 1, '0') + "|" + ReLen("", 1, '0') + "\n";
    ////        return pos;
    ////    }

    ////    //这里缺少一个参数玻璃厚度 当前设为50
    ////    public static string LayOutGLX(DataTable dt, int i)
    ////    {
    ////        int[] value = GetCodeGLX(dt.Rows[0]["Order_singlename"].ToString());
    ////        string[] a = dt.Rows[0]["Order_singlename"].ToString().Split('+');
    ////        string describe = "";
    ////        if (a.Length >= 2 * i - 2)
    ////        {
    ////            describe = a[2 * i - 2];
    ////        }

    ////        //                                                                                         输入信息少玻璃厚度！
    ////        //记录类型<GLX>第X片玻璃信息| 项目数量索引（全部相同为0）|  玻璃描述| 玻璃表面类型：0   | 玻璃厚度                 | DGU图案/图层：0（0无1正面2反面）| ID（推车/支架信息）：1   | 图案方向0（0无方向1水平2垂直） |  玻璃面板ID（条形码） | GPS.prod中的窗格号码  |  GPS.prod中的组件号：00  | 材料类别：00
    ////        string glx = "<GL" + i + ">|" + ReLen(indexGLX.ToString(), 5, '0') + "|" + ReLen(describe, 40) + "|0|" + ReLen("50", 5, '0') + "|0|" + ReLen("", 10) + "|0|" + ReLen("", 10, '0') + "|" + ReLen(i.ToString(), 1, '0') + "|" + ReLen("", 2, '0') + "|" + ReLen("", 2, '0') + "\n";
    ////        return glx;
    ////    }


    ////    //对方说目前客户使用的框架类型是5（TPA）框架高60 框架宽就是Order_singlename中间那部分
    ////    public static string LayOutFRX(DataTable dt, int i)
    ////    {
    ////        int[] value = GetCodeFRX(dt.Rows[0]["Order_singlename"].ToString());
    ////        string[] a = dt.Rows[0]["Order_singlename"].ToString().Split('+');
    ////        string describe = "";
    ////        if (a.Length >= 2 * i - 1)
    ////        {
    ////            describe = a[2 * i - 1];
    ////        }
    ////        string tHeight = System.Text.RegularExpressions.Regex.Replace(describe, @"[^0-9]+", "");
    ////        //记录类型<FRX>第X个框架信息 |项目数量索引（全部相同为0) | 框架描述                      | 框架类型|框架颜色| 框架宽| 框架高|ID                             |条形码
    ////        string frx = "<FR" + i + ">|" + ReLen(indexFRX.ToString(), 5, '0') + "|" + ReLen(describe, 40) + "|05|00|" + ReLen(tHeight + '0', 5, '0') + "|00060|" + ReLen(dt.Rows[0]["WcsID"].ToString(), 10) + "|" + ReLen("", 10, '0') + "\n";
    ////        return frx;
    ////    }



    ////    //字符串固定长度
    ////    public static string ReLen(String strX, int n, char a = ' ')
    ////    {
    ////        if (StrLength(strX) <= n)
    ////        {
    ////            return strX.PadLeft(n - (StrLength(strX) - strX.Length), a);
    ////        }
    ////        else
    ////        {
    ////            strX = strX.Substring(1);
    ////            return ReLen(strX, n, a);
    ////        }

    ////    }

    ////    //一个汉字占三个字符
    ////    public static int StrLength(string s)
    ////    {
    ////        int length = 0;
    ////        for (int i = 0; i < s.Length; i++)
    ////        {
    ////            if (s[i] > 127)
    ////                length += 3;
    ////            else
    ////                length += 1;
    ////        }
    ////        return length;
    ////    }

    ////}
}
