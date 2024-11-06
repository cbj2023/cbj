using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SUTAIMES.GetData
{
    class ClsDispostErp
    {
        Program tSystem;
        public ClsDispostErp(Program tSys)
        {
            tSystem = tSys;
        }
        public void GetErpData(string _Optimize,ref string _RetStr)
        {
            string tLogStr = "";

            string Url = "";
            string result2 = "";
            ///使用JsonWriter写字符串：
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("ucode");
            writer.WriteValue(clsMyPublic.mErp_ucode );// ("auto");
            writer.WritePropertyName("upwd");
            writer.WriteValue(clsMyPublic.mErp_upwd );// ("auto139");
            writer.WritePropertyName("passkey");
            writer.WriteValue(clsMyPublic.mErp_passkey );// ("8BD2AD871782BD660A75C5A6D2902851DF1D64AF829D65BB");
            writer.WritePropertyName("djno");
            writer.WriteValue(_Optimize);
            writer.WriteEndObject();
            writer.Flush();
            string jsonText = "";
            jsonText = sw.GetStringBuilder().ToString();

            Url = clsMyPublic.mErpUrl ;
            if (Url.Trim().Length == 0)
            {
                Url = "https://www.qdlkd.net/ypstock/lkd/lxauto/lxjhlinetest";//"https://www.qdlkd.net/ypstock/lkd/lxauto/lxjhline";
            }
            DateTime tNow = DateTime.Now;
            result2 = clsMyPublic.PostUrl(Url, jsonText);
            double tt = DateTime.Now.Subtract(tNow).TotalMilliseconds;
            //texErpStr1.Text = tt.ToString() + " " + DateTime.Now.ToString("yy-MM-dd HH:mm:ss ") + result2;
            tLogStr = " 基础数据 " + result2;

            GetData.ClsGetData tClsGetData = new GetData.ClsGetData(); DataTable tDataTable = new DataTable();
            tDataTable = tClsGetData.GetLINSHIERPNew(result2,"");
            string tRetStr = "";
            tSystem.mClsDBUPdate.ExecuteTvp("Pro_ToExcelB", tDataTable, ref tRetStr);
           
            if (tDataTable != null)
            {
                _RetStr = "OK";
            }
            else if (tDataTable.Rows.Count > 0)
            {
                _RetStr = "NG";
            }

            Url = "";
            Url = clsMyPublic.mErpUrlLay ;
            if (Url.Trim().Length == 0)
            {
                Url = "https://www.qdlkd.net/ypstock/lkd/lxauto/lxjhposinfo";
            }
            result2 = clsMyPublic.PostUrl(Url, jsonText);
            tDataTable = tClsGetData.GetLinShiEepLay(result2, _Optimize);
            tRetStr = "";
            tSystem.mClsDBUPdate.ExecuteTvp("Pro_ToExcelLayB", tDataTable, ref tRetStr);
        

            tSystem.mClsDBUPdate.DBClose();
            tLogStr = tLogStr + "\r\n" + " 坐标数据 " + result2;
            tSystem.mFile.WriteLog("", tLogStr + "N");
        }
    }
}
