using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace SUTAIMES.GetData
{
    class ClsGetYuanData
    {
        public class SheetBasic
        {
            public string sheetid { get; set; } //原片基础ID
            public string sheetname { get; set; } //原片名称
            public string ply { get; set; } //厚度
            public string color{ get; set; } //颜色
            public string unit { get; set; } //单位
            public string sheettype{ get; set; }//原片分类
            public string fmemo { get; set; } //备注
            public string colorno{ get; set; } //颜色编号
        }

        public class SheetIn
        {
            public string sheetsid { get; set; } //库存ID 唯一
            public string height { get; set; }//高
            public string width { get; set; }//宽
            public string packnum { get; set; }//实际数量
            public string zarea { get; set; }//面积
            public string boxnum { get; set; }//箱重
            public string zps { get; set; }//总片数
            public string ylnum { get; set; }//预留数
            public string kynum { get; set; }//可用数
            public string fmemo { get; set; }//备注
            public string rkfmemo { get; set; }//入库备注
            public string packqty { get; set; }//装箱率
            public string sheettype { get; set; }//原片分类
            public string sheetname { get; set; }//原片名称
            public string ply { get; set; }//原片厚度
            public string unit { get; set; }//单位
            public string djname { get; set; }//等级名称
            
            public string sheetid { get; set; }//分配ID

            public string kwcode { get; set; }//

            public string cdname { get; set; }//产地

            public string kwname { get; set; }//库位

            public string ztun { get; set; }//吨重
            public string ckcode { get; set; }//仓库编号
            public string ckname { get; set; }//仓库名称
        }


        public DataTable GetSheetIn(string _InputStr)
        {
            string[] tArrName = { };
            string[] tArrValue = { };

            List<SheetIn> tListData = new List<SheetIn>();
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
                            SheetIn _Data = (SheetIn)serializer.Deserialize(new JsonTextReader(sr), typeof(SheetIn));

                            tListData.Add(_Data);
                        }
                    }
                }
            }
            if (tListData.Count > 0)
            {
                DataTable _tDt = new DataTable();
                _tDt = ToDataTable<SheetIn>(tListData);

                return _tDt;
            }
            return null;

        }


        public DataTable GetSheetBasic(string _InputStr)
        {
            string[] tArrName = { };
            string[] tArrValue = { };

            List<SheetBasic> tListData = new List<SheetBasic>();
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
                            SheetBasic _Data = (SheetBasic)serializer.Deserialize(new JsonTextReader(sr), typeof(SheetBasic));

                            tListData.Add(_Data);
                        }
                    }
                }
            }
            if (tListData.Count > 0)
            {
                DataTable _tDt = new DataTable();
                _tDt = ToDataTable<SheetBasic>(tListData);

                return _tDt;
            }
            return null;

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
