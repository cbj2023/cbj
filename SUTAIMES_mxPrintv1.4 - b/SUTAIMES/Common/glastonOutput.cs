using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.IO;
using Microsoft.Win32;
namespace SUTAIMES.Common
{
    class glastonOutput
    {
        public static  string SetLenData(DataTable dt)
        {
            string _str = "";
            _str += "HL" + DateTime.Now.ToString("dd.MM.yyhh:mm") + "                    Lenhardt\r\n";
            IEnumerable<IGrouping<string, DataRow>> result = dt.Rows.Cast<DataRow>().GroupBy(dr => dr["Order_singleid"].ToString());
            if (result != null && result.Count() > 0)
            {
                foreach (IGrouping<string, DataRow> rows in result)
                {
                    DataTable subData = rows.ToArray().CopyToDataTable();
                    DataView dv = subData.DefaultView;
                    dv.Sort = "Single_tag desc";
                    subData = dv.ToTable();
                    string[] information = subData.Rows[0]["Order_singlename"].ToString().Split('+');
                    //Array.Reverse(information);
                    _str += "P                       1" + get(subData.Rows.Count.ToString(), 1) + "    0    0" + get("0", 8) + "     00000000\r\n";
                    for (int i = 0; i < subData.Rows.Count; i++)
                    {
                        if (information.Length <= 1)
                        {
                            return "false";
                        }
                        string spacer_type = "2";//1金属 2TPS 关键字未确认
                        string spacer_width = spacer_type == "0" ? "0" : System.Text.RegularExpressions.Regex.Replace(information[1].Substring(0, 2), @"[^0-9]+", "");
                        string gas_type = information[1].Contains("R") ? "1" : "1";//1充氩气 0无气体  //220730
                        _str += "S" + "0" + get("0", 4) + get(subData.Rows[i]["ply"].ToString() + "0", 4) + "     0     0     00     00000        " + get(subData.Rows.Count.ToString(), 1) + " 0   0   0  2Lenhardt0\r\n";
                        _str += "M" + get("2", 2) + get(subData.Rows[i]["single_long"].ToString(), 5) + get(subData.Rows[i]["single_short"].ToString(), 5) + "\r\n";
                        _str += "L" + get("2", 2) + subData.Rows[i]["single_long"].ToString() + ".0000;" + subData.Rows[i]["single_short"].ToString() + ".0000\r\n";
                        //R：间隔框宽度 气体类型 间隔框类型 密封深度
                        _str += "R" + get(spacer_width.ToString() + "0", 4) + get(gas_type, 2) + "0" + get(spacer_type, 1) + get(subData.Rows[i]["sealing_depth"].ToString(), 3) + " 0    0    0 0    0   0                                                                                                                                                      02LENHARDT0 0 0\r\n";
                        _str += "M" + get("2", 2) + get(subData.Rows[i]["single_long"].ToString(), 5) + get(subData.Rows[i]["single_short"].ToString(), 5) + "\r\n";
                        _str += "L" + get("2", 2) + subData.Rows[i]["single_long"].ToString() + ".0000;" + subData.Rows[i]["single_short"].ToString() + ".0000\r\n";
                    }
                    _str += "T  0         0                                                                                                                                                                                                                                                                                                                                                                                                                                                                            \r\n";
                }
                _str += "E\r\n";
            }

            return _str;
        }
        //字符串固定长度
        private static  string get(String strX, int n, char a = ' ')
        {
            int length = 0;
            for (int i = 0; i < strX.Length; i++)
            {
                if (strX[i] > 127)
                    length += 3;  // 求长度，一个汉字占三个字符
                else
                    length += 1;
            }
            if (length <= n)
            {
                return strX.PadLeft(n - (length - strX.Length), a);
            }
            else
            {
                strX = strX.Substring(1);
                return get(strX, n, a);
            }

        }
    }
}
