using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.IO;
using System.Net;

using System.Security.Cryptography.X509Certificates;

using System.Net.Security;

using System.Data;
namespace SUTAIMES
{


    class clsMyPublic
    {
        public static string limit = null;//用户权限
        public static string user = null;//用户
        public static string m_AppExePath = null;
        public static string m_IniPath = null;

        public static string mIniSytem="1";//  1链接PLC   2不链接PLC

        public static string mMonitor = "999";//监控位置

        public static string mPLCIFA = "0";//前端PLC
        public static string mPLCIPA = "192.168.11.40";

        public static string mPLCIFB = "0";//中空PLC
        public static string mPLCIPB = "192.168.11.60";

        public static string mErpRetIF = "0";

        public static string mstrConnectionA = "";
        public static string mstrConnectionB = "";

        public static int m_LoginSuccess = 0;      //0 登录失败或者未登录 1 登录成功

        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        //初始化系统参数

        public static string mURl1 = "";
        public static string mURl2 = "";
        public static string mZKEnport = "";
        public static string mZKExit = "1";

        public static string mMBExit = "1";
        public static string mYuanDB = "0";
        public static string mYuanAuto = "0";

        public static string mUser_name = "";
        public static string mPassword = "";

        public static DataRow[] mZKDataRow;

        public static string mHttpIF = "";
        public static string mMBauto = "0";
        public static string mZKauto = "0";

        public static string mLine = "0";

        public static string mPrintLine = "";
        public static string mPrintQC = "";
        public static string mPrintName = "";

        public static string[] autoPrintName =new string[3]{"","",""};
        public static string[] autoPrintLine = new string[3] { "", "", "" };
        public static string[] autoPrintQC = new string[3] { "", "", "" };

        public static string mErpUrl="";
        public static string mErp_ucode = "";
        public static string mErp_upwd = "";
        public static string mErp_passkey="";
        public static string mErpUrlLay = "";
        public static string mErpUrlLot = "";

        public static string mSYSNAME = "";

        public static string mPLCIFBY = "0";//原片仓PLC
        public static string mPLCIPBY = "192.168.11.10";

        public static void gIniSysParam()
        {
            try
            {
                m_AppExePath = Application.StartupPath;

                //读取ini文件路径
                m_LoginSuccess = 0;
                m_IniPath = Application.StartupPath + @"\Config.ini";

                StringBuilder stmp = new StringBuilder(255);

                GetPrivateProfileString("SYS", "SYSNAME", "", stmp, 255, m_IniPath);
                mSYSNAME = stmp.ToString();
                //数据库信息
                //WMS数据库
                GetPrivateProfileString("SYS", "INISYS", "", stmp, 255, m_IniPath);
                mIniSytem = stmp.ToString();

                GetPrivateProfileString("SYS", "Monitor", "", stmp, 255, m_IniPath);
                mMonitor = stmp.ToString();

                GetPrivateProfileString("SYS", "PLCIFA", "", stmp, 255, m_IniPath);
                mPLCIFA = stmp.ToString();

                GetPrivateProfileString("SYS", "PLCIPA", "", stmp, 255, m_IniPath);
                mPLCIPA = stmp.ToString();

                GetPrivateProfileString("SYS", "PLCIFB", "", stmp, 255, m_IniPath);
                mPLCIFB = stmp.ToString();

                GetPrivateProfileString("SYS", "PLCIPB", "", stmp, 255, m_IniPath);
                mPLCIPB = stmp.ToString();


                GetPrivateProfileString("SYS", "strConnectionA", "", stmp, 255, m_IniPath);
                mstrConnectionA = stmp.ToString();

                GetPrivateProfileString("SYS", "strConnectionB", "", stmp, 255, m_IniPath);
                mstrConnectionB = stmp.ToString();

                GetPrivateProfileString("SYS", "URL1", "", stmp, 255, m_IniPath);
                mURl1 = stmp.ToString();

                GetPrivateProfileString("SYS", "URL2", "", stmp, 255, m_IniPath);
                mURl2 = stmp.ToString();

                GetPrivateProfileString("SYS", "ZKExit", "", stmp, 255, m_IniPath);
                mZKExit = stmp.ToString();

                GetPrivateProfileString("SYS", "ErpRetIF", "", stmp, 255, m_IniPath);
                mErpRetIF = stmp.ToString();


                GetPrivateProfileString("SYS", "MBExit", "", stmp, 255, m_IniPath);
                mMBExit = stmp.ToString();

                GetPrivateProfileString("SYS", "YuanDB", "", stmp, 255, m_IniPath);
                mYuanDB = stmp.ToString();

                GetPrivateProfileString("SYS", "YuanAuto", "", stmp, 255, m_IniPath);
                mYuanAuto = stmp.ToString();

                GetPrivateProfileString("SYS", "User_name", "", stmp, 255, m_IniPath);
                mUser_name = stmp.ToString();

                GetPrivateProfileString("SYS", "Password", "", stmp, 255, m_IniPath);
                mPassword = stmp.ToString();


                GetPrivateProfileString("SYS", "HttpIF", "", stmp, 255, m_IniPath);
                mHttpIF = stmp.ToString();


                GetPrivateProfileString("SYS", "MBauto", "", stmp, 255, m_IniPath);
                mMBauto = stmp.ToString();


                GetPrivateProfileString("SYS", "ZKauto", "", stmp, 255, m_IniPath);
                mZKauto = stmp.ToString();

                GetPrivateProfileString("SYS", "Line", "", stmp, 255, m_IniPath);
                mLine = stmp.ToString();

                GetPrivateProfileString("PRINT", "PrintLine", "", stmp, 255, m_IniPath);
                mPrintLine = stmp.ToString();

                GetPrivateProfileString("PRINT", "PrintQC", "", stmp, 255, m_IniPath);
                mPrintQC = stmp.ToString();

                GetPrivateProfileString("PRINT", "PrintName", "", stmp, 255, m_IniPath);
                mPrintName = stmp.ToString();


                GetPrivateProfileString("PRINT", "autoPrintName1", "", stmp, 255, m_IniPath);
                autoPrintName[0] = stmp.ToString();

                GetPrivateProfileString("PRINT", "autoPrintName2", "", stmp, 255, m_IniPath);
                autoPrintName[1] = stmp.ToString();

                GetPrivateProfileString("PRINT", "autoPrintName3", "", stmp, 255, m_IniPath);
                autoPrintName[2] = stmp.ToString();



                GetPrivateProfileString("PRINT", "autoPrintLine1", "", stmp, 255, m_IniPath);
                autoPrintLine[0] = stmp.ToString();

                GetPrivateProfileString("PRINT", "autoPrintLine2", "", stmp, 255, m_IniPath);
                autoPrintLine[1] = stmp.ToString();

                GetPrivateProfileString("PRINT", "autoPrintLine3", "", stmp, 255, m_IniPath);
                autoPrintLine[2] = stmp.ToString();

                GetPrivateProfileString("PRINT", "autoPrintQC1", "", stmp, 255, m_IniPath);
                autoPrintQC[0] = stmp.ToString();

                GetPrivateProfileString("PRINT", "autoPrintQC2", "", stmp, 255, m_IniPath);
                autoPrintQC[1] = stmp.ToString();

                GetPrivateProfileString("PRINT", "autoPrintQC3", "", stmp, 255, m_IniPath);
                autoPrintQC[2] = stmp.ToString();

                GetPrivateProfileString("PRINT", "Left1", "", stmp, 255, m_IniPath);
                mLeft1 = stmp.ToString();

                GetPrivateProfileString("PRINT", "Left2", "", stmp, 255, m_IniPath);
                mLeft2 = stmp.ToString();

                GetPrivateProfileString("PRINT", "Top1", "", stmp, 255, m_IniPath);
                mTop1 = stmp.ToString();

                GetPrivateProfileString("PRINT", "TopTemp", "", stmp, 255, m_IniPath);
                mTopTemp = stmp.ToString();

                GetPrivateProfileString("ERP", "ErpUrl", "", stmp, 255, m_IniPath);
                mErpUrl = stmp.ToString();

                GetPrivateProfileString("ERP", "Erp_ucode", "", stmp, 255, m_IniPath);
                mErp_ucode = stmp.ToString();

                GetPrivateProfileString("ERP", "Erp_upwd", "", stmp, 255, m_IniPath);
                mErp_upwd = stmp.ToString();

                GetPrivateProfileString("ERP", "Erp_passkey", "", stmp, 255, m_IniPath);
                mErp_passkey = stmp.ToString();


                GetPrivateProfileString("ERP", "ErpUrlLay", "", stmp, 255, m_IniPath);
                mErpUrlLay = stmp.ToString();

                GetPrivateProfileString("ERP", "ErpUrlLot", "", stmp, 255, m_IniPath);
                mErpUrlLot = stmp.ToString();

                GetPrivateProfileString("SYS", "PLCIFBY", "", stmp, 255, m_IniPath);
                mPLCIFBY = stmp.ToString();

                GetPrivateProfileString("SYS", "PLCIPBY", "", stmp, 255, m_IniPath);
                mPLCIPBY = stmp.ToString();

                GetPrivateProfileString("SYS", "ZKEnport", "", stmp, 255, m_IniPath);
                mZKEnport = stmp.ToString();
                
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "  " + ex.StackTrace);

            }
        }
        public static string mLeft1 = "", mLeft2 = "", mTop1 = "", mTopTemp="";

        public static void SetPrivete(string _Key, string _Value)
        {

            WritePrivateProfileString("SYS", _Key, _Value, m_IniPath);
        }
        public static void SetWritePrivateProfileString(string section, string key, string val, string filePath)
        {
            try
            {
                WritePrivateProfileString( section,  key,  val,  filePath);
            }
            catch
            {
            }
        }

        public static bool IsJArray(string tStrValue)
        {
            if (tStrValue.IndexOf("[") == 0 & tStrValue.Substring(tStrValue.Length - 1, 1) == "]")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsJson(string tStrValue)
        {
            if (tStrValue.IndexOf("{") == 0 & tStrValue.Substring(tStrValue.Length - 1, 1) == "}")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// https传值
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string PostUrl(string url, string postData)
        {
       
            HttpWebRequest request = null;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                request = WebRequest.Create(url) as HttpWebRequest;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request.ProtocolVersion = HttpVersion.Version11;


                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3 ;
                // 这里设置了协议类型。
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | SecurityProtocolType.Ssl3;
                request.KeepAlive = false;
                ServicePointManager.CheckCertificateRevocationList = true;
                ServicePointManager.DefaultConnectionLimit = 100;
                ServicePointManager.Expect100Continue = false;
                request.Timeout = 20000;
            }
            else
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 20000;
            }

            try
            {
                request.Method = "POST";    //使用get方式发送数据
                request.ContentType = "application/json;charset=UTF-8";//"application/x-www-form-urlencoded";
                request.Referer = null;
                request.AllowAutoRedirect = true;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                request.Accept = "*/*";

                byte[] data = Encoding.UTF8.GetBytes(postData);
                Stream newStream = request.GetRequestStream();
                newStream.Write(data, 0, data.Length);
                newStream.Close();

                //获取网页响应结果
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                //client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                string result = string.Empty;
                using (StreamReader sr = new StreamReader(stream))
                {
                    result = sr.ReadToEnd();
                }

                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        ///////////

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
        ///
        #region 加密使用

        /// <summary>
        /// MD5 16位加密 加密后密码为小写
        /// </summary>
        /// <param name="ConvertString"></param>
        /// <returns></returns>
        public static string GetMd5Str(string ConvertString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(ConvertString)), 4, 8);
            t2 = t2.Replace("-", "");

            t2 = t2.ToLower();

            return t2;
        }
        /// <summary>
        /// AES 算法加密
        /// </summary>
        /// <param name="content">明文</param>
        /// <param name="Key">密钥</param>
        /// <returns>加密后的密文</returns>
        public static string AESEncrypt(string encryptStr, string key = "12345678987654321234567890987654")
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(encryptStr);
            RijndaelManaged rDel = new RijndaelManaged
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string AESDecrypt(string decryptStr, string key = "12345678987654321234567890987654")
        {
            try
            {
                byte[] keyArray = Encoding.UTF8.GetBytes(key);
                byte[] toEncryptArray = Convert.FromBase64String(decryptStr);
                RijndaelManaged rDel = new RijndaelManaged
                {
                    Key = keyArray,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                ICryptoTransform cTransform = rDel.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Encoding.UTF8.GetString(resultArray);
            }
            catch
            {
                return "";
            }
        }

        #endregion

    }
}
